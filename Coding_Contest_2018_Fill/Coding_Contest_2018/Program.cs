using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;

namespace Coding_Contest_2018
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static class Program
    {
        private const int level = 2;
        private static int which = 1;
        private const int größe = 3;

        private static readonly List<ConsoleColor> cl = new List<ConsoleColor>
        {
            ConsoleColor.Blue, ConsoleColor.Cyan, ConsoleColor.DarkBlue, ConsoleColor.DarkCyan, ConsoleColor.DarkGray,
            ConsoleColor.DarkGreen,
            ConsoleColor.DarkMagenta, ConsoleColor.DarkRed, ConsoleColor.DarkYellow, ConsoleColor.Green,
            ConsoleColor.Magenta,
            ConsoleColor.Red, ConsoleColor.Yellow
        };

        //private static int neg = -int.Parse(100000000000.ToString().Substring(0, größe - 1));
        private static int neg = -10;
        private static int zahl = 10;

        private static void Main(string[] args)
        {
            for (int f = 0; f < 3; f++)
            {
                which = f;
                string filePathRead = $"../../level{level}_{f}.in";
                string filePathWrite = $"../../level{level}_{f}_out.txt";
                int r = 0;
                using (var sr = new StreamReader(filePathRead))
                {
                    string zeile = sr.ReadLine();
                    // ReSharper disable once PossibleNullReferenceException
                    var split = zeile.Split(' ');
                    int row = int.Parse(split[0]);
                    int col = int.Parse(split[1]);
                    var grid = new int[row, col];

                    while (!sr.EndOfStream)
                    {
                        zeile = sr.ReadLine();
                        // ReSharper disable once PossibleNullReferenceException
                        split = zeile.Split(' ');
                        for (int i = 0; i < split.Length; i++)
                        {
                            if (split[i].Length < größe)
                                do
                                {
                                    split[i] += "0";
                                } while (split[i].Length != größe);

                            grid[r, i] = int.Parse(split[i]);
                        }

                        Console.WriteLine(string.Join(" ", split));
                        r++;
                    }

                    Console.ReadKey(true);
                    if (f == 2)
                        FloodFillQueue(ref grid, 11, 1, -10);
                    else
                        ÜbergeordneterCrawler(ref grid);
                    //var dict = new Dictionary<int, int>();
                    //for (int rw = 0; rw < grid.GetLength(0); rw++)
                    //for (int cw = 0; cw < grid.GetLength(0); cw++)
                    //    if (grid[rw, cw] < 0)
                    //    {
                    //        if (!dict.ContainsKey(grid[rw, cw])) dict[grid[rw, cw]] = 0;
                    //        dict[grid[rw, cw]]++;
                    //    }

                    //List<int> counts = new List<int>();
                    //foreach (var item in dict)
                    //{
                    //    counts.Add(item.Value);
                    //}
                    //counts.Sort();
                    //using (StreamWriter sw = new StreamWriter(filePathWrite, false))
                    //{
                    //    string output = "";
                    //    for (int i = 0; i < counts.Count; i++)
                    //    {
                    //        if (i != counts.Count - 1)
                    //        {
                    //            //Console.Write($"{i} {counts[i]} ");
                    //            sw.Write($"{i} {counts[i]} ");
                    //        }
                    //        else
                    //        {
                    //            //Console.Write($"{i} {counts[i]}");
                    //            sw.Write($"{i} {counts[i]}");
                    //        }
                    //    }
                    //    //Console.WriteLine();
                    //}
                }

                Console.Clear();
            }

            #region AAAAAAAAAAAAAAAAAA

            /*
            int aus = 0;
            int house = 0;
            bool gleiches = false;
            List<int> h = new List<int>();
            Dictionary<int, int[,]> höhen = new Dictionary<int, int[,]>();
            int[,] test = new int[grid.GetLength(0), grid.GetLength(1)];
            Dictionary<int, int> dict = new Dictionary<int, int>();
            for (int rw = 0; rw < grid.GetLength(0); rw++)
            {
                for (int cw = 0; cw < grid.GetLength(1); cw++)
                {
                    bool l = false;
                    bool re = false;
                    bool u = false;
                    bool o = false;

                    if (grid[rw, cw] > 0)
                    {
                        if (!höhen.Keys.Contains(grid[rw, cw]))
                        {
                            höhen[grid[rw, cw]] = new int[row, col];
                            höhen[grid[rw, cw]][rw, cw] = grid[rw, cw];
                        }
                        int derh = grid[rw, cw];
                        if (!h.Contains(derh))
                            h.Add(derh);
                        if (cw > 0)
                        {
                            if (derh == grid[rw, cw - 1])
                            {
                                l = true;
                                höhen[derh][rw, cw - 1] = derh;
                            }
                        }
                        if (cw < grid.GetLength(0) - 1)
                        {
                            if (derh == grid[rw, cw + 1])
                            {
                                re = true;
                                höhen[derh][rw, cw + 1] = derh;
                            }
                        }
                        if (rw > 0)
                        {
                            if (derh == grid[rw - 1, cw])
                            {
                                o = true;
                                höhen[derh][rw - 1, cw] = derh;
                            }
                        }
                        if (rw < grid.GetLength(1) - 1)
                        {
                            if (derh == grid[rw + 1, cw])
                            {
                                u = true;
                                höhen[derh][rw + 1, cw] = derh;
                            }
                        }
                        if (u == false && o == false && re == false && l == false)
                        {
                            for (int kw = 0; kw < höhen[derh].GetLength(0); kw++)
                            {
                                for (int lw = 0; lw < höhen[derh].GetLength(0); lw++)
                                {
                                    if (höhen[derh][kw, lw] != 0)
                                    {
                                        if (!dict.Keys.Contains(aus))
                                            dict[aus] = 0;
                                        dict[aus]++;
                                    }
                                }
                            }
                            if (höhen.Keys.Contains(grid[rw, cw]))
                            {
                                höhen.Remove(grid[rw, cw]);
                            }
                        }
                    }
                }
                if (house == 1)
                    break;
                aus = 0;

            }
            for (int ha = 0; ha < h.Count; ha++)
            {
                for (int kw = 0; kw < höhen[h[ha]].GetLength(0); kw++)
                {
                    for (int lw = 0; lw < höhen[h[ha]].GetLength(0); lw++)
                    {
                        if (höhen[h[ha]][kw, lw] == h[ha])
                        {
                            if (!dict.Keys.Contains(h[ha]))
                                dict[h[ha]] = 0;
                            dict[h[ha]]++;
                        }
                    }
                }
                if (h[ha] == 58)
                    ÜbergeordneterCrawler(höhen[h[ha]]);
            }
            */

            #endregion

            ConsoleKey k;
            do
            {
                k = Console.ReadKey(true).Key;
            } while (k != ConsoleKey.Escape);
        }

        private static void ÜbergeordneterCrawler(ref int[,] grid)
        {
            for (int r = 0; r < grid.GetLength(0); r++)
                for (int c = 0; c < grid.GetLength(1); c++)
                    if (grid[r, c] > 0)
                    {
                        zahl = grid[r, c];
                        Suche(ref grid, r, c);
                        Console.ReadKey(true);
                        neg--;
                    }

            //GridAufteilen(grid);
        }

        private static void Suche(ref int[,] grid, int row, int col)
        {
            while (true)
            {
                if (row < 0 || row > grid.GetLength(0) - 1) return;
                if (col < 0 || col > grid.GetLength(0) - 1) return;
                if (grid[row, col] != zahl) return;

                grid[row, col] = neg;
                Console.SetCursorPosition(col * (grid[row, col].ToString().Length + 1), row);
                Console.ForegroundColor = cl[Math.Abs(neg) % cl.Count];
                Console.Write(neg);
                Thread.Sleep(10);
                Console.ForegroundColor = ConsoleColor.White;
                Suche(ref grid, row, col - 1);
                Suche(ref grid, row, col + 1);
                Suche(ref grid, row - 1, col);
                row = row + 1;
            }
        }

        //private static void SucheLab(ref int[,] grid, int row, int col, int i)
        //{
        //    while (true)
        //    {
        //        if (row < 0 || row > grid.GetLength(0) - 1) return;
        //        if (col < 0 || col > grid.GetLength(0) - 1) return;
        //        if (grid[row, col] <= 0) return;

        //        grid[row, col] = i;
        //        Console.SetCursorPosition(col * (grid[row, col].ToString().Length + 1), row);
        //        Console.ForegroundColor = cl[Math.Abs(neg) % cl.Count];
        //        Console.Write(i);
        //        Console.ReadKey(true);
        //        Console.ForegroundColor = ConsoleColor.White;
        //        i--;
        //        SucheLab(ref grid, row, col - 1, i);
        //        SucheLab(ref grid, row, col + 1, i);
        //        SucheLab(ref grid, row - 1, col, i);
        //        SucheLab(ref grid, row + 1, col, i);
        //    }
        //}
        private static void FloodFillQueue(ref int[,] grid, int row, int col, int i)
        {
            Queue<Tuple<int, int, int>> pixels = new Queue<Tuple<int,int, int>>();
            if (row < 0 || row > grid.GetLength(0) - 1) return;
            if (col < 0 || col > grid.GetLength(0) - 1) return;
            if (grid[row, col] <= 0) return;
            pixels.Enqueue(new Tuple<int, int, int>(row, col, i));
            Console.ForegroundColor = ConsoleColor.Red;
            while (pixels.Count > 0)
            {
                var a = pixels.Dequeue();
                if (a.Item1 < grid.GetLength(0) && a.Item1 > 0 &&
                        a.Item2 < grid.GetLength(0) && a.Item2 > 0)//make sure we stay within bounds
                {
                    if (grid[a.Item1, a.Item2] > 0)
                    {
                        grid[a.Item1, a.Item2] = i;
                        pixels.Enqueue(new Tuple<int, int, int>(a.Item1 - 1, a.Item2, a.Item3 - 1));
                        pixels.Enqueue(new Tuple<int, int, int>(a.Item1 + 1, a.Item2, a.Item3 - 1));
                        pixels.Enqueue(new Tuple<int, int, int>(a.Item1, a.Item2 - 1, a.Item3 - 1));
                        pixels.Enqueue(new Tuple<int, int, int>(a.Item1, a.Item2 + 1, a.Item3 - 1));
                        Console.SetCursorPosition(a.Item2 * (grid[a.Item1, a.Item2].ToString().Length + 1), a.Item1);
                        Console.Write(a.Item3);
                        Console.ReadKey(true);
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        //public static void GridAufteilen(int[,] grid)
        //{
        //    using (var sw = new StreamWriter($"Ausgabe_{level}_{which}.txt", false))
        //    {
        //        for (int r = 0; r < grid.GetLength(0); r++)
        //        {
        //            for (int c = 0; c < grid.GetLength(0); c++)
        //                sw.Write(grid[r, c] == 0 ? "000 " : $"{grid[r, c]} ");
        //            sw.WriteLine();
        //        }
        //    }
        //}
    }
}