using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseXNA
{
    public class Level
    {
        private List<Texture2D> tileTextures = new List<Texture2D>();
        private int tilesPerRow;
        private Queue<Vector2> waypoints = new Queue<Vector2>();
        private int tileSize;
        private Vector2 start = new Vector2(0, 13);
        private Vector2 end = new Vector2(29, 10);

        int[,] map = new int[,]
        {
            {5, 5, 5, 5, 5, 5, 5, 15, 15, 15, 15, 15, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 14, 14, 14, 16, 16, 16, 16, 16,},
            {5, 5, 5, 5, 5, 5, 5, 5, 15, 15, 15, 15, 15, 5, 5, 5, 1, 2, 2, 2, 3, 5, 14, 14, 16, 16, 16, 16, 16, 16,},
            {5, 5, 5, 5, 5, 5, 5, 5, 5, 15, 15, 15, 15, 15, 15, 5, 4, 15, 15, 15, 6, 5, 5, 14, 16, 16, 16, 16, 16, 16,},
            {5, 5, 5, 5, 5, 5, 5, 5, 5, 15, 15, 15, 15, 15, 5, 5, 4, 15, 15, 15, 6, 5, 5, 14, 14, 16, 16, 14, 16, 16,},
            {15, 15, 15, 5, 5, 5, 5, 5, 5, 5, 5, 5, 15, 1, 2, 2, 12, 5, 15, 15, 6, 5, 5, 5, 15, 15, 14, 14, 14, 14,},
            {15, 15, 15, 5, 5, 5, 1, 2, 3, 5, 5, 5, 5, 4, 5, 5, 5, 5, 5, 15, 13, 2, 2, 2, 2, 2, 2, 3, 14, 14,},
            {15, 15, 15, 15, 5, 5, 4, 5, 6, 5, 5, 5, 5, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 15, 15, 15, 5, 6, 5, 14,},
            {15, 15, 15, 15, 5, 5, 4, 5, 6, 5, 5, 5, 5, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 15, 16, 14, 14, 6, 5, 5,},
            {15, 15, 15, 15, 5, 5, 4, 5, 6, 5, 5, 5, 5, 4, 5, 5, 5, 5, 14, 14, 14, 5, 5, 15, 15, 16, 14, 6, 5, 5,},
            {15, 15, 15, 15, 5, 5, 4, 5, 13, 2, 2, 2, 2, 12, 5, 15, 15, 15, 14, 16, 14, 15, 5, 15, 5, 14, 14, 6, 5, 5,},
            {15, 15, 5, 5, 5, 5, 4, 15, 5, 5, 5, 5, 5, 5, 15, 15, 16, 16, 16, 16, 14, 15, 15, 5, 5, 5, 5, 13, 2, 2,},
            {15, 5, 5, 5, 5, 5, 4, 15, 15, 5, 5, 14, 14, 15, 15, 14, 16, 16, 16, 16, 16, 16, 15, 5, 5, 5, 5, 5, 5, 5,},
            {5, 5, 5, 5, 5, 5, 4, 5, 15, 5, 14, 14, 14, 14, 16, 16, 16, 16, 16, 16, 16, 16, 14, 14, 15, 5, 5, 5, 5, 5,},
            {2, 2, 2, 2, 2, 2, 12, 15, 15, 5, 14, 14, 14, 14, 16, 16, 16, 16, 16, 16, 16, 16, 16, 14, 15, 15, 15, 15, 5, 5,},
            {5, 5, 5, 5, 5, 5, 15, 15, 5, 14, 14, 14, 14, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 15, 15, 15, 15, 15, 5},
        };

        public int Width
        {
            get { return map.GetLength(1); }
        }

        public int Height
        {
            get { return map.GetLength(0); }
        }

        public Queue<Vector2> Waypoints
        {
            get { return waypoints; }
        }

        public Level(Texture2D texture, int tileSize)
        {
            tileTextures.Add(texture);
            tilesPerRow = texture.Width / tileSize;
            this.tileSize = tileSize;

            waypoints.Enqueue(start*tileSize);
            Vector2 current = start;
            int x = (int)start.X;
            int y = (int)start.Y;
            while(x != end.X || y != end.Y)
            {
                int i = map[y, x+1];
                if (i >= 0 && i < 14 && i != 5 && !alreadyExistsInWayPoints(x+1, y))
                {
                    waypoints.Enqueue(new Vector2(x+1, y) * tileSize);
                    x++;
                    //System.Console.WriteLine("(" + x + "," + y + ")");
                    continue;
                }
                i = map[y+1, x];
                if (i >= 0 && i < 14 && i != 5 && !alreadyExistsInWayPoints(x, y+1))
                {
                    waypoints.Enqueue(new Vector2(x, y + 1) * tileSize);
                    y++;
                    //System.Console.WriteLine("(" + x + "," + y + ")");
                    continue;
                }
                i = map[y-1, x];
                if (i >= 0 && i < 14 && i != 5 && !alreadyExistsInWayPoints(x, y-1))
                {
                    waypoints.Enqueue(new Vector2(x, y - 1) * tileSize);
                    y--;
                    //System.Console.WriteLine("(" + x + "," + y + ")");
                    continue;
                }
                i = map[y, x-1];
                if (i >= 0 && i < 14 && i != 5 && !alreadyExistsInWayPoints(x-1, y))
                {
                    waypoints.Enqueue(new Vector2(x - 1, y) * tileSize);
                    x--;
                    //System.Console.WriteLine("(" + x + "," + y + ")");
                    continue;
                }
            }
            waypoints.Enqueue(end*tileSize);
        }

        private bool alreadyExistsInWayPoints(int x, int y)
        {
            for (int i = 0; i < waypoints.Count(); i++)
            {
                if (waypoints.ElementAt(i).X == x*tileSize && waypoints.ElementAt(i).Y == y*tileSize)
                    return true;
            }
            return false;
        }

        public int GetIndex(int cellX, int cellY)
        {
            // -1 : In order to add the toolbar 
            if (cellX < 0 || cellX > Width - 1 || cellY < 0 || cellY > Height - 1)
                return 0;

            return map[cellY, cellX];
        }

        public void Draw(SpriteBatch batch)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int textureIndex = map[y, x];

                    if (textureIndex == -1)
                        continue;

                    Rectangle destinationRect = new Rectangle(x * tileSize,
                                                            y * tileSize,
                                                            tileSize,
                                                            tileSize);

                    int sourceX = ((textureIndex - 1) % tilesPerRow);
                    int sourceY = ((textureIndex - 1) / tilesPerRow);

                    Rectangle sourceRectangle = new Rectangle(sourceX * tileSize,
                                                              sourceY * tileSize,
                                                              tileSize,
                                                              tileSize);

                    batch.Draw(tileTextures[0], destinationRect, sourceRectangle, Color.White);
                }
            }
        }
    }
}
