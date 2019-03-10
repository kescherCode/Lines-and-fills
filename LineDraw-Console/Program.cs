using System;
using System.Collections.Generic;
using System.Threading;
using static System.Console;
using static System.ConsoleKey;

namespace LineDraw_Console
{
    internal struct Line
    {
        public readonly (int X, int Y) A;
        public readonly (int X, int Y) B;

        public Line((int X, int Y) a, (int X, int Y) b)
        {
            A = a;
            B = b;
        }
    }

    internal static class Program
    {
        private static void WriteStatus(string msg)
        {
            int x = CursorLeft;
            int y = CursorTop;
            SetCursorPosition(0, WindowHeight - 1);
            Write(msg);
            SetCursorPosition(x, y);
        }

        private static void Main(string[] args)
        {
            try
            {
//            Thread.Sleep(3000);
                var lines = new List<Line>();

                ConsoleColor defaultBgColor = BackgroundColor;
                ConsoleColor defaultFgColor = ForegroundColor;

                bool done = false;
                (int X, int Y)[] selection = {(0, 0), (0, 0)};
                while (!done)
                {
                    WriteStatus("".PadLeft(WindowWidth));
                    WriteStatus(
                        $"Select points|C:{CursorLeft},{CursorTop}|A:{selection[0].X},{selection[0].Y}|B:{selection[1].X},{selection[1].Y}");
                    // ReSharper disable once SwitchStatementMissingSomeCases
                    switch (ReadKey(true).Key)
                    {
                        case D1:
                            selection[0] = (CursorLeft, CursorTop);
                            Console.Write("o");
                            Console.SetCursorPosition(selection[0].X, selection[0].Y);
                            break;
                        case D2:
                            selection[1] = (CursorLeft, CursorTop);
                            Write("o");
                            Console.SetCursorPosition(selection[1].X, selection[1].Y);
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
                        case Enter:
                            lines.Add(new Line(selection[0], selection[1]));
                            done = true;
                            break;
                    }
                }

                foreach (Line line in lines)
                    for (double i = 0; i <= 1.001; i+=0.001)
                    {
                        Thread.Sleep(1);
                        (int x, int y) = (line.B.X - line.A.X, line.B.Y - line.A.Y);
                        (int X, int Y) = ((int)(line.A.X + i * x), (int)(line.A.Y + i * y));
                        SetCursorPosition(X, Y);
                        Write('+');
                    }
            }
            catch (Exception e)
            {
                SetCursorPosition(0, 0);
                WriteLine(e.Message);
                WriteLine(e.StackTrace);
            }

            ReadKey(true);
        }
    }
}