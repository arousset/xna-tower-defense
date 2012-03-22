using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseXNA
{
    public class Goldbar
    {
        private Texture2D texture;
        // A class to access the font we created
        private SpriteFont font;

        // The position of the toolbar
        private Vector2 position;
        // The position of the text
        private Vector2 textPosition;

        // Constructor
        public Goldbar(Texture2D texture, SpriteFont font, Vector2 position)
        {
            this.texture = texture;
            this.font = font;

            this.position = position;
            // Offset the text to the bottom right corner
            textPosition = new Vector2(845, position.Y+27);
        }

        public void Draw(SpriteBatch spriteBatch, Player player)
        {
            spriteBatch.Draw(texture, position, Color.White);

            string text = string.Format("{0}",player.Money);
            spriteBatch.DrawString(font, text, textPosition, Color.White);
        }
    }
}
