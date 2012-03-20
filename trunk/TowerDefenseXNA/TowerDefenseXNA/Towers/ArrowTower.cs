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
    public class ArrowTower : Tower
    {
        
        // Constructor
        public ArrowTower(Texture2D texture, Texture2D bulletTexture, Texture2D rangeTexture, Vector2 position)
            : base(texture, bulletTexture, rangeTexture, position)
        {
            this.damage = 15; // Set the damage
            this.cost = 15;   // Set the initial cost
            this.radius = 80; // Set the radius
        }


        // Methods
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (bulletTimer >= 1.00f && target != null)
            {
                Bullet bullet = new Bullet(bulletTexture, Vector2.Subtract(center, new Vector2(bulletTexture.Width / 2)), rotation, 3, damage);

                bulletList.Add(bullet);
                bulletTimer = 0;
            }

            for (int i = 0; i < bulletList.Count; i++)
            {
                Bullet bullet = bulletList[i];

                bullet.SetRotation(rotation);
                bullet.Update(gameTime);

                if (!IsInRange(bullet.Center))
                    bullet.Kill();

                if (target != null && Vector2.Distance(bullet.Center, target.Center) < 12)
                {
                    target.CurrentHealth -= bullet.Damage;
                    bullet.Kill();
                }

                if (bullet.IsDead())
                {
                    bulletList.Remove(bullet);
                    i--;
                }
            }
        }

    }
}
