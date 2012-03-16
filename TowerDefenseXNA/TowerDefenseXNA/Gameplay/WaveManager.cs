using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseXNA
{
    public class WaveManager
    {
        private int numberOfWaves; // How many waves the game will have
        private float timeSinceLastWave; // How long since the last wave ended
        private Queue<Wave> waves = new Queue<Wave>(); // A queue of all our waves
        //private Texture2D enemyTexture; // The texture used to draw the enemies
        private bool waveFinished = true; // Is the current wave over?
        private Level level; // A reference to our level class
        private bool firstWave = true;

        private int[] wave1 = { 1, 1, 1, 1, 1, 1 };
        private int[] wave2 = { 2, 1, 1, 2, 1, 1, 1 };
        private int[] wave3 = { 2, 1, 2, 1, 1, 2, 1, 1 };
        private int[] wave4 = { 2, 2, 1, 1, 2, 1, 1, 2, 1 };
        private int[] wave5 = { 2, 1, 2, 1, 2, 1, 2, 1, 2, 1 };
        

        public WaveManager(Level level, int numberOfWaves, Texture2D enemyTextureNormal,Texture2D enemyTextureFast, Texture2D healthTexture, Player player)
        {
            int[][] waveList = { wave1, wave2, wave3, wave4, wave5 };
            this.numberOfWaves = numberOfWaves;
            //this.enemyTexture = enemyTexture;
            this.level = level;

            for (int i = 0; i < numberOfWaves; i++)
            {
                /*int initialNumerOfEnemies = 6;
                int numberModifier = (i / 6) + 1;*/

                Wave wave; 
                wave = new Wave(i, 6+i, level, enemyTextureNormal, enemyTextureFast, healthTexture, waveList[i], player);
                waves.Enqueue(wave);
            }
        }

        public void StartNextWave()
        {
            if (waveFinished)
            {
                if (!firstWave)
                {
                    waves.Dequeue();
                }
                if (waves.Count > 0) // If there are still waves left
                {
                    waves.Peek().Start(); // Start the next one
                    timeSinceLastWave = 0; // Reset timer
                    waveFinished = false;
                    firstWave = false;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            CurrentWave.Update(gameTime); // Update the wave
            if (CurrentWave.RoundOver) // Check if it has finished
            {
                waveFinished = true;
            }
            if (waveFinished) // If it has finished
            {
                timeSinceLastWave += (float)gameTime.ElapsedGameTime.TotalSeconds; // Start the timer
            }
            
            /*if (timeSinceLastWave > 30.0f) // If 30 seconds has passed
            {
                waves.Dequeue(); // Remove the finished wave
                StartNextWave(); // Start the next wave
            }*/
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentWave.Draw(spriteBatch);
        }

        public Wave CurrentWave // Get the wave at the front of the queue
        {
            get { return waves.Peek(); }
        }


        public List<Enemy> Enemies // Get a list of the current enemeies
        {
            get { return CurrentWave.Enemies; }
        }


        public int Round // Returns the wave number
        {
            get { return CurrentWave.RoundNumber + 1; }
        }

        public int NbRounds
        {
            get { return numberOfWaves; }
        }

        public bool WaveReady
        {
            get
            { 
                if(waves.Count <= 0)
                    return false;
                return waveFinished;
            }
        }
    }
}
