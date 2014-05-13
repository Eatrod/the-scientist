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
    public class MoleSprite:AnimatedSprite
    {
        private Vector2 shotVector;
        private bool finished;
        private bool throwBomb;
        private float elapsedAttack;
        private float delayAttack;
        public bool Finished
        {
            get { return finished; }
            set { finished = value; }
        }
        public bool ThrowBomb
        {
            get { return throwBomb; }
            set { throwBomb = value; }
        }
        public MoleSprite(Texture2D texture, Vector2 Position):base(texture)
        {
            this.Position = Position;
            this.finished = false;
            this.throwBomb = false;
            this.delayAttack = 2000f;
            this.elapsedAttack = 0.0f;
            this.shotVector = Vector2.Zero;

            FrameAnimation rise = new FrameAnimation(7, 32, 50, 0, 0);
            FrameAnimation back = new FrameAnimation(7, 32, 50, 0, 100);
            FrameAnimation attack = new FrameAnimation(9, 32, 50, 0, 50);

            this.Animations.Add("Rise", rise);
            this.Animations.Add("Back", back);
            this.Animations.Add("Attack", attack);
            this.CurrentAnimationName = "Rise";
            this.Animations["Rise"].FramesPerSeconds = 0.15f;
            this.Animations["Back"].FramesPerSeconds = 0.15f;
            this.Animations["Attack"].FramesPerSeconds = 0.05f;

        }
        public void SearchTowardsTarget(AnimatedSprite player)
        {
            shotVector = player.Origin - this.Position;
        }
        public override void Update(GameTime gameTime)
        {
            if(this.CurrentAnimationName == "Rise" && this.CurrentAnimation.CurrentFrame >= 6)
            {
                this.CurrentAnimationName = "Attack";
            }
            if(this.CurrentAnimationName == "Attack" && this.CurrentAnimation.CurrentFrame >= 8)
            {         
                throwBomb = true;
                this.CurrentAnimationName = "Back";
                
            }
            if(this.CurrentAnimationName == "Back" && this.CurrentAnimation.CurrentFrame >= 6)
            {
                this.finished = true;
            }
            base.Update(gameTime);
        }
    }
}
