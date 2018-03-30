﻿using System;
using System.Collections.Generic;

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
            X = rng.Next(-500 + (int)Radius, 500 - (int)Radius);
            Y = rng.Next(-500 + (int)Radius, 500 - (int)Radius);
        }
        
        public float X { get; set; }
        public float Y { get; set; }
        public float Radius { get; set; }
    }

    // Quadrant object
    internal class Quadrant
    {
        public List<Player> players = new List<Player>();
        public List<Bullet> bullets = new List<Bullet>();
    }
    
    class Program
    {
        public static void Main()
        {
            Random generator = new Random();
            Quadrant[] quadrants = new Quadrant[16];
            Player[] players = new Player[30];
            Bullet[] bullets = new Bullet[90];
            List<Player> playersList = new List<Player>();
            List<Bullet> bulletsList = new List<Bullet>();
            bool[,] buildingCoords = new bool[1000, 1000];
            var points = new Dictionary<string, int>();

            DateTime then = DateTime.Now;
            for (int i = 0; i < 250; i++)
            {
                for (int j = 0; j < 250; j++)
                {
                    points[i.ToString() + j.ToString()] = 0;
                }

                for (int j = 250; j < 500; j++)
                {
                    points[i.ToString() + j.ToString()] = 4;
                }

                for (int j = 500; j < 750; j++)
                {
                    points[i.ToString() + j.ToString()] = 8;
                }

                for (int j = 750; j < 1000; j++)
                {
                    points[i.ToString() + j.ToString()] = 12;
                }
            }

            for (int i = 250; i < 500; i++)
            {
                for (int j = 0; j < 250; j++)
                {
                    points[i.ToString() + j.ToString()] = 1;
                }

                for (int j = 250; j < 500; j++)
                {
                    points[i.ToString() + j.ToString()] = 5;
                }

                for (int j = 500; j < 750; j++)
                {
                    points[i.ToString() + j.ToString()] = 9;
                }

                for (int j = 750; j < 1000; j++)
                {
                    points[i.ToString() + j.ToString()] = 13;
                }
            }

            for (int i = 500; i < 750; i++)
            {
                for (int j = 0; j < 250; j++)
                {
                    points[i.ToString() + j.ToString()] = 2;
                }

                for (int j = 250; j < 500; j++)
                {
                    points[i.ToString() + j.ToString()] = 6;
                }

                for (int j = 500; j < 750; j++)
                {
                    points[i.ToString() + j.ToString()] = 10;
                }

                for (int j = 750; j < 1000; j++)
                {
                    points[i.ToString() + j.ToString()] = 14;
                }
            }

            for (int i = 750; i < 1000; i++)
            {
                for (int j = 0; j < 250; j++)
                {
                    points[i.ToString() + j.ToString()] = 3;
                }

                for (int j = 250; j < 500; j++)
                {
                    points[i.ToString() + j.ToString()] = 7;
                }

                for (int j = 500; j < 750; j++)
                {
                    points[i.ToString() + j.ToString()] = 11;
                }

                for (int j = 750; j < 1000; j++)
                {
                    points[i.ToString() + j.ToString()] = 15;
                }
            }

            Console.WriteLine("Map Population: took {0}", DateTime.Now - then);

            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new Player(generator);
            }

            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = new Bullet(generator);
            }

            for (int i = 0; i < 16; i++)
            {
                quadrants[i] = new Quadrant();
            }

            foreach (Player player in players)
            {
                var pointsToAdd = new Dictionary<int, bool>();
                float fixX = player.X + 500;
                float fixY = player.Y + 500;
                
                // Test all four points to check for overlap in multiple quadrants
                String testRight = (fixX + player.Radius).ToString() + (fixY).ToString();
                String testLeft = (fixX - player.Radius).ToString() + (fixY).ToString();
                String testTop = (fixX).ToString() + (fixY + player.Radius).ToString();
                String testBottom = (fixX).ToString() + (fixY - player.Radius).ToString();
                
                String testTopRight = (fixX + player.Radius).ToString() + (fixY + player.Radius).ToString();
                String testTopLeft = (fixX - player.Radius).ToString() + (fixY + player.Radius).ToString();
                String testBottomRight = (fixX + player.Radius).ToString() + (fixY - player.Radius).ToString();
                String testBottomLeft = (fixX - player.Radius).ToString() + (fixY - player.Radius).ToString();


                pointsToAdd[points[testRight]] = true;
                pointsToAdd[points[testLeft]] = true;
                pointsToAdd[points[testTop]] = true;
                pointsToAdd[points[testBottom]] = true;
                
                pointsToAdd[points[testTopRight]] = true;
                pointsToAdd[points[testTopLeft]] = true;
                pointsToAdd[points[testBottomRight]] = true;
                pointsToAdd[points[testBottomLeft]] = true;

                foreach(KeyValuePair<int, bool> entry in pointsToAdd)
                {
                    Console.WriteLine(entry.Key);
                    quadrants[entry.Key].players.Add(player);
                }
                
                playersList.Add(player);
                Console.WriteLine("Done a Player");
            }

            foreach (Bullet bullet in bullets)
            {
                var pointsToAdd = new Dictionary<int, bool>();
                float fixX = bullet.X + 500;
                float fixY = bullet.Y + 500;
                
                String testRight = (fixX + bullet.Radius).ToString() + (fixY).ToString();
                String testLeft = (fixX - bullet.Radius).ToString() + (fixY).ToString();
                String testTop = (fixX).ToString() + (fixY + bullet.Radius).ToString();
                String testBottom = (fixX).ToString() + (fixY - bullet.Radius).ToString();
                
                String testTopRight = (fixX + bullet.Radius).ToString() + (fixY + bullet.Radius).ToString();
                String testTopLeft = (fixX - bullet.Radius).ToString() + (fixY + bullet.Radius).ToString();
                String testBottomRight = (fixX + bullet.Radius).ToString() + (fixY - bullet.Radius).ToString();
                String testBottomLeft = (fixX - bullet.Radius).ToString() + (fixY - bullet.Radius).ToString();
                
                pointsToAdd[points[testRight]] = true;
                pointsToAdd[points[testLeft]] = true;
                pointsToAdd[points[testTop]] = true;
                pointsToAdd[points[testBottom]] = true;
                
                pointsToAdd[points[testTopRight]] = true;
                pointsToAdd[points[testTopLeft]] = true;
                pointsToAdd[points[testBottomRight]] = true;
                pointsToAdd[points[testBottomLeft]] = true;

                foreach(KeyValuePair<int, bool> entry in pointsToAdd)
                {
                    Console.WriteLine(entry.Key);
                    quadrants[entry.Key].bullets.Add(bullet);
                }
                
                bulletsList.Add(bullet);
                Console.WriteLine("Done a Bullet");
            }

            
            // Naive Speed check
            int collisions = 0;
            then = DateTime.Now;
            foreach (Player player in playersList)
            {
                for (int i = bulletsList.Count - 1; i >= 0; i--)
                {
                    if (CircleCollided(player.X, player.Y, player.Radius, bulletsList[i].X, bulletsList[i].Y, bulletsList[i].Radius))
                    {
                        bulletsList.RemoveAt(i);
                        collisions++;
                    }
                }
            }

            Console.WriteLine("Naive: took {0} with {1} collisions", DateTime.Now - then, collisions);

            // Quadrant Speed check
            collisions = 0;
            then = DateTime.Now;
            foreach (Quadrant quad in quadrants)
            {
                if (quad.players.Count > 0 && quad.bullets.Count > 0)
                {
                    foreach (Player player in quad.players)
                    {
                        // Backwards using index so we can delete bullets as they're used
                        for (int i = quad.bullets.Count - 1; i >= 0; i--)
                        {
                            if (CircleCollided(player.X, player.Y, player.Radius, quad.bullets[i].X, quad.bullets[i].Y,
                                quad.bullets[i].Radius))
                            {
                                // Do Something
                                quad.bullets.RemoveAt(i);
                                collisions++;
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Quadrant: took {0} with {1} collisions", DateTime.Now - then, collisions);
        }


        private static Boolean CircleCollided(float x1, float y1, float r1, float x2, float y2, float r2)
        {
            double distance = Math.Sqrt( (x1-x2)*(x1-x2)+(y1-y2)*(y1-y2) );
            return (distance < (r1 + r2));
        }
        
    }
}