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
using Microsoft.Xna.Framework.Net;
using TiledLib;
#if WINDOWS
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
#endif   


namespace GameStateManagement 
{
    class GameplayScreenMulti : GameScreen 
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
        NetworkSession networkSession;
        KeyboardState currentKeyboardState;
        GamePadState currentGamePadState;
        Texture2D[] towerTextures;
        Texture2D rankgold;
        Texture2D ranksilver;
        Texture2D replacebt;
        Texture2D upgradebt;
        Texture2D sellbt;
        Texture2D rangeTexture;
        Texture2D bulletTexture;
        SoundEffect[] bulletsAudio;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        GameComponentCollection Components;
        Game game;

        PacketWriter packetWriter = new PacketWriter();
        PacketReader packetReader = new PacketReader();
        
        string errorMessage;

        bool debug = false;
        float pauseAlpha;
        int levelNb;

        const int maxGamers = 2;
        const int maxLocalGamers = 2;
        

        #endregion



        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreenMulti(int levelNb, GameComponentCollection Components, Game game)
        {
            
            this.levelNb = levelNb;
            this.Components = Components;
            this.game = game;
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
           // ScreenManager.Game.Content.RootDirectory = "Content";
           Components.Add(new GamerServicesComponent(game));
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");


            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            tiledMap = content.Load<Texture2D>("Tilesmap");

            switch (levelNb)
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

            bulletsAudio = new SoundEffect[]
            {
                bulletSound,
                bullet2Sound
            };

            SoundEffectList = new List<SoundEffectInstance>()
            {
                bulletSound.CreateInstance(),
                bullet2Sound.CreateInstance()
            };

            towerTextures = new Texture2D[]
  	        {
  	                content.Load<Texture2D>("Towers/Arrow"),
   	                content.Load<Texture2D>("Towers/Spike"),
                    content.Load<Texture2D>("Towers/Slow"),
                    content.Load<Texture2D>("Towers/Fire")
   	        };

            bulletTexture = content.Load<Texture2D>("Towers/bullet4");
            rangeTexture = content.Load<Texture2D>("GUI/Range");

            sellbt = content.Load<Texture2D>("GUI/sell_button");
            upgradebt = content.Load<Texture2D>("GUI/upgrade_button");
            replacebt = content.Load<Texture2D>("GUI/replace_button");

            ranksilver = content.Load<Texture2D>("GUI/ranksilver");
            rankgold = content.Load<Texture2D>("GUI/rankgold");

            //player = new TowerDefenseXNA.Player(lvl, towerTextures, bulletTexture, rangeTexture, lvl.playerLife, lvl.playerMoney, bulletsAudio, sellbt, upgradebt, replacebt, font, ranksilver, rankgold);

            enemyTextureFast = content.Load<Texture2D>("Enemies/Fast");
            enemyTextureNormal = content.Load<Texture2D>("Enemies/Normal");
            healthTexture = content.Load<Texture2D>("Enemies/Health bar");
            waveManager = new TowerDefenseXNA.WaveManager(lvl, lvl.wavelist.Length, enemyTextureNormal, enemyTextureFast, healthTexture, player, lvl.wavelist);

            Texture2D topBar = content.Load<Texture2D>("GUI/Toolbar");
            font = content.Load<SpriteFont>("Arial");
            Texture2D healthinformations = content.Load<Texture2D>("GUI/Coeur");
            Texture2D goldinfos = content.Load<Texture2D>("GUI/gold_large");

            toolBar = new TowerDefenseXNA.Toolbar(topBar, font, new Vector2(lvl.Width + 324, lvl.Height + 433));
            healthbar = new TowerDefenseXNA.Healthbar(healthinformations, font, new Vector2(lvl.Width + 860, lvl.Height - 10));
            goldbar = new TowerDefenseXNA.Goldbar(goldinfos, font, new Vector2(lvl.Width + 790, lvl.Height - 17));
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
            Texture2D arrow_not_available = content.Load<Texture2D>("GUI/Tower/blocked");

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

            spikeButton = new TowerDefenseXNA.Button(spikeNormal, spikeHover, spikePressed, new Vector2(355 + 32, lvl.Height * 32 - 32));
            nspikeButton = new TowerDefenseXNA.Button(spike_not_available, spike_not_available, spike_not_available, new Vector2(355 + 32, lvl.Height * 32 - 32));

            slowButton = new TowerDefenseXNA.Button(slowNormal, slowHover, slowPressed, new Vector2(355 + 64, lvl.Height * 32 - 32));
            nslowButton = new TowerDefenseXNA.Button(slow_not_available, slow_not_available, slow_not_available, new Vector2(355 + 64, lvl.Height * 32 - 32));

            fireButton = new TowerDefenseXNA.Button(fireNormal, fireHover, firePressed, new Vector2(355 + 96, lvl.Height * 32 - 32));
            nfireButton = new TowerDefenseXNA.Button(fire_not_available, fire_not_available, fire_not_available, new Vector2(355 + 96, lvl.Height * 32 - 32));

            startWaveButton = new TowerDefenseXNA.Button(startWaveNormal, startWaveOver, startWavePressed, new Vector2(lvl.Width + 553, lvl.Height + 438));
            startWaveButton.OnPress += new EventHandler(startButton_OnPress);

            arrowButton.OnPress += new EventHandler(arrowButton_OnPress);
            spikeButton.OnPress += new EventHandler(spikeButton_OnPress);
            slowButton.OnPress += new EventHandler(slowButton_OnPress);
            fireButton.OnPress += new EventHandler(fireButton_OnPress);


            font = content.Load<SpriteFont>("Arial");

            //Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            //ScreenManager.Game.ResetElapsedTime();
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) 
        {
            HandleInput();
 
            if (networkSession == null)
            {
                // If we are not in a network session, update the
                // menu screen that will let us create or join one.
                UpdateMenuScreen();
            }
            else
            {
                // If we are in a network session, update it.
                UpdateNetworkSession();
            }

            base.Update(gameTime, false, false); 
        }


