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
        //Enemy enemy1;
        Level lvl;
        Texture2D tiledMap;
        Toolbar toolBar;
        Player player;
        Button arrowButton;
        Button startWaveButton;
        WaveManager waveManager;
        Texture2D enemyTextureNormal;
        Texture2D enemyTextureFast;
        Texture2D healthTexture;
        bool debug = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.graphics.IsFullScreen = false;
            this.graphics.PreferredBackBufferWidth = 1280; // 960
            this.graphics.PreferredBackBufferHeight = 720; // 720

            tiledMap = Content.Load<Texture2D>("Tilesmap");
            lvl = new Level(tiledMap, 32);
            graphics.PreferredBackBufferWidth = lvl.Width * 32;
            graphics.PreferredBackBufferHeight = 32 + lvl.Height * 32;

            this.graphics.ApplyChanges();
            this.Window.Title = "Xna Tower Defense";
            IsMouseVisible = true;
            this.Window.AllowUserResizing = false;

            Texture2D towerTexture = Content.Load<Texture2D>("Towers/Arrow");
            Texture2D bulletTexture = Content.Load<Texture2D>("Towers/bullet4");
            player = new Player(lvl, towerTexture, bulletTexture);

            enemyTextureFast = Content.Load<Texture2D>("Enemies/Fast");
            enemyTextureNormal = Content.Load<Texture2D>("Enemies/Normal");
            healthTexture = Content.Load<Texture2D>("Enemies/Health bar");
            waveManager = new WaveManager(lvl, 5, enemyTextureNormal, enemyTextureFast, healthTexture, player);

            Texture2D topBar = Content.Load<Texture2D>("GUI/Toolbar");
            SpriteFont font = Content.Load<SpriteFont>("Arial");

            toolBar = new Toolbar(topBar, font, new Vector2(0, lvl.Height * 32));

            base.Initialize();
        }

        protected override void LoadContent()
       {       
            spriteBatch = new SpriteBatch(GraphicsDevice);
            map = Content.Load<Map>("Map");
            
           
            // Tower 
            

            Texture2D startWave = Content.Load<Texture2D>("GUI/StartWave");


            // The "Normal" texture for the arrow button.
            Texture2D arrowNormal = Content.Load<Texture2D>("GUI/Tower/Normal");
            // The "MouseOver" texture for the arrow button.
            Texture2D arrowHover = Content.Load<Texture2D>("GUI/Tower/Over");
            // The "Pressed" texture for the arrow button.
            Texture2D arrowPressed = Content.Load<Texture2D>("GUI/Tower/Pressed");
            

            // Initialize the buttons.
            arrowButton = new Button(arrowNormal, arrowHover, arrowPressed, new Vector2(0, lvl.Height * 32));
            startWaveButton = new Button(startWave, startWave, startWave, new Vector2(lvl.Width*32 - 32, lvl.Height * 32+5));

            startWaveButton.Clicked += new EventHandler(startButton_Clicked);
            arrowButton.Clicked += new EventHandler(arrowButton_Clicked);
            
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                debug = !debug;
            }    

            waveManager.Update(gameTime);
            
            arrowButton.Update(gameTime);
            startWaveButton.Update(gameTime);

            player.Update(gameTime, waveManager.Enemies);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            lvl.Draw(spriteBatch);
            waveManager.Draw(spriteBatch);
            player.Draw(spriteBatch);
            toolBar.Draw(spriteBatch, player, waveManager);
            arrowButton.Draw(spriteBatch);
            if(waveManager.WaveReady)
                startWaveButton.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void arrowButton_Clicked(object sender, EventArgs e)
        {
            player.NewTowerType = "Arrow Tower";
        }

        private void startButton_Clicked(object sender, EventArgs e)
        {
            waveManager.StartNextWave();
        }
    }
}
