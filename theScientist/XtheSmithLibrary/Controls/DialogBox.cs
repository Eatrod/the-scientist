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
using TileEngine.Sprite.Npc;
using TileEngine.Sprite.Npc.NPC_Story;

namespace XtheSmithLibrary.Controls
{
    public class DialogBox : PictureBox
    {
        #region Field Region

        public NPC_Story npc;
        public StoryProgress story;
        public List<NPC_Story> npcStoryList;
        public PlayerCharacter player;
        public Conversation conversation;
        public static char[] NewLine = { '\r', '\n' };
        private Rectangle rectangle;
        private StringBuilder stringBuilder;
        private int currentHandler;
        public List<SpriteFont> fonts;
        private Texture2D dialogArrow;

        Vector2 arrowVector;
        Vector2 handlerVector;
        Rectangle arrowRect;

        #endregion

        #region Properties Region

        #endregion

        #region Constructor Region
        public DialogBox(List<SpriteFont> fonts,Texture2D texture, Rectangle rectangle, string text, Texture2D dialogArrow) : base(texture, rectangle)
        {
            tabStop = false;
            this.text = text;
            this.rectangle = rectangle;
            this.fonts = fonts;
            this.dialogArrow = dialogArrow;
            handlerVector = new Vector2(rectangle.X + 70, rectangle.Y + 80);
            arrowRect = new Rectangle((int)handlerVector.X-38, (int)handlerVector.Y+7, 22, 19);
            arrowVector.Y = handlerVector.Y+7;
        }

        #endregion

        #region Abstract Methods


        public override void Update(GameTime gameTime)
        {
            if (npc == null || conversation == null)
                return;
            if (InputHandler.KeyboardState.IsKeyDown(Keys.Up) && InputHandler.LastKeyboardState.IsKeyUp(Keys.Up))
            {
                currentHandler--;
                if (currentHandler < 0)
                    currentHandler = conversation.Handlers.Count - 1;
                arrowRect.Y = (int)arrowVector.Y + currentHandler*21;
            }

            if (InputHandler.KeyboardState.IsKeyDown(Keys.Down) && InputHandler.LastKeyboardState.IsKeyUp(Keys.Down))
            {
                currentHandler = (currentHandler + 1) % conversation.Handlers.Count;
                arrowRect.Y = (int)arrowVector.Y + currentHandler*21;
            }

            if (InputHandler.KeyboardState.IsKeyDown(Keys.Space) && InputHandler.LastKeyboardState.IsKeyUp(Keys.Space))
            {
                arrowRect.Y = (int)arrowVector.Y;
                conversation.Handlers[currentHandler].Invoke(npc,player, story);
                this.Text = npc.text.Text;
                this.conversation = npc.text;
                currentHandler = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {          
            base.Draw(spriteBatch);
            string stringWhoSaid = WhoSaid(Text);
            this.BuildText();
            this.DrawText(spriteBatch, stringWhoSaid);
            this.DrawHandlers(spriteBatch);
            this.DrawCharacterPortraits(spriteBatch);
        }
  

        #endregion

        #region Methods

        private void BuildText()
        {
            stringBuilder = new StringBuilder();
            WrapWord(new StringBuilder(text), stringBuilder, SpriteFont, rectangle);
            Text = stringBuilder.ToString();
            if (rectangle.Y < 800 && rectangle.X < 1025)
            {
                SpriteFont.LineSpacing = 25;
            }
        }

        private void DrawText(SpriteBatch spriteBatch, string stringWhoSaid)
        {
            //spriteBatch.DrawString(fonts[1], stringWhoSaid, new Vector2(rectangle.X + 45, rectangle.Y + 10), Color.White);
            spriteBatch.DrawString(fonts[0], Text.Split(':')[1], new Vector2(rectangle.X + 20, rectangle.Y + 25), Color.White);
        }

        private void DrawHandlers(SpriteBatch spriteBatch)
        {
            handlerVector = new Vector2(rectangle.X + 70, rectangle.Y + 80);
            for (int i = 1; i < conversation.Handlers.Count + 1; ++i)
            {
                string handler = conversation.Handlers[i - 1].Caption;

                color = (i - 1 == currentHandler) ? Color.Gold : Color.Lime;
                SpriteFont.Spacing = -2;

                spriteBatch.Draw(dialogArrow, arrowRect, Color.White);
                spriteBatch.DrawString(fonts[0], handler, handlerVector, color);
                handlerVector.Y += 20;
            }
        }

        private void DrawCharacterPortraits(SpriteBatch spriteBatch)
        {
            Rectangle pictureRectangle = new Rectangle(rectangle.X + 15, rectangle.Y - 255, 200, 275);
            foreach (var npc in npcStoryList)
            {
                if (WhoSaid(Text) == npc.NPCName)
                {
                    pictureRectangle.X = rectangle.X + 475;
                    spriteBatch.Draw(npc.picture, pictureRectangle, Color.White);
                }
                else if (WhoSaid(Text) == "Ignazio")
                {
                    spriteBatch.Draw(player.portrait, pictureRectangle, Color.White);
                }
            }
        }

        public string WhoSaid(string text)
        {
            string[] talker = text.Split(':');
            return talker[0];
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
                if (currentTargetSize.X > bounds.Width)
                {
                    target.Insert(lastWhiteSpace, NewLine);
                    target.Remove(lastWhiteSpace + NewLine.Length, 1);
                }
            }
        }

        #endregion
    }
}
