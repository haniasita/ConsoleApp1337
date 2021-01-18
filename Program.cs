using System;
using System.Drawing;
using System.Diagnostics;
using Console = Colorful.Console;

namespace ConsoleApp1337
{
    class Program
    {
        static void Main(string[] args)
        {
            // Hide console cursor
            Console.CursorVisible = false;

            // Declare ms deltatime stopwatch
            Stopwatch ticktime = new Stopwatch();

            // Initialize world
            int[,] World = new int[16, 16];

            // Create game map
            World = CreateWorld(World);

            // Declare player movement vector
            int[] PlayerXY = new int[2];

            // Declare clock control int
            int clock = 1;

            while (true)
            {
                while (clock == 1 && !Console.KeyAvailable)
                {
                    ticktime.Reset();
                    ticktime.Start();

                    clock = 0;

                    clock = Draw(World);

                    ticktime.Stop();

                    Tick(World, Controller(PlayerXY), 0);

                    Console.Write($"Speed : {ticktime.ElapsedMilliseconds}ms", Color.Gray);

                }
            }

            System.Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }

        static int[] Controller(int[] PlayerXY)
        {
            {
                // Checks for console input and outputs a movement vector (PlayerXY)

                PlayerXY[0] = 0;
                PlayerXY[1] = 0;

                if (Console.ReadKey(true).Key == ConsoleKey.UpArrow)
                {
                    PlayerXY[1] = -1;
                }
                else if (Console.ReadKey(true).Key == ConsoleKey.DownArrow)
                {
                    PlayerXY[1] = 1;
                }

                if (Console.ReadKey(true).Key == ConsoleKey.LeftArrow)
                {
                    PlayerXY[0] = -1;
                }
                else if (Console.ReadKey(true).Key == ConsoleKey.RightArrow)
                {
                    PlayerXY[0] = 1;
                }

                Console.Write("Controller tick", Color.Red);

                return PlayerXY;
            }
        }

        static int[,] Tick(int[,] World, int[] Move, int Event)
        {
            // Processes and updates game world according to tile numbers

            // Events :
            // 0 - idle
            // 1 - player moves
            
            for (int y = 0; y < World.GetLength(1); y++)
            {
                for (int x = 0; x < World.GetLength(0); x++)
                {
                    if (World[x, y] == 3 && Event == 1)
                    {
                        World[x + Move[0], y + Move[1]] = 3;

                        World[x, y] = 0;
                    }
                }
            }

            return World;
        }

        static int Draw(int[,] World)
        {
            // Draws a given world on the screen
            // Only 16 color definitions possible with Windows terminal

            int r = 0;
            int g = 0;
            int b = 0;

            for (int y = 0; y < World.GetLength(1); y++)
            {
                for (int x = 0; x < World.GetLength(0); x++)
                {
                    switch (World[x, y])
                    {
                        case 0:
                            // Grass
                            r = 0;
                            g = 128;
                            b = 0;
                            break;

                        case 1:
                            // Water
                            r = 0;
                            g = 0;
                            b = 255;
                            break;

                        case 2:
                            // Enemy
                            r = 255;
                            g = 0;
                            b = 0;
                            break;

                        case 3:
                            // Player
                            r = 128;
                            g = 255;
                            b = 64;
                            break;
                    }

                    Console.SetCursorPosition(x, y);
                    Console.WriteLine(World[x, y], Color.FromArgb(r, g, b));
                }
            }

            return 1;
        }

        static int[,] CreateWorld(int[,] World)
        {
            // Creates random world using 2D array World as reference

            Random Seed = new Random();

            for (int y = 0; y < World.GetLength(1); y++)
            {
                for (int x = 0; x < World.GetLength(0); x++)
                {
                    if (x != 0 || y != 0 || x != World.GetLength(0) - 1 || y != World.GetLength(1) - 1)
                    {
                        World[x, y] = 0;
                    }

                    if (x == 0 || y == 0 || x == World.GetLength(0) - 1 || y == World.GetLength(1) - 1)
                    {
                        World[x, y] = 1;
                    }

                    if (Seed.Next(0, 16) > 14 && x != 0 && y != 0 && x != World.GetLength(0) - 1 && y != World.GetLength(1) - 1)
                    {
                        World[x, y] = 2;
                    }

                    if (x == 8 && y == 8)
                    {
                        World[x, y] = 3;
                    }
                }
            }
            return World;
        }
    }
}
