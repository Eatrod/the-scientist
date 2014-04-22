using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Sprite
{
    public class LifePotatoSprite : Sprite
    {
        #region Field Region
        #endregion

        #region Property Region
        #endregion

        #region Constructor region
        public LifePotatoSprite(Texture2D texture) : base(texture)
        {

        }

        public LifePotatoSprite(Texture2D texture, Vector2 position, bool toInventory, bool increaseHealth) :
            base(texture)
        {
            SetSpritePositionInGameWorld(position);
            Origionoffset.X = texture.Width / 2;
            Origionoffset.Y = texture.Height / 2;
        }
        #endregion

        #region XNA Region
        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rec = new Rectangle(0, 0, texture.Width, texture.Height);
            spriteBatch.Draw(
                texture,
                Position,
                rec,
                new Color(255, 255, 255, (byte)MathHelper.Clamp(Alpha, 0, 255)));
        }
        #endregion

        #region Method Region
        public override void ClampToArea(int width, int height)
        {
            if (Position.X < 0)
                Position.X = 0;

            if (Position.Y < 0)
                Position.Y = 0;

            if (Position.X > width - texture.Width)
                Position.X = width - texture.Width;

            if (Position.Y > height - texture.Height)
                Position.Y = height - texture.Height;
        }

        public override Rectangle Bounds
        {
            get
            {

                Rectangle rect = new Rectangle(0, 0, texture.Width, texture.Height);
                rect.X = (int)Position.X;
                rect.Y = (int)Position.Y;
                return rect;
            }
        }

        public override Vector2 Center
        {
            get
            {
                return Position + new Vector2(
                    texture.Width / 2,
                    texture.Height / 2);
            }
        }
        #endregion
    }
}
