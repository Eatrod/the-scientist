using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FishingGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont Font;

        Texture2D LeftFish;
        Texture2D RightFish;
        Texture2D CapturedFish;

        Texture2D SharkLeft;
        Texture2D SharkRight;

        Texture2D SpecialFishLeft;
        Texture2D SpecialFishRight;
        Texture2D SpecialFishCaptured;

        Texture2D Boat;
        Texture2D Background;
        Texture2D Line;
        Texture2D Hook;

        Stim stim;
        FishingBoat fishingboat;
        FishingRod fishingrod;

        KeyboardState ks;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 650;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Font = Content.Load<SpriteFont>("SpriteFont1");

            LeftFish = Content.Load<Texture2D>("fishleft");
            RightFish = Content.Load<Texture2D>("fishright");
            CapturedFish = Content.Load<Texture2D>("capturedfish");

            SharkLeft = Content.Load<Texture2D>("sharkleft");
            SharkRight = Content.Load<Texture2D>("sharkright");

            SpecialFishLeft = Content.Load<Texture2D>("specialfishleft");
            SpecialFishRight = Content.Load<Texture2D>("specialfishright");
            SpecialFishCaptured = Content.Load<Texture2D>("specialfishcaptured");

            Boat = Content.Load<Texture2D>("boat");
            Background = Content.Load<Texture2D>("background");
            Line = Content.Load<Texture2D>("line");
            Hook = Content.Load<Texture2D>("hook");
            ks = new KeyboardState();
            stim = new Stim(SpecialFishLeft, SpecialFishRight,SpecialFishCaptured,SharkLeft,SharkRight,CapturedFish,LeftFish, RightFish, 40);
            fishingboat = new FishingBoat(Boat, new Vector2(150, 0));
            fishingrod = new FishingRod(
                new Vector2(fishingboat.Position.X + fishingboat.Texture.Width /2, fishingboat.Position.Y + fishingboat.Texture.Height / 2),
                new Vector2(fishingboat.Position.X + fishingboat.Texture.Width / 2, fishingboat.Position.Y + fishingboat.Texture.Height),
                Boat, Line, Hook);

            // TODO: use this.Content to load your game content here
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
            ks = Keyboard.GetState();
            stim.UpdateFishes(fishingrod);
            fishingboat.UpdatePosition(ks);
            fishingrod.UpdateFishingRod(fishingboat, ks, stim.Fishes);
            fishingrod.CheckIfFishCaptured(stim.Fishes);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(Background, new Vector2(0, 0), Color.White);
            fishingrod.DrawFishingRod(spriteBatch);
            stim.DrawFishes(spriteBatch);
            fishingboat.DrawBoat(spriteBatch);
            spriteBatch.DrawString(Font, "Captured regular fishes: " + fishingrod.CapturedFishes, new Vector2(0, 0), Color.Black);
            spriteBatch.DrawString(Font, "Captured special fishes: " + fishingrod.CapturedSpecialFishes, new Vector2(0, 20), Color.Red);
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
