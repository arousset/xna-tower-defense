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
        private Texture2D bulletTexture;
        private Texture2D[] towerTextures;
        private Texture2D rangeTexture;
        private Tower selectedTower;
        private Tower selectedTower_radius;
        private Tower selectedTower_replace;
        SoundEffect[] bulletsAudio;
        private bool replace;
        private bool replace2;
        private int deplacement_costaCondcordia;
        
        private Texture2D btsell;
        private Texture2D btreplace;
        private Texture2D btupgrade;

        private Texture2D ranksilver;
        private Texture2D rankgold;

        private Button bt_sell;
        private Button bt_upgrade;
        private Button bt_replace;
        
        private Rectangle btsell_position;
        private Rectangle btreplace_position;
        private Rectangle btupgrade_position;

        private bool first;
        private int rotate;
        private SpriteFont font;
        private int index;

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
        public Player(Level level, Texture2D[] towerTextures, Texture2D bulletTexture, Texture2D rangeTexture, int life, int money, SoundEffect[] bulletsAudio, Texture2D btsell, Texture2D btreplace, Texture2D btupgrade, SpriteFont font, Texture2D ranksilver, Texture2D rankgold)
        {
            this.bulletsAudio = bulletsAudio;   
            this.level = level;
            this.lives = life;
            this.money = money;
            this.towerTextures = towerTextures;
            this.bulletTexture = bulletTexture;
            this.rangeTexture = rangeTexture;
            this.btsell = btsell;
            this.btupgrade = btupgrade;
            this.btreplace = btreplace;
            this.ranksilver = ranksilver;
            this.rankgold = rankgold;

            bt_sell = new TowerDefenseXNA.Button(btsell, btsell, btsell, new Vector2(10, 100));
            bt_upgrade = new TowerDefenseXNA.Button(btupgrade, btupgrade, btupgrade, new Vector2(10, 100));
            bt_replace = new TowerDefenseXNA.Button(btreplace, btreplace, btreplace, new Vector2(100, 10));

            bt_sell.OnPress += new EventHandler(SellButtonOnPress);
            bt_upgrade.OnPress += new EventHandler(upgradeButtonOnPress);
            bt_replace.OnPress += new EventHandler(replaceButtonOnPress);
            first = true;
            rotate = 32;
            index = 0;
            replace = false;
            this.font =font;
            deplacement_costaCondcordia = 10;
        }

        // Methods menu !
        public void SellButtonOnPress(object sender, EventArgs e)
        {
            if (selectedTower_radius != null)
            {
                money += (int)((float)selectedTower_radius.Cost * 0.75f);
                towers.Remove(selectedTower_radius);
                selectedTower = null;
                selectedTower_radius = null;
            }
        }

        public void upgradeButtonOnPress(object sender, EventArgs e)
        {
            if (selectedTower_radius != null)
            {
                Console.WriteLine("upgrade");
                switch (selectedTower_radius.Name)
                {
                    case "ArrowTower":
                        switch (selectedTower_radius.Level_tower)
                        {
                            case 1:
                                selectedTower_radius.Radius = selectedTower_radius.Radius + selectedTower_radius.Radius * 0.20f;
                                selectedTower_radius.Cost = selectedTower_radius.Cost + (int)(selectedTower_radius.Cost * 0.12f);
                                selectedTower_radius.Damage = selectedTower_radius.Damage + (int)(selectedTower_radius.Damage * 0.15f);
                                selectedTower_radius.Level_tower += 1;
                                this.money = money - 15;
                                break;
                            case 2:
                                selectedTower_radius.Radius = selectedTower_radius.Radius + selectedTower_radius.Radius * 0.20f;
                                selectedTower_radius.Cost = selectedTower_radius.Cost + (int)(selectedTower_radius.Cost * 0.12f);
                                selectedTower_radius.Damage = selectedTower_radius.Damage + (int)(selectedTower_radius.Damage * 0.15f);
                                selectedTower_radius.Level_tower += 1;
                                this.money = money - 15;
                                break;
                            case 3:
                                selectedTower_radius.Radius = selectedTower_radius.Radius + selectedTower_radius.Radius * 0.20f;
                                selectedTower_radius.Cost = selectedTower_radius.Cost + (int)(selectedTower_radius.Cost * 0.12f);
                                selectedTower_radius.Damage = selectedTower_radius.Damage + (int)(selectedTower_radius.Damage * 0.15f);
                                selectedTower_radius.Level_tower += 1;
                                if (selectedTower_radius.Level_tower == 3)
                                {
                                    this.money = money - 15;
                                }
                                break;
                        }
                        break;

                    case "SpikeTower":
                        switch (selectedTower_radius.Level_tower)
                        {
                            case 1:
                                selectedTower_radius.Radius = selectedTower_radius.Radius + selectedTower_radius.Radius * 0.20f;
                                selectedTower_radius.Cost = selectedTower_radius.Cost + (int)(selectedTower_radius.Cost * 0.12f);
                                selectedTower_radius.Damage = selectedTower_radius.Damage + (int)(selectedTower_radius.Damage * 0.15f);
                                selectedTower_radius.Level_tower += 1;
                                this.money = money - 15;
                                break;
                            case 2:
                                selectedTower_radius.Radius = selectedTower_radius.Radius + selectedTower_radius.Radius * 0.20f;
                                selectedTower_radius.Cost = selectedTower_radius.Cost + (int)(selectedTower_radius.Cost * 0.12f);
                                selectedTower_radius.Damage = selectedTower_radius.Damage + (int)(selectedTower_radius.Damage * 0.15f);
                                selectedTower_radius.Level_tower += 1;
                                this.money = money - 15;
                                break;
                            case 3:
                                selectedTower_radius.Radius = selectedTower_radius.Radius + selectedTower_radius.Radius * 0.20f;
                                selectedTower_radius.Cost = selectedTower_radius.Cost + (int)(selectedTower_radius.Cost * 0.12f);
                                selectedTower_radius.Damage = selectedTower_radius.Damage + (int)(selectedTower_radius.Damage * 0.15f);
                                selectedTower_radius.Level_tower += 1;
                                if (selectedTower_radius.Level_tower == 3)
                                {
                                    this.money = money - 15;
                                }
                                break;
                        }
                        break;

                    case "SlowTower":
                        switch (selectedTower_radius.Level_tower)
                        {
                            case 1:
                                selectedTower_radius.Radius = selectedTower_radius.Radius + selectedTower_radius.Radius * 0.20f;
                                selectedTower_radius.Cost = selectedTower_radius.Cost + (int)(selectedTower_radius.Cost * 0.12f);
                                selectedTower_radius.Damage = selectedTower_radius.Damage + (int)(selectedTower_radius.Damage * 0.15f);
                                selectedTower_radius.Level_tower += 1;
                                this.money = money - 15;
                                break;
                            case 2:
                                selectedTower_radius.Radius = selectedTower_radius.Radius + selectedTower_radius.Radius * 0.20f;
                                selectedTower_radius.Cost = selectedTower_radius.Cost + (int)(selectedTower_radius.Cost * 0.12f);
                                selectedTower_radius.Damage = selectedTower_radius.Damage + (int)(selectedTower_radius.Damage * 0.15f);
                                selectedTower_radius.Level_tower += 1;
                                this.money = money - 15;
                                break;
                            case 3:
                                selectedTower_radius.Radius = selectedTower_radius.Radius + selectedTower_radius.Radius * 0.20f;
                                selectedTower_radius.Cost = selectedTower_radius.Cost + (int)(selectedTower_radius.Cost * 0.12f);
                                selectedTower_radius.Damage = selectedTower_radius.Damage + (int)(selectedTower_radius.Damage * 0.15f);
                                selectedTower_radius.Level_tower += 1;
                                if (selectedTower_radius.Level_tower == 3)
                                {
                                    this.money = money - 15;
                                }
                                break;
                        }
                        break;

                    case "FireTower":
                        switch (selectedTower_radius.Level_tower)
                        {
                            case 1:
                                selectedTower_radius.Radius = selectedTower_radius.Radius + selectedTower_radius.Radius * 0.20f;
                                selectedTower_radius.Cost = selectedTower_radius.Cost + (int)(selectedTower_radius.Cost * 0.12f);
                                selectedTower_radius.Damage = selectedTower_radius.Damage + (int)(selectedTower_radius.Damage * 0.15f);
                                selectedTower_radius.Level_tower += 1;
                                this.money = money - 15;
                                break;
                            case 2:
                                selectedTower_radius.Radius = selectedTower_radius.Radius + selectedTower_radius.Radius * 0.20f;
                                selectedTower_radius.Cost = selectedTower_radius.Cost + (int)(selectedTower_radius.Cost * 0.12f);
                                selectedTower_radius.Damage = selectedTower_radius.Damage + (int)(selectedTower_radius.Damage * 0.15f);
                                selectedTower_radius.Level_tower += 1;
                                this.money = money - 15;
                                break;
                            case 3:
                                selectedTower_radius.Radius = selectedTower_radius.Radius + selectedTower_radius.Radius * 0.20f;
                                selectedTower_radius.Cost = selectedTower_radius.Cost + (int)(selectedTower_radius.Cost * 0.12f);
                                selectedTower_radius.Damage = selectedTower_radius.Damage + (int)(selectedTower_radius.Damage * 0.15f);
                                selectedTower_radius.Level_tower += 1;
                                if (selectedTower_radius.Level_tower == 3)
                                {
                                    this.money = money - 15;
                                }
                                break;
                        }
                        break;
                }
                
                selectedTower = null;
                selectedTower_radius = null;
            }
        }

        // A faire Ya de la couille !!!!
        public void replaceButtonOnPress(object sender, EventArgs e)
        {
            int cellX = 0;
            int cellY = 0;
            tileX = 0;
            tileY = 0;
            int index = 0;

            if (selectedTower_radius != null)
            {
                switch (selectedTower_radius.Name)
                {
                    case "SpikeTower":
                        newTowerIndex = 1;
                        break;
                    case "ArrowTower":
                        newTowerIndex = 0;
                        break;
                    case "SlowTower":
                        newTowerIndex = 2;
                        break;
                    case "FireTower":
                        newTowerIndex = 3;
                        break;
                }
                selectedTower_replace = selectedTower_radius;
                replace = true;
                Console.WriteLine(selectedTower_replace);
            }
            selectedTower = null;
            selectedTower_radius = null;
        }



        public void Update(GameTime gameTime, List<Enemy> enemies)
        {
            replace2 = false;
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (first)
                {
                    if (selectedTower != null)
                    {
                        Rectangle radiusRectDown = new Rectangle(
                            (int)selectedTower.Center.X - rotate / 2,
                            (int)selectedTower.Center.Y + rotate / 2,
                            (int)32,
                            (int)32);

                        Rectangle radiusRectLeft = new Rectangle(
                            (int)selectedTower.Center.X + rotate / 2,
                            (int)selectedTower.Center.Y - rotate / 2,
                            (int)32,
                            (int)32);

                        Rectangle radiusRectRight = new Rectangle(
                            (int)selectedTower.Center.X - (rotate + rotate / 2),
                            (int)selectedTower.Center.Y - rotate / 2,
                            (int)32,
                            (int)32);

                        Rectangle radiusRectUp = new Rectangle(
                            (int)selectedTower.Center.X - rotate / 2,
                            (int)selectedTower.Center.Y - (rotate + rotate / 2),
                            (int)32,
                            (int)32);

                            btsell_position = radiusRectDown;
                            btreplace_position = radiusRectLeft;
                            btupgrade_position = radiusRectRight;
                        
                        if (bt_sell.Bounds == radiusRectDown)
                        {
                            btsell_position = radiusRectLeft;
                            btreplace_position = radiusRectUp;
                            btupgrade_position = radiusRectDown;
                            bt_sell.Bounds = btsell_position;
                            bt_replace.Bounds = btreplace_position;
                            bt_upgrade.Bounds = btupgrade_position;
                            Console.WriteLine("on affecte down");
                        } else if(bt_sell.Bounds == radiusRectLeft)
                            {
                                btsell_position = radiusRectUp;
                                btreplace_position = radiusRectRight;
                                btupgrade_position = radiusRectLeft;
                                bt_sell.Bounds = btsell_position;
                                bt_replace.Bounds = btreplace_position;
                                bt_upgrade.Bounds = btupgrade_position;
                                Console.WriteLine("on affecte left");
                            } else if (bt_sell.Bounds == radiusRectUp)
                                {
                                    btsell_position = radiusRectRight;
                                    btreplace_position = radiusRectDown;
                                    btupgrade_position = radiusRectUp;
                                    bt_sell.Bounds = btsell_position;
                                    bt_replace.Bounds = btreplace_position;
                                    bt_upgrade.Bounds = btupgrade_position;
                                    Console.WriteLine("on affecte up");
                                } else if (bt_sell.Bounds == radiusRectRight)
                                    {
                                        btsell_position = radiusRectDown;
                                        btreplace_position = radiusRectLeft;
                                        btupgrade_position = radiusRectRight;
                                        bt_sell.Bounds = btsell_position;
                                        bt_replace.Bounds = btreplace_position;
                                        bt_upgrade.Bounds = btupgrade_position;
                                        Console.WriteLine("on affecte right");
                                    }
                        first = false;
                    }
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                if (!first)
                {
                    Console.WriteLine("on appuieeee PAS");
                    first = true;
                }
            }
                       
            mouseState = Mouse.GetState();

            cellX = (int)(mouseState.X / 32); // Convert the position of the mouse
            cellY = (int)(mouseState.Y / 32); // from array space to level space

            tileX = cellX * 32; // Convert from array space to level space
            tileY = cellY * 32; // Convert from array space to level space
           
            if (mouseState.LeftButton == ButtonState.Released && oldState.LeftButton == ButtonState.Pressed)
            {
                // Replace a tower
                if (replace)
                {
                    replace2 = true;
                    cellX = (int)(mouseState.X / 32); // Convert the position of the mouse
                    cellY = (int)(mouseState.Y / 32); // from array space to level space
                    tileX = cellX * 32; // Convert from array space to level space
                    tileY = cellY * 32; // Convert from array space to level space
                      

                    replace = false;
                    if (cellX != 0 && cellY != 0 && tileX != 0 && tileY != 0)
                    {
                        Console.WriteLine(index);
                        Rectangle carr = new Rectangle(tileX, tileY, 32, 32);
                        Console.WriteLine(selectedTower_replace.Position);
                        if (IsCellClear())
                        {
                            if (selectedTower_replace.Name == "SpikeTower")
                            {
                                selectedTower_replace.Position = new Vector2(tileX + (32 / 2), tileY + (32 / 2));
                                selectedTower_replace.Bounds = carr;
                                selectedTower_replace.Center = new Vector2(tileX + (32 / 2), tileY + (32 / 2));

                                selectedTower_replace.Selected = false;
                                selectedTower_replace = null;
                                selectedTower = null;
                            }
                            else
                            {
                                selectedTower_replace.Position = new Vector2(tileX, tileY);
                                selectedTower_replace.Bounds = carr;
                                selectedTower_replace.Center = new Vector2(tileX, tileY);

                                selectedTower_replace.Selected = false;
                                selectedTower_replace = null;
                                selectedTower = null;
                            }
                            money -= 10;
                        }
                    }
                }
                
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
                            //selectedTower_radius.Selected = false;
                            //selectedTower_radius = null;
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
                            if (replace2 == false)
                            {
                                selectedTower = tower;
                                Console.WriteLine(selectedTower);
                                selectedTower_radius = tower;
                                selectedTower_replace = tower;
                                Console.WriteLine(selectedTower_replace);
                                tower.Selected = true;
                                selectedTower_radius.Selected = true;
                                Rectangle radiusRectDown1 = new Rectangle(
                                    (int)selectedTower.Center.X - rotate / 2,
                                    (int)selectedTower.Center.Y + rotate / 2,
                                    (int)32,
                                    (int)32);

                                Rectangle radiusRectLeft1 = new Rectangle(
                                    (int)selectedTower.Center.X + rotate / 2,
                                    (int)selectedTower.Center.Y - rotate / 2,
                                    (int)32,
                                    (int)32);

                                Rectangle radiusRectRight1 = new Rectangle(
                                    (int)selectedTower.Center.X - (rotate + rotate / 2),
                                    (int)selectedTower.Center.Y - rotate / 2,
                                    (int)32,
                                    (int)32);

                                Rectangle radiusRectUp1 = new Rectangle(
                                    (int)selectedTower.Center.X - rotate / 2,
                                    (int)selectedTower.Center.Y - (rotate + rotate / 2),
                                    (int)32,
                                    (int)32);

                                btsell_position = radiusRectDown1;
                                btreplace_position = radiusRectLeft1;
                                btupgrade_position = radiusRectRight1;
                            }
                        }
                    }
                }
            }
            
            if (mouseState.RightButton == ButtonState.Released && oldState.RightButton == ButtonState.Pressed)
            {   // La feintasse ! du siou del Dios ! 
                if (selectedTower != null)
                {
                    selectedTower.Selected = false;
                    selectedTower_radius.Selected = false;
                }
                selectedTower = null;
                newTowerType = string.Empty;
                selectedTower_radius = null;
                replace = false;
                selectedTower_replace = null;
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
            bt_sell.Update(gameTime);
            bt_replace.Update(gameTime);
            bt_upgrade.Update(gameTime);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tower tower in towers)
            {
                tower.Draw(spriteBatch);
                if (tower.Level_tower == 2)
                {
                    spriteBatch.Draw(ranksilver, tower.Bounds, Color.White);
                }

                if (tower.Level_tower >= 3)
                {
                    spriteBatch.Draw(rankgold, tower.Bounds, Color.White);
                }
            }

            if (selectedTower != null)
            {
                bt_sell.Bounds = btsell_position;
                bt_sell.Draw(spriteBatch);

                if (money > deplacement_costaCondcordia)
                {
                    bt_replace.Bounds = btreplace_position;
                    bt_replace.Draw(spriteBatch);
                }

                if (selectedTower.Level_tower < 3)
                {
                    bt_upgrade.Bounds = btupgrade_position;
                    bt_upgrade.Draw(spriteBatch);
                }
            }

            if (replace)
            {
                Texture2D previewTexture = towerTextures[newTowerIndex];
                spriteBatch.Draw(previewTexture, new Rectangle(tileX, tileY, previewTexture.Width, previewTexture.Height), Color.White);

                if (selectedTower_radius != null)
                {
                    Vector2 radiusPosition = selectedTower_radius.Center - new Vector2(selectedTower_radius.Radius);
                    Console.WriteLine(selectedTower_radius.Radius);
                    Rectangle radiusRect = new Rectangle(
                        (int)tileX - (int)selectedTower_radius.Radius + (previewTexture.Width / 2),
                        (int)tileY - (int)selectedTower_radius.Radius + (previewTexture.Height / 2),
                        (int)selectedTower_radius.Radius * 2,
                        (int)selectedTower_radius.Radius * 2);

                    spriteBatch.Draw(rangeTexture, radiusRect, Color.White);
                }
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

                if (selectedTower_radius != null)
                {
                    Vector2 radiusPosition = selectedTower_radius.Center - new Vector2(selectedTower_radius.Radius);
                    Console.WriteLine(selectedTower_radius.Radius);
                    Rectangle radiusRect = new Rectangle(
                        (int)tileX - (int)selectedTower_radius.Radius + (previewTexture.Width / 2),
                        (int)tileY - (int)selectedTower_radius.Radius + (previewTexture.Height / 2),
                        (int)selectedTower_radius.Radius * 2,
                        (int)selectedTower_radius.Radius * 2);
                    
                    spriteBatch.Draw(rangeTexture, radiusRect, Color.White);
                }
            }
        }

        private bool IsCellClear()
        {
            // test menu barre
            if(cellX > 10 && cellX < 19 && cellY == 14)
                return false;

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
                        towerToAdd = new ArrowTower(towerTextures[0], bulletTexture, rangeTexture, new Vector2(tileX, tileY), bulletsAudio[0]);
                        selectedTower_radius = towerToAdd;
                        break;
                    }
                case "Spike Tower":
                    {
                        towerToAdd = new SpikeTower(towerTextures[1], bulletTexture, rangeTexture, new Vector2(tileX, tileY), bulletsAudio[1]);
                        selectedTower_radius = towerToAdd;
                        break;
                    }
                case "Slow Tower":
                    {
                        towerToAdd = new SlowTower(towerTextures[2], bulletTexture, rangeTexture, new Vector2(tileX, tileY), bulletsAudio[0]);
                        selectedTower_radius = towerToAdd;
                        break;
                    }
                case "Fire Tower":
                    {
                        towerToAdd = new FireTower(towerTextures[3], bulletTexture, rangeTexture, new Vector2(tileX, tileY), bulletsAudio[0]);
                        selectedTower_radius = towerToAdd;
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

