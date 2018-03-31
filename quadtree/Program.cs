using System;
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
        public static void Main()
        {
            Random generator = new Random();
            Quadrant[] quadrants = new Quadrant[16];
            Player[] players = new Player[30];
            Bullet[] bullets = new Bullet[90];
            List<Player> playersList = new List<Player>();
            List<Bullet> bulletsList = new List<Bullet>();
            bool[,] buildingCoords = new bool[1000, 1000];
            var pointToQuad = new Dictionary<string, int>();

            DateTime then = DateTime.Now;
            for (int i = 0; i < 250; i++)
            {
                for (int j = 0; j < 250; j++)
                {
                    pointToQuad[i.ToString() + j.ToString()] = 0;
                }

                for (int j = 250; j < 500; j++)
                {
                    pointToQuad[i.ToString() + j.ToString()] = 4;
                }

                for (int j = 500; j < 750; j++)
                {
                    pointToQuad[i.ToString() + j.ToString()] = 8;
                }

                for (int j = 750; j < 1000; j++)
                {
                    pointToQuad[i.ToString() + j.ToString()] = 12;
                }
            }

            for (int i = 250; i < 500; i++)
            {
                for (int j = 0; j < 250; j++)
                {
                    pointToQuad[i.ToString() + j.ToString()] = 1;
                }

                for (int j = 250; j < 500; j++)
                {
                    pointToQuad[i.ToString() + j.ToString()] = 5;
                }

                for (int j = 500; j < 750; j++)
                {
                    pointToQuad[i.ToString() + j.ToString()] = 9;
                }

                for (int j = 750; j < 1000; j++)
                {
                    pointToQuad[i.ToString() + j.ToString()] = 13;
                }
            }

            for (int i = 500; i < 750; i++)
            {
                for (int j = 0; j < 250; j++)
                {
                    pointToQuad[i.ToString() + j.ToString()] = 2;
                }

                for (int j = 250; j < 500; j++)
                {
                    pointToQuad[i.ToString() + j.ToString()] = 6;
                }

                for (int j = 500; j < 750; j++)
                {
                    pointToQuad[i.ToString() + j.ToString()] = 10;
                }

                for (int j = 750; j < 1000; j++)
                {
                    pointToQuad[i.ToString() + j.ToString()] = 14;
                }
            }

            for (int i = 750; i < 1000; i++)
            {
                for (int j = 0; j < 250; j++)
                {
                    pointToQuad[i.ToString() + j.ToString()] = 3;
                }

                for (int j = 250; j < 500; j++)
                {
                    pointToQuad[i.ToString() + j.ToString()] = 7;
                }

                for (int j = 500; j < 750; j++)
                {
                    pointToQuad[i.ToString() + j.ToString()] = 11;
                }

                for (int j = 750; j < 1000; j++)
                {
                    pointToQuad[i.ToString() + j.ToString()] = 15;
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

            DateTime thenish = DateTime.Now;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here
           
            foreach (Player player in players)
            {
                var uniquePoints = new HashSet<int>();
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


                uniquePoints.Add(pointToQuad[testRight]);
                uniquePoints.Add(pointToQuad[testLeft]);
                uniquePoints.Add(pointToQuad[testTop]);
                uniquePoints.Add(pointToQuad[testBottom]);
                
                uniquePoints.Add(pointToQuad[testTopRight]);
                uniquePoints.Add(pointToQuad[testTopLeft]);
                uniquePoints.Add(pointToQuad[testBottomRight]);
                uniquePoints.Add(pointToQuad[testBottomLeft]);

                foreach(int quadrant in uniquePoints)
                {
                    quadrants[quadrant].players.Add(player);
                }
                
                playersList.Add(player);
            }
            Console.WriteLine("Players: took {0} to assign", DateTime.Now - thenish);
            watch.Stop();
            var elapsedMs = watch.ElapsedTicks / (System.Diagnostics.Stopwatch.Frequency / (1000L * 1000L));
            Console.WriteLine("Players: took {0} to assign in microseconds", elapsedMs);

            watch = System.Diagnostics.Stopwatch.StartNew();

            thenish = DateTime.Now;
            foreach (Bullet bullet in bullets)
            {
                var uniquePoints = new HashSet<int>();
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
                
                uniquePoints.Add(pointToQuad[testRight]);
                uniquePoints.Add(pointToQuad[testLeft]);
                uniquePoints.Add(pointToQuad[testTop]);
                uniquePoints.Add(pointToQuad[testBottom]);
                
                uniquePoints.Add(pointToQuad[testTopRight]);
                uniquePoints.Add(pointToQuad[testTopLeft]);
                uniquePoints.Add(pointToQuad[testBottomRight]);
                uniquePoints.Add(pointToQuad[testBottomLeft]);

                foreach(int quadrant in uniquePoints)
                {
                    quadrants[quadrant].bullets.Add(bullet);
                }
                
                bulletsList.Add(bullet);
            }
            Console.WriteLine("Bullets: took {0} to assign", DateTime.Now - thenish);
            watch.Stop();
            elapsedMs = watch.ElapsedTicks / (System.Diagnostics.Stopwatch.Frequency / (1000L * 1000L));
            Console.WriteLine("bullets: took {0} to assign - in microseconds", elapsedMs);

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
                    for (int i = quad.players.Count - 1; i >= 0; i--)
                    {
                        // Backwards using index so we can delete bullets as they're used
                        for (int j = quad.bullets.Count - 1; j >= 0; j--)
                        {
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
                                    //quad.players[i].Health -= *Bullet Damage* 100;
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

            Console.WriteLine("Quadrant: took {0} with {1} collisions", DateTime.Now - then, collisions);

        }


        private static Boolean CircleCollided(float x1, float y1, float r1, float x2, float y2, float r2)
        {
            double distance = Math.Sqrt( (x1-x2)*(x1-x2)+(y1-y2)*(y1-y2) );
            return (distance < (r1 + r2));
        }
        
    }
}