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
using TileEngine;

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
        LinkLabel writeOver;
        LinkLabel regretSave;
        float maxItemWidth = 0f;

        object thisLock = new object();

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

            writeOver = new LinkLabel();
            writeOver.Text = "Yes please!";
            writeOver.Size = writeOver.SpriteFont.MeasureString(writeOver.Text);
            //writeOver.Visible = false;
            //writeOver.Enabled = false;
            //ControlManager.AddItem(writeOver);

            regretSave = new LinkLabel();
            regretSave.Text = "Nooo!";
            regretSave.Size = regretSave.SpriteFont.MeasureString(regretSave.Text);
            //regretSave.Visible = false;
            //regretSave.Enabled = false;
            //ControlManager.AddItem(regretSave);

            ControlManager.NextControl();
            ControlManager.FocusChanged += new EventHandler(ControlManager_FocusChanged);
            Vector2 position = new Vector2(350, 500);
            foreach (Control c in ControlManager)
            {
                if (c is LinkLabel && c.Enabled)
                {
                    if (c.Size.X > maxItemWidth)
                        maxItemWidth = c.Size.X;
                    c.Position = position;
                    position.Y += c.Size.Y + 5f;
                }
            }
            ControlManager_FocusChanged(startGame, null);
        }

        void SwitchToRealySaveGame()
        {
            lock (thisLock)
            {
                //Remove control not to use
                ControlManager.RemoveItem(startGame);
                //startGame.Visible = false;
                //startGame.Enabled = false;
                startGame.Selected -= menuItem_Selected;
                ControlManager.RemoveItem(contGame);
                //contGame.Visible = false;
                //contGame.Enabled = false;
                contGame.Selected -= menuItem_Selected;
                ControlManager.RemoveItem(saveGame);
                //saveGame.Visible = false;
                //saveGame.Enabled = false;
                saveGame.Selected -= menuItem_Selected;
                ControlManager.RemoveItem(loadGame);
                //loadGame.Visible = false;
                //loadGame.Enabled = false;
                loadGame.Selected -= menuItem_Selected;
                ControlManager.RemoveItem(exitGame);
                //exitGame.Visible = false;
                //exitGame.Enabled = false;
                exitGame.Selected -= menuItem_Selected;

                //Add controls to use
                ControlManager.AddItem(writeOver);
                //writeOver.Visible = true;
                //writeOver.Enabled = true;
                writeOver.Selected += menuItem_Selected;
                ControlManager.AddItem(regretSave);
                //regretSave.Visible = true;
                //regretSave.Enabled = true;
                regretSave.Selected += menuItem_Selected;
            }

            //ControlManager.selectedControl = 0;
            //ControlManager.NextControl();
            //Vector2 position = new Vector2(350, 500);
            //foreach (Control c in ControlManager)
            //{
            //    if (c is LinkLabel && c.Enabled)
            //    {
            //        if (c.Size.X > maxItemWidth)
            //            maxItemWidth = c.Size.X;
            //        c.Position = position;
            //        position.Y += c.Size.Y + 5f;
            //    }
            //}

            //ControlManager_FocusChanged(writeOver, null);
        }

        void SwitchBackToOriginalMenu()
        {
            //Remove not used item
            ControlManager.RemoveItem(writeOver);
            //writeOver.Visible = false;
            //writeOver.Enabled = false;
            writeOver.Selected -= menuItem_Selected;

            ControlManager.RemoveItem(regretSave);
            //regretSave.Visible = false;
            //regretSave.Enabled = false;
            regretSave.Selected -= menuItem_Selected;

            //Add original items
            ControlManager.AddItem(startGame);
            //startGame.Visible = true;
            //startGame.Enabled = true;
            loadGame.Selected += menuItem_Selected;
            ControlManager.AddItem(contGame);
            //contGame.Visible = true;
            //contGame.Enabled = true;
            contGame.Selected += menuItem_Selected;
            ControlManager.AddItem(saveGame);
            //saveGame.Visible = true;
            //saveGame.Enabled = true;
            saveGame.Selected += menuItem_Selected;
            ControlManager.AddItem(loadGame);
            //loadGame.Visible = true;
            //loadGame.Enabled = true;
            loadGame.Selected += menuItem_Selected;
            ControlManager.AddItem(exitGame);
            //exitGame.Visible = true;
            //exitGame.Enabled = true;
            exitGame.Selected += menuItem_Selected;

            //ControlManager.selectedControl = 0;
            //ControlManager.NextControl();
            //Vector2 position = new Vector2(350, 500);
            //foreach (Control c in ControlManager)
            //{
            //    if (c is LinkLabel && c.Enabled)
            //    {
            //        if (c.Size.X > maxItemWidth)
            //            maxItemWidth = c.Size.X;
            //        c.Position = position;
            //        position.Y += c.Size.Y + 5f;
            //    }
            //}
            //ControlManager_FocusChanged(startGame, null);
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
                StartGame();
            }
            if (sender == contGame)
            {
                StateManager.ChangeState(GameRef.PotatoTown);
            }
            if (sender == saveGame)
            {
                //Lägga in för att visa att man sparat spelet
                SwitchToRealySaveGame();
                //GameRef.SaveGameToFile();
            }
            if (sender == loadGame)
            {
                LoadGame();
            }
            if (sender == exitGame)
            {
                GameRef.Exit();
            }
            if(sender == writeOver)
            {
                GameRef.SaveGameToFile();
                SwitchBackToOriginalMenu();
            }
            if(sender == regretSave)
            {
                SwitchBackToOriginalMenu();
            }
        }

        public override void Update(GameTime gameTime)
        {
            lock (thisLock)
            {
                ControlManager.Update(gameTime, 0);
            }
            calculateControlPosition();
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            GameRef.spriteBatch.Begin();
            base.Draw(gameTime);

            ControlManager.Draw(GameRef.spriteBatch);

            GameRef.spriteBatch.End();
        }

        #endregion

        #region Game State Method Region
        private void StartGame()
        {
            Point startCell;
            GameRef.storyProgress = new StoryProgress();
            GameRef.PotatoTown = new PotatoTown(GameRef, GameRef.stateManager, "Screen1");
            StateManager.ChangeState(GameRef.PotatoTown);
            
            startCell = GameRef.BaseGamePlayScreen.FindCellWithIndexInCurrentTilemap(
                50,
                GameRef.PotatoTown);
            PlayerScreen.player.Life = 100;
            GameRef.PotatoTown.Gate1Locked = true;
            //PlayerScreen.player.SetSpritePositionInGameWorld(new Vector2(16, 6));
            PlayerScreen.player.SetSpritePositionInGameWorld(
                new Vector2(startCell.X, startCell.Y));
        }
        private void LoadGame()
        {
            Point startCell;
            GameRef.LoadGameFromFile();
            GameState gs = GetState(GameRef.lastGameScreen);
            StateManager.ChangeState(gs);
            startCell = GameRef.BaseGamePlayScreen.FindCellWithIndexInCurrentTilemap(
                50,
                (PlayerScreen)gs);
            PlayerScreen.player.Life = GameRef.playerLife;
            PlayerScreen.player.Stamina = GameRef.playerStamina;
            PlayerScreen.player.SetSpritePositionInGameWorld(
                new Vector2(startCell.X, startCell.Y));
        }
        private GameState GetState(string stateName)
        {
            foreach (GameState gs in GameRef.gamePlayScreens)
            {
                if (stateName.CompareTo(gs.Tag.ToString()) == 0)
                    return gs;
            }
            return null;
        }
        #endregion

        public void calculateControlPosition()
        {
            Vector2 position = new Vector2(350, 500);
            foreach (Control c in ControlManager)
            {
                if (c is LinkLabel && c.Enabled)
                {
                    if (c.Size.X > maxItemWidth)
                        maxItemWidth = c.Size.X;
                    c.Position = position;
                    position.Y += c.Size.Y + 5f;
                }
                if(c.HasFocus)
                {
                    Vector2 controlPosition = new Vector2(c.Position.X + maxItemWidth + 10f,
                        c.Position.Y);
                    arrowImage.SetPosition(controlPosition);
                }
            }
        }
        
    }
}
