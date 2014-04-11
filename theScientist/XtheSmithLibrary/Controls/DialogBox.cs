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
        private NPC_Story npc;
        private Conversation conversation;

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
            spriteBatch.DrawString(SpriteFont, Text, new Vector2(0,1000), Color.Black);
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
        }

        #endregion

        #region Methods

        
        #endregion
    }
}
