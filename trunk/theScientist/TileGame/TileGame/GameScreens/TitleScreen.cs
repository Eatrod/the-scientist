using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using XtheSmithLibrary;
using XtheSmithLibrary.Controls;

namespace TileGame.GameScreens
{
    public class TitleScreen : BaseGameState
    {
        #region Field region

        Texture2D backgroundImage1;
        Texture2D backgroundImage2;
        Texture2D backgroundImage3;
        Texture2D backgroundImage4;
        Texture2D backgroundImage5;
        Texture2D backgroundImage6;
        Texture2D backgroundImage7;
        Texture2D backgroundImage8;

        Texture2D currentPicture;
        LinkLabel startLabel;

        public float elapsedTime = 0f;
        public float timeDelay = 300f;

        int counter = 0;

        #endregion

        #region Constructor region
        public TitleScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {
        }

        #endregion

        #region XNA Method region
        protected override void LoadContent()
        {
            ContentManager Content = GameRef.Content;
            backgroundImage1 = Content.Load<Texture2D>(@"Backgrounds\PotatoWarsSplashScreen_1");
            backgroundImage2 = Content.Load<Texture2D>(@"Backgrounds\PotatoWarsSplashScreen_2");
            backgroundImage3 = Content.Load<Texture2D>(@"Backgrounds\PotatoWarsSplashScreen_3");
            backgroundImage4 = Content.Load<Texture2D>(@"Backgrounds\PotatoWarsSplashScreen_4");
            backgroundImage5 = Content.Load<Texture2D>(@"Backgrounds\PotatoWarsSplashScreen_5");
            backgroundImage6 = Content.Load<Texture2D>(@"Backgrounds\PotatoWarsSplashScreen_6");
            backgroundImage7 = Content.Load<Texture2D>(@"Backgrounds\PotatoWarsSplashScreen_7");
            backgroundImage8 = Content.Load<Texture2D>(@"Backgrounds\PotatoWarsSplashScreen_8");
            
            
            
            base.LoadContent();

            startLabel = new LinkLabel();
            startLabel.Position = new Vector2(350, 600);
            startLabel.Text = "";
            startLabel.Color = Color.White;
            startLabel.TabStop = true;
            startLabel.HasFocus = true;
            startLabel.Selected += new EventHandler(startLabel_Selected);
            ControlManager.Add(startLabel);
        }
        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, PlayerIndex.One);

            elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (counter == 1)
            {
                currentPicture = backgroundImage1;
                if (elapsedTime > timeDelay)
                {
                    counter++;
                    elapsedTime = 0f;
                }
            }
            else if (counter == 2)
            {
                currentPicture = backgroundImage8;
                if (elapsedTime > timeDelay)
                {
                    counter++;
                    elapsedTime = 0f;
                }
            } 

            else if (counter == 3)
            {
                currentPicture = backgroundImage5;
                if (elapsedTime > timeDelay)
                {
                    counter++;
                    elapsedTime = 0f;
                }
            }
            
            else 
            {
                currentPicture = backgroundImage4;
                if (elapsedTime > timeDelay)
                {
                    counter = 1;
                    elapsedTime = 0f;
                }
            }
          

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
             GameRef.spriteBatch.Begin();

             base.Draw(gameTime);

             GameRef.spriteBatch.Draw(
                currentPicture,
                GameRef.ScreenRectangle,
                Color.White);

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
