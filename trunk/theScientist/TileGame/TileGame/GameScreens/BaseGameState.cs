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
    public abstract partial class BaseGameState : GameState
    {
        
        #region Fields region

        protected Game1 GameRef;
        protected ControlManager ControlManager;
        protected PlayerIndex playerIndexControl;

        #endregion

        #region Properties region
        #endregion

        #region Constructor Region

        public BaseGameState(Game game, GameStateManager manager)
            : base(game, manager)
        {
            GameRef = (Game1)game;

            playerIndexControl = PlayerIndex.One;
        }

        #endregion

        #region XNA Method Region
        protected override void LoadContent()
        {
            ContentManager Content = Game.Content;
            SpriteFont menuFont = Content.Load<SpriteFont>(@"Fonts\Venice");
            ControlManager = new ControlManager(menuFont);
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        #endregion

    }
}
