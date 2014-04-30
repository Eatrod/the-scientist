using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TileEngine;
using TileEngine.Sprite;

namespace XtheSmithLibrary.Controls
{
    public class ThinkingBox : PictureBox
    {
        private KeyboardState lastState;
        public string text;
        private Rectangle rectangle;
        private PlayerCharacter player;
        private static char[] NewLine = { '\r', '\n' };

        public ThinkingBox(Texture2D texture, string text, Rectangle rect, PlayerCharacter player) : base(texture, rect)
        {
            this.player = player;
            this.text = text;
            this.rectangle = rect;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Space) && lastState.IsKeyUp(Keys.Space))
            {
                this.enabled = false;
                this.visible = false;
            }

            lastState = newState;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            StringBuilder stringBuilder = new StringBuilder();
            WrapWord(new StringBuilder(text), stringBuilder, SpriteFont, rectangle);
            Text = stringBuilder.ToString();
            if (rectangle.Y < 800 && rectangle.X < 1025)
            {
                SpriteFont.LineSpacing = 25;
            }
            spriteBatch.DrawString(SpriteFont, Text, new Vector2(200, rectangle.Y - 5), Color.Black);


            Rectangle pictureRectangle = new Rectangle(0, rectangle.Y, 200, 100);
            spriteBatch.Draw(player.portrait, pictureRectangle, Color.White);
        }

        public static void WrapWord(StringBuilder original, StringBuilder target, SpriteFont font, Rectangle bounds)
        {
            int lastWhiteSpace = 0;
            Vector2 currentTargetSize;
            for (int i = 0; i < original.Length; i++)
            {
                char character = original[i];
                if (char.IsWhiteSpace(character))
                {
                    lastWhiteSpace = target.Length;
                }
                target.Append(character);
                currentTargetSize = font.MeasureString(target);
                if (currentTargetSize.X > bounds.Width - 200)
                {
                    target.Insert(lastWhiteSpace, NewLine);
                    target.Remove(lastWhiteSpace + NewLine.Length, 1);
                }
            }
        }
    }
}
