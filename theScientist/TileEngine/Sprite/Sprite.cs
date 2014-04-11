using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Sprite
{
    public class Sprite : BaseSprite
    {
        public Sprite(Texture2D texture) : base(texture)
        {
            //this.texture = texture;
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

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rec = new Rectangle(0,0, texture.Width, texture.Height);
            spriteBatch.Draw(
                texture,
                Position,
                rec,
                new Color(255, 255, 255, (byte)MathHelper.Clamp(Alpha, 0, 255)));
        }

    }
}
