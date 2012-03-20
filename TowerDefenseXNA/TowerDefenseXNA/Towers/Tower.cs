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
    public class Tower : Sprite
    {
        protected Texture2D bulletTexture;
        
        protected int cost; // Price to buy
        protected int damage; 
        protected float radius; // How far the tower can shoot
        protected Enemy target;

        protected float bulletTimer; // How long ago was a bullet fired
        protected List<Bullet> bulletList = new List<Bullet>();
        protected bool selected;
        protected Texture2D rangeTexture;

        public Enemy Target
        {
            get { return target; }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public int Cost
        {
            get { return cost; }
        }
        public int Damage
        {
            get { return damage; }
        }

        public float Radius
        {
            get { return radius; }
        }

        public virtual bool HasTarget
        {
            // Check if the tower has a target.
            get { return target != null; }
        }

        public Tower(Texture2D texture, Texture2D bulletTexture, Texture2D rangeTexture, Vector2 position) : base(texture, position)
        {
            this.bulletTexture = bulletTexture;
            this.rangeTexture = rangeTexture;
        }

        // Methods
        public bool IsInRange(Vector2 position)
        {
            return Vector2.Distance(center, position) <= radius;
        }

        public virtual void GetClosestEnemy(List<Enemy> enemies)
        {
            target = null;
            float smallestRange = radius;

            foreach (Enemy enemy in enemies)
            {
                if (Vector2.Distance(center, enemy.Center) < smallestRange)
                {
                    smallestRange = Vector2.Distance(center, enemy.Center);
                    target = enemy;
                }
            }
        }

        // A way to move in the direction of the enemy
        protected void FaceTarget()
        {
            Vector2 direction = center - target.Center;
            direction.Normalize();

            rotation = (float)Math.Atan2(-direction.X, direction.Y);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            bulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (target != null)
            {
                FaceTarget();

                if (!IsInRange(target.Center) || target.IsDead)
                {
                    target = null;
                    bulletTimer = 0;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (selected == true)
            {
                Vector2 radiusPosition = center - new Vector2(radius);

                Rectangle radiusRect = new Rectangle(
                    (int)radiusPosition.X,
                    (int)radiusPosition.Y,
                    (int)radius * 2,
                    (int)radius * 2);

                spriteBatch.Draw(rangeTexture, radiusRect, Color.White);
            }
            
            foreach (Bullet bullet in bulletList)
                bullet.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }



    }
}