        void UpdateMenuScreen()
        {
            Console.WriteLine(IsActive);
            
            if (IsActive)
            {
                if (Gamer.SignedInGamers.Count == 0)
                {
                    // If there are no profiles signed in, we cannot proceed.
                    // Show the Guide so the user can sign in.
                   //Guide.ShowSignIn(maxLocalGamers, false);
                }
                else if (IsPressed(Keys.A, Buttons.A))
                {
                    // Create a new session?
                    CreateSession();
                }
                else if (IsPressed(Keys.B, Buttons.B))
                {
                    // Join an existing session?
                    JoinSession();
                }
            }
        }

        void CreateSession()
        {
            DrawMessage("Creating session...");

            try
            {
                networkSession = NetworkSession.Create(NetworkSessionType.SystemLink, maxLocalGamers, maxGamers);

                HookSessionEvents();
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
        }


        /// <summary>
        /// Joins an existing network session.
        /// </summary>
        void JoinSession()
        {
            DrawMessage("Joining session...");

            try
            {
                // Search for sessions.
                using (AvailableNetworkSessionCollection availableSessions =
                            NetworkSession.Find(NetworkSessionType.SystemLink,
                                                maxLocalGamers, null))
                {
                    if (availableSessions.Count == 0)
                    {
                        errorMessage = "No network sessions found.";
                        return;
                    }

                    // Join the first session we found.
                    networkSession = NetworkSession.Join(availableSessions[0]);

                    HookSessionEvents();
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
        }


        void GamerJoinedEventHandler(object sender, GamerJoinedEventArgs e)
        {
            int gamerIndex = networkSession.AllGamers.IndexOf(e.Gamer);

            e.Gamer.Tag = new TowerDefenseXNA.Player(lvl, towerTextures, bulletTexture, rangeTexture, lvl.playerLife, lvl.playerMoney, bulletsAudio, sellbt, upgradebt, replacebt, font, ranksilver, rankgold);//Tank(gamerIndex, Content, 960, 720);
            //e.Gamer.Tag = new TowerDefenseXNA.Tank(gamerIndex, content, 1067, 600);
        }


        /// <summary>
        /// Event handler notifies us when the network session has ended.
        /// </summary>
        void SessionEndedEventHandler(object sender, NetworkSessionEndedEventArgs e)
        {
            errorMessage = e.EndReason.ToString();

            networkSession.Dispose();
            networkSession = null;
        }

        /// <summary>
        /// After creating or joining a network session, we must subscribe to
        /// some events so we will be notified when the session changes state.
        /// </summary>
        void HookSessionEvents()
        {
            networkSession.GamerJoined += GamerJoinedEventHandler;
            networkSession.SessionEnded += SessionEndedEventHandler;
        }


        void UpdateNetworkSession()
        {
            // Read inputs for locally controlled tanks, and send them to the server.
            foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
            {
                UpdateLocalGamer(gamer);
            }

            // If we are the server, update all the tanks and transmit
            // their latest positions back out over the network.
            if (networkSession.IsHost)
            {
                UpdateServer();
            }

            // Pump the underlying session object.
            networkSession.Update();

            // Make sure the session has not ended.
            if (networkSession == null)
                return;

            // Read any incoming network packets.
            foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
            {
                if (gamer.IsHost)
                {
                    ServerReadInputFromClients(gamer);
                }
                else
                {
                    ClientReadGameStateFromServer(gamer);
                }
            }
        }


        void UpdateLocalGamer(LocalNetworkGamer gamer)
        {
            // Look up what tank is associated with this local player,
            // and read the latest user inputs for it. The server will
            // later use these values to control the tank movement.
    //TowerDefenseXNA.Player localplayer = gamer.Tag as TowerDefenseXNA.Player;
            TowerDefenseXNA.Tank localplayer = gamer.Tag as TowerDefenseXNA.Tank;
            ReadTankInputs(localplayer, gamer.SignedInGamer.PlayerIndex);

            // Only send if we are not the server. There is no point sending packets
            // to ourselves, because we already know what they will contain!
            if (!networkSession.IsHost)
            {
                // Write our latest input state into a network packet.
                packetWriter.Write(localplayer.TankInput);
                packetWriter.Write(localplayer.TurretInput);

                // Send our input data to the server.
                gamer.SendData(packetWriter, SendDataOptions.InOrder, networkSession.Host);
            }
        }


        void UpdateServer()
        {
            // Loop over all the players in the session, not just the local ones!
            foreach (NetworkGamer gamer in networkSession.AllGamers)
            {
                // Look up what tank is associated with this player.
      TowerDefenseXNA.Player player = gamer.Tag as TowerDefenseXNA.Player;
      //          TowerDefenseXNA.Tank player = gamer.Tag as TowerDefenseXNA.Tank;
                // Update the tank.
      // TowerDefenseXNA.Player.Update();
      //player.Update(gameTime);

                // Write the tank state into the output network packet.
                packetWriter.Write(gamer.Id);
               /* packetWriter.Write(player.Position);
                packetWriter.Write(player.TankRotation);
                packetWriter.Write(player.TurretRotation);*/
            }

            // Send the combined data for all tanks to everyone in the session.
            LocalNetworkGamer server = (LocalNetworkGamer)networkSession.Host;

            server.SendData(packetWriter, SendDataOptions.InOrder);
        }


        void ServerReadInputFromClients(LocalNetworkGamer gamer)
        {
            // Keep reading as long as incoming packets are available.
            while (gamer.IsDataAvailable)
            {
                NetworkGamer sender;

                // Read a single packet from the network.
                gamer.ReceiveData(packetReader, out sender);

                if (!sender.IsLocal)
                {
                    // Look up the tank associated with whoever sent this packet.
      //TowerDefenseXNA.Player remoteplayer = sender.Tag as TowerDefenseXNA.Player;
                    TowerDefenseXNA.Tank remoteplayer = sender.Tag as TowerDefenseXNA.Tank;
                    // Read the latest inputs controlling this tank.
                    remoteplayer.TankInput = packetReader.ReadVector2();
                    remoteplayer.TurretInput = packetReader.ReadVector2();
                }
            }
        }

        void ClientReadGameStateFromServer(LocalNetworkGamer gamer)
        {
            // Keep reading as long as incoming packets are available.
            while (gamer.IsDataAvailable)
            {
                NetworkGamer sender;

                // Read a single packet from the network.
                gamer.ReceiveData(packetReader, out sender);

                // This packet contains data about all the players in the session.
                // We keep reading from it until we have processed all the data.
                while (packetReader.Position < packetReader.Length)
                {
                    // Read the state of one tank from the network packet.
                    byte gamerId = packetReader.ReadByte();
                    Vector2 position = packetReader.ReadVector2();
                    float tankRotation = packetReader.ReadSingle();
                    float turretRotation = packetReader.ReadSingle();

                    // Look up which gamer this state refers to.
                    NetworkGamer remoteGamer = networkSession.FindGamerById(gamerId);

                    // This might come back null if the gamer left the session after
                    // the host sent the packet but before we received it. If that
                    // happens, we just ignore the data for this gamer.
                    if (remoteGamer != null)
                    {
                        // Update our local state with data from the network packet.
        //TowerDefenseXNA.Player player = remoteGamer.Tag as TowerDefenseXNA.Player;
                        TowerDefenseXNA.Tank player = remoteGamer.Tag as TowerDefenseXNA.Tank;
                        player.Position = position;
                        player.TankRotation = tankRotation;
                        player.TurretRotation = turretRotation; 
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch = ScreenManager.SpriteBatch;
            if (networkSession == null)
            {
                // If we are not in a network session, draw the
                // menu screen that will let us create or join one.
                DrawMenuScreen();
            }
            else
            {
                // If we are in a network session, draw it.
                DrawNetworkSession();
            }

            base.Draw(gameTime);
        }

        void DrawMenuScreen()
        {
            string message = string.Empty;
            
            if (!string.IsNullOrEmpty(errorMessage))
                message += "Error:\n" + errorMessage.Replace(". ", ".\n") + "\n\n";

            message += "A = create session\n" +
                       "B = join session";

            spriteBatch.Begin();

            spriteBatch.DrawString(font, message, new Vector2(161, 161), Color.Black);
            spriteBatch.DrawString(font, message, new Vector2(160, 160), Color.White);

            spriteBatch.End();
        }


        void DrawNetworkSession()
        {
            spriteBatch.Begin();

            // For each person in the session...
            foreach (NetworkGamer gamer in networkSession.AllGamers)
            {
                // Look up the tank object belonging to this network gamer.
     TowerDefenseXNA.Player player = gamer.Tag as TowerDefenseXNA.Player;
     //           TowerDefenseXNA.Tank player = gamer.Tag as TowerDefenseXNA.Tank;
                // Draw the tank.
                player.Draw(spriteBatch);

                // Draw a gamertag label.
                string label = gamer.Gamertag;
                Color labelColor = Color.Black;
                Vector2 labelOffset = new Vector2(100, 150);

                if (gamer.IsHost)
                    label += " (server)";

                // Flash the gamertag to yellow when the player is talking.
                if (gamer.IsTalking)
                    labelColor = Color.Yellow;

                spriteBatch.DrawString(font, label, new Vector2(10,10), labelColor, 0, labelOffset, 0.6f, SpriteEffects.None, 0);
            }

            spriteBatch.End();
        }

        void DrawMessage(string message)
        {
           // if (!BeginDraw())
             //   return;

            //GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.DrawString(font, message, new Vector2(161, 161), Color.Black);
            spriteBatch.DrawString(font, message, new Vector2(160, 160), Color.White);

            spriteBatch.End();

           // EndDraw();
        }

        private void HandleInput()
        {
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            // Check for exit.
            if (IsActive && IsPressed(Keys.Escape, Buttons.Back))
            {
              //  Exit();
            }
        }

        bool IsPressed(Keys key, Buttons button)
        {
            return (currentKeyboardState.IsKeyDown(key) ||
                    currentGamePadState.IsButtonDown(button));
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

        //void ReadTankInputs(TowerDefenseXNA.Player player, PlayerIndex playerIndex)
         void ReadTankInputs(TowerDefenseXNA.Tank player, PlayerIndex playerIndex)
        {
            // Read the gamepad.
            GamePadState gamePad = GamePad.GetState(playerIndex);

            Vector2 tankInput = gamePad.ThumbSticks.Left;
            Vector2 turretInput = gamePad.ThumbSticks.Right;

            // Read the keyboard.
            KeyboardState keyboard = Keyboard.GetState(playerIndex);

            if (keyboard.IsKeyDown(Keys.Left))
                tankInput.X = -1;
            else if (keyboard.IsKeyDown(Keys.Right))
                tankInput.X = 1;

            if (keyboard.IsKeyDown(Keys.Up))
                tankInput.Y = 1;
            else if (keyboard.IsKeyDown(Keys.Down))
                tankInput.Y = -1;

            if (keyboard.IsKeyDown(Keys.A))
                turretInput.X = -1;
            else if (keyboard.IsKeyDown(Keys.D))
                turretInput.X = 1;

            if (keyboard.IsKeyDown(Keys.W))
                turretInput.Y = 1;
            else if (keyboard.IsKeyDown(Keys.S))
                turretInput.Y = -1;

            // Normalize the input vectors.
            if (tankInput.Length() > 1)
                tankInput.Normalize();

            if (turretInput.Length() > 1)
                turretInput.Normalize();

            // Store these input values into the tank object.
             // player.TankInput = tankInput;
             // player.TurretInput = turretInput;
        }


    }
}
