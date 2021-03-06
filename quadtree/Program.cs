﻿/*
 *
 * So the big takeaway here is that quadrant/sector collision is significantly faster (order of magnitude),
 * but adding the objects to their sectors is too expensive to perform every tick. The options are to go with
 * the naive check, which is acceptable, or attempt to find a way to only add objects to sectors once, then
 * get their changes. A real quadtree could theoretically perform that, but may take too long to implement with
 * other responibilities.
 * 
 */

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace quadtree
{
    // Bullet object, randomly initialised
    internal class Bullet
    {
        public Bullet(Random rng)
        {


            Radius = 5;
            X = rng.Next(-500 + (int)Radius, 500 - (int)Radius);
            Y = rng.Next(-500 + (int)Radius, 500 - (int)Radius);
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Radius { get; set; }
    }

    // Player object, randomly initialised
    internal class Player
    {
        public Player(Random rng)
        {
            Radius = 10;
            Health = 100;
            X = rng.Next(-500 + (int)Radius, 500 - (int)Radius);
            Y = rng.Next(-500 + (int)Radius, 500 - (int)Radius);
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Radius { get; set; }
        public int Health;
    }

    // Quadrant object
    internal class Quadrant
    {
        public List<Player> players = new List<Player>();
        public List<Bullet> bullets = new List<Bullet>();
    }

    class Program
    {
        static System.Diagnostics.Stopwatch stopwatch;


        static Dictionary<string, int> populateMap()
        {
            var pointToQuad = new Dictionary<string, int>();

            startClock();
            for (int i = 0; i < 250; i++)
            {
                for (int j = 0; j < 250; j++)
                {
                    pointToQuad[i.ToString() + "," + j.ToString()] = 0;
                }

                for (int j = 250; j < 500; j++)
                {
                    pointToQuad[i.ToString() + "," + j.ToString()] = 4;
                }

                for (int j = 500; j < 750; j++)
                {
                    pointToQuad[i.ToString() + "," + j.ToString()] = 8;
                }

                for (int j = 750; j < 1000; j++)
                {
                    pointToQuad[i.ToString() + "," + j.ToString()] = 12;
                }
            }

            for (int i = 250; i < 500; i++)
            {
                for (int j = 0; j < 250; j++)
                {
                    pointToQuad[i.ToString() + "," + j.ToString()] = 1;
                }

                for (int j = 250; j < 500; j++)
                {
                    pointToQuad[i.ToString() + "," + j.ToString()] = 5;
                }

                for (int j = 500; j < 750; j++)
                {
                    pointToQuad[i.ToString() + "," + j.ToString()] = 9;
                }

                for (int j = 750; j < 1000; j++)
                {
                    pointToQuad[i.ToString() + "," + j.ToString()] = 13;
                }
            }

            for (int i = 500; i < 750; i++)
            {
                for (int j = 0; j < 250; j++)
                {
                    pointToQuad[i.ToString() + "," + j.ToString()] = 2;
                }

                for (int j = 250; j < 500; j++)
                {
                    pointToQuad[i.ToString() + "," + j.ToString()] = 6;
                }

                for (int j = 500; j < 750; j++)
                {
                    pointToQuad[i.ToString() + "," + j.ToString()] = 10;
                }

                for (int j = 750; j < 1000; j++)
                {
                    pointToQuad[i.ToString() + "," + j.ToString()] = 14;
                }
            }

            for (int i = 750; i < 1000; i++)
            {
                for (int j = 0; j < 250; j++)
                {
                    pointToQuad[i.ToString() + "," + j.ToString()] = 3;
                }

                for (int j = 250; j < 500; j++)
                {
                    pointToQuad[i.ToString() + "," + j.ToString()] = 7;
                }

                for (int j = 500; j < 750; j++)
                {
                    pointToQuad[i.ToString() + "," + j.ToString()] = 11;
                }

                for (int j = 750; j < 1000; j++)
                {
                    pointToQuad[i.ToString() + "," + j.ToString()] = 15;
                }
            }
            Console.WriteLine("Map Population: took {0} nanoseconds", stopClock());

            return pointToQuad;
        }

        static Quadrant[] quadrants = new Quadrant[16];

        public static void Main()
        {
            Random generator = new Random();
            int numOfPlayers = 30;
            Bullet[] bullets = new Bullet[90];
            List<Player> playersList = new List<Player>();
            List<Bullet> bulletsList = new List<Bullet>();
            bool[,] buildingCoords = new bool[1000, 1000];

            var pointToQuad = populateMap();

            for (int i = 0; i < numOfPlayers; i++)
            {
                playersList.Add(new Player(generator));
            }

            for (int i = 0; i < bullets.Length; i++)
            {
                bulletsList.Add(new Bullet(generator));
            }

            for (int i = 0; i < 16; i++)
            {
                quadrants[i] = new Quadrant();
            }

            startClock();
            foreach (Player player in playersList)
            {
                var uniquePoints = new HashSet<int>();
                float fixX = player.X + 500;
                float fixY = player.Y + 500;

                // Test all four points to check for overlap in multiple quadrants
                String testRight = (fixX + player.Radius).ToString() + "," + (fixY).ToString();
                String testLeft = (fixX - player.Radius).ToString() + "," + (fixY).ToString();
                String testTop = (fixX).ToString() + "," + (fixY + player.Radius).ToString();
                String testBottom = (fixX).ToString() + "," + (fixY - player.Radius).ToString();

                String testTopRight = (fixX + player.Radius).ToString() + "," + (fixY + player.Radius).ToString();
                String testTopLeft = (fixX - player.Radius).ToString() + "," + (fixY + player.Radius).ToString();
                String testBottomRight = (fixX + player.Radius).ToString() + "," + (fixY - player.Radius).ToString();
                String testBottomLeft = (fixX - player.Radius).ToString() + "," + (fixY - player.Radius).ToString();

                uniquePoints.Add(pointToQuad[testRight]);
                uniquePoints.Add(pointToQuad[testLeft]);
                uniquePoints.Add(pointToQuad[testTop]);
                uniquePoints.Add(pointToQuad[testBottom]);

                uniquePoints.Add(pointToQuad[testTopRight]);
                uniquePoints.Add(pointToQuad[testTopLeft]);
                uniquePoints.Add(pointToQuad[testBottomRight]);
                uniquePoints.Add(pointToQuad[testBottomLeft]);

                foreach (int quadrant in uniquePoints)
                {
                    quadrants[quadrant].players.Add(player);
                }
            }
            Console.WriteLine("Players: took {0} nanoseconds to assign", stopClock());

            List<Task> tasks = new List<Task>();

            startClock();
            foreach (Bullet bullet in bulletsList)
            {
                var uniquePoints = new HashSet<int>();
                float fixX = bullet.X + 500;
                float fixY = bullet.Y + 500;

                String testRight = (fixX + bullet.Radius).ToString() + "," + (fixY).ToString();
                String testLeft = (fixX - bullet.Radius).ToString() + "," + (fixY).ToString();
                String testTop = (fixX).ToString() + "," + (fixY + bullet.Radius).ToString();
                String testBottom = (fixX).ToString() + "," + (fixY - bullet.Radius).ToString();

                String testTopRight = (fixX + bullet.Radius).ToString() + "," + (fixY + bullet.Radius).ToString();
                String testTopLeft = (fixX - bullet.Radius).ToString() + "," + (fixY + bullet.Radius).ToString();
                String testBottomRight = (fixX + bullet.Radius).ToString() + "," + (fixY - bullet.Radius).ToString();
                String testBottomLeft = (fixX - bullet.Radius).ToString() + "," + (fixY - bullet.Radius).ToString();

                uniquePoints.Add(pointToQuad[testRight]);
                uniquePoints.Add(pointToQuad[testLeft]);
                uniquePoints.Add(pointToQuad[testTop]);
                uniquePoints.Add(pointToQuad[testBottom]);

                uniquePoints.Add(pointToQuad[testTopRight]);
                uniquePoints.Add(pointToQuad[testTopLeft]);
                uniquePoints.Add(pointToQuad[testBottomRight]);
                uniquePoints.Add(pointToQuad[testBottomLeft]);

                foreach (int quadrant in uniquePoints)
                {
                    quadrants[quadrant].bullets.Add(bullet);
                }
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Bullets: took {0} nanoseconds to assign", stopClock());

            // Naive Speed check
            int collisions = 0;

            startClock();
            foreach (Player player in playersList)
            {
                for (int i = bulletsList.Count - 1; i >= 0; i--)
                {
                    if (buildingCoords[(int) bulletsList[i].X + 500, (int) bulletsList[i].Y + 500])
                    {
                        bulletsList.RemoveAt(i);
                        break;
                    }
                    
                    if (CircleCollided(player.X, player.Y, player.Radius, bulletsList[i].X, bulletsList[i].Y, bulletsList[i].Radius))
                    {
                        bulletsList.RemoveAt(i);
                        collisions++;
                    }
                }
            }
            Console.WriteLine("Naive: took {0} nanoseconds with {1} collisions", stopClock(), collisions);

            // Quadrant Speed check
            collisions = 0;

            startClock();
            foreach (Quadrant quad in quadrants)
            {
                if (quad.players.Count > 0 && quad.bullets.Count > 0)
                {
                    for (int i = quad.players.Count - 1; i >= 0; i--)
                    {
                        // Backwards using index so we can delete bullets as they're used
                        for (int j = quad.bullets.Count - 1; j >= 0; j--)
                        {
                            if (buildingCoords[(int) bulletsList[i].X + 500, (int) bulletsList[i].Y + 500])
                            {
                                bulletsList.RemoveAt(i);
                                break;
                            }
                            
                            if (CircleCollided(quad.players[i].X, quad.players[i].Y, quad.players[i].Radius, quad.bullets[j].X, quad.bullets[j].Y,
                                quad.bullets[j].Radius))
                            {
                                // Do Something
                                collisions++;
                                quad.bullets.RemoveAt(j);

                                if (quad.players[i].Health >= 100) // Change that 100 to *Bullet Damage*
                                {
                                    quad.players.RemoveAt(i);
                                    break;
                                    //quad.players[i].Health -= *Bullet Damage*;
                                }
                                else
                                {
                                    //quad.players.RemoveAt(i);
                                    //break;
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Quadrant: took {0} nanoseconds with {1} collisions", stopClock(), collisions);
        }

        private static bool CircleCollided(float x1, float y1, float r1, float x2, float y2, float r2)
        {
            double distance = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
            return (distance < (r1 + r2));
        }

        private static void startClock()
        {
            stopwatch = System.Diagnostics.Stopwatch.StartNew();
        }

        private static long stopClock()
        {
            stopwatch.Stop();
            return stopwatch.ElapsedTicks / (System.Diagnostics.Stopwatch.Frequency / (1000L * 1000L));
        }

    }
}