using System;
using System.Collections.Generic;
using System.Threading;
using static System.Console;
using static System.ConsoleKey;
using static System.Environment;
using static System.Math;

namespace LineDraw_Console
{
    internal struct Line
    {
        public readonly (double X, double Y) A;
        public readonly (double X, double Y) B;

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
            Write(msg);
            SetCursorPosition(x, y);
        }

        private static void Main(string[] args)
        {
            int exitCode = 0;
            if (UserInteractive) Exit(exitCode);
            try
            {
                WriteLine("Maximize the window and press any key to continue.");
                ReadKey(true);
                var lines = new List<Line>();

                bool done = false, delay = true;
                double steps = 10;
                (int X, int Y)[] selection = {(0, 0), (0, 0)};
                while (!done)
                {
                    WriteStatus("".PadLeft(WindowWidth));
                    WriteStatus(
                        $"Select points|C:{CursorLeft},{CursorTop}|" +
                        $"A:{selection[0].X},{selection[0].Y}|" +
                        $"B:{selection[1].X},{selection[1].Y}|" +
                        $"Delay:{delay}|" +
                        $"Steps:{steps}");
                    // ReSharper disable once SwitchStatementMissingSomeCases
                    switch (ReadKey(true).Key)
                    {
                        case D1:
                            selection[0] = (CursorLeft, CursorTop);
                            break;
                        case D2:
                            selection[1] = (CursorLeft, CursorTop);
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
                            lines.Add(new Line(selection[0], selection[1]));
                            done = true;
                            break;
                    }
                }

                double step = 1 / steps;
                foreach (Line line in lines)
                    for (double i = 0; i <= 1.01; i += step)
                    {
                        Thread.Sleep(1);
                        // 2D vector between A and B
                        (double dx, double dy) = (
                            line.B.X - line.A.X,
                            line.B.Y - line.A.Y
                            );
                        // Calculate the coordinate according to the percentages
                        (int x, int y) = (
                            (int) Round(line.A.X + i * dx),
                            (int) Round(line.A.Y + i * dy)
                            );

                        SetCursorPosition(x, y);
                        Write('-');
                    }
            }
            catch (Exception e)
            {
                // Catch any previously unknown exceptions
                SetCursorPosition(0, 0);
                WriteLine(e.Message);
                WriteLine(e.StackTrace);
                exitCode = 1;
            }

            ReadKey(true);
            Exit(exitCode);
        }
    }
}