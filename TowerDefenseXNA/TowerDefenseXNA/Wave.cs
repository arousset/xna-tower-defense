﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseXNA
{
    class Wave
    {
        private int numOfEnemies; // Number of enemies to spawn
        private int waveNumber; // What wave is this?
        private bool started;
        private float spawnTimer = 0; // When should we spawn an enemy
        private int enemiesSpawned = 0; // How mant enemies have spawned
        private bool enemyAtEnd; // Has an enemy reached the end of the path?
        private bool spawningEnemies; // Are we still spawing enemies?
        private Level level; // A reference of the level
        private Texture2D enemyTexture; // A texture for the enemies
        private Texture2D healthTexture;
        public List<Enemy> enemies = new List<Enemy>(); // List of enemies

        public Wave(int waveNumber, int numOfEnemies, Level level, Texture2D enemyTexture, Texture2D healthTexture)
        {
            this.waveNumber = waveNumber;
            this.numOfEnemies = numOfEnemies;
            this.level = level;
            this.enemyTexture = enemyTexture;
            this.healthTexture = healthTexture;
        }

        private void AddEnemy()
        {

            Enemy enemy = new Enemy(enemyTexture,
            level.Waypoints.Peek(), 50, 1, 0.5f);
            enemy.SetWaypoints(level.Waypoints);
            enemies.Add(enemy);
            spawnTimer = 0;
            enemiesSpawned++;
        }

        public void Start()
        {
            spawningEnemies = true;
            started = true;
        }

        public void Update(GameTime gameTime)
        {
            if (enemiesSpawned == numOfEnemies)
                spawningEnemies = false; // We have spawned enough enemies
            if (spawningEnemies)
            {
                spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (spawnTimer > 2)
                    AddEnemy(); // Time to add a new enemey
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];
                enemy.Update(gameTime);
                if (enemy.IsDead)
                {
                    if (enemy.CurrentHealth > 0) // Enemy is at the end
                    {
                        enemyAtEnd = true;
                    }
                    enemies.Remove(enemy);
                    i--;
                }
            }
        }

        public bool RoundOver
        {
            get
            {
                return enemies.Count == 0 && enemiesSpawned == numOfEnemies;
            }
        }

        public int RoundNumber
        {
            get { return waveNumber; }
        }

        public bool Started
        {
            get { return started; }
        }

        public bool EnemyAtEnd
        {
            get { return enemyAtEnd; }
            set { enemyAtEnd = value; }
        }

        public List<Enemy> Enemies
        {
            get { return enemies; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
                float healthPercentage = enemy.HealthPercentage;
                float visibleWidth = (float)healthTexture.Width * healthPercentage;
                Rectangle healthRectangle = new Rectangle((int)enemy.Position.X,
                                         (int)enemy.Position.Y-5,
                                         (int)visibleWidth,
                                         healthTexture.Height);

                float red = (healthPercentage < 0.5 ? 1 : 1 - (2 * healthPercentage - 1));
                float green = (healthPercentage > 0.5 ? 1 : (2 * healthPercentage));

                Color healthColor = new Color(red, green, 0);

                spriteBatch.Draw(healthTexture, healthRectangle, healthColor);
            }
        }
    }
}