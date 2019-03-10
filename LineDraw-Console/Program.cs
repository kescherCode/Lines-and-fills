using System;
using System.Collections.Generic;
using static System.Console;

namespace LineDraw_Console
{
    internal struct Scalar2D
    {
        public double X;
        public double Y;

        public Scalar2D(double x = 0d, double y = 0d)
        {
            X = x;
            Y = y;
        }

        public double Length => Math.Sqrt(X * X + Y * Y);

        public static Scalar2D operator *(double scale, Scalar2D x)
        {
            x.X *= scale;
            x.Y *= scale;
            return x;
        }

        public static Scalar2D operator /(double scale, Scalar2D x)
        {
            x.X /= scale;
            x.Y /= scale;
            return x;
        }

        public static Scalar2D operator -(Scalar2D left, Scalar2D right)
        {
            var p = new Scalar2D(left.X, left.Y);
            p.X -= right.X;
            p.Y -= right.Y;
            return p;
        }

        public static Scalar2D operator +(Scalar2D left, Scalar2D right)
        {
            var p = new Scalar2D(left.X, left.Y);
            p.X += right.X;
            p.Y += right.Y;
            return p;
        }

        public static bool operator ==(Scalar2D left, Scalar2D right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Scalar2D left, Scalar2D right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var scalar = (Scalar2D) obj;
            return X.Equals(scalar.X) && Y.Equals(scalar.Y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public bool Equals(Scalar2D scalar2D)
        {
            Scalar2D scalar = scalar2D;
            return X.Equals(scalar.X) && Y.Equals(scalar.Y);
        }
    }

    internal struct Line
    {
        private readonly Scalar2D A;
        private readonly Scalar2D B;

        public Line(Scalar2D a, Scalar2D b)
        {
            A = a;
            B = b;
        }

        public (int X, int Y) CoordsToDraw(int percentage)
        {
            Scalar2D diff = A - B;
            Scalar2D coords = A + percentage * diff;
            return ((int) coords.X, (int) coords.Y);
        }
    }

    internal class Program
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
            var lines = new List<Line>();

            ConsoleColor defaultBgColor = BackgroundColor;
            ConsoleColor defaultFgColor = ForegroundColor;

            bool done = false;
            WriteStatus("Select points");
            Scalar2D[] selection = {new Scalar2D(0, 0), new Scalar2D(0, 0)};
            while (!done)
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        selection[0] = new Scalar2D(CursorLeft, CursorTop);
                        break;
                    case ConsoleKey.D2:
                        selection[1] = new Scalar2D(CursorLeft, CursorTop);
                        break;
                    case ConsoleKey.Enter:
                        done = true;
                        break;
                }

            foreach (Line line in lines)
                for (int i = 0; i < 101; i++)
                {
                    (int X, int Y) coords = line.CoordsToDraw(i);
                    SetCursorPosition(coords.X, coords.Y);
                    Write('+');
                }

            ReadKey(true);
        }
    }
}