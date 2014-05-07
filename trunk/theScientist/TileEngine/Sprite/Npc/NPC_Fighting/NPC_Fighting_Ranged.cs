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
    public class NPC_Fighting_Ranged: NPC_Fighting_Guard
    {
        private bool leftOrRight;
        private float elapsedThrowBomb;
        private float delayThrowBomb;
        private bool bombThrow;
        private bool lockAndLoad;
        private bool animationTime;
        private float elapsedAnimationTime;
        private float delayAnimationTime;
        private bool walkAround;
        

        public bool BombThrow
        {
            get { return bombThrow; }
            set { bombThrow = value; }
        }
        public NPC_Fighting_Ranged(Texture2D texture, Script script, Random random): base(texture,null)
        {
            this.walkAround = true;
            this.PatrollingCircle = 300f;
            this.ElapsedHitByArrow = 0.0f;
            this.DelayHitByArrow = 300f;
            this.ElapsedRespawn = 0.0f;
            this.DelayRespawn = 25000f;
            this.elapsedThrowBomb = 0.0f;
            this.delayThrowBomb = 2500f;
            this.animationTime = false;
            this.elapsedAnimationTime = 0.0f;
            this.delayAnimationTime = 750f;
            this.bombThrow = false;
            this.lockAndLoad = false;
            this.animationTime = false;
            this.AggroRange = 300f;
            this.Random = random;
            this.Direction = 0;
            this.Speed = 0.5f;
            this.ElapsedDirection = 0.0f;
            this.DelayDirection = 4000f;


            FrameAnimation down = new FrameAnimation(1, 50, 80, 0, 0);
            FrameAnimation left = new FrameAnimation(1, 50, 80, 0, 80);
            FrameAnimation right = new FrameAnimation(1, 50, 80, 0, 160);
            FrameAnimation up = new FrameAnimation(1, 50, 80, 0, 240);
            FrameAnimation nothing = new FrameAnimation(1, 0, 0, 0, 0);


            FrameAnimation walkDown = new FrameAnimation(2, 50, 80, 50, 0);
            FrameAnimation walkLeft = new FrameAnimation(4, 50, 80, 0, 80);
            FrameAnimation walkRight = new FrameAnimation(4, 50, 80, 0, 160);
            FrameAnimation walkUp = new FrameAnimation(2, 50, 80, 50, 240);

            FrameAnimation throwLeft = new FrameAnimation(2, 50, 80, 250, 80);
            FrameAnimation throwRight = new FrameAnimation(2, 50, 80, 250, 160);

            this.Animations.Add("ThrowLeft", throwLeft);
            this.Animations.Add("ThrowRight", throwRight);
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
        public void UpdateRangedFighter(GameTime gameTime, AnimatedSprite player)
        {
            if (Vector2.Distance(player.Origin, this.Origin) < AggroRange && !animationTime)
            {
                Aggro = true;
                lockAndLoad = true;
                this.walkAround = false;
            }
            else if (Vector2.Distance(player.Origin,this.Origin) > AggroRange)
            {
                Aggro = false;
                lockAndLoad = false;
                animationTime = false;
                bombThrow = false;
                this.walkAround = true;
            }
            if(Aggro && lockAndLoad && !walkAround)
            {
                elapsedThrowBomb += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if(elapsedThrowBomb > delayThrowBomb)
                {
                    if (player.Position.X < this.Position.X)
                        leftOrRight = false;
                    else
                        leftOrRight = true;
                    this.animationTime = true;
                    this.lockAndLoad = false;
                    elapsedThrowBomb = 0.0f;
                }
            }
            if (!Aggro && Collided)
            {
                Direction += 180;
                Direction = Direction % 360;
                Collided = false;
            }
            if(animationTime)
            {
                elapsedAnimationTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (leftOrRight)
                    this.CurrentAnimationName = "ThrowRight";
                else
                    this.CurrentAnimationName = "ThrowLeft";
                this.CurrentAnimation.FramesPerSeconds = 0.40f;
                if(elapsedAnimationTime > delayAnimationTime)
                {
                    this.bombThrow = true;
                    this.animationTime = false;
                    this.CurrentAnimation.CurrentFrame = 0;
                    elapsedAnimationTime = 0.0f;
                }
            }
            if (HitByArrow)
            {
                ElapsedHitByArrow += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                this.Position += this.ArrowDirection;
                if (ElapsedHitByArrow > DelayHitByArrow)
                {
                    HitByArrow = false;
                    ElapsedHitByArrow = 0.0f;
                }
            }
        }
        public override void Update(GameTime gameTime)
        {
            if (this.Life <= 0)
                this.Dead = true;
            if (!Dead)
            {
                if (StartingFlag)
                {
                    this.StartingPosition = Position;
                    StartingFlag = false;
                }
                if (!animationTime && Aggro)
                {
                    this.CurrentAnimationName = "Down";
                }
                else if (walkAround)
                {
                    GetRandomDirection(gameTime);

                    this.Motion = new Vector2(
                       (float)Math.Cos(MathHelper.ToRadians(Direction)),
                       (float)Math.Sin(MathHelper.ToRadians(-Direction)));
                    this.Position += this.Motion * this.Speed;
                    UpdateSpriteAnimation(Motion);
                }
            }
            else
            {
                this.CurrentAnimationName = "Nothing";
                
                ElapsedRespawn += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if(ElapsedRespawn > DelayRespawn)
                {
                    this.Aggro = false;
                    this.animationTime = false;
                    this.bombThrow = false;
                    this.Dead = false;
                    this.HitByArrow = false;
                    this.ElapsedRespawn = 0.0f;
                    this.Position = this.StartingPosition;
                    this.lockAndLoad = false;
                    this.DirtPileCreated = false;
                    this.Life = this.FullHp;
                }
            }
            base.Update(gameTime);
        }
        
    }
}
