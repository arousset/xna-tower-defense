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
#if WINDOWS
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
#endif   
#endregion

namespace GameStateManagement
{
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        Map map;
        //Enemy enemy1;
        TowerDefenseXNA.Level lvl;
        Texture2D tiledMap;
        TowerDefenseXNA.Toolbar toolBar;
        TowerDefenseXNA.Healthbar healthbar;
        TowerDefenseXNA.Goldbar goldbar;
        TowerDefenseXNA.Player player;
        TowerDefenseXNA.Button arrowButton;
        TowerDefenseXNA.Button narrowButton;
        TowerDefenseXNA.Button spikeButton;
        TowerDefenseXNA.Button nspikeButton;
        TowerDefenseXNA.Button slowButton;
        TowerDefenseXNA.Button nslowButton;
        TowerDefenseXNA.Button fireButton;
        TowerDefenseXNA.Button nfireButton;
        TowerDefenseXNA.Button startWaveButton;
        TowerDefenseXNA.WaveManager waveManager;
        Texture2D enemyTextureNormal;
        Texture2D enemyTextureFast;
        Texture2D healthTexture;
        List<SoundEffectInstance> SoundEffectList;
        SoundEffect bulletSound;

        bool debug = false;
        float pauseAlpha;
        SpriteFont font;
        int levelNb;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen(int levelNb)
        {
            this.levelNb = levelNb;
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            tiledMap = content.Load<Texture2D>("Tilesmap");

            switch(levelNb)
            {
                case 1:
                    lvl = content.Load<TowerDefenseXNA.Level>("Levels/Level01");
                    break;
                case 2:
                    lvl = content.Load<TowerDefenseXNA.Level>("Levels/Level02");
                    break;
                case 3:
                    lvl = content.Load<TowerDefenseXNA.Level>("Levels/Level03");
                    break;
                case 4:
                    lvl = content.Load<TowerDefenseXNA.Level>("Levels/Level04");
                    break;
                case 5:
                    lvl = content.Load<TowerDefenseXNA.Level>("Levels/Level05");
                    break;
                default:
                    lvl = content.Load<TowerDefenseXNA.Level>("Levels/Level01");
                    break;
            }
            lvl.init();

            bulletSound = content.Load<SoundEffect>("Audio/bullet");
            SoundEffect bullet2Sound = content.Load<SoundEffect>("Audio/bullet2");

            SoundEffect[] bulletsAudio = new SoundEffect[]
            {
                bulletSound,
                bullet2Sound
            };

            SoundEffectList = new List<SoundEffectInstance>()
            {
                bulletSound.CreateInstance(),
                bullet2Sound.CreateInstance()
            };

            Texture2D[] towerTextures = new Texture2D[]
  	        {
  	                content.Load<Texture2D>("Towers/Arrow"),
   	                content.Load<Texture2D>("Towers/Spike"),
                    content.Load<Texture2D>("Towers/Slow"),
                    content.Load<Texture2D>("Towers/Fire")
   	        };

            Texture2D bulletTexture = content.Load<Texture2D>("Towers/bullet4");
            Texture2D rangeTexture = content.Load<Texture2D>("GUI/Range");

            Texture2D sellbt = content.Load<Texture2D>("GUI/sell_button");
            Texture2D upgradebt = content.Load<Texture2D>("GUI/upgrade_button");
            Texture2D replacebt = content.Load<Texture2D>("GUI/replace_button");

            Texture2D ranksilver = content.Load<Texture2D>("GUI/ranksilver");
            Texture2D rankgold = content.Load<Texture2D>("GUI/rankgold");

            player = new TowerDefenseXNA.Player(lvl, towerTextures, bulletTexture, rangeTexture, lvl.playerLife, lvl.playerMoney, bulletsAudio, sellbt, upgradebt, replacebt, font, ranksilver, rankgold);

            enemyTextureFast = content.Load<Texture2D>("Enemies/Fast");
            enemyTextureNormal = content.Load<Texture2D>("Enemies/Normal");
            healthTexture = content.Load<Texture2D>("Enemies/Health bar");
            waveManager = new TowerDefenseXNA.WaveManager(lvl, lvl.wavelist.Length, enemyTextureNormal, enemyTextureFast, healthTexture, player, lvl.wavelist);

            Texture2D topBar = content.Load<Texture2D>("GUI/Toolbar");
            font = content.Load<SpriteFont>("Arial");
            Texture2D healthinformations = content.Load<Texture2D>("GUI/Coeur");
            Texture2D goldinfos = content.Load<Texture2D>("GUI/gold_large");

            toolBar = new TowerDefenseXNA.Toolbar(topBar, font, new Vector2(lvl.Width+324, lvl.Height+433));
            healthbar = new TowerDefenseXNA.Healthbar(healthinformations, font, new Vector2(lvl.Width+860, lvl.Height-10));
            goldbar = new TowerDefenseXNA.Goldbar(goldinfos, font, new Vector2(lvl.Width+790, lvl.Height-17));
            ///////
           

            map = content.Load<Map>("Map");


            // Tower 
            Texture2D startWaveNormal = content.Load<Texture2D>("GUI/StartWave");
            Texture2D startWaveOver = content.Load<Texture2D>("GUI/StartWave_over");
            Texture2D startWavePressed = content.Load<Texture2D>("GUI/StartWave_pressed");


            // The "Normal" texture for the arrow button.
            Texture2D arrowNormal = content.Load<Texture2D>("GUI/Tower/Normal");
            // The "MouseOver" texture for the arrow button.
            Texture2D arrowHover = content.Load<Texture2D>("GUI/Tower/Over");
            // The "Pressed" texture for the arrow button.
            Texture2D arrowPressed = content.Load<Texture2D>("GUI/Tower/Pressed");
            // Not available 
            Texture2D arrow_not_available = content.Load < Texture2D>("GUI/Tower/blocked");

            Texture2D spikeNormal = content.Load<Texture2D>("GUI/Spike_Tower/SpikeNormal");
  	        // The "MouseOver" texture for the spike button.
 	        Texture2D spikeHover = content.Load<Texture2D>("GUI/Spike_Tower/SpikeOver");
  	        // The "Pressed" texture for the spike button.
  	        Texture2D spikePressed = content.Load<Texture2D>("GUI/Spike_Tower/SpikePressed");
            // Not available 
            Texture2D spike_not_available = content.Load<Texture2D>("GUI/Spike_Tower/blocked");

            Texture2D slowNormal = content.Load<Texture2D>("GUI/Slow Tower/Normal");
            // The "MouseOver" texture for the spike button.
            Texture2D slowHover = content.Load<Texture2D>("GUI/Slow Tower/Mouse Over");
            // The "Pressed" texture for the spike button.
            Texture2D slowPressed = content.Load<Texture2D>("GUI/Slow Tower/Pressed");
            // Not available 
            Texture2D slow_not_available = content.Load<Texture2D>("GUI/Slow Tower/blocked");

            // The "Normal" texture for the arrow button.
            Texture2D fireNormal = content.Load<Texture2D>("GUI/Fire Tower/Normal");
            // The "MouseOver" texture for the arrow button.
            Texture2D fireHover = content.Load<Texture2D>("GUI/Fire Tower/Mouse Over");
            // The "Pressed" texture for the arrow button.
            Texture2D firePressed = content.Load<Texture2D>("GUI/Fire Tower/Pressed");
            // Not available 
            Texture2D fire_not_available = content.Load<Texture2D>("GUI/Fire Tower/blocked");

            // The "Normal" texture for the sell button.
            Texture2D sellNormal = content.Load<Texture2D>("GUI/Sell Button/Normal");
            // The "MouseOver" texture for the sell button.
            Texture2D sellHover = content.Load<Texture2D>("GUI/Sell Button/Mouse Over");
            // The "Pressed" texture for the sell button.
            Texture2D sellPressed = content.Load<Texture2D>("GUI/Sell Button/Pressed");

            // Initialize the buttons.

            arrowButton = new TowerDefenseXNA.Button(arrowNormal, arrowHover, arrowPressed, new Vector2(355, lvl.Height * 32 - 32));
            narrowButton = new TowerDefenseXNA.Button(arrow_not_available, arrow_not_available, arrow_not_available, new Vector2(355, lvl.Height * 32 - 32));

            spikeButton = new TowerDefenseXNA.Button(spikeNormal, spikeHover, spikePressed, new Vector2(355+32, lvl.Height * 32 - 32));
            nspikeButton = new TowerDefenseXNA.Button(spike_not_available, spike_not_available, spike_not_available, new Vector2(355 + 32, lvl.Height * 32 - 32));
            
            slowButton = new TowerDefenseXNA.Button(slowNormal, slowHover, slowPressed, new Vector2(355+64, lvl.Height * 32 - 32));
            nslowButton = new TowerDefenseXNA.Button(slow_not_available, slow_not_available, slow_not_available, new Vector2(355 + 64, lvl.Height * 32 - 32));
            
            fireButton = new TowerDefenseXNA.Button(fireNormal, fireHover, firePressed, new Vector2(355+96, lvl.Height * 32 - 32));
            nfireButton = new TowerDefenseXNA.Button(fire_not_available, fire_not_available, fire_not_available, new Vector2(355 + 96, lvl.Height * 32 - 32));

            startWaveButton = new TowerDefenseXNA.Button(startWaveNormal, startWaveOver, startWavePressed, new Vector2(lvl.Width + 553, lvl.Height + 438));
            startWaveButton.OnPress += new EventHandler(startButton_OnPress);
            
            arrowButton.OnPress += new EventHandler(arrowButton_OnPress);
            spikeButton.OnPress += new EventHandler(spikeButton_OnPress);
            slowButton.OnPress += new EventHandler(slowButton_OnPress);
            fireButton.OnPress += new EventHandler(fireButton_OnPress);

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
            foreach (SoundEffectInstance SEI in SoundEffectList)
            {
                /*if (PauseMenuScreen.currentAudio == 0)
                    SEI.Volume = 1.0f;
                else
                    SEI.Volume = 0.0f;*/
            }

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
                    /*XmlWriterSettings xmlSettings = new XmlWriterSettings();
                    xmlSettings.Indent = true;
                    using (XmlWriter xmlWriter = XmlWriter.Create("Level01.xml", xmlSettings))
                    {
                        IntermediateSerializer.Serialize(xmlWriter, lvl, null);
                    }  */
                }

