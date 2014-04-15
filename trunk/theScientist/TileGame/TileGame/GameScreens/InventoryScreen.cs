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
    public class InventoryScreen : BaseGameState
    {
        #region Field region

        Texture2D backgroundImage;

        #endregion

        #region Property Region
        #endregion

        #region Constructor region
        public InventoryScreen(Game game, GameStateManager manager)
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

            backgroundImage = Content.Load<Texture2D>(@"Backgrounds\Inventory test v2");

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, PlayerIndex.One);
            base.Update(gameTime);

            if (InputHandler.KeyReleased(Keys.I))
                StateManager.PopState();
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

        #endregion
    }
}
