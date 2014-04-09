using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using XtheSmithLibrary;
using TileGame.GameScreens;

namespace TileGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region XNA Field Region

        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public List<GameState> gamePlayScreens;

        #endregion

        #region Game State Region

        public GameStateManager stateManager;
        public TitleScreen TitleScreen;
        public StartMenuScreen StartMenuScreen;
        public GamePlayScreen GamePlayScreen;
        public BaseGamePlayScreen BaseGamePlayScreen;
        public GamePlayScreen2 GamePlayScreen2;

        public string lastGameScreen;
        public Vector2 playerPosition;
        public float playerLife;
        public float playerStamina;
        

        #endregion
        
        #region Screen Field Region
       
        const int screenWidth = 1024;
        const int screenHeight = 768;
        public readonly Rectangle ScreenRectangle;

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;

            ScreenRectangle = new Rectangle(
            0,
            0,
            screenWidth,
            screenHeight);

            Content.RootDirectory = "Content";

            Components.Add(new InputHandler(this));

            stateManager = new GameStateManager(this);
            Components.Add(stateManager);

            gamePlayScreens = new List<GameState>();
            TitleScreen = new TitleScreen(this, stateManager);
            StartMenuScreen = new StartMenuScreen(this, stateManager);
            GamePlayScreen = new GamePlayScreen(this, stateManager, "Screen1");
            GamePlayScreen2 = new GamePlayScreen2(this, stateManager, "Screen2");
            BaseGamePlayScreen = new BaseGamePlayScreen(this, stateManager);
            stateManager.ChangeState(TitleScreen);
            gamePlayScreens.Add(GamePlayScreen);
            gamePlayScreens.Add(GamePlayScreen2);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        public void SaveGameToFile()
        {
            using (StreamWriter file = new StreamWriter(@"savedgame.txt"))
            {
                file.WriteLine("[State]");
                file.WriteLine(lastGameScreen);
                file.WriteLine("[Player]");
                file.WriteLine(playerPosition.ToString());
                file.WriteLine(playerLife);
                file.WriteLine(playerStamina);
            }
        }

        public void LoadGameFromFile()
        {
            using (StreamReader file = new StreamReader(@"savedgame.txt"))
            {
                string crap = file.ReadLine();
                lastGameScreen = file.ReadLine();
                crap = file.ReadLine();
                string pos = file.ReadLine();
                string[] positions = pos.Split(' ');
                string xPos = positions[0].Substring(3);
                string yPos = positions[1].Substring(2);
                int index = xPos.IndexOf(',');
                if(index >= 0)
                    xPos = xPos.Remove(index);
                yPos = yPos.Replace("}", "");
                index = yPos.IndexOf(',');
                if (index >= 0)
                    yPos = yPos.Remove(index);
                float x = (float)Convert.ToInt32(xPos);
                float y = (float)Convert.ToInt32(yPos);
                playerPosition = new Vector2(x,y);
                string life = file.ReadLine();
                index = life.IndexOf(',');
                if (index >= 0)
                    life = life.Remove(index);
                playerLife = Convert.ToInt32(life);
                string stamina = file.ReadLine();
                index = stamina.IndexOf(',');
                if (index >= 0)
                    stamina = stamina.Remove(index);
                playerStamina = Convert.ToInt32(stamina);
            }
        }
    }
}
