using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;

namespace TileEngine.Sprite
{
    public class Explosion:AnimatedSprite
    {
        private float timeToLive;
        private float elapsedTime;
        private bool finished;
        public bool Finished
        {
            get { return finished; }
            set { finished = value; }
        }
        public Explosion(Texture2D texture, Vector2 Position): base(texture)
        {
            this.Position = Position;
            this.elapsedTime = 0.0f;
            this.timeToLive = 2000f;
            this.finished = false;
            FrameAnimation burn = new FrameAnimation(4, 12, 12, 0, 0);
            this.Animations.Add("Burn", burn);
            this.CurrentAnimationName = "Burn";
            this.CurrentAnimation.FramesPerSeconds = 0.20f;

            
        }
        public void UpdateExplosion(GameTime gameTime)
        {
            this.isAnimating = true;
            this.Animating = true;
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsedTime > timeToLive)
                finished = true;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
