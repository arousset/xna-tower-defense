using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseXNA
{
    public class Enemy : Sprite
    {
        protected float startHealth;
        protected float currentHealth;
        public bool alive = true;
        protected float speed = 0.5f;
        protected int bountyGiven;
        Player player;
        private Queue<Vector2> waypoints = new Queue<Vector2>();

        public Enemy(Texture2D texture, Vector2 position, float health, int bountyGiven, float speed, Player player) : base(texture, position)
        {
            this.startHealth = health;
            this.position = position;
            this.currentHealth = startHealth;
            this.speed = speed;
            this.bountyGiven = bountyGiven;
            this.player = player;
        }

        public void SetWaypoints(Queue<Vector2> waypoints)
        {
            foreach (Vector2 waypoint in waypoints)
                this.waypoints.Enqueue(waypoint);
            this.position = this.waypoints.Dequeue();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (currentHealth <= 0)
                alive = false;
            if (waypoints.Count > 0)
            {
                if (DistanceToDestination < speed)
                {
                    position = waypoints.Peek();
                    waypoints.Dequeue();
                }
                else
                {
                    Vector2 direction = waypoints.Peek() - position;
                    direction.Normalize();
                    velocity = Vector2.Multiply(direction, speed);
                    position += velocity;
                }
                if (currentHealth <= 0)
                {
                    alive = false;
                    player.giveGold(bountyGiven);
                }
            }
        }

        public float DistanceToDestination
        {
            get { return Vector2.Distance(position, waypoints.Peek()); }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                base.Draw(spriteBatch);
            }
        }

        public float HealthPercentage
        {
            get { return currentHealth / startHealth; }
        }

        public float CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        public bool IsDead
        {
            get { return !alive; }
        }

    }
}
