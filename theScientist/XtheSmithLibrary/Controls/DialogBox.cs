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

        private string text;
        public NPC_Story npc;
        public StoryProgress story;
        public List<NPC_Story> npcStoryList;
        public PlayerCharacter player;
        public Conversation conversation;
        public static char[] NewLine = { '\r', '\n' };
        private Rectangle rectangle;
        private StringBuilder stringBuilder;
        private int currentHandler;
        private KeyboardState lastState;

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
            KeyboardState newState = Keyboard.GetState();

            if (npc == null || conversation == null)
                return;

            if (newState.IsKeyDown(Keys.Left) && lastState.IsKeyUp(Keys.Left))
            {
                currentHandler--;
                if (currentHandler < 0)
                    currentHandler = conversation.Handlers.Count - 1;
            }

            if (newState.IsKeyDown(Keys.Right) && lastState.IsKeyUp(Keys.Right))
            {
                currentHandler = (currentHandler + 1) % conversation.Handlers.Count;
            }

            if (newState.IsKeyDown(Keys.Space) && lastState.IsKeyUp(Keys.Space))
            {
                conversation.Handlers[currentHandler].Invoke(npc,player, story);
                currentHandler = 0;
            }

            lastState = newState;
        }

        public override void Draw(SpriteBatch spriteBatch)
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

            for (int i = 0; i < conversation.Handlers.Count; ++i)
            {
                string handler = conversation.Handlers[i].Caption;

                Color color = (i == currentHandler) ? Color.Orange : Color.Black;

                spriteBatch.DrawString(SpriteFont, handler, new Vector2(600+80*i, rectangle.Y + 60), color);
            }

            Rectangle pictureRectangle = new Rectangle(0, rectangle.Y, 200, 100);
            foreach (var npc in npcStoryList)
            {
                if (whoSaid(Text) == npc.NPCName)
                    spriteBatch.Draw(npc.picture, pictureRectangle, Color.White);
                else if (whoSaid(Text) == "Ignazio")
                {
                    spriteBatch.Draw(player.portrait, pictureRectangle, Color.White);
                }
            }
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
        }   

        #endregion

        #region Methods

        public string whoSaid(string text)
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
                if (currentTargetSize.X > bounds.Width - 200)
                {
                    target.Insert(lastWhiteSpace, NewLine);
                    target.Remove(lastWhiteSpace + NewLine.Length, 1);
                }
            }
        }

        #endregion
    }
}
