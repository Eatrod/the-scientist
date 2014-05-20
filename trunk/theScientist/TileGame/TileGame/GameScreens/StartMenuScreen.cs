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
        PictureBox helpMenuImage;
        LinkLabel startGame;
        LinkLabel contGame;
        LinkLabel saveGame;
        LinkLabel loadGame;
        LinkLabel exitGame;
        LinkLabel writeOver;
        LinkLabel regretSave;
        LinkLabel startOver;
        LinkLabel regretStartOver;
        LinkLabel helpMenu;
        Label saveText;
        Label startOverText;
        Texture2D arrowTexture;
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
                Content.Load<Texture2D>(@"BackGrounds\PotatoWarsYellow"),
                GameRef.ScreenRectangle);
            ControlManager.Add(backgroundImage);
            arrowTexture = Content.Load<Texture2D>(@"BackGrounds\selected");
            
            arrowImage = new PictureBox(
                arrowTexture,
                new Rectangle(
                0,
                0,
                arrowTexture.Width,
                arrowTexture.Height));
            ControlManager.Add(arrowImage);

            helpMenuImage = new PictureBox(
                Content.Load<Texture2D>(@"BackGrounds\Controls"),
                GameRef.ScreenRectangle);
            helpMenuImage.Visible = false;

            startGame = new LinkLabel();
            startGame.Text = "The story begins";
            startGame.Size = startGame.SpriteFont.MeasureString(startGame.Text);
            startGame.Selected += new EventHandler(menuItem_Selected);
            ControlManager.Add(startGame);

            contGame = new LinkLabel();
            contGame.Text = "The story continues";
            contGame.Size = contGame.SpriteFont.MeasureString(contGame.Text);
            if (!GameRef.firstRun)
            {
                contGame.Selected += menuItem_Selected;
                ControlManager.Add(contGame);
            }

            SpriteFont fontLarge = Content.Load<SpriteFont>(@"Fonts\Large_Venice");

            helpMenu = new LinkLabel();
            helpMenu.Text = "The story's controls!";
            helpMenu.Size = helpMenu.SpriteFont.MeasureString(helpMenu.Text);
            helpMenu.Selected += menuItem_Selected;
            ControlManager.Add(helpMenu);

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

            regretSave = new LinkLabel();
            regretSave.Text = "Nooo!";
            regretSave.Size = regretSave.SpriteFont.MeasureString(regretSave.Text);

            saveText = new Label();
            saveText.Text = "Write over your last save, Smith?";
            saveText.Size = saveText.SpriteFont.MeasureString(saveText.Text);
            saveText.SpriteFont = fontLarge;

            startOver = new LinkLabel();
            startOver.Text = "Yes please!";
            startOver.Size = startOver.SpriteFont.MeasureString(startOver.Text);

            regretStartOver = new LinkLabel();
            regretStartOver.Text = "Nooo!";
            regretStartOver.Size = regretStartOver.SpriteFont.MeasureString(regretStartOver.Text);

            startOverText = new Label();
            startOverText.Text = "Really start a new story, Smith?";
            startOverText.Size = startOverText.SpriteFont.MeasureString(startOverText.Text);
            
            startOverText.SpriteFont = fontLarge;

            ControlManager.NextControl();
            ControlManager.FocusChanged += new EventHandler(ControlManager_FocusChanged);
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width / 2 , GraphicsDevice.Viewport.Height / 2 );
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
                startGame.Selected -= menuItem_Selected;

                ControlManager.RemoveItem(contGame);
                contGame.Selected -= menuItem_Selected;

                ControlManager.RemoveItem(saveGame);
                saveGame.Selected -= menuItem_Selected;

                ControlManager.RemoveItem(loadGame);
                loadGame.Selected -= menuItem_Selected;

                ControlManager.RemoveItem(exitGame);
                exitGame.Selected -= menuItem_Selected;

                ControlManager.RemoveItem(helpMenu);
                helpMenu.Selected -= menuItem_Selected;
                

                //Add controls to use
                ControlManager.AddItem(saveText);

                ControlManager.AddItem(writeOver);
                writeOver.Selected += menuItem_Selected;

                ControlManager.AddItem(regretSave);
                regretSave.Selected += menuItem_Selected;
            }

        }

        void SwitchToHelpMenuScreen()
        {
            ControlManager.RemoveItem(startGame);
            startGame.Selected -= menuItem_Selected;

            ControlManager.RemoveItem(contGame);
            contGame.Selected -= menuItem_Selected;

            ControlManager.RemoveItem(saveGame);
            saveGame.Selected -= menuItem_Selected;

            ControlManager.RemoveItem(loadGame);
            loadGame.Selected -= menuItem_Selected;

            ControlManager.RemoveItem(exitGame);
            exitGame.Selected -= menuItem_Selected;

            ControlManager.RemoveItem(helpMenu);
            helpMenu.Selected -= menuItem_Selected;
        }

        public void SwitchBackToOriginalMenu()
        {
            //Remove not used item
            ControlManager.RemoveItem(writeOver);
            writeOver.Selected -= menuItem_Selected;

            ControlManager.RemoveItem(regretSave);
            regretSave.Selected -= menuItem_Selected;

            ControlManager.RemoveItem(startOver);
            startOver.Selected -= menuItem_Selected;

            ControlManager.RemoveItem(regretStartOver);
            regretStartOver.Selected -= menuItem_Selected;

            ControlManager.RemoveItem(saveText);
            ControlManager.RemoveItem(startOverText);

            //Add original items
            ControlManager.AddItem(startGame);
            startGame.Selected += menuItem_Selected;
            if (!GameRef.firstRun)
            {
                ControlManager.AddItem(contGame);
                contGame.Selected += menuItem_Selected;
            }
            ControlManager.AddItem(helpMenu);
            helpMenu.Selected += menuItem_Selected;
            ControlManager.AddItem(saveGame);
            saveGame.Selected += menuItem_Selected;
            ControlManager.AddItem(loadGame);
            loadGame.Selected += menuItem_Selected;
            ControlManager.AddItem(exitGame);
            exitGame.Selected += menuItem_Selected;

        }

        void SwitchToRealyRestartGame()
        {
            //Remove control not to use
            ControlManager.RemoveItem(startGame);
            startGame.Selected -= menuItem_Selected;

            ControlManager.RemoveItem(contGame);
            contGame.Selected -= menuItem_Selected;

            ControlManager.RemoveItem(saveGame);
            saveGame.Selected -= menuItem_Selected;

            ControlManager.RemoveItem(loadGame);
            loadGame.Selected -= menuItem_Selected;

            ControlManager.RemoveItem(exitGame);
            exitGame.Selected -= menuItem_Selected;

            ControlManager.RemoveItem(helpMenu);
            helpMenu.Selected -= menuItem_Selected;

            //Add controls to use
            ControlManager.AddItem(startOverText);

            ControlManager.AddItem(startOver);
            startOver.Selected += menuItem_Selected;

            ControlManager.AddItem(regretStartOver);
            regretStartOver.Selected += menuItem_Selected;
        }

        void ControlManager_FocusChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;
           // Vector2 position = new Vector2(control.Position.X + maxItemWidth + 10f,
           //control.Position.Y);
            Vector2 position = new Vector2(control.Position.X - 10f,
           control.Position.Y);
            //arrowImage.DestinationRectangle = new Rectangle(0, 0, (int)control.Size.X, (int)control.Size.Y);
            //arrowImage.SourceRectangle = new Rectangle(0,0,(int)control.Size.X, (int)control.Size.Y);
            arrowImage.SetPosition(position);
        }
        private void menuItem_Selected(object sender, EventArgs e)
         {
            if (sender == startGame)
            {
                if (GameRef.firstRun)
                {
                    GameRef.firstRun = false;
                    SwitchToRealyRestartGame();
                    StartGame();
                }
                else
                {
                    SwitchToRealyRestartGame();
                }
            }
            if (sender == contGame)
            {
                SwitchToRealyRestartGame();
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
                GameRef.firstRun = false;
                SwitchToRealyRestartGame();
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
            if(sender == helpMenu)
            {
                SwitchToHelpMenuScreen();
                helpMenuImage.Visible = true;
                ControlManager.AddItem(helpMenuImage);
            }
            if(sender == startOver)
            {
                GameRef.firstRun = false;
                StartGame();
                
            }
            if(sender == regretStartOver)
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
            if (helpMenuImage.Visible)
            {
                //InputHandler.Flush();
                if (InputHandler.KeyReleased(Keys.Space) || InputHandler.KeyReleased(Keys.Enter) || InputHandler.KeyReleased(Keys.Escape))
                {
                    InputHandler.Flush();
                    helpMenuImage.Visible = false;
                    ControlManager.RemoveItem(helpMenuImage);
                    SwitchBackToOriginalMenu();
                }
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
            //GameRef.PotatoTown = new PotatoTown(GameRef, GameRef.stateManager, "Screen1");
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
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width / 2 - 200 , GraphicsDevice.Viewport.Height / 3 * 2);
            foreach (Control c in ControlManager)
            {
                if (c is LinkLabel || c is Label)
                {
                    if (c.Size.X > maxItemWidth)
                        maxItemWidth = c.Size.X;
                    c.Position = position;
                    position.Y += c.Size.Y + 5f;
                }
                if(c.HasFocus)
                {
                    Vector2 controlPosition = new Vector2(c.Position.X - 10f,
                        c.Position.Y);
                    //arrowImage.DestinationRectangle = new Rectangle(0, 0, (int)c.Size.X, (int)c.Size.Y);
                    //arrowImage.SourceRectangle = new Rectangle(0, 0, (int)c.Size.X, (int)c.Size.Y);
                    arrowImage.SetPosition(controlPosition);
                }
            }
        }
        
    }
}
