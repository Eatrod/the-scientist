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
using TileGame.Music;

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
        BackgroundMusic titleMusic;
        BackgroundMusic potatotownMusic;
        Dictionary<GameState, BackgroundMusic> musicStates = new Dictionary<GameState,BackgroundMusic>();
        StoryProgress storyProgress = new StoryProgress();
        

        #endregion

        #region Field Region
        public Random random;
        #endregion

        #region Game State Region

        public GameStateManager stateManager;
        public TitleScreen TitleScreen;
        public StartMenuScreen StartMenuScreen;
        public NotebookScreen NotebookScreen;
        public PotatoTown GamePlayScreen;
        public PlayerScreen BaseGamePlayScreen;
        public GamePlayScreen2 GamePlayScreen2;
        public InventoryScreen InventoryScreen;

        public string lastGameScreen;
        public Vector2 playerPosition;
        public float playerLife;
        public float playerStamina;
        

        #endregion
        
        #region Screen Field Region
       
        const int screenWidth = 1248; //1024
        const int screenHeight = 768; //768
        public readonly Rectangle ScreenRectangle;

        #endregion

        #region Constructor Region
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
            NotebookScreen = new NotebookScreen(this, stateManager);
            GamePlayScreen = new PotatoTown(this, stateManager, "Screen1");
            GamePlayScreen2 = new GamePlayScreen2(this, stateManager, "Screen2");
            BaseGamePlayScreen = new PlayerScreen(this, stateManager);
            InventoryScreen = new InventoryScreen(this, stateManager);
            stateManager.ChangeState(TitleScreen);
            gamePlayScreens.Add(GamePlayScreen);
            gamePlayScreens.Add(GamePlayScreen2);
        }
        #endregion

        #region XNA Methods Region
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;

            random = new Random();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            titleMusic =  new BackgroundMusic(Content.Load<Song>("Sounds/Music/Night_vision_gameMusic"));
            potatotownMusic = new BackgroundMusic(Content.Load<Song>("Sounds/Music/Eruption_gameMusic"));

            MediaPlayer.IsRepeating = true;

            musicStates.Add(TitleScreen, titleMusic);
            musicStates.Add(StartMenuScreen, titleMusic);
            musicStates.Add(GamePlayScreen, potatotownMusic);
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

            GameState currentState = stateManager.CurrentState;

            //GameState oldState;
            if (musicStates.ContainsKey(currentState))
            {
                if (!musicStates[currentState].SongStart)
                {
                    MediaPlayer.Play(musicStates[currentState].Song);
                    musicStates[currentState].SongStart = true;
                    //oldState = currentState;
                    //musicStates[oldState].SongStart = false;
                }
            }

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
        #endregion

        #region Methods Region
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
                file.WriteLine("[StoryLine]");
                file.WriteLine("[Properties]");
                file.WriteLine(StoryProgress.GetAllProperties());
                file.WriteLine("[Keys]");
                file.WriteLine(StoryProgress.GetAllKeys());
            }
        }

        public void LoadGameFromFile()
        {
            using (StreamReader file = new StreamReader(@"savedgame.txt"))
            {
                string crap = file.ReadLine();
                if (crap.Equals("[State]"))
                    lastGameScreen = file.ReadLine();

                crap = file.ReadLine();
                if (crap.Equals("[Player]"))
                {
                    string pos = file.ReadLine();
                    string[] positions = pos.Split(' ');
                    SetPlayerPosition(positions);

                    string life = file.ReadLine();
                    life = CleanString(life);
                    playerLife = Convert.ToInt32(life);

                    string stamina = file.ReadLine();
                    stamina = CleanString(stamina);
                    playerStamina = Convert.ToInt32(stamina);
                }
                crap = file.ReadLine();
                crap = file.ReadLine();
                if(crap.Equals("[Properties]"))
                {
                    StoryProgress.SetAllProperties(file.ReadLine());
                }
                crap = file.ReadLine();
                if (crap.Equals("[Keys]"))
                {
                    StoryProgress.SetAllKeys(file.ReadLine());
                }
            }
        }

        public string CleanString(string text)
        {
            text = text.Trim('{','}',':','X','Y');
            int index = text.IndexOf(',');
            if (index >= 0)
                text = text.Remove(index);

            return text;
        }

        public void SetPlayerPosition(string[] positions)
        {
            string xPos = positions[0].Substring(3);
            string yPos = positions[1].Substring(2);

            xPos = CleanString(xPos);
            yPos = CleanString(yPos);
            float x = (float)Convert.ToInt32(xPos);
            float y = (float)Convert.ToInt32(yPos);
            playerPosition = new Vector2(x, y);
        }
        #endregion
    }
}
