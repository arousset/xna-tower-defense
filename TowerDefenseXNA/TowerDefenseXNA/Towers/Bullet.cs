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
    public class Bullet : Sprite
    {
        private int damage;
        private int age;    // life of a bullet ! ^^
        private int speed;

        public int Damage
        {
            get { return damage; }
        }

        public bool IsDead()
        {
            return age > 100;
        }
        
        // Constructor
        public Bullet(Texture2D texture, Vector2 position, float rotation, int speed, int damage) : base(texture, position)
        {
            this.rotation = rotation;
            this.damage = damage;
            this.speed = speed;
        }


        // Methods
        public void Kill() // a bullet
        {
            this.age = 200;
        }

        public override void Update(GameTime gameTime)
        {
            age++; // the life of a bullet is incremented
            position += velocity;

            base.Update(gameTime);
        }

        public void SetRotation(float value)
        {
            rotation = value;

            velocity = Vector2.Transform(new Vector2(0, -speed),
                Matrix.CreateRotationZ(rotation));
        }

    }
}
