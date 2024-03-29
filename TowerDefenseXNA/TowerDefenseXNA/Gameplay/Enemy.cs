﻿using System;
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
        private float speedModifier;
        private float modifierDuration;
        private float modiferCurrentTime;

        private float burn_Modifier;
        private float burn_Duration;
        private float burn_CurrentTime;

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
            this.burn_Modifier = 5.0f;
        }

        public float Burne
        {
            get { return burn_Modifier; }
            set { burn_Modifier = value; }
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

                    // Store the original speed.
                    float temporarySpeed = speed;

                    // If the modifier has finished,
                    if (modiferCurrentTime > modifierDuration)
                    {
                        // reset the modifier.
                        speedModifier = 0;
                        modiferCurrentTime = 0;
                    }

                    if (speedModifier != 0 && modiferCurrentTime <= modifierDuration)
                    {
                        // Modify the speed of the enemy.
                        temporarySpeed *= speedModifier;
                        // Update the modifier timer.
                        modiferCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }

                    if (burn_Modifier != 0 && burn_CurrentTime <= burn_Duration)
                    {
                        // Modify the speed of the enemy.
                        currentHealth -= burn_Modifier;
                        // Update the modifier timer.
                        burn_CurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    }

                    velocity = Vector2.Multiply(direction, temporarySpeed);

                    position += velocity;
                }
                if (currentHealth <= 0)
                {
                    alive = false;
                    player.giveGold(bountyGiven);
                }
            }
            else
            {
                alive = false;
            }

        }

        public int BountyGiven
        {
            get { return bountyGiven; }
        }

        public float SpeedModifier
        {
            get { return speedModifier; }
            set { speedModifier = value; }
        }

        public float ModifierDuration
        {
            get { return modifierDuration; }
            set
            {
                modifierDuration = value;
                modiferCurrentTime = 0;
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
