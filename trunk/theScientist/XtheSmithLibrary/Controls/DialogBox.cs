using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileEngine.Sprite.Npc;
using TileEngine.Sprite.Npc.NPC_Story;

namespace XtheSmithLibrary.Controls
{
    public class DialogBox : PictureBox
    {
        #region Field Region

        private string text;
        public NPC_Story npc;
        private Conversation conversation;
        public static char[] NewLine = { '\r', '\n' };
        private Rectangle rectangle;
        private StringBuilder stringBuilder;

        #endregion

        #region Properties Region

        public string Text
        {
            get { return text; }
            set { this.text = value; }
        }

        #endregion

        #region Constructor Region
        public DialogBox(Texture2D texture, Rectangle rectangle, string text) : base(texture, rectangle)
        {
            tabStop = false;
            this.text = text;
            this.rectangle = rectangle;
        }

        #endregion

        #region Abstract Methods


        public override void Update(GameTime gameTime)
        {
            //conversation.Handlers[0].Invoke(npc);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            StringBuilder stringBuilder = new StringBuilder();
            WrapWord(new StringBuilder(text), stringBuilder, SpriteFont, rectangle);
            Text = stringBuilder.ToString();
            spriteBatch.DrawString(SpriteFont, Text, new Vector2(200,1000), Color.Black);

            Rectangle pictureRectangle = new Rectangle(0, rectangle.Y, 200, 100);
            if (npc.picture != null)
                spriteBatch.Draw(npc.picture,pictureRectangle, Color.White);
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
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

        #endregion

        #region Methods

        
        #endregion
    }
}
