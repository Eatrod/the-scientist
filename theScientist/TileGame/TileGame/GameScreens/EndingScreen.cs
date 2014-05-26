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

namespace TileGame.GameScreens
{
    public class EndingScreen : BaseGameState
    {
        #region Field region

        protected PictureBox endingStory;
        protected PictureBox endingCredits;

        bool firstRun = true;

        public float elapsedTime = 0f;
        public float timeDelay = 300f;

        int counter = 0;

        #endregion

        #region Constructor region
        public EndingScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {
        }

        #endregion

        #region XNA Method region
        public override void Initialize()
        {
            //InputHandler.Flush();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            ContentManager Content = GameRef.Content;
            base.LoadContent();


            endingStory = new PictureBox(
                Content.Load<Texture2D>("BackGrounds\\EndingStory"),
                   GameRef.ScreenRectangle);
            endingCredits = new PictureBox(
                Content.Load<Texture2D>("BackGrounds\\EndingCredits"),
                   GameRef.ScreenRectangle);
            
            ControlManager.Add(endingStory);
            endingStory.Visible = true;;
            ControlManager.Add(endingCredits);
            endingCredits.Visible = false;
            
            
            
        }
        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, PlayerIndex.One);
            
            
            if (InputHandler.KeyPressed(Keys.Space))
            {
                InputHandler.Flush();
                if (endingStory.Visible && !endingCredits.Visible)
                {
                    endingStory.Visible = false;
                    endingCredits.Visible = true;
                }
                else
                {

                    /*endingCredits.Visible = true;
                    endingStory.Visible = true;
                    GameRef.firstRun = true;
                    GameRef.StartMenuScreen.SwitchBackToOriginalMenu();
                    InputHandler.Flush();
                    StateManager.ChangeState(GameRef.TitleScreen);*/
                }
            }
            //InputHandler.Flush();
            //firstRun = false;

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
             GameRef.spriteBatch.Begin();

             base.Draw(gameTime);

             //GameRef.spriteBatch.Draw(
             //   currentPicture,
             //   GameRef.ScreenRectangle,
             //   Color.White);

             ControlManager.Draw(GameRef.spriteBatch);

             GameRef.spriteBatch.End();
        }
        #endregion

        #region Title Screen Methods
        private void startLabel_Selected(object sender, EventArgs e)
        {
            StateManager.PushState(GameRef.StartMenuScreen);
        }

        #endregion
    }
}
