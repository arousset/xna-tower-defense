using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TowerDefenseXNA
{
    public class Player
    {
        private int money = 100;
        private int lives = 30;

        // Players towers
        private List<Tower> towers = new List<Tower>();

        private MouseState mouseState; // state for current frame
        private MouseState oldState; // state for previous frame
        private Level level;
        private Texture2D towerTexture;
        private Texture2D bulletTexture;
        private Texture2D[] towerTextures;
        private Texture2D rangeTexture;
        private Tower selectedTower;

        // Tower placement
        private int cellX;
        private int cellY;
        private int tileX;
        private int tileY;

        private string newTowerType;
        private int newTowerIndex;

        public bool TowerSelected
        {
            get { return selectedTower != null; }
        }

        public string NewTowerType
        {
            set { newTowerType = value; }
        }

        public int Money
        {
            get { return money; }
            set { money = value; }
        }

        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        public int NewTowerIndex
        {
            set { newTowerIndex = value; }
        }

        // Constructor 
        public Player(Level level, Texture2D[] towerTextures, Texture2D bulletTexture, Texture2D rangeTexture)
        {
            this.level = level;

            this.towerTextures = towerTextures;
            this.bulletTexture = bulletTexture;
            this.rangeTexture = rangeTexture;
        }

        // Method Sell or UPDATE PAR LA SUITE !
        public void SellButtonOnPress(object sender, EventArgs e)
        {
            money += (int)((float)selectedTower.Cost * 0.75f);
            // Mettre code pour fiare upgrade
            selectedTower = null;

        }



        public void Update(GameTime gameTime, List<Enemy> enemies)
        {
            mouseState = Mouse.GetState();

            cellX = (int)(mouseState.X / 32); // Convert the position of the mouse
            cellY = (int)(mouseState.Y / 32); // from array space to level space

            tileX = cellX * 32; // Convert from array space to level space
            tileY = cellY * 32; // Convert from array space to level space
           
            if (mouseState.LeftButton == ButtonState.Released && oldState.LeftButton == ButtonState.Pressed)
            {
                if (string.IsNullOrEmpty(newTowerType) == false)
                {
                    AddTower();
                }
                else
                {
                    if (selectedTower != null)
                    {
                        if (!selectedTower.Bounds.Contains(mouseState.X, mouseState.Y))
                        {
                            selectedTower.Selected = false;
                            selectedTower = null;
                        }
                    }

                    foreach (Tower tower in towers)
                    {
                        if (tower == selectedTower)
                        {
                            continue;
                        }

                        if (tower.Bounds.Contains(mouseState.X, mouseState.Y))
                        {
                            selectedTower = tower;
                            tower.Selected = true;
                        }
                    }
                }
            }

            if (mouseState.RightButton == ButtonState.Released && oldState.RightButton == ButtonState.Pressed)
            {
                selectedTower = null;
                newTowerType = string.Empty;
            }

            foreach (Tower tower in towers)
            {
                if (tower.HasTarget == false)
                {
                    tower.GetClosestEnemy(enemies);
                }
                tower.Update(gameTime);
            }

            oldState = mouseState; // Set the oldState so it becomes the state of the previous frame.

            if (selectedTower != null)
            {
                Vector2 radiusPosition = selectedTower.Center - new Vector2(selectedTower.Radius);
                Console.WriteLine(selectedTower.Radius);
                Rectangle radiusRect = new Rectangle(
                    (int)radiusPosition.X,
                    (int)radiusPosition.Y,
                    (int)selectedTower.Radius * 2,
                    (int)selectedTower.Radius * 2);

                //Draw(rangeTexture, radiusRect, Color.White);
            }

            
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tower tower in towers)
            {
                tower.Draw(spriteBatch);
            }
        }

        public void DrawPreview(SpriteBatch spriteBatch)
        {
            // Draw the tower preview.
            if (string.IsNullOrEmpty(newTowerType) == false)
            {
                int cellX = (int)(mouseState.X / 32); // Convert the position of the mouse
                int cellY = (int)(mouseState.Y / 32); // from array space to level space

                int tileX = cellX * 32; // Convert from array space to level space
                int tileY = cellY * 32; // Convert from array space to level space

                Texture2D previewTexture = towerTextures[newTowerIndex];
                spriteBatch.Draw(previewTexture, new Rectangle(tileX, tileY, previewTexture.Width, previewTexture.Height), Color.White);

                if (selectedTower != null)
                {
                    Vector2 radiusPosition = selectedTower.Center - new Vector2(selectedTower.Radius);
                    Console.WriteLine(selectedTower.Radius);
                    Rectangle radiusRect = new Rectangle(
                        (int)tileX - (int)selectedTower.Radius + (previewTexture.Width/2),
                        (int)tileY - (int)selectedTower.Radius + (previewTexture.Height / 2),
                        (int)selectedTower.Radius * 2,
                        (int)selectedTower.Radius * 2);

                    spriteBatch.Draw(rangeTexture, radiusRect, Color.White);
                }
            }
        }

        private bool IsCellClear()
        {
            bool inBounds = cellX >= 0 && cellY >= 0 && // Make sure tower is within limits
                cellX < level.Width && cellY < level.Height;

            bool spaceClear = true;

            foreach (Tower tower in towers) // Check that there is no tower here
            {
                spaceClear = (tower.Position != new Vector2(tileX, tileY));

                if (!spaceClear)
                    break;
            }

            bool onPath = (level.GetIndex(cellX, cellY) != 1);

            return inBounds && spaceClear && onPath; // If both checks are true return true
        }

        public void giveGold(int gold)
        {
            money += gold;
        }


        public void AddTower()
        {
            Tower towerToAdd = null;

            switch (newTowerType)
            {
                case "Arrow Tower":
                    {
                        towerToAdd = new ArrowTower(towerTextures[0], bulletTexture, rangeTexture, new Vector2(tileX, tileY));
                        selectedTower = towerToAdd;
                        break;
                    }
                case "Spike Tower":
                    {
                        towerToAdd = new SpikeTower(towerTextures[1], bulletTexture, rangeTexture, new Vector2(tileX, tileY));
                        break;
                    }
                case "Slow Tower":
                    {
                        towerToAdd = new SlowTower(towerTextures[2], bulletTexture, rangeTexture, new Vector2(tileX, tileY));
                        break;
                    }
                case "Fire Tower":
                    {
                        towerToAdd = new FireTower(towerTextures[3], bulletTexture, rangeTexture, new Vector2(tileX, tileY));
                        break;
                    }
            }


            // Only add the tower if there is a space and if the player can afford it.
            if (IsCellClear() == true && towerToAdd.Cost <= money)
            {
                towers.Add(towerToAdd);
                money -= towerToAdd.Cost;

                // Reset the newTowerType field.
                newTowerType = string.Empty;
            }
        }




    }
}

