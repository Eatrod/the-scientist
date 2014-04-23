using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Sprite;
using TileEngine.Sprite.Npc;
using TileEngine.Sprite.Npc.NPC_Neutral;

namespace XtheSmithLibrary.Controls
{
    public class TextBubble : PictureBox
    {
        #region Field Region

        private string text;
        public NPC_Neutral npc;
        public PlayerCharacter player;
        public Conversation conversation;
        public static char[] NewLine = { '\r', '\n' };
        private Rectangle rectangle;
        private StringBuilder stringBuilder;
        private Texture2D texture;

        #endregion

        #region Properties Region

        public string Text
        {
            get { return text; }
            set { this.text = value; }
        }

        #endregion

        #region Constructor Region
        public TextBubble(Texture2D texture, Rectangle rectangle, string text, SpriteFont font) : base(texture, rectangle)
        {
            tabStop = false;
            this.text = text;
            this.rectangle = rectangle;
            SpriteFont = font;
            this.texture = texture;
        }

        #endregion

        #region Abstract Methods

        public override void Draw(SpriteBatch spriteBatch)
        {
            SetPosition(new Vector2(npc.Position.X+30,npc.Position.Y-150));
            rectangle.Width = 150;
            rectangle.Height = 150;
            base.Draw(spriteBatch);
            StringBuilder stringBuilder = new StringBuilder();
            WrapWord(new StringBuilder(text), stringBuilder, SpriteFont, rectangle);
            Text = stringBuilder.ToString();
            //spriteBatch.Draw(texture, rectangle, Color.White);
            spriteBatch.DrawString(SpriteFont, Text, new Vector2(npc.Position.X+75,npc.Position.Y-140), Color.Black);
        }


        #endregion

        #region Methods        

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
                if (currentTargetSize.X > bounds.Width - 50)
                {
                    target.Insert(lastWhiteSpace, NewLine);
                    target.Remove(lastWhiteSpace + NewLine.Length, 1);
                }
            }
        } 
        
        #endregion
    }
}
