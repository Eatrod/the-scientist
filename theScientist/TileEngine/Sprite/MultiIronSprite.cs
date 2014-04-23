using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Sprite
{
    public class MultiIronSprite : AnimatedSprite
    {
        #region Field Region
        public Dictionary<string, FrameAnimation> Animations =
           new Dictionary<string, FrameAnimation>();

        string currentAnimation = null;
        bool animating = true;
        #endregion

        #region Property Region
        public bool Animating
        {
            get { return animating; }
            set { }
        }
        #endregion

        #region Constructor region
        public MultiIronSprite(Texture2D texture) : base(texture)
        {

        }

        public MultiIronSprite(Texture2D texture, Vector2 position) :
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
            FrameAnimation animation = CurrentAnimation;
            if (animation == null)
            {
                if (Animations.Count > 0)
                {
                    string[] keys = new string[Animations.Count];
                    Animations.Keys.CopyTo(keys, 0);

                    currentAnimation = keys[0];
                    animation = CurrentAnimation;
                }
                else
                    return;
            }
            
            animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            FrameAnimation animation = CurrentAnimation;

            if (animation != null)
            {
                spriteBatch.Draw(
                    texture,
                    Position,
                    animation.CurrentRectangle,
                    new Color(255, 255, 255, (byte)MathHelper.Clamp(Alpha, 0, 255)));
            }
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
                Rectangle rect = CurrentAnimation.CurrentRectangle;
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
                    CurrentAnimation.CurrentRectangle.Width / 2,
                    CurrentAnimation.CurrentRectangle.Height / 2);
            }
        }

        public string CurrentAnimationName
        {
            get { return currentAnimation; }
            set
            {
                if (Animations.ContainsKey(value))
                    currentAnimation = value;
            }
        }

        public FrameAnimation CurrentAnimation
        {
            get
            {
                if (!string.IsNullOrEmpty(currentAnimation))
                    return Animations[currentAnimation];
                else
                    return null;
            }
        }
        #endregion
    }
}
