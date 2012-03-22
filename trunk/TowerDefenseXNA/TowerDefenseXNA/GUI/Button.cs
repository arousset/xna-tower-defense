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
    // State of button
    public enum ButtonStatus
    {
        Normal,
        MouseOver,
        Pressed,
    }
    

    public class Button : Sprite
    {
        // Store the MouseState of the last frame.
        private MouseState previousState;

        // The the different state textures.
        private Texture2D hoverTexture;
        private Texture2D pressedTexture;
        private Texture2D impossibleTexture;

        // A rectangle that covers the button.
        private Rectangle bounds;

        // Store the current state of the button.
        private ButtonStatus state = ButtonStatus.Normal;

        // For Drag&Drop
        public event EventHandler Clicked;
        public event EventHandler OnPress;

        // Constructor
        public Button(Texture2D texture, Texture2D hoverTexture, Texture2D pressedTexture, Vector2 position) : base(texture, position)
        {
            this.hoverTexture = hoverTexture;
            this.pressedTexture = pressedTexture;
            this.bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public override void Update(GameTime gameTime)
        {
            // Determine if the mouse if over the button.
            MouseState mouseState = Mouse.GetState();

            int mouseX = mouseState.X;
            int mouseY = mouseState.Y;

            bool isMouseOver = bounds.Contains(mouseX, mouseY);

            // Update the button state.
            if (isMouseOver && state != ButtonStatus.Pressed)
            {
                state = ButtonStatus.MouseOver;
            }
            else if (isMouseOver == false && state != ButtonStatus.Pressed)
            {
                state = ButtonStatus.Normal;
            }

            // Check if the player holds down the button.
            if (mouseState.LeftButton == ButtonState.Pressed &&
                previousState.LeftButton == ButtonState.Released)
            {
                if (isMouseOver == true)
                {
                    // Update the button state.
                    state = ButtonStatus.Pressed;
                }
            }

            // Check if the player releases the button.
            if (mouseState.LeftButton == ButtonState.Released &&
                previousState.LeftButton == ButtonState.Pressed)
            {
                if (isMouseOver == true)
                {
                    // update the button state.
                    state = ButtonStatus.MouseOver;

                    if (OnPress != null)
                    {
                        // Fire the OnPress event.
                        OnPress(this, EventArgs.Empty);
                    }
                }

                else if (state == ButtonStatus.Pressed)
                {
                    state = ButtonStatus.Normal;
                }
            }

            previousState = mouseState;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (state)
            {
                case ButtonStatus.Normal:
                    spriteBatch.Draw(texture, bounds, Color.White);
                    break;
                case ButtonStatus.MouseOver:
                    spriteBatch.Draw(hoverTexture, bounds, Color.White);
                    break;
                case ButtonStatus.Pressed:
                    spriteBatch.Draw(pressedTexture, bounds, Color.White);
                    break;
                default:
                    spriteBatch.Draw(texture, bounds, Color.White);
                    break;
            }
        }

        

       

    }
}
