#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
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
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        Map map;
        //Enemy enemy1;
        TowerDefenseXNA.Level lvl;
        Texture2D tiledMap;
        TowerDefenseXNA.Toolbar toolBar;
        TowerDefenseXNA.Player player;
        TowerDefenseXNA.Button arrowButton;
        TowerDefenseXNA.Button spikeButton;
        TowerDefenseXNA.Button slowButton;
        TowerDefenseXNA.Button sellButton;
        TowerDefenseXNA.Button startWaveButton;
        TowerDefenseXNA.WaveManager waveManager;
        Texture2D enemyTextureNormal;
        Texture2D enemyTextureFast;
        Texture2D healthTexture;
        bool debug = false;
        float pauseAlpha;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            tiledMap = content.Load<Texture2D>("Tilesmap");
            lvl = new TowerDefenseXNA.Level(tiledMap, 32);

            Texture2D[] towerTextures = new Texture2D[]
  	        {
  	                content.Load<Texture2D>("Towers/Arrow"),
   	                content.Load<Texture2D>("Towers/Spike"),
                    content.Load<Texture2D>("Towers/Slow")
   	        };

            Texture2D bulletTexture = content.Load<Texture2D>("Towers/bullet4");
            Texture2D rangeTexture = content.Load<Texture2D>("GUI/Range");
            player = new TowerDefenseXNA.Player(lvl, towerTextures, bulletTexture, rangeTexture);

            enemyTextureFast = content.Load<Texture2D>("Enemies/Fast");
            enemyTextureNormal = content.Load<Texture2D>("Enemies/Normal");
            healthTexture = content.Load<Texture2D>("Enemies/Health bar");
            waveManager = new TowerDefenseXNA.WaveManager(lvl, 5, enemyTextureNormal, enemyTextureFast, healthTexture, player);

            Texture2D topBar = content.Load<Texture2D>("GUI/Toolbar");
            SpriteFont font = content.Load<SpriteFont>("Arial");

            toolBar = new TowerDefenseXNA.Toolbar(topBar, font, new Vector2(0, lvl.Height * 32));
            ///////

            map = content.Load<Map>("Map");


            // Tower 


            Texture2D startWave = content.Load<Texture2D>("GUI/StartWave");

            // The "Normal" texture for the arrow button.
            Texture2D arrowNormal = content.Load<Texture2D>("GUI/Tower/Normal");
            // The "MouseOver" texture for the arrow button.
            Texture2D arrowHover = content.Load<Texture2D>("GUI/Tower/Over");
            // The "Pressed" texture for the arrow button.
            Texture2D arrowPressed = content.Load<Texture2D>("GUI/Tower/Pressed");

            Texture2D spikeNormal = content.Load<Texture2D>("GUI/Spike_Tower/SpikeNormal");
  	        // The "MouseOver" texture for the spike button.
 	        Texture2D spikeHover = content.Load<Texture2D>("GUI/Spike_Tower/SpikeOver");
  	        // The "Pressed" texture for the spike button.
  	        Texture2D spikePressed = content.Load<Texture2D>("GUI/Spike_Tower/SpikePressed");

            Texture2D slowNormal = content.Load<Texture2D>("GUI/Slow Tower/Normal");
            // The "MouseOver" texture for the spike button.
            Texture2D slowHover = content.Load<Texture2D>("GUI/Slow Tower/Mouse Over");
            // The "Pressed" texture for the spike button.
            Texture2D slowPressed = content.Load<Texture2D>("GUI/Slow Tower/Pressed");

            // The "Normal" texture for the sell button.
            Texture2D sellNormal = content.Load<Texture2D>("GUI/Sell Button/Normal");
            // The "MouseOver" texture for the sell button.
            Texture2D sellHover = content.Load<Texture2D>("GUI/Sell Button/Mouse Over");
            // The "Pressed" texture for the sell button.
            Texture2D sellPressed = content.Load<Texture2D>("GUI/Sell Button/Pressed");

            // Initialize the buttons.
            arrowButton = new TowerDefenseXNA.Button(arrowNormal, arrowHover, arrowPressed, new Vector2(0, lvl.Height * 32));
            spikeButton = new TowerDefenseXNA.Button(spikeNormal, spikeHover, spikePressed, new Vector2(32, lvl.Height * 32));
            slowButton = new TowerDefenseXNA.Button(slowNormal, slowHover, slowPressed, new Vector2(32*2, lvl.Height * 32));
            
            sellButton = new TowerDefenseXNA.Button(sellNormal, sellHover, sellPressed, new Vector2(32 * 15, lvl.Height * 32));
           
            startWaveButton = new TowerDefenseXNA.Button(startWave, startWave, startWave, new Vector2(lvl.Width * 32 - 32, lvl.Height * 32 + 5));
            startWaveButton.OnPress += new EventHandler(startButton_OnPress);
            
            arrowButton.OnPress += new EventHandler(arrowButton_OnPress);
            spikeButton.OnPress += new EventHandler(spikeButton_OnPress);
            slowButton.OnPress += new EventHandler(slowButton_OnPress);

            sellButton.OnPress += new EventHandler(player.SellButtonOnPress);
            //Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    debug = !debug;
                }

                

                waveManager.Update(gameTime);

                arrowButton.Update(gameTime);
                spikeButton.Update(gameTime);
                slowButton.Update(gameTime);
                startWaveButton.Update(gameTime);

                if (player.TowerSelected)
                {
                    sellButton.Update(gameTime);
                }

                player.Update(gameTime, waveManager.Enemies);
                if (player.Lives <= 0)
                    ScreenManager.AddScreen(new LooseMenuScreen(content), ControllingPlayer);
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(content), ControllingPlayer);
            }
            else
            {
                
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.CornflowerBlue, 0, 0);

            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

           

            lvl.Draw(spriteBatch);
            waveManager.Draw(spriteBatch);
            player.Draw(spriteBatch);
            player.DrawPreview(spriteBatch);
            toolBar.Draw(spriteBatch, player, waveManager);
            arrowButton.Draw(spriteBatch);
            spikeButton.Draw(spriteBatch);
            slowButton.Draw(spriteBatch);

            if (player.TowerSelected)
            {
                sellButton.Draw(spriteBatch);
            }

            if (waveManager.WaveReady)
                startWaveButton.Draw(spriteBatch);
            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        private void arrowButton_Clicked(object sender, EventArgs e)
        {
            player.NewTowerType = "Arrow Tower";
            player.NewTowerIndex = 0;
        }
        private void spikeButton_Clicked(object sender, EventArgs e)
        {
            player.NewTowerType = "Spike Tower";
            player.NewTowerIndex = 1;
        }
        private void slowButton_Clicked(object sender, EventArgs e)
        {
            player.NewTowerType = "Slow Tower";
            player.NewTowerIndex = 2;
        }

        private void arrowButton_OnPress(object sender, EventArgs e)
        {
            player.NewTowerType = "Arrow Tower";
            player.NewTowerIndex = 0;
        }

        private void spikeButton_OnPress(object sender, EventArgs e)
        {
            player.NewTowerType = "Spike Tower";
            player.NewTowerIndex = 1;
        }

        private void slowButton_OnPress(object sender, EventArgs e)
        {
            player.NewTowerType = "Slow Tower";
            player.NewTowerIndex = 2;
        }

        private void startButton_OnPress(object sender, EventArgs e)
        {
            waveManager.StartNextWave();
        }


        #endregion
    }
}
