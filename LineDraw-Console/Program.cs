using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Console;
using static System.ConsoleColor;
using static System.ConsoleKey;
using static System.Environment;
using static System.Math;
using static System.PlatformID;
using static System.Threading.Thread;

namespace LineDraw_Console
{
    internal struct Line
    {
        public (double X, double Y) A;
        public (double X, double Y) B;

        public Line((double X, double Y) a, (double X, double Y) b)
        {
            A = a;
            B = b;
        }
    }

    internal static class Program
    {
        private static void WriteStatus(string msg)
        {
            int x = CursorLeft, y = CursorTop;
            SetCursorPosition(0, WindowHeight - 1);
            Write("".PadLeft(WindowWidth - 1));
            SetCursorPosition(0, WindowHeight - 1);
            Write(msg);
            SetCursorPosition(x, y);
        }

        private static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) return src;

            var arr = (T[]) Enum.GetValues(src.GetType());
            int j = Array.IndexOf(arr, src) + 1;
            return arr.Length == j ? arr[0] : arr[j];
        }

        // This function provides a fix for Console.ForegroundColor being "-1".
        private static void FixConsoleColor()
        {
            var arr = (ConsoleColor[]) Enum.GetValues(ForegroundColor.GetType());
            int j = Array.IndexOf(arr, ForegroundColor);
            if (j >= 0) return;

            ForegroundColor = White;
            BackgroundColor = Black;
        }

        private static void Automated(ref int[] args, int steps, bool delay)
        {
            if (OSVersion.Platform == Win32NT)
            {
                BufferHeight = WindowHeight;
                BufferWidth = WindowWidth;
            }

            CursorVisible = false;
            var lines = new List<Line>();

            for (int i = 0; i < args.Length; i += 4)
                lines.Add(
                    new Line(
                        (args[i], args[i + 1]),
                        (args[i + 2], args[i + 3])
                    ));

            foreach (Line line in lines) DrawLine(line, steps, delay, ForegroundColor);

            ReadKey(true);
            Exit(0);
        }

        private static void Interactive()
        {
            if (OSVersion.Platform == Win32NT)
            {
                BufferHeight = WindowHeight;
                BufferWidth = WindowWidth;
            }

            int exitCode = 0;
            Clear();
            WriteLine("Maximize the window and press any key to continue.");
            ReadKey(true);
            Clear();

            bool exit = false, delay = true;
            int steps = 10;

            ConsoleColor fgColor = ForegroundColor;
            while (!exit)
                try
                {
                    var line = new Line((-1, -1), (-1, -1));
                    bool done = false;
                    while (!done)
                    {
                        WriteStatus(
                            $"Select points|C:{CursorLeft},{CursorTop}|" +
                            $"A:{line.A.X},{line.B.Y}|" +
                            $"B:{line.B.X},{line.B.Y}|" +
                            $"Color:{fgColor}|" +
                            $"Delay:{delay}|" +
                            $"Steps:{steps}");
                        // ReSharper disable once SwitchStatementMissingSomeCases
                        CursorVisible = true;
                        switch (ReadKey(true).Key)
                        {
                            #region Keys

                            case D1:
                                (int xo, int yo) = (CursorLeft, CursorTop);
                                line.A = (CursorLeft, CursorTop);
                                if (line.B.X < 0 || line.B.Y < 0)
                                    line.B = (CursorLeft, CursorTop);
                                Write("A\b");
                                SetCursorPosition(xo, yo);
                                break;
                            case D2:
                                (xo, yo) = (CursorLeft, CursorTop);
                                line.B = (CursorLeft, CursorTop);
                                Write("B\b");
                                SetCursorPosition(xo, yo);
                                break;
                            case UpArrow:
                                if (CursorTop > 0)
                                    CursorTop--;
                                else CursorTop = WindowHeight - 2;
                                break;
                            case DownArrow:
                                if (CursorTop < WindowHeight - 2)
                                    CursorTop++;
                                else CursorTop = 0;
                                break;
                            case LeftArrow:
                                if (CursorLeft > 0)
                                    CursorLeft--;
                                else CursorLeft = WindowWidth - 1;
                                break;
                            case RightArrow:
                                if (CursorLeft < WindowWidth - 1)
                                    CursorLeft++;
                                else CursorLeft = 0;
                                break;
                            case D:
                                delay = !delay;
                                break;
                            case C:
                                fgColor = fgColor.Next();
                                break;
                            case R:
                                Clear();
                                // using (var w = new StreamWriter($"{Path.GetTempPath()}/coords.txt", false,
                                //     Encoding.ASCII))
                                // {
                                //     w.Write(string.Empty);
                                // }
                                if (OSVersion.Platform == Win32NT)
                                {
                                    BufferHeight = WindowHeight;
                                    BufferWidth = WindowWidth;
                                }

                                SetCursorPosition(0, 0);
                                break;
                            case OemPlus:
                            case Add:
                                if (steps < 1000)
                                    steps *= 10;
                                break;
                            case OemMinus:
                            case Subtract:
                                if (steps > 10)
                                    steps /= 10;
                                break;
                            case Enter:
                                CursorVisible = false;
                                if (line.A.X < 0 || line.A.Y < 0 || line.B.X < 0 || line.B.Y < 0)
                                {
                                    WriteStatus("Some coordinates are not set!");
                                    Sleep(1000);
                                    break;
                                }

                                done = true;
//                                Task.Run(
//                                    () =>
//                                    {
//                                        using (var w = new StreamWriter($"{Path.GetTempPath()}/coords.txt", true,
//                                            Encoding.ASCII))
//                                        {
//                                            w.Write($"{line.A.X} {line.A.Y} {line.B.X} {line.B.Y} ");
//                                        }
//                                    });
                                break;
                            case Escape:
                                CursorVisible = false;
                                exitCode = 0;
                                Exit(exitCode);
                                break;
                        }

                        #endregion Keys
                    }

                    WriteStatus("Drawing line...");
                    DrawLine(line, steps, delay, fgColor);
                    WriteStatus("");
                }
                catch (Exception e)
                {
                    // Catch any previously unknown exceptions
                    SetCursorPosition(0, 0);
                    WriteLine(e.Message);
                    WriteLine(e.StackTrace);

                    exit = true;
                    exitCode = 1;
                }

            CursorVisible = false;
            ReadKey(true);
            Exit(exitCode);
        }

        private static void DrawLine(Line line, int steps, bool delay, ConsoleColor fgColor)
        {
            double step = 1.0 / steps;
            // 2D vector between A and B
            (double dx, double dy) = (
                line.B.X - line.A.X,
                line.B.Y - line.A.Y
            );

            ConsoleColor prev = ForegroundColor;
            ForegroundColor = fgColor;
            for (double i = 0;; i += step)
            {
                // Calculate the coordinate according to the percentages
                (int x, int y) = (
                    (int) Round(line.A.X + i * dx),
                    (int) Round(line.A.Y + i * dy)
                );

                SetCursorPosition(x, y);
                Write('█');

                // ReSharper disable CompareOfFloatsByEqualityOperator
                if (x == line.B.X && y == line.B.Y) break;
                // ReSharper restore CompareOfFloatsByEqualityOperator
                if (!delay) continue;
                Sleep(1000 / steps);
            }

            ForegroundColor = prev;
        }

        private static void Main(string[] args)
        {
            FixConsoleColor();
            bool validArgs = false;
            var listArgs = new List<int>();
            if (args.Length > 1 && args.Length % 2 == 0)
            {
                validArgs = true;
                bool x = false;
                foreach (string arg in args)
                {
                    x = !x;
                    if (int.TryParse(arg, out int parsed) && parsed > -1 &&
                        (x ? parsed < WindowWidth : parsed < WindowHeight))
                    {
                        listArgs.Add(parsed);
                        continue;
                    }

                    validArgs = false;
                    WriteLine($"arg >{arg}< was invalid. Falling back to interactive mode...");
                    Sleep(3000);
                    break;
                }
            }

            var intArgs = listArgs.ToArray();

            if (validArgs)
                Automated(ref intArgs, 10000, false);
            else Interactive();
        }
    }
}