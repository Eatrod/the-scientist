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
        private float delayAggro;
        private float elapsedAggro;
        public SoundEffect swordSound;
        private bool soundPlayed = false;
        
      
        public NPC_Fighting_Patrolling(Texture2D texture, Script script, Random random) :base(texture,script)
        {

            this.delayAggro = 10000f;
            this.elapsedAggro = 0.0f;
            this.TimeToStrike = false;
            this.DelayTimeToStrike = 250f;
            this.ElapsedTimeToStrike = 0.0f;
            this.ElapsedStrike = 0.0f;
            this.DelayStrike = 1000f;
            this.ElapsedHitByMelee = 0.0f;
            this.DelayHitByMelee = 250f;
            this.DirtPileCreated = false;
            this.AggroSpeed = 2.5f;
            this.PatrollingCircle = 200f;
            this.DelayHitByArrow = 300f;
            this.ElapsedHitByArrow = 0.0f;
            this.DelayRespawn = 120000f;
            this.ElapsedRespawn = 0.0f;
            this.Dead = false;
            this.OldPosition = Vector2.Zero;
            this.StrikeForce = 2.5f;
            this.Collided = false;
            this.Random = random;
            this.Direction = 0;
            this.speed = 0.5f;
            this.StartingFlag = true;
            this.VectorTowardsStart = Vector2.Zero;
            this.VectorTowardsTarget = Vector2.Zero;
            this.AggroStartingPosition = Vector2.Zero;
            this.Aggro = false;
            this.AggroRange = 100;
            this.AggroCircle = 500;
            this.GoingHome = false;
            this.ElapsedDirection = 0.0f;
            this.DelayDirection = 3000f;
            this.delaySeek = random.Next(500,2000);
            this.elapsedSeek = 0.0f;

            FrameAnimation down = new FrameAnimation(1, 50, 80, 150, 0);
            FrameAnimation left = new FrameAnimation(1, 50, 80, 0, 80);
            FrameAnimation right = new FrameAnimation(1, 50, 80, 0, 160);
            FrameAnimation up = new FrameAnimation(1, 50, 80, 0, 240);
            FrameAnimation nothing = new FrameAnimation(1, 0, 0, 0, 0);

            FrameAnimation walkDown = new FrameAnimation(2, 50, 80, 50, 0);
            FrameAnimation walkLeft = new FrameAnimation(4, 50, 80, 0, 80);
            FrameAnimation walkRight = new FrameAnimation(4, 50, 80, 0, 160);
            FrameAnimation walkUp = new FrameAnimation(2, 50, 80, 50, 240);

            FrameAnimation attackStartDown = new FrameAnimation(1, 50, 80, 200, 0);
            FrameAnimation attackStartLeft = new FrameAnimation(1, 50, 80, 250, 80);
            FrameAnimation attackStartRight = new FrameAnimation(1, 50, 80, 250, 160);
            FrameAnimation attackStartUp = new FrameAnimation(1, 50, 80, 200, 240);

            FrameAnimation attackFinishDown = new FrameAnimation(1, 50, 80, 250, 0);
            FrameAnimation attackFinishLeft = new FrameAnimation(1, 63, 80, 300, 80);
            FrameAnimation attackFinishRight = new FrameAnimation(1, 63, 80, 300, 160);
            FrameAnimation attackFinishUp = new FrameAnimation(1, 50, 80, 250, 240);

            this.Animations.Add("AttackStartRight", attackStartRight);
            this.Animations.Add("AttackStartLeft", attackStartLeft);
            this.Animations.Add("AttackStartUp", attackStartUp);
            this.Animations.Add("AttackStartDown", attackStartDown);
            this.Animations.Add("AttackFinishRight", attackFinishRight);
            this.Animations.Add("AttackFinishLeft", attackFinishLeft);
            this.Animations.Add("AttackFinishUp", attackFinishUp);
            this.Animations.Add("AttackFinishDown", attackFinishDown);
            this.Animations.Add("Nothing", nothing);
            this.Animations.Add("Right", right);
            this.Animations.Add("Left", left);
            this.Animations.Add("Up", up);
            this.Animations.Add("Down", down);
            this.Animations.Add("WalkRight", walkRight);
            this.Animations.Add("WalkLeft", walkLeft);
            this.Animations.Add("WalkUp", walkUp);
            this.Animations.Add("WalkDown", walkDown);

            this.Animations["WalkRight"].FramesPerSeconds = 0.25f;
            this.Animations["WalkLeft"].FramesPerSeconds = 0.25f;
        }
        public override void Update(GameTime gameTime)
        {
            if (this.Life <= 0)
                Dead = true;
            if (!Dead)
            {
                if (StartingFlag)
                {
                    this.StartingPosition = Position;
                    StartingFlag = false;
                }

                GetRandomDirection(gameTime);

                this.Motion = new Vector2(
                   (float)Math.Cos(MathHelper.ToRadians(Direction)),
                   (float)Math.Sin(MathHelper.ToRadians(-Direction)));
                if(!TimeToStrike && !MeleeHit)
                    UpdateSpriteAnimation(Motion);


                if (Aggro && !StrikeMode)
                {
                    this.elapsedAggro += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    this.speed = AggroSpeed;
                    this.Position += VectorTowardsTarget * speed;
                    UpdateSpriteAnimation(VectorTowardsTarget);
                    if(elapsedAggro >= delayAggro)
                    {
                        Aggro = false;
                        GoingHome = true;
                        this.elapsedAggro = 0.0f;
                    }
                    
                }
                else if (StrikeMode && !MeleeHit &&!TimeToStrike)
                {
                    this.elapsedAggro = 0.0f;
                    UpdateSpriteStandingStillAnimation(VectorTowardsTarget);
                    soundPlayed = false;
                }
                else if(MeleeHit)
                {
                    UpdateSpriteStartAttackAnimation();
                }
                else if(TimeToStrike)
                {
                    if (!soundPlayed)
                    {
                        swordSound.Play();
                        soundPlayed = true;
                    }
                    UpdateSpriteFinishAttackAnimation();
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
                    Position += Motion * speed;
                }
                if (!Aggro && !GoingHome && Collided)
                {
                    Direction += 180;
                    Direction = Direction % 360;
                    Collided = false;
                }
            }
            else
            {
                CurrentAnimationName = "Nothing";
                ElapsedRespawn += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                this.HitByArrow = false;
                if(ElapsedRespawn > DelayRespawn)
                {
                    this.Life = this.FullHp;
                    this.Dead = false;
                    this.Aggro = false;                    
                    this.ElapsedRespawn = 0.0f;
                    DirtPileCreated = false;
                    this.Position = StartingPosition;
                }
            }
            base.Update(gameTime);
            
        }
        //public void GetRandomDirection(GameTime gameTime)
        //{
        //    ElapsedDirection += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        //    if (Vector2.Distance(StartingPosition, Position) > PatrollingCircle && !Aggro && !GoingHome)
        //    {
        //        Direction += 180;
        //        Direction = Direction % 360;
        //    }
        //    if (ElapsedDirection > DelayDirection && !GoingHome)
        //    {
        //        Direction = random.Next(0, 360);
        //        ElapsedDirection = 0;
        //    }


        //}
        private void UpdateSpriteStandingStillAnimation(Vector2 motion)
        {

            float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

            if (motionAngle >= -MathHelper.PiOver4 && motionAngle <= MathHelper.PiOver4)
            {
                CurrentAnimationName = "Right"; //Right
                //motion = new Vector2(1f, 0f);
            }
            else if (motionAngle >= MathHelper.PiOver4 && motionAngle <= 3f * MathHelper.PiOver4)
            {
                CurrentAnimationName = "Down"; //Down
                //motion = new Vector2(0f, 1f);
            }
            else if (motionAngle <= -MathHelper.PiOver4 && motionAngle >= -3f * MathHelper.PiOver4)
            {
                CurrentAnimationName = "Up"; // Up
                //motion = new Vector2(0f, -1f);
            }
            else
            {
                CurrentAnimationName = "Left"; //Left
                //motion = new Vector2(-1f, 0f);
            }
        }
        private void UpdateSpriteFinishAttackAnimation()
        {
            if (this.CurrentAnimationName == "AttackStartRight")
            {
                CurrentAnimationName = "AttackFinishRight";
            }
            else if (this.CurrentAnimationName == "AttackStartLeft")
            {
                CurrentAnimationName = "AttackFinishLeft";
            }
            else if (this.CurrentAnimationName == "AttackStartUp")
            {
                CurrentAnimationName = "AttackFinishUp";
            }
            else if (this.CurrentAnimationName == "AttackStartDown")
            {
                CurrentAnimationName = "AttackFinishDown";
            }

        }
        private void UpdateSpriteStartAttackAnimation()
        {
            if (this.CurrentAnimationName == "Right" || this.CurrentAnimationName == "WalkRight")
            {
                CurrentAnimationName = "AttackStartRight";               
            }
            else if (this.CurrentAnimationName == "Left" || this.CurrentAnimationName == "WalkLeft")
            {
                CurrentAnimationName = "AttackStartLeft";
            }
            else if (this.CurrentAnimationName == "Up" || this.CurrentAnimationName == "WalkUp")
            {
                CurrentAnimationName = "AttackStartUp";
            }
            else if (this.CurrentAnimationName == "Down" || this.CurrentAnimationName == "WalkDown")
            {
                CurrentAnimationName = "AttackStartDown";
            }
           
        }
    }
}
