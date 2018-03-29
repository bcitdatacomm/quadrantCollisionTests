using System;
using System.Collections.Generic;

namespace quadtree
{
    // Bullet object, randomly initialised
    internal class Bullet
    {
        public Bullet()
        {
            Random rng = new Random();
            
            X = rng.Next(-500, 500);
            Y = rng.Next(-500, 500);
        }
        
        public float X { get; set; }
        public float Y { get; set; }
    }

    // Player object, randomly initialised
    internal class Player
    {
        public Player()
        {
            Random rng = new Random();
            
            X = rng.Next(-500, 500);
            Y = rng.Next(-500, 500);
        }
        
        public float X { get; set; }
        public float Y { get; set; }
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
            Quadrant[] quadrants = new Quadrant[16];
            Player[] players = new Player[30];
            Bullet[] bullets = new Bullet[90];
            bool[,] buildingCoords = new bool[1000, 1000];
            var points = new Dictionary<string, int>();

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

            for(int i = 0; i < players.Length; i++)
            {
                players[i] = new Player();
            }
            
            for(int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = new Bullet();
            }

            for (int i = 0; i < 16; i++)
            {
                quadrants[i] = new Quadrant();
            }

            foreach (Player player in players)
            {
                String test = (player.X + 500).ToString() + (player.Y + 500).ToString();
                quadrants[points[test]].players.Add(player);
            }
            
            foreach (Bullet bullet in bullets)
            {
                String test = (bullet.X + 500).ToString() + (bullet.Y + 500).ToString();
                quadrants[points[test]].bullets.Add(bullet);
            }

            // Naive Collisions check
            int collisions = 0;
            foreach (Player player in players)
            {
                foreach (Bullet bullet in bullets)
                {
                    bool temp = CircleCollided(player.X, player.Y, bullet.X, bullet.Y);

                    if (temp == true)
                    {
                        collisions++;
                    }
                    
                }
            }
            Console.WriteLine("Naive: Got {0} collisions", collisions);
            
            // Naive Speed check
            DateTime then = DateTime.Now;
            foreach (Player player in players)
            {
                foreach (Bullet bullet in bullets)
                {
                    bool temp = CircleCollided(player.X, player.Y, bullet.X, bullet.Y);
                }
            }
            Console.WriteLine("Naive: took {0}", DateTime.Now - then);
            
            
            // Quadrant Collisions check
            collisions = 0;
            foreach (Quadrant quad in quadrants)
            {
                foreach (Player player in quad.players)
                {
                    foreach (Bullet bullet in quad.bullets)
                    {
                        bool temp = CircleCollided(player.X, player.Y, bullet.X, bullet.Y);
                        if (temp == true)
                        {
                            collisions++;
                        }
                    }
                }
            }
            Console.WriteLine("Quadrant: Got {0} collisions", collisions);
            
            // Quadrant Speed check
            then = DateTime.Now;
            foreach (Quadrant quad in quadrants)
            {
                foreach (Player player in quad.players)
                {
                    foreach (Bullet bullet in quad.bullets)
                    {
                        bool temp = CircleCollided(player.X, player.Y, bullet.X, bullet.Y);
                    }
                }
            }
            Console.WriteLine("Quadrant: took {0}", DateTime.Now - then);
           
            Console.WriteLine("Memory used: {0}", GC.GetTotalMemory(true));
        }

        private static Boolean CircleCollided(float x1, float y1, float x2, float y2)
        {
            float radius = 10;
            float distance = ((x1-x2)*(x1-x2)+(y1-y2)*(y1-y2));
            return (distance < (radius * 2));
        }
        
    }
}