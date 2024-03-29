﻿using System;
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
        public List<Tower> towers = new List<Tower>();

        public int[] last_coup = new int[7];
        // ---------- protocol ------------
        /* numero de coup
         * type coup (-1 : coup vide | 0 : ajouter une tour | 1 : vendre une tour | 2 : deplacer tour | 3 : amélioré tour | 4 : lancer vague)
         * type tour ( 1 : grise | 2 spike | 3 slower | 4 fire)
         * coordonné tourX
         * coordonne tourY
         * New coordonné tourX
         * New coordonné tourY
         */
        // représente le numéro de coup lu
        public int compteur_read = 0;
        public int compteur_write = 0;


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
        private bool modify_value;

        // Tower placement
        private int cellX;
        private int cellY;
        private int tileX;
        private int tileY;

        private string newTowerType;
        private int newTowerIndex;

        public bool Modify_value
        {
            get { return modify_value; }
            set { modify_value = value; }
        }
       
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
            modify_value = false;
            last_coup[0] = -1;
            last_coup[1] = -1;
            last_coup[2] = -1;
            last_coup[3] = -1;
            last_coup[4] = -1;
            last_coup[5] = -1;
            last_coup[6] = -1;
        }

        // Methods menu !
        public void SellButtonOnPress(object sender, EventArgs e)
        {
            if (selectedTower_radius != null)
            {
                last_coup[0] = compteur_write + 1;
                compteur_write++;
                last_coup[1] = 1;
                int x = (int)(selectedTower_radius.Position.X / 32);
                int y = (int)(selectedTower_radius.Position.Y / 32);
                last_coup[3] = x;
                last_coup[4] = y;
                last_coup[5] = 0;
                last_coup[6] = 0;

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

                last_coup[0] = compteur_write + 1;
                compteur_write++;
                last_coup[1] = 3;
                last_coup[3] = (int)selectedTower_radius.Position.X / 32;
                last_coup[4] = (int)selectedTower_radius.Position.Y / 32;
                last_coup[5] = 0;
                last_coup[6] = 0;
                
                selectedTower = null;
                selectedTower_radius = null;
            }
        }


        public void replaceButtonOnPress(object sender, EventArgs e)
        {
            tileX = 0;
            tileY = 0;
            
            if (selectedTower_radius != null)
            {
                last_coup[1] = 2;
                switch (selectedTower_radius.Name)
                {
                    case "SpikeTower":
                        newTowerIndex = 1;
                        last_coup[2] = 2;
                        break;
                    case "ArrowTower":
                        newTowerIndex = 0;
                        last_coup[2] = 1;
                        break;
                    case "SlowTower":
                        newTowerIndex = 2;
                        last_coup[2] = 3;
                        break;
                    case "FireTower":
                        newTowerIndex = 3;
                        last_coup[2] = 4;
                        break;
                }
                selectedTower_replace = selectedTower_radius;
                last_coup[3] = (int)selectedTower_replace.Position.X / 32;
                last_coup[4] = (int)selectedTower_replace.Position.Y / 32;
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

                    last_coup[5] = cellX;
                    last_coup[6] = cellY;
                    last_coup[0] = compteur_write + 1;
                    compteur_write++;
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
                        last_coup[2] = 0;
                        break;
                    }
                case "Spike Tower":
                    {
                        towerToAdd = new SpikeTower(towerTextures[1], bulletTexture, rangeTexture, new Vector2(tileX, tileY), bulletsAudio[1]);
                        selectedTower_radius = towerToAdd;
                        last_coup[2] = 1;
                        break;
                    }
                case "Slow Tower":
                    {
                        towerToAdd = new SlowTower(towerTextures[2], bulletTexture, rangeTexture, new Vector2(tileX, tileY), bulletsAudio[0]);
                        selectedTower_radius = towerToAdd;
                        last_coup[2] = 2;
                        break;
                    }
                case "Fire Tower":
                    {
                        towerToAdd = new FireTower(towerTextures[3], bulletTexture, rangeTexture, new Vector2(tileX, tileY), bulletsAudio[0]);
                        selectedTower_radius = towerToAdd;
                        last_coup[2] = 3;
                        break;
                    }
            }



            // Only add the tower if there is a space and if the player can afford it.
            if (IsCellClear() == true && towerToAdd.Cost <= money)
            {
                towers.Add(towerToAdd);
                money -= towerToAdd.Cost;
                //modify_value = true;
                last_coup[0] = compteur_write+1;
                compteur_write++;
                last_coup[1] = 0;
                last_coup[3] = (int)towerToAdd.Position.X/32;
                last_coup[4] = (int)towerToAdd.Position.Y/32;
                last_coup[5] = 0;
                last_coup[6] = 0;
                // Reset the newTowerType field.
                newTowerType = string.Empty;
            }
        }


        public void AddTowerMulti(int type_tour, int coordX, int coordY)
        {
            Tower towerToAdd = null;
            float tileX = coordX * 32;
            float tileY = coordY * 32;
            cellX = (int)tileX;
            cellY = (int)tileY;

            switch (type_tour)
            {
                case 0:
                    {
                        towerToAdd = new ArrowTower(towerTextures[0], bulletTexture, rangeTexture, new Vector2(tileX, tileY), bulletsAudio[0]);
                        break;
                    }
                case 1:
                    {
                        towerToAdd = new SpikeTower(towerTextures[1], bulletTexture, rangeTexture, new Vector2(tileX, tileY), bulletsAudio[1]);
                        break;
                    }
                case 2:
                    {
                        towerToAdd = new SlowTower(towerTextures[2], bulletTexture, rangeTexture, new Vector2(tileX, tileY), bulletsAudio[0]);
                        break;
                    }
                case 3:
                    {
                        towerToAdd = new FireTower(towerTextures[3], bulletTexture, rangeTexture, new Vector2(tileX, tileY), bulletsAudio[0]);
                        break;
                    }
            }

            // Only add the tower if there is a space and if the player can afford it.
            if (towerToAdd.Cost <= money)
            {
                towers.Add(towerToAdd);
                money -= towerToAdd.Cost;
            }
        }

        public void RemoveTowerMulti(int type_tour, int coordX, int coordY)
        {
            //public List<Tower> towers = new List<Tower>();
            Tower towerToDelete = null;
            for (int i = 0; i < towers.Count; i++)
            {
                if (towers.ElementAt(i).Position.X/32 == coordX && towers.ElementAt(i).Position.Y/32 == coordY)
                {
                    towerToDelete = towers.ElementAt(i);
                }
            }
            if (towerToDelete != null)
            {
                money += (int)((float)towerToDelete.Cost * 0.75f);
                towers.Remove(towerToDelete);
            }
        }

        public void UpgradeMulti(int type_tour, int coordX, int coordY)
        {
            Tower towerToUpgrade = null;
            for (int i = 0; i < towers.Count; i++)
            {
                if (towers.ElementAt(i).Position.X/32 == coordX && towers.ElementAt(i).Position.Y/32 == coordY)
                {
                    towerToUpgrade = towers.ElementAt(i);
                }
            }

            if (towerToUpgrade != null)
            {
                Console.WriteLine("upgrade");
                switch (towerToUpgrade.Name)
                {
                    case "ArrowTower":
                        switch (towerToUpgrade.Level_tower)
                        {
                            case 1:
                                towerToUpgrade.Radius = towerToUpgrade.Radius + towerToUpgrade.Radius * 0.20f;
                                towerToUpgrade.Cost = towerToUpgrade.Cost + (int)(towerToUpgrade.Cost * 0.12f);
                                towerToUpgrade.Damage = towerToUpgrade.Damage + (int)(towerToUpgrade.Damage * 0.15f);
                                towerToUpgrade.Level_tower += 1;
                                this.money = money - 15;
                                break;
                            case 2:
                                towerToUpgrade.Radius = towerToUpgrade.Radius + towerToUpgrade.Radius * 0.20f;
                                towerToUpgrade.Cost = towerToUpgrade.Cost + (int)(towerToUpgrade.Cost * 0.12f);
                                towerToUpgrade.Damage = towerToUpgrade.Damage + (int)(towerToUpgrade.Damage * 0.15f);
                                towerToUpgrade.Level_tower += 1;
                                this.money = money - 15;
                                break;
                            case 3:
                                towerToUpgrade.Radius = towerToUpgrade.Radius + towerToUpgrade.Radius * 0.20f;
                                towerToUpgrade.Cost = towerToUpgrade.Cost + (int)(towerToUpgrade.Cost * 0.12f);
                                towerToUpgrade.Damage = towerToUpgrade.Damage + (int)(towerToUpgrade.Damage * 0.15f);
                                towerToUpgrade.Level_tower += 1;
                                if (towerToUpgrade.Level_tower == 3)
                                {
                                    this.money = money - 15;
                                }
                                break;
                        }
                        break;

                    case "SpikeTower":
                        switch (towerToUpgrade.Level_tower)
                        {
                            case 1:
                                towerToUpgrade.Radius = towerToUpgrade.Radius + towerToUpgrade.Radius * 0.20f;
                                towerToUpgrade.Cost = towerToUpgrade.Cost + (int)(towerToUpgrade.Cost * 0.12f);
                                towerToUpgrade.Damage = towerToUpgrade.Damage + (int)(towerToUpgrade.Damage * 0.15f);
                                towerToUpgrade.Level_tower += 1;
                                this.money = money - 15;
                                break;
                            case 2:
                                towerToUpgrade.Radius = towerToUpgrade.Radius + towerToUpgrade.Radius * 0.20f;
                                towerToUpgrade.Cost = towerToUpgrade.Cost + (int)(towerToUpgrade.Cost * 0.12f);
                                towerToUpgrade.Damage = towerToUpgrade.Damage + (int)(towerToUpgrade.Damage * 0.15f);
                                towerToUpgrade.Level_tower += 1;
                                this.money = money - 15;
                                break;
                            case 3:
                                towerToUpgrade.Radius = towerToUpgrade.Radius + towerToUpgrade.Radius * 0.20f;
                                towerToUpgrade.Cost = towerToUpgrade.Cost + (int)(towerToUpgrade.Cost * 0.12f);
                                towerToUpgrade.Damage = towerToUpgrade.Damage + (int)(towerToUpgrade.Damage * 0.15f);
                                towerToUpgrade.Level_tower += 1;
                                if (towerToUpgrade.Level_tower == 3)
                                {
                                    this.money = money - 15;
                                }
                                break;
                        }
                        break;

                    case "SlowTower":
                        switch (towerToUpgrade.Level_tower)
                        {
                            case 1:
                                towerToUpgrade.Radius = towerToUpgrade.Radius + towerToUpgrade.Radius * 0.20f;
                                towerToUpgrade.Cost = towerToUpgrade.Cost + (int)(towerToUpgrade.Cost * 0.12f);
                                towerToUpgrade.Damage = towerToUpgrade.Damage + (int)(towerToUpgrade.Damage * 0.15f);
                                towerToUpgrade.Level_tower += 1;
                                this.money = money - 15;
                                break;
                            case 2:
                                towerToUpgrade.Radius = towerToUpgrade.Radius + towerToUpgrade.Radius * 0.20f;
                                towerToUpgrade.Cost = towerToUpgrade.Cost + (int)(towerToUpgrade.Cost * 0.12f);
                                towerToUpgrade.Damage = towerToUpgrade.Damage + (int)(towerToUpgrade.Damage * 0.15f);
                                towerToUpgrade.Level_tower += 1;
                                this.money = money - 15;
                                break;
                            case 3:
                                towerToUpgrade.Radius = towerToUpgrade.Radius + towerToUpgrade.Radius * 0.20f;
                                towerToUpgrade.Cost = towerToUpgrade.Cost + (int)(towerToUpgrade.Cost * 0.12f);
                                towerToUpgrade.Damage = towerToUpgrade.Damage + (int)(towerToUpgrade.Damage * 0.15f);
                                towerToUpgrade.Level_tower += 1;
                                if (towerToUpgrade.Level_tower == 3)
                                {
                                    this.money = money - 15;
                                }
                                break;
                        }
                        break;

                    case "FireTower":
                        switch (towerToUpgrade.Level_tower)
                        {
                            case 1:
                                towerToUpgrade.Radius = towerToUpgrade.Radius + towerToUpgrade.Radius * 0.20f;
                                towerToUpgrade.Cost = towerToUpgrade.Cost + (int)(towerToUpgrade.Cost * 0.12f);
                                towerToUpgrade.Damage = towerToUpgrade.Damage + (int)(towerToUpgrade.Damage * 0.15f);
                                towerToUpgrade.Level_tower += 1;
                                this.money = money - 15;
                                break;
                            case 2:
                                towerToUpgrade.Radius = towerToUpgrade.Radius + towerToUpgrade.Radius * 0.20f;
                                towerToUpgrade.Cost = towerToUpgrade.Cost + (int)(towerToUpgrade.Cost * 0.12f);
                                towerToUpgrade.Damage = towerToUpgrade.Damage + (int)(towerToUpgrade.Damage * 0.15f);
                                towerToUpgrade.Level_tower += 1;
                                this.money = money - 15;
                                break;
                            case 3:
                                towerToUpgrade.Radius = towerToUpgrade.Radius + towerToUpgrade.Radius * 0.20f;
                                towerToUpgrade.Cost = towerToUpgrade.Cost + (int)(towerToUpgrade.Cost * 0.12f);
                                towerToUpgrade.Damage = towerToUpgrade.Damage + (int)(towerToUpgrade.Damage * 0.15f);
                                towerToUpgrade.Level_tower += 1;
                                if (towerToUpgrade.Level_tower == 3)
                                {
                                    this.money = money - 15;
                                }
                                break;
                        }
                        break;
                }

            }
        }

        public void RunWaveMulti()
        {
            last_coup[0] = compteur_write + 1;
            compteur_write++;
            last_coup[1] = 4;
            last_coup[3] = 0; 
            last_coup[4] = 0;
            last_coup[5] = 0;
            last_coup[6] = 0;
        }

        public void ReplaceTowerMulti(int type_tour, int coordX, int coordY, int NcoordX, int NcoordY) 
        {
            Tower towerToReplace = null;
            for (int i = 0; i < towers.Count; i++)
            {
                if (towers.ElementAt(i).Position.X / 32 == coordX && towers.ElementAt(i).Position.Y / 32 == coordY)
                {
                    towerToReplace = towers.ElementAt(i);
                }
            }

            Rectangle carr = new Rectangle(NcoordX * 32, NcoordX * 32, 32, 32);
            if (towerToReplace.Name == "SpikeTower")
            {
                towerToReplace.Position = new Vector2(NcoordX * 32 + (32 / 2), NcoordY * 32 + (32 / 2));
                towerToReplace.Bounds = carr;
                towerToReplace.Center = new Vector2(NcoordX * 32 + (32 / 2), NcoordY * 32 + (32 / 2));

                towerToReplace.Selected = false;
                towerToReplace = null;
            }
            else
            {
                towerToReplace.Position = new Vector2(NcoordX * 32, NcoordY * 32);
                towerToReplace.Bounds = carr;
                towerToReplace.Center = new Vector2(NcoordX * 32, NcoordY * 32);

                towerToReplace.Selected = false;
                towerToReplace = null;
            }
            money -= 10;
        }
    }
}



                   