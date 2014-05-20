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
        private float elapsedTripped;
        public float delayTripped;
        private float elapsedSeek;
        private float delaySeek;
        private Random random;
        private Vector2 targetedPosition;
        private Vector2 fruitPosition;
        public bool HitFlag;
        private bool gotIT;
        private int gangnamCounter;
        private bool slowWalk;
        public float delayCelebration;
        private float elapsedCelebration;
        public bool StartFlag;
        public bool StopFlag;
        public bool AngryFlag;
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
            this.gangnamCounter = 0;
            this.AngryFlag = false;
            this.elapsedTripped = 0.0f;
            this.delayTripped = 1000f;
            this.HitFlag = false;
            this.StartFlag = true;
            this.StopFlag = false;
            this.delayCelebration = 2000f;
            this.elapsedCelebration = 0.0f;
            this.slowWalk = false;
            this.Speed = 4.0f;
            this.random = random;
            this.gotIT = false;
            this.delaySeek = 200f;
            this.elapsedSeek = 200f;
            this.Speed = 2.0f;
            this.TargetedPosition = Vector2.Zero;
            this.Position = new Vector2(35 * 32, 28 * 32);
            //FrameAnimation down = new FrameAnimation(1, 50, 80, 150, 0);
            //FrameAnimation left = new FrameAnimation(1, 50, 80, 0, 80);
            //FrameAnimation right = new FrameAnimation(1, 50, 80, 0, 160);
            //FrameAnimation up = new FrameAnimation(1, 50, 80, 0, 240);
            //FrameAnimation nothing = new FrameAnimation(1, 0, 0, 0, 0);
            FrameAnimation losing = new FrameAnimation(6, 50, 80, 0, 880);
            FrameAnimation rise = new FrameAnimation(39,50,80,0,800);
            FrameAnimation monkey = new FrameAnimation(12, 50, 80, 450, 0);
            FrameAnimation celebration = new FrameAnimation(8, 50, 80, 0, 400);
            FrameAnimation swimUp = new FrameAnimation(8, 50, 80, 0, 480);
            FrameAnimation swimRight = new FrameAnimation(8, 50, 80, 0, 560);
            FrameAnimation swimLeft = new FrameAnimation(8, 50, 80, 0, 640);
            FrameAnimation swimDown = new FrameAnimation(8, 50, 80, 0, 720);
            FrameAnimation walkDown = new FrameAnimation(8, 50, 80, 0, 0);
            FrameAnimation mudWalk = new FrameAnimation(8, 61, 80, 0, 160);
            FrameAnimation walkLeft = new FrameAnimation(8, 50, 80, 0, 80);
            FrameAnimation walkRight = new FrameAnimation(8, 50, 80, 0, 320);
            FrameAnimation walkUp = new FrameAnimation(8, 50, 80, 0, 240);
            FrameAnimation trippRight = new FrameAnimation(4, 50, 80, 400, 400);
            FrameAnimation trippLeft = new FrameAnimation(4, 50, 80, 600, 400);
            FrameAnimation trippMud = new FrameAnimation(1, 61, 80, 488, 160);
            FrameAnimation gangnam = new FrameAnimation(19, 70, 100, 0, 1100);
            FrameAnimation gangnam2 = new FrameAnimation(13, 70, 100, 0, 1200);
            this.Animations.Add("Gangnam", gangnam);
            this.Animations.Add("Gangnam2", gangnam2);
            this.Animations["Gangnam2"].FramesPerSeconds = 0.08f;
            this.Animations["Gangnam"].FramesPerSeconds = 0.08f;

            this.Animations.Add("Losing", losing);
            this.Animations.Add("Rise", rise);
            this.Animations.Add("SwimUp", swimUp);
            this.Animations.Add("SwimRight", swimRight);
            this.Animations.Add("SwimLeft", swimLeft);
            this.Animations.Add("SwimDown", swimDown);
            this.Animations.Add("Monkey", monkey);
            this.Animations.Add("TrippMud", trippMud);
            this.Animations.Add("TrippRight", trippRight);
            this.Animations.Add("TrippLeft", trippLeft);
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
            this.fruitPosition = fruit.Position;
        }
        public override void Update(GameTime gameTime)
        {
            if (this.StartFlag)
            {
                if(this.CurrentAnimationName != "Gangnam" && this.CurrentAnimationName != "Gangnam2")
                {
                    this.CurrentAnimationName = "Rise";
                    this.CurrentAnimation.FramesPerSeconds = 0.03f;
                }
                if (this.CurrentAnimationName == "Rise" && this.CurrentAnimation.CurrentFrame >= 38)
                {
                    this.Position.Y -= 10f;
                    this.CurrentAnimation.CurrentFrame = 0;
                    this.CurrentAnimationName = "Gangnam";
                    this.CurrentAnimation.FramesPerSeconds = 0.08f;
                }
                if(this.CurrentAnimationName == "Gangnam" && this.CurrentAnimation.CurrentFrame >= 18)
                {
                    this.CurrentAnimation.CurrentFrame = 0;
                    this.CurrentAnimationName = "Gangnam2";
                    this.CurrentAnimation.FramesPerSeconds = 0.08f;
                }
                if(this.CurrentAnimationName == "Gangnam2" && this.CurrentAnimation.CurrentFrame >= 12)
                {
                    this.CurrentAnimation.CurrentFrame = 0;
                    this.CurrentAnimationName = "Gangnam";
                    this.CurrentAnimation.FramesPerSeconds = 0.08f;
                }
            }
            else if (StopFlag)
            {
                if (FruitForMiniGameSprite.npcPoints >= 1000)
                {
                    this.CurrentAnimationName = "Monkey";
                    this.CurrentAnimation.FramesPerSeconds = 0.10f;
                }
                if (FruitForMiniGameSprite.playerPoints >= 1000)
                {
                    this.CurrentAnimationName = "Losing";
                    this.CurrentAnimation.FramesPerSeconds = 0.1f;
                }
            }
            else
            {
                
                
                if (HitFlag)
                {
                    elapsedTripped += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    this.Speed = 0.0f;
                    if (this.CurrentAnimationName == "MudWalk" ||
                        this.CurrentAnimationName == "TrippMud" ||
                        this.CurrentAnimationName == "SwimLeft" ||
                        this.CurrentAnimationName == "SwimRight" ||
                        this.CurrentAnimationName == "SwimUp" ||
                        this.CurrentAnimationName == "SwimDown")
                    {
                        this.CurrentAnimationName = "TrippMud";
                    }
                    else
                    {
                        if (Position.X > fruitPosition.X)
                        {
                            this.CurrentAnimationName = "TrippLeft";
                        }
                        else
                        {
                            this.CurrentAnimationName = "TrippRight";
                        }
                        this.CurrentAnimation.FramesPerSeconds = 0.10f;
                    }
                    if (elapsedTripped > delayTripped)
                    {
                        elapsedTripped = 0.0f;
                        HitFlag = false;
                        gotIT = false;
                        elapsedCelebration = 0.0f;
                    }
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
                            if (AngryFlag)
                            {
                                this.Speed = 2f;
                                this.Position += targetedPosition * speed;
                                UpdateSpriteAnimationSwim(targetedPosition);
                                this.CurrentAnimation.FramesPerSeconds = 0.10f;
                            }
                            else
                            {
                                Speed = 1.5f;
                                this.CurrentAnimationName = "MudWalk";
                                this.CurrentAnimation.FramesPerSeconds = 0.20f;
                            }

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
                
            }
            base.Update(gameTime);
        }
        public void UpdateSpriteAnimation(Vector2 motion)
        {
            float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

            if (motionAngle >= -MathHelper.PiOver4 && motionAngle <= MathHelper.PiOver4)
                CurrentAnimationName = "WalkRight";
            else if (motionAngle >= MathHelper.PiOver4 && motionAngle <= 3f * MathHelper.PiOver4)
                CurrentAnimationName = "WalkDown";
            else if (motionAngle <= -MathHelper.PiOver4 && motionAngle >= -3f * MathHelper.PiOver4)
                CurrentAnimationName = "WalkUp";
            else
                CurrentAnimationName = "WalkLeft";        
        }
        public void UpdateSpriteAnimationSwim(Vector2 motion)
        {
            float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

            if (motionAngle >= -MathHelper.PiOver4 && motionAngle <= MathHelper.PiOver4)
                CurrentAnimationName = "SwimRight";
            else if (motionAngle >= MathHelper.PiOver4 && motionAngle <= 3f * MathHelper.PiOver4)
                CurrentAnimationName = "SwimDown";
            else if (motionAngle <= -MathHelper.PiOver4 && motionAngle >= -3f * MathHelper.PiOver4)
                CurrentAnimationName = "SwimUp";
            else
                CurrentAnimationName = "SwimLeft";
        }
    }
}
