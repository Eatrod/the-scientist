using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using XtheSmithLibrary;
using XtheSmithLibrary.Controls;
using TileGame;

namespace TileGame.GameScreens
{
    public class NotebookScreen : BaseGameState
    {
        #region Field region

        Message message;
        int textMessage = 0;
        Texture2D backgroundImage;
        //List<message> messageList = new List<message>();
        Dictionary<int, Message> messageDict = new Dictionary<int, Message>();
        Label leftText;
        Label rightText;
        #endregion

        #region Property Region
        #endregion

        #region Constructor Region
        public NotebookScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {

        }
        #endregion

        #region XNA Method Region
        public override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            ContentManager Content = GameRef.Content;
            backgroundImage = Content.Load<Texture2D>(@"Backgrounds\book");

            //leftText = 

            InsertTextToMessageDictionary(0, "Nu måste du hitta nyckeln, den ligger där man minst anar");
            InsertTextToMessageDictionary(1, "Nu måste du hitta nyckeln, den ligger där man minst anar");
            InsertTextToMessageDictionary(2, "Invånarna har talat om för mig att vakterna är lätta att supa ned");

            messageDict[0].Unlocked = true;
            messageDict[1].Unlocked = true;
            messageDict[2].Unlocked = true;
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, PlayerIndex.One);
            base.Update(gameTime);
            if (InputHandler.KeyReleased(Keys.N))
                StateManager.PopState();
            if(InputHandler.KeyReleased(Keys.Left))
            {
                textMessage--;
                //textMessage 
            }
        }
        public override void Draw(GameTime gameTime)
        {
            GameRef.spriteBatch.Begin();
            base.Draw(gameTime);

            GameRef.spriteBatch.Draw(
                backgroundImage,
                GameRef.ScreenRectangle,
                Color.White);

            ControlManager.Draw(GameRef.spriteBatch);

            GameRef.spriteBatch.End();
        }
        #endregion

        #region Game State Method Region
        #endregion

        #region Methods Region
        public void InsertTextToMessageDictionary(int key, string text)
        {
            if (!messageDict.ContainsKey(key))
                messageDict.Add(key, new Message(text));
            else
                messageDict[key].Text = text;
        }
        #endregion
    }
}
