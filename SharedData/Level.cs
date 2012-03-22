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
        public int playerLife;
        public int playerMoney;
        public int tilesPerRow;
        public Queue<Vector2> waypoints = new Queue<Vector2>();
        public int tileSize;
        public Vector2 start;
        public Vector2 end;
        public int[][] wavelist;
        
        public int[][] map;


        public int Width
        {
            get { return  map[0].Length;}
        }

        public int Height
        {
            get { return map.Length; }
        }

        public Queue<Vector2> Waypoints
        {
            get { return waypoints; }
        }

        public int[][] Wavelist
        {
            get { return wavelist; }
        }

        public Level()
        {
        }

        public void init()
        {
             waypoints.Enqueue(start * tileSize);
            Vector2 current = start;
            int x = (int)start.X;
            int y = (int)start.Y;
            while (x != end.X || y != end.Y)
            {
                int i = map[y][x + 1];
                if (i >= 0 && i < 14 && i != 5 && !alreadyExistsInWayPoints(x + 1, y))
                {
                    waypoints.Enqueue(new Vector2(x + 1, y) * tileSize);
                    x++;
                    continue;
                }
                i = map[y + 1][x];
                if (i >= 0 && i < 14 && i != 5 && !alreadyExistsInWayPoints(x, y + 1))
                {
                    waypoints.Enqueue(new Vector2(x, y + 1) * tileSize);
                    y++;
                    continue;
                }
                i = map[y - 1][x];
                if (i >= 0 && i < 14 && i != 5 && !alreadyExistsInWayPoints(x, y - 1))
                {
                    waypoints.Enqueue(new Vector2(x, y - 1) * tileSize);
                    y--;
                    continue;
                }
                i = map[y][x - 1];
                if (i >= 0 && i < 14 && i != 5 && !alreadyExistsInWayPoints(x - 1, y))
                {
                    waypoints.Enqueue(new Vector2(x - 1, y) * tileSize);
                    x--;
                    continue;
                }
            }
            waypoints.Enqueue(end * tileSize);
        }

        public Level(Texture2D texture, int tileSize, int[][] map, Vector2 start, Vector2 end, int[][] wavelist, int playerLife, int playerMoney)
        {
            this.playerLife = playerLife;
            this.playerMoney = playerMoney;
            tilesPerRow = texture.Width / tileSize;
            this.tileSize = tileSize;
            this.map = map;
            this.start = start;
            this.end = end;
            this.wavelist = wavelist;

            waypoints.Enqueue(start * tileSize);
            Vector2 current = start;
            int x = (int)start.X;
            int y = (int)start.Y;
            while (x != end.X || y != end.Y)
            {
                int i = map[y][x + 1];
                if (i >= 0 && i < 14 && i != 5 && !alreadyExistsInWayPoints(x + 1, y))
                {
                    waypoints.Enqueue(new Vector2(x + 1, y) * tileSize);
                    x++;
                    continue;
                }
                i = map[y + 1][x];
                if (i >= 0 && i < 14 && i != 5 && !alreadyExistsInWayPoints(x, y + 1))
                {
                    waypoints.Enqueue(new Vector2(x, y + 1) * tileSize);
                    y++;
                    continue;
                }
                i = map[y - 1][x];
                if (i >= 0 && i < 14 && i != 5 && !alreadyExistsInWayPoints(x, y - 1))
                {
                    waypoints.Enqueue(new Vector2(x, y - 1) * tileSize);
                    y--;
                    continue;
                }
                i = map[y][x - 1];
                if (i >= 0 && i < 14 && i != 5 && !alreadyExistsInWayPoints(x - 1, y))
                {
                    waypoints.Enqueue(new Vector2(x - 1, y) * tileSize);
                    x--;
                    continue;
                }
            }
            waypoints.Enqueue(end * tileSize);
        }

        private bool alreadyExistsInWayPoints(int x, int y)
        {
            for (int i = 0; i < waypoints.Count(); i++)
            {
                if (waypoints.ElementAt(i).X == x * tileSize && waypoints.ElementAt(i).Y == y * tileSize)
                    return true;
            }
            return false;
        }

        public int GetIndex(int cellX, int cellY)
        {
            if (cellX < 0 || cellX > Width - 1 || cellY < 0 || cellY > Height - 1)
                return 0;

            return map[cellY][cellX];
        }

        public void Draw(SpriteBatch batch, Texture2D tileMap)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int textureIndex = map[y][x];

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

                    //batch.Draw(tileTextures[0], destinationRect, sourceRectangle, Color.White);
                    batch.Draw(tileMap, destinationRect, sourceRectangle, Color.White);
                }
            }
        }
    }
}
