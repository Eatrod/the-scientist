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

namespace TileEngine.Sprite.Npc.NPC_Fighting
{
    public class NPC_Fighting_Patrolling: NPC_Fighting_Guard
    {
        private int direction;
        private Random random;
        private Vector2 motion;
        private float delayDirection;
        private float elapsedDirection;
        private bool collided;
        public bool Collided
        {
            get { return collided; }
            set { collided = value; }
        }
       
        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        public Vector2 Motion
        {
            get { return motion; }
            set { motion = value; }
            
        }
        public NPC_Fighting_Patrolling(Texture2D texture, Script script, Random random, int[,] Map) :base(texture,script,Map)
        {
            this.AggroSpeed = 1.5f;
            this.PatrollingCircle = 200f;
            this.DelayHitByArrow = 300f;
            this.ElapsedHitByArrow = 0.0f;
            this.DelayRespawn = 10000f;
            this.ElapsedRespawn = 0.0f;
            this.Dead = false;
            this.OldPosition = Vector2.Zero;
            this.EndPosition = Vector2.Zero;
            this.ElapsedSearch = 10001.0f;
            this.DelaySearch = 1000f;
            this.Time2 = 0.0f;
            this.StrikeForce = 5.0f;
            this.collided = false;
            this.random = random;
            this.direction = 0;
            this.speed = 0.5f;
            this.StartingFlag = true;
            this.VectorTowardsStart = Vector2.Zero;
            this.VectorTowardsTarget = Vector2.Zero;
            this.AggroStartingPosition = Vector2.Zero;
            this.Aggro = false;
            this.AggroRange = 100;
            this.AggroCircle = 500;
            this.GoingHome = false;
            this.elapsedDirection = 0.0f;
            this.delayDirection = 3000f;
            

            FrameAnimation down = new FrameAnimation(1, 50, 80, 0, 0);
            FrameAnimation left = new FrameAnimation(1, 50, 80, 0, 80);
            FrameAnimation right = new FrameAnimation(1, 50, 80, 0, 160);
            FrameAnimation up = new FrameAnimation(1, 50, 80, 0, 240);
            FrameAnimation dead = new FrameAnimation(1, 100, 55, 0, 320);

            FrameAnimation walkDown = new FrameAnimation(2, 50, 80, 50, 0);
            FrameAnimation walkLeft = new FrameAnimation(2, 50, 80, 50, 80);
            FrameAnimation walkRight = new FrameAnimation(2, 50, 80, 50, 160);
            FrameAnimation walkUp = new FrameAnimation(2, 50, 80, 50, 240);

            this.Animations.Add("Dead", dead);

            this.Animations.Add("Right", right);
            this.Animations.Add("Left", left);
            this.Animations.Add("Up", up);
            this.Animations.Add("Down", down);
            this.Animations.Add("WalkRight", walkRight);
            this.Animations.Add("WalkLeft", walkLeft);
            this.Animations.Add("WalkUp", walkUp);
            this.Animations.Add("WalkDown", walkDown);
            
            


        }
        public override void Update(GameTime gameTime)
        {
            if (this.Life <= 0)
                Dead = true;
            if (!Dead)
            {
                this.ElapsedSearch += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (StartingFlag)
                {
                    this.StartingPosition = Position;
                    StartingFlag = false;
                }

                GetRandomDirection(gameTime);

                this.Motion = new Vector2(
                   (float)Math.Cos(MathHelper.ToRadians(direction)),
                   (float)Math.Sin(MathHelper.ToRadians(-direction)));

                UpdateSpriteAnimation(motion);


                if (Aggro && !StrikeMode)
                {
                    this.speed = AggroSpeed;
                    this.Position += VectorTowardsTarget * speed;

                    UpdateSpriteAnimation(VectorTowardsTarget);
                }
                else if (StrikeMode)
                {

                }
                else if (GoingHome)
                {
                    this.speed = 3.0f;
                    Position += VectorTowardsStart * speed;
                    UpdateSpriteAnimation(VectorTowardsStart);
                }
                else
                {
                    this.speed = 0.5f;
                    Position += motion * speed;
                }
                if (!Aggro && !GoingHome && Collided)
                {
                    direction += 180;
                    direction = direction % 360;
                    Collided = false;
                }
            }
            else
            {
                CurrentAnimationName = "Dead";
                ElapsedRespawn += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if(ElapsedRespawn > DelayRespawn)
                {
                    this.Life = this.FullHp;
                    this.Dead = false;
                    this.Aggro = false;
                    this.GoingHome = true;
                    this.ElapsedRespawn = 0.0f;
                }
            }
            base.Update(gameTime);
            
        }

        public void GetRandomDirection(GameTime gameTime)
        {
            elapsedDirection += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Vector2.Distance(StartingPosition, Position) > PatrollingCircle && !Aggro && !GoingHome)
            {
                direction += 180;
                direction = direction % 360;
            }
            if(elapsedDirection > delayDirection && !GoingHome)
            {
                direction = random.Next(0, 360);
                elapsedDirection = 0;
            }
            

        }

        //private void UpdateSpriteAnimation(Vector2 motion)
        //{

        //    float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

        //    if (motionAngle >= -MathHelper.PiOver4 && motionAngle <= MathHelper.PiOver4)
        //    {
        //        CurrentAnimationName = "WalkRight"; //Right
        //        //motion = new Vector2(1f, 0f);
        //    }
        //    else if (motionAngle >= MathHelper.PiOver4 && motionAngle <= 3f * MathHelper.PiOver4)
        //    {
        //        CurrentAnimationName = "WalkDown"; //Down
        //        //motion = new Vector2(0f, 1f);
        //    }
        //    else if (motionAngle <= -MathHelper.PiOver4 && motionAngle >= -3f * MathHelper.PiOver4)
        //    {
        //        CurrentAnimationName = "WalkUp"; // Up
        //        //motion = new Vector2(0f, -1f);
        //    }
        //    else
        //    {
        //        CurrentAnimationName = "WalkLeft"; //Left
        //        //motion = new Vector2(-1f, 0f);
        //    }
        //}
    }
}
