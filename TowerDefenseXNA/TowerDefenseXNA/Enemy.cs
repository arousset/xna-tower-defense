using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseXNA
{
    class Enemy : Sprite
    {
        protected float startHealth;
        protected float currentHealth;
        protected bool alive = true;
        protected float speed = 0.5f;
        protected int bountyGiven;

        public Enemy(Texture2D texture, Vector2 position, float health, int bountyGiven, float speed) : base(texture, position)
        {
            this.startHealth = health;
            this.currentHealth = startHealth;
            this.speed = speed;
            this.bountyGiven = bountyGiven;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (currentHealth <= 0)
                alive = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                float healthPercentage = (float)currentHealth / (float)startHealth;
                Color color = new Color(new Vector3(1 - healthPercentage, 1 - healthPercentage, 1 - healthPercentage));
                base.Draw(spriteBatch, color);
            }
        }

        public float CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        public bool IsDead
        {
            get { return currentHealth <= 0; }
        }

    }
}
