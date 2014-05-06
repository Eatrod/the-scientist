using System;
using Microsoft.Xna.Framework;

namespace TileEngine.Sprite
{
    public class FrameAnimation : ICloneable
    {
        Rectangle[] frames;
        int currentFrame = 0;

        float frameLength = .35f;
        float timer = 0;

        //public int FramesPerSeconds
        //{
        //    get
        //    {
        //        return (int)(1f / frameLength);
        //    }
        //    set
        //    {
        //        frameLength = 1f / (float)Math.Max(1f / (float)value, .01f);
        //    }
        //}

        public float FramesPerSeconds
        {
            get
            {
                return frameLength;
            }
            set
            {
                frameLength = value;
            }
        }

        public Rectangle CurrentRectangle
        {
            get { return frames[currentFrame]; }
            set { frames[currentFrame] = value; }
        }
        public int CurrentFrameAlternative
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }
        public int CurrentFrame
        {
            get { return currentFrame; }
            set 
            {
                currentFrame = (int)MathHelper.Clamp(value, 0, frames.Length - 1);
            }
        }

        public FrameAnimation(
            int numberOfFrames,
            int frameWidth,
            int frameHeight,
            int xOffset,
            int yOffset)
        {
            frames = new Rectangle[numberOfFrames];

            for (int i = 0; i < numberOfFrames; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Width = frameWidth;
                rect.Height = frameHeight;
                rect.X = xOffset + (i * frameWidth);
                rect.Y = yOffset;

                frames[i] = rect;
            }

        }

        private FrameAnimation()
        { 
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= frameLength)
            {
                timer = 0f;
                currentFrame = (currentFrame + 1) % frames.Length;
            }
        }

        public object Clone()
        {
            FrameAnimation animation = new FrameAnimation();
            
            animation.frameLength = frameLength;
            animation.frames = frames;

            return animation;
        }
    }
}
