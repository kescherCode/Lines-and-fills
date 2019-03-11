using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Coding_Contest_2018
{
    class Program
    {
        static int level = 2;
        static int which = 1;
        static int größe = 3;
        static List<ConsoleColor> cl = new List<ConsoleColor>()
        {
            ConsoleColor.Blue, ConsoleColor.Cyan, ConsoleColor.DarkBlue, ConsoleColor.DarkCyan, ConsoleColor.DarkGray, ConsoleColor.DarkGreen,
            ConsoleColor.DarkMagenta, ConsoleColor.DarkRed, ConsoleColor.DarkYellow, ConsoleColor.Green, ConsoleColor.Magenta,
            ConsoleColor.Red, ConsoleColor.Yellow
        };
        static void Main(string[] args)
        {
            for (int f = 0; f < 2; f++)
            {
                which = f;
                string filePathRead = $"../../level{level}_{f}.in";
                string filePathWrite = $"../../level{level}_{f}_out.txt";
                int col = 0;
                int row = 0;
                int anz = 0;
                int[,] grid = new int[1, 1];
                string[] split;
                int r = 0, c = 0;
                using (StreamReader sr = new StreamReader(filePathRead))
                {
                    string zeile = sr.ReadLine();
                    if (anz == 0)
                    {
                        split = zeile.Split(' ');
                        row = int.Parse(split[0]);
                        col = int.Parse(split[1]);
                        grid = new int[row, col];
                    }
                    while (!sr.EndOfStream)
                    {
                        zeile = sr.ReadLine();                        
                        split = zeile.Split(' ');
                        for (int i = 0; i < split.Length; i++)
                        {
                            if(split[i].Length < größe)
                            {
                                do
                                {
                                    split[i] += "0";
                                } while (split[i].Length != größe);
                            }
                            grid[r, i] = int.Parse(split[i]);
                        }
                        Console.WriteLine(String.Join(" ", split));
                        r++;
                    }
                    Console.ReadKey(true);
                    ÜbergeordneterCrawler(ref grid);
                    Dictionary<int, int> dict = new Dictionary<int, int>();
                    for (int rw = 0; rw < grid.GetLength(0); rw++)
                    {
                        for (int cw = 0; cw < grid.GetLength(0); cw++)
                        {
                            if (grid[rw, cw] < 0)
                            {
                                if (!dict.ContainsKey(grid[rw, cw]))
                                {
                                    dict[grid[rw, cw]] = 0;
                                }
                                dict[grid[rw, cw]]++;
                            }
                        }
                    }
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
            #region
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
            ConsoleKey k = Console.ReadKey(true).Key;
            do
            {
                k = Console.ReadKey(true).Key;
            } while (k != ConsoleKey.Escape);
        }

        static int neg = -int.Parse(100000000000.ToString().Substring(0, größe - 1));
        static int zahl = 10;
        public static void ÜbergeordneterCrawler(ref int[,] grid)
        {
            for (int r = 0; r < grid.GetLength(0); r++)
            {
                for (int c = 0; c < grid.GetLength(1); c++)
                {
                    if (grid[r, c] > 0)
                    {
                        zahl = grid[r, c];
                        Suche(ref grid, r, c);
                        Console.ReadKey(true);
                        neg--;
                    }
                }
            }
            //GridAufteilen(grid);
        }

        public static void Suche(ref int[,] grid, int row, int col)
        {
            if (row < 0 || row > grid.GetLength(0) - 1)
                return;
            if (col < 0 || col > grid.GetLength(0) - 1)
                return;
            if (grid[row, col] == zahl)
            {
                grid[row, col] = neg;
                Console.SetCursorPosition(col * (grid[row, col].ToString().Length + 1), row);
                Console.ForegroundColor = cl[Math.Abs(neg) % cl.Count];
                Console.Write(neg);
                System.Threading.Thread.Sleep(10);
                Console.ForegroundColor = ConsoleColor.White;
                Suche(ref grid, row, col - 1);
                Suche(ref grid, row, col + 1);
                Suche(ref grid, row - 1, col);
                Suche(ref grid, row + 1, col);
            }
        }

        public static void GridAufteilen(int[,] grid)
        {
            using (StreamWriter sw = new StreamWriter($"Ausgabe_{level}_{which}.txt", false))
            {
                string output = "";
                for (int r = 0; r < grid.GetLength(0); r++)
                {
                    for (int c = 0; c < grid.GetLength(0); c++)
                    {
                        if (grid[r, c] == 0)
                            sw.Write($"000 ");
                        else
                            sw.Write($"{grid[r, c]} ");

                    }
                    sw.WriteLine();
                }

            }

        }
    }
}

