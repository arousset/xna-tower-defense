﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TowerDefenseXNA
{
    public class Toolbar
    {
        private Texture2D texture;
        // A class to access the font we created
        private SpriteFont font;

        // The position of the toolbar
        private Vector2 position;
        // The position of the text
        private Vector2 textPosition;

        // Constructor
        public Toolbar(Texture2D texture, SpriteFont font, Vector2 position)
        {
            this.texture = texture;
            this.font = font;

            this.position = position;
            // Offset the text to the bottom right corner
            textPosition = new Vector2(502, position.Y + 10);
        }

        public void Draw(SpriteBatch spriteBatch, Player player, WaveManager wavemanager)
        {
            spriteBatch.Draw(texture, position, Color.White);

            string text = string.Format("Wave {0}/{1}", wavemanager.Round, wavemanager.NbRounds);
            spriteBatch.DrawString(font, text, textPosition, Color.White);
        }
    }
}
