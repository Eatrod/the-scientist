using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine
{
    public class AnimatedSprite
    {
        public Dictionary<string, FrameAnimation> Animations =
            new Dictionary<string, FrameAnimation>();

        string currentAnimation = null;
        bool animating = true;
        Texture2D texture;

        public Vector2 Position = Vector2.Zero;
        public Vector2 Origionoffset = Vector2.Zero;

        public Vector2 Origin
        {
            get { return Position + Origionoffset; }
        }

        public Vector2 Center
        {
            get 
            {
                return Position + new Vector2(
                    CurrentAnimation.CurrentRectangle.Width /2 ,
                    CurrentAnimation.CurrentRectangle.Height /2 );
            }
        }

        public Rectangle Bounds
        {
            get
            {
                Rectangle rect = CurrentAnimation.CurrentRectangle;
                rect.X = (int)Position.X;
                rect.Y = (int)Position.Y;
                return rect;
            }
        }


        float radius = 10f;

        float speed = 2f;
        public float Speed
        {
            get { return speed; }
            set { speed = (float)Math.Max(value, .1f); }
        }

        public float CollisionRadius
        {
            get { return radius; }
            set { radius = (float)Math.Max(value,1f);}
        }

        public bool isAnimating
        {
            get { return animating; }
            set { animating = value; }
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

        public string CurrentAnimationName
        {
            get { return currentAnimation; }
            set 
            {
                if (Animations.ContainsKey(value))
                    currentAnimation = value;
            }
        }

        public AnimatedSprite(Texture2D texture)
        {
            this.texture = texture;
        }

        public static bool AreColliding(AnimatedSprite a, AnimatedSprite b)
        {
            Vector2 d = b.Origin - a.Origin;

            return (d.Length() < b.CollisionRadius + a.CollisionRadius);
        }

        public void ClampToArea(int width, int height)
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


        public void Update(GameTime gameTime)
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

        public void Draw(SpriteBatch spritBatch)
        {
            FrameAnimation animation = CurrentAnimation;

            if (animation != null)
                spritBatch.Draw(
                    texture,
                    Position,
                    animation.CurrentRectangle,
                    Color.White);

        }
    }
}
