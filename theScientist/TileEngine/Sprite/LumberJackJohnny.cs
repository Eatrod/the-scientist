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
    public class LumberJackJohnny: AnimatedSprite
    {
        private float elapsedSeek;
        private float delaySeek;
        private Random random;
        private Vector2 vectorTowardsTarget;
        private bool gotIT;
        public Vector2 VectorTowardsTarget
        {
            get 
            {
                vectorTowardsTarget.Normalize();
                return vectorTowardsTarget;
            }
            set { vectorTowardsTarget = value; }
        }
        public bool GotIT
        {
            get { return gotIT; }
            set { gotIT = value; }
        }
        public LumberJackJohnny(Texture2D texture, Random random): base(texture)
        {
            this.random = random;
            this.gotIT = false;
            this.delaySeek = 1500f;
            this.elapsedSeek = 0.0f;
            this.Speed = 2.0f;
            this.vectorTowardsTarget = Vector2.Zero;
            this.Position = new Vector2(55 * 32, 40 * 32);
        }
        public void SearchTowardsTarget(GameTime gameTime, AnimatedSprite fruit)
        {
            elapsedSeek += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsedSeek > delaySeek)          
                vectorTowardsTarget = fruit.Origin - this.Origin;       
        }
        public override void Update(GameTime gameTime)
        {
            this.Position += vectorTowardsTarget * Speed;
            UpdateSpriteAnimation(vectorTowardsTarget);
            if(gotIT)
            {
                gotIT = false;
            }
            base.Update(gameTime);
        }
        public void UpdateSpriteAnimation(Vector2 motion)
        {

            float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

            if (motionAngle >= -MathHelper.PiOver4 && motionAngle <= MathHelper.PiOver4)
            {
                CurrentAnimationName = "WalkRight"; //Right
                //motion = new Vector2(1f, 0f);
            }
            else if (motionAngle >= MathHelper.PiOver4 && motionAngle <= 3f * MathHelper.PiOver4)
            {

                CurrentAnimationName = "WalkDown"; //Down
                //motion = new Vector2(0f, 1f);
            }
            else if (motionAngle <= -MathHelper.PiOver4 && motionAngle >= -3f * MathHelper.PiOver4)
            {

                CurrentAnimationName = "WalkUp"; // Up
                //motion = new Vector2(0f, -1f);
            }
            else
            {

                CurrentAnimationName = "WalkLeft"; //Left
                //motion = new Vector2(-1f, 0f);
            }
        }
    }
}
