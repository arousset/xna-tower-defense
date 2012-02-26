using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TiledLib;

namespace TowerDefenseXNA
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map map;
        Enemy enemy1;
        Level lvl;
        Texture2D tiledMap;
        //Tower tower;
        Player player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.graphics.IsFullScreen = false;
            this.graphics.PreferredBackBufferWidth = 960;
            this.graphics.PreferredBackBufferHeight = 480;
            this.graphics.ApplyChanges();
            this.Window.Title = "Xna Tower Defense";
            IsMouseVisible = true;
            this.Window.AllowUserResizing = false;
            base.Initialize();
        }

        protected override void LoadContent()
       { 
            spriteBatch = new SpriteBatch(GraphicsDevice);
            map = Content.Load<Map>("Map");
            
            // Enemy
            Texture2D enemyTexture = Content.Load<Texture2D>("Enemies/Normal");
            tiledMap = Content.Load<Texture2D>("Tilesmap");
            lvl = new Level(tiledMap, 32);
            enemy1 = new Enemy(enemyTexture, Vector2.Zero, 100, 10, 0.5f);
            enemy1.SetWaypoints(lvl.Waypoints);
            // Tower 
            Texture2D towerTexture = Content.Load<Texture2D>("Towers/Arrow");
            Texture2D bulletTexture = Content.Load<Texture2D>("Towers/bullet4");

            player = new Player(lvl, towerTexture, bulletTexture); // Create a new player (in future multiplayer ;)
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
           // enemy1.CurrentHealth -= 1;
            enemy1.Update(gameTime);


            List<Enemy> enemies = new List<Enemy>();
            enemies.Add(enemy1);

            player.Update(gameTime, enemies);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            lvl.Draw(spriteBatch);
            enemy1.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