                waveManager.Update(gameTime);
                if (player.Money < 15) // See if it can be increase...
                    narrowButton.Update(gameTime);
                else
                    arrowButton.Update(gameTime);

                if (player.Money < 40)
                    nspikeButton.Update(gameTime);
                else
                    spikeButton.Update(gameTime);

                if (player.Money < 25)
                    nslowButton.Update(gameTime);
                else
                    slowButton.Update(gameTime);

                if (player.Money < 25)
                    nfireButton.Update(gameTime);
                else
                    fireButton.Update(gameTime);

                startWaveButton.Update(gameTime);

                if (player.TowerSelected)
                {
                    // on verra par la suite s'il faut mettre qqch
                }

                player.Update(gameTime, waveManager.Enemies);
                if (player.Lives <= 0)
                    ScreenManager.AddScreen(new LooseMenuScreen(content, levelNb), ControllingPlayer);
                if (waveManager.Round == waveManager.NbRounds && waveManager.WaveReady)
                    ScreenManager.AddScreen(new WinMenuScreen(content, levelNb), ControllingPlayer);
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

            lvl.Draw(spriteBatch, tiledMap);
            waveManager.Draw(spriteBatch);
            player.Draw(spriteBatch);
               
            player.DrawPreview(spriteBatch);
            toolBar.Draw(spriteBatch, player, waveManager);
            healthbar.Draw(spriteBatch, player);
            goldbar.Draw(spriteBatch, player);

            if (player.Money < 15)
                narrowButton.Draw(spriteBatch);
            else
                arrowButton.Draw(spriteBatch);

            if (player.Money < 40)
                nspikeButton.Draw(spriteBatch);
            else
                spikeButton.Draw(spriteBatch);

            if (player.Money < 25)
                nslowButton.Draw(spriteBatch);
            else
                slowButton.Draw(spriteBatch);

            if (player.Money < 25)
                nfireButton.Draw(spriteBatch);
            else
                fireButton.Draw(spriteBatch);
///////////////////////
            if (debug)
            {
                string text = string.Format("Level {0}", levelNb);
                spriteBatch.DrawString(font, text, new Vector2(10, 10), Color.White);
            }
////////////////
            if (player.TowerSelected)
            {
                //string text = string.Format("Level {0}", levelNb);
                //spriteBatch.DrawString(font, text, new Vector2(10, 10), Color.White);
            }
///////////////////////
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
        private void fireButton_Clicked(object sender, EventArgs e)
        {
            player.NewTowerType = "Fire Tower";
            player.NewTowerIndex = 3;
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

        private void fireButton_OnPress(object sender, EventArgs e)
        {
            player.NewTowerType = "Fire Tower";
            player.NewTowerIndex = 3;
        }

       

        private void startButton_OnPress(object sender, EventArgs e)
        {
            waveManager.StartNextWave();
        }


        #endregion
    }
}
