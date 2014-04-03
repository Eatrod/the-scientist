using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine.Sprite
{
    public class AnimatedSprite : BaseSprite
    {
        public Dictionary<string, FrameAnimation> Animations =
            new Dictionary<string, FrameAnimation>();

        string currentAnimation = null;
        bool animating = true;
        bool takingDamage = false;      //kanske flyttas till bas sprite 
        
        double oldTime = 0;  //taking damage see kommentar.

        public override Vector2 Center
        {
            get 
            {
                return Position + new Vector2(
                    CurrentAnimation.CurrentRectangle.Width /2 ,
                    CurrentAnimation.CurrentRectangle.Height /2 );
            }
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

        public bool isAnimating
        {
            get { return animating; }
            set { animating = value; }
        }

        public bool areTakingDamage
        {
            get { return takingDamage; }
            set { takingDamage = value; }
        }  //kanske flyttas

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

        public string CurrentAnimationName
        {
            get { return currentAnimation; }
            set 
            {
                if (Animations.ContainsKey(value))
                    currentAnimation = value;
            }
        }

        public override void ClampToArea(int width, int height)
        {
            if (Position.X < 0)
                Position.X = 0;

            if (Position.Y < 0)
                Position.Y = 0;

            if (Position.X > width - CurrentAnimation.CurrentRectangle.Width)
                Position.X = width - CurrentAnimation.CurrentRectangle.Width;

            if (Position.Y > height - CurrentAnimation.CurrentRectangle.Height)
                Position.Y = height - CurrentAnimation.CurrentRectangle.Height;
        }  

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
            SettingSpriteBlink(gameTime);
            animation.Update(gameTime);
        }

        private void SettingSpriteBlink(GameTime gameTime)
        {
            if ((gameTime.TotalGameTime.TotalMilliseconds - oldTime) > 150)
            {
                if (takingDamage == true)
                {
                    if (faded == true)
                    {
                        Alpha = 255.0f;
                        faded = false;
                    }
                    else
                    {
                        Alpha = 0.0f;
                        faded = true;
                    }
                }
                else
                {
                    Alpha = 255.0f;
                    faded = false;
                }
                oldTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

        } // Kanske flyttas

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

        public AnimatedSprite(Texture2D texture)
        {
            this.texture = texture;
        }  //konstruktor 
    }
}
