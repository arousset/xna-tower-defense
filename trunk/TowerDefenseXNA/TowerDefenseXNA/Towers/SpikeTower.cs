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
    public class SpikeTower : Tower
    {
        // A list of directions that the tower can shoot in.
        private Vector2[] directions = new Vector2[8];
        private List<Enemy> targets = new List<Enemy>();

        public override bool HasTarget
        {
            get { return false; }
        }


        // Constructors
        public SpikeTower(Texture2D texture, Texture2D bulletTexture, Vector2 position) : base(texture, bulletTexture, position) 
        {
            this.damage = 20; 
            this.cost = 40;   
            this.radius = 48; 

            // Store a list of all the directions the tower can shoot.
            directions = new Vector2[]
            {
               new Vector2(-1, -1), // North West
               new Vector2( 0, -1), // North
               new Vector2( 1, -1), // North East
               new Vector2(-1,  0), // West
               new Vector2( 1,  0), // East
               new Vector2(-1,  1), // South West
               new Vector2( 0,  1), // South
               new Vector2( 1,  1), // South East
            };
        }

        public override void GetClosestEnemy(List<Enemy> enemies)
        {
            // Do a fresh search for targets.
            targets.Clear();

            // Loop over all the enemies.
            foreach (Enemy enemy in enemies)
            {
                // Check wether this enemy is in shooting distance.
                if (IsInRange(enemy.Center))
                {
                    // Make it a target.
                    targets.Add(enemy);
                }
            }
        }


        public override void Update(GameTime gameTime)
        {
            bulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Decide if it is time to shoot.
            if (bulletTimer >= 1.0f && targets.Count != 0)
            {
                // For every direction the tower can shoot,
                for (int i = 0; i < directions.Length; i++)
                {
                    // create a new bullet that moves in that direction.
                    Bullet bullet = new Bullet(bulletTexture, Vector2.Subtract(center,
                        new Vector2(bulletTexture.Width / 2)), directions[i], 6, damage);

                    bulletList.Add(bullet);
                }

                bulletTimer = 0;
            }

            // Loop through all the bullets.
            for (int i = 0; i < bulletList.Count; i++)
            {
                Bullet bullet = bulletList[i];
                bullet.Update(gameTime);

                // Kill the bullet when it is out of range.
                if (!IsInRange(bullet.Center))
                {
                    bullet.Kill();
                }

                // Loop through all the possible targets
                for (int t = 0; t < targets.Count; t++)
                {
                    // If this bullet hits a target and is in range,
                    if (targets[t] != null && Vector2.Distance(bullet.Center, targets[t].Center) < 12)
                    {
                        // hurt the enemy.
                        targets[t].CurrentHealth -= bullet.Damage;
                        bullet.Kill();

                        // This bullet can't kill anyone else.
                        break;
                    }
                }

                // Remove the bullet if it is dead.
                if (bullet.IsDead())
                {
                    bulletList.Remove(bullet);
                    i--;
                }
            }

        }








    }
}
