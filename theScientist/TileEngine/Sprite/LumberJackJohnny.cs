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
        private Vector2 targetedPosition;
        private bool gotIT;
        private bool slowWalk;
        private float delayCelebration;
        private float elapsedCelebration;
        public bool StartFlag;
        public bool SlowWalk
        {
            get { return slowWalk; }
            set { slowWalk = value; }
        }
        
        public Vector2 TargetedPosition
        {
            get 
            {
                targetedPosition.Normalize();
                return targetedPosition;
            }
            set { targetedPosition = value; }
        }
        public bool GotIT
        {
            get { return gotIT; }
            set { gotIT = value; }
        }
        public LumberJackJohnny(Texture2D texture, Random random): base(texture)
        {
            this.StartFlag = true;
            this.delayCelebration = 2000f;
            this.elapsedCelebration = 0.0f;
            this.slowWalk = false;
            this.Speed = 4.0f;
            this.random = random;
            this.gotIT = false;
            this.delaySeek = 1500f;
            this.elapsedSeek = 1500f;
            this.Speed = 2.0f;
            this.TargetedPosition = Vector2.Zero;
            this.Position = new Vector2(35 * 32, 28 * 32);
            //FrameAnimation down = new FrameAnimation(1, 50, 80, 150, 0);
            //FrameAnimation left = new FrameAnimation(1, 50, 80, 0, 80);
            //FrameAnimation right = new FrameAnimation(1, 50, 80, 0, 160);
            //FrameAnimation up = new FrameAnimation(1, 50, 80, 0, 240);
            //FrameAnimation nothing = new FrameAnimation(1, 0, 0, 0, 0);
            FrameAnimation monkey = new FrameAnimation(12, 50, 80, 450, 0);
            FrameAnimation celebration = new FrameAnimation(8, 50, 80, 0, 400);
            FrameAnimation walkDown = new FrameAnimation(8, 50, 80, 0, 0);
            FrameAnimation mudWalk = new FrameAnimation(8, 61, 80, 0, 160);
            FrameAnimation walkLeft = new FrameAnimation(8, 50, 80, 0, 80);
            FrameAnimation walkRight = new FrameAnimation(8, 50, 80, 0, 320);
            FrameAnimation walkUp = new FrameAnimation(8, 50, 80, 0, 240);

            //this.Animations.Add("Nothing", nothing);
            //this.Animations.Add("Right", right);
            //this.Animations.Add("Left", left);
            //this.Animations.Add("Up", up);
            //this.Animations.Add("Down", down);
            this.Animations.Add("Monkey", monkey);
            this.Animations.Add("Celebration", celebration);
            this.Animations.Add("WalkRight", walkRight);
            this.Animations.Add("WalkLeft", walkLeft);
            this.Animations.Add("WalkUp", walkUp);
            this.Animations.Add("MudWalk", mudWalk);
            this.Animations.Add("WalkDown", walkDown);
            this.CurrentAnimationName = "WalkLeft";

        }
        public void SearchTowardsTarget(GameTime gameTime, AnimatedSprite fruit)
        {
            elapsedSeek += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsedSeek > delaySeek)
            {
                TargetedPosition = fruit.Origin - this.Origin;
                elapsedSeek = 0.0f;
            }
        }
        public override void Update(GameTime gameTime)
        {
            if (this.StartFlag)
            {
                this.CurrentAnimationName = "Monkey";
                this.CurrentAnimation.FramesPerSeconds = 0.10f;
            }
            else
            {
                if (gotIT)
                {
                    elapsedCelebration += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    this.CurrentAnimationName = "Celebration";
                    this.CurrentAnimation.FramesPerSeconds = 0.05f;
                    if (elapsedCelebration > delayCelebration)
                    {
                        gotIT = false;
                        this.elapsedCelebration = 0.0f;
                    }

                }
                else
                {
                    if (slowWalk)
                    {
                        Speed = 1f;
                        this.CurrentAnimationName = "MudWalk";
                        this.CurrentAnimation.FramesPerSeconds = 0.20f;

                    }
                    else
                    {
                        Speed = 3.0f;
                        UpdateSpriteAnimation(TargetedPosition);
                        this.CurrentAnimation.FramesPerSeconds = 0.08f;
                    }

                    this.Position += TargetedPosition * Speed;
                }
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
