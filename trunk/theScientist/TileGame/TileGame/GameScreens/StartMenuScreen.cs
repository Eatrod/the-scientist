using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using XtheSmithLibrary;
using XtheSmithLibrary.Controls;

namespace TileGame.GameScreens
{
    public class StartMenuScreen : BaseGameState
    {
        #region Field region

        PictureBox backgroundImage;
        PictureBox arrowImage;
        LinkLabel startGame;
        LinkLabel contGame;
        LinkLabel saveGame;
        LinkLabel loadGame;
        LinkLabel exitGame;
        float maxItemWidth = 0f;

        #endregion

        #region Property Region
        #endregion
        #region Constructor Region

        public StartMenuScreen(Game game, GameStateManager manager)
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
            base.LoadContent();

            ContentManager Content = Game.Content; 
            
            backgroundImage = new PictureBox(
                Content.Load<Texture2D>(@"Backgrounds\titlescreen"),
                GameRef.ScreenRectangle);
            ControlManager.Add(backgroundImage);
            Texture2D arrowTexture = Content.Load<Texture2D>(@"GUI\leftarrowUp");

            arrowImage = new PictureBox(
                arrowTexture,
                new Rectangle(
                0,
                0,
                arrowTexture.Width,
                arrowTexture.Height));
            ControlManager.Add(arrowImage);

            startGame = new LinkLabel();
            startGame.Text = "The story begins";
            startGame.Size = startGame.SpriteFont.MeasureString(startGame.Text);
            startGame.Selected += new EventHandler(menuItem_Selected);
            ControlManager.Add(startGame);

            contGame = new LinkLabel();
            contGame.Text = "The story continues";
            contGame.Size = contGame.SpriteFont.MeasureString(contGame.Text);
            contGame.Selected += menuItem_Selected;
            ControlManager.Add(contGame);

            saveGame = new LinkLabel();
            saveGame.Text = "The story saves";
            saveGame.Size = saveGame.SpriteFont.MeasureString(saveGame.Text);
            saveGame.Selected += menuItem_Selected;
            ControlManager.Add(saveGame);

            loadGame = new LinkLabel();
            loadGame.Text = "The story loads";
            loadGame.Size = loadGame.SpriteFont.MeasureString(loadGame.Text);
            loadGame.Selected += menuItem_Selected;
            ControlManager.Add(loadGame);

            exitGame = new LinkLabel();
            exitGame.Text = "The story ends";
            exitGame.Size = exitGame.SpriteFont.MeasureString(exitGame.Text);
            exitGame.Selected += menuItem_Selected;
            ControlManager.Add(exitGame);

            ControlManager.NextControl();
            ControlManager.FocusChanged += new EventHandler(ControlManager_FocusChanged);
            Vector2 position = new Vector2(350, 500);
            foreach (Control c in ControlManager)
            {
                if (c is LinkLabel)
                {
                    if (c.Size.X > maxItemWidth)
                        maxItemWidth = c.Size.X;
                    c.Position = position;
                    position.Y += c.Size.Y + 5f;
                }
            }
            ControlManager_FocusChanged(startGame, null);
        }

        void ControlManager_FocusChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;
            Vector2 position = new Vector2(control.Position.X + maxItemWidth + 10f,
           control.Position.Y);
            arrowImage.SetPosition(position);
        }
        private void menuItem_Selected(object sender, EventArgs e)
        {
            if (sender == startGame)
            {
                StateManager.ChangeState(GameRef.GamePlayScreen);
                BaseGamePlayScreen.player.Life = 100;
                GameRef.GamePlayScreen.Gate1Locked = true;
                BaseGamePlayScreen.player.SetSpritePositionInGameWorld(new Vector2(4, 3));
            }
            if (sender == contGame)
            {
                StateManager.ChangeState(GameRef.GamePlayScreen);
            }
            if (sender == saveGame)
            {
                GameRef.SaveGameToFile();
            }
            if (sender == loadGame)
            {
                GameRef.LoadGameFromFile();
                GameState gs = GetState(GameRef.lastGameScreen);
                StateManager.ChangeState(gs);
                BaseGamePlayScreen.player.Life = GameRef.playerLife;
                BaseGamePlayScreen.player.Stamina = GameRef.playerStamina;
                BaseGamePlayScreen.player.SetSpritePositionInGameWorld(GameRef.playerPosition.X,
                    GameRef.playerPosition.Y);
            }
            if (sender == exitGame)
            {
                GameRef.Exit();
            }
        }

        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, 0);
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            GameRef.spriteBatch.Begin();
            base.Draw(gameTime);

            ControlManager.Draw(GameRef.spriteBatch);

            GameRef.spriteBatch.End();
        }

        private GameState GetState(string stateName)
        {
            foreach(GameState gs in GameRef.gamePlayScreens)
            {
                if (stateName.CompareTo(gs.Tag.ToString()) == 0)
                    return gs;
            }
            return null;
        }

        #endregion

        #region Game State Method Region
        #endregion

    }
}
