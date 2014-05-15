using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine.Tiles;
using TileEngine.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;

namespace TileEngine.Sprite.Npc.NPC_Fighting
{
    public class NPC_Fighting_Farmer: NPC_Fighting
    {
        private bool dirtPileCreated;
        private bool leftOrRight;
        private Random random;
        private float elapsedMaxRunning;
        private float delayMaxRunning;
        private float delayDirection;
        private float elapsedDirection;
        private float elapsedAggro;
        private float delayAggro;
        private float chargeDamage;
        private bool running;
        private int direction;
        private Vector2 motion;
        private Vector2 attackDirection;
        private Point cellPosition;
        private bool collided;
        public bool DirtPileCreated
        {
            get { return dirtPileCreated; }
            set { dirtPileCreated = value; }
        }

        public bool Running
        {
            get { return running; }
            set { running = value; }
        }
        public Vector2 AttackDirection
        {
            get { return attackDirection; }
            set { attackDirection = value; }
        }
        public bool Collided
        {
            get { return collided; }
            set { collided = value; }
        }
        public Vector2 Motion
        {
            get { return motion; }
            set { motion = value; }
        }
        public NPC_Fighting_Farmer(Texture2D texture, Script script, Random random, Vector2 StartingPosition)
            : base(texture, script)
        {
            this.elapsedMaxRunning = 0.0f;
            this.delayMaxRunning = 6000f;
            this.StartingPosition = StartingPosition;
            this.ElapsedRespawn = 0.0f;
            this.DelayRespawn = 120000f;
            this.dirtPileCreated = false;
            this.chargeDamage = 10;
            this.elapsedAggro = 0;
            this.ElapsedHit = 0.0f;
            this.HitDelay = 500f;
            this.elapsedDirection = 0;
            this.delayAggro = 1200f;
            this.Aggro = false;
            this.collided = false;
            this.random = random;
            this.speed = 0.5f;
            this.direction = 0;
            this.cellPosition = Engine.ConvertPostionToCell(Position);
            this.delayDirection = this.random.Next(2500,5000);
            this.motion = Vector2.Zero;
           
            FrameAnimation down = new FrameAnimation(1, 50, 80, 0, 0);
            FrameAnimation left = new FrameAnimation(1, 50, 80, 0, 80);
            FrameAnimation right = new FrameAnimation(1, 50, 80, 0, 160);            
            FrameAnimation up = new FrameAnimation(1, 50, 80, 0, 240);

            FrameAnimation charge = new FrameAnimation(2, 50, 80, 250, 80);
            FrameAnimation glide = new FrameAnimation(2, 50, 80, 350, 80);

            FrameAnimation hitLeft = new FrameAnimation(1, 50, 80, 250, 160);
            FrameAnimation hitRight = new FrameAnimation(1, 50, 80, 300, 160);

            FrameAnimation walkDown = new FrameAnimation(2,50,80,50,0);
            FrameAnimation walkLeft = new FrameAnimation(4, 50, 80, 0, 80);
            FrameAnimation walkRight = new FrameAnimation(4, 50, 80, 0, 160);
            FrameAnimation walkUp = new FrameAnimation(2, 50, 80, 50, 240);
            FrameAnimation nothing = new FrameAnimation(1, 0, 0, 0, 0);

            this.Animations.Add("Nothing", nothing);
            this.Animations.Add("Charge", charge);
            this.Animations.Add("Glide", glide);
            this.Animations.Add("HitLeft", hitLeft);
            this.Animations.Add("HitRight", hitRight);


            this.Animations.Add("Right", right);
            this.Animations.Add("Left", left);
            this.Animations.Add("Up", up);
            this.Animations.Add("Down", down);
            this.Animations.Add("WalkRight", walkRight);
            this.Animations.Add("WalkLeft", walkLeft);
            this.Animations.Add("WalkUp", walkUp);
            this.Animations.Add("WalkDown", walkDown);

            this.Animations["Charge"].FramesPerSeconds = 0.15f;
            this.Animations["Glide"].FramesPerSeconds = 0.15f;
            this.Animations["WalkRight"].FramesPerSeconds = 0.25f;
            this.Animations["WalkLeft"].FramesPerSeconds = 0.25f;

        }
        public void CheckForCollisionWithOtherNPCs(List<AnimatedSprite> Npcs, CharacterSprite player)
        {
            foreach (NPC_Fighting_Farmer npc in Npcs)
            {
                if (npc.Position.X < player.Position.X)
                    leftOrRight = false;
                else
                    leftOrRight = true;

                if (player.MovementBounds().Intersects(this.MovementBounds()))
                {
                    this.running = false;
                    this.Aggro = false;
                    this.HitFlag = true;
                    player.Life -= chargeDamage;
                    player.AttackersDirection = -this.AttackersDirection;
                    player.HitFlag = true;
                    this.elapsedAggro = 0;
                    break;
                }
                
            }
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.Life <= 0)
                Dead = true;
            if (!Dead)
            {
                if (!running)
                {
                    elapsedMaxRunning = 0.0f;
                }
                cellPosition = Engine.ConvertPostionToCell(Position);
                if (cellPosition.X >= 34 && cellPosition.Y <= 18)
                {

                    Position.X -= 10f;
                    collided = true;
                    
                }
                elapsedDirection += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (collided && !HitFlag)
                {
                    if (running)
                    {
                        HitFlag = true;
                        ElapsedHit = 0;
                    }
                    direction += 180;
                    direction = direction % 360;
                    running = false;
                    Aggro = false;
                    collided = false;

                }
                else if ((elapsedDirection > delayDirection) && !Aggro && !running && !HitFlag)
                {
                    direction = random.Next(0, 360);
                    elapsedDirection = 0.0f;
                }

                this.motion = new Vector2(
                    (float)Math.Cos(MathHelper.ToRadians(direction)),
                    (float)Math.Sin(MathHelper.ToRadians(-direction)));

                UpdateSpriteAnimation(motion);
               

                if (Aggro)
                {
                    elapsedAggro += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    this.CurrentAnimationName = "Charge";
                    if (elapsedAggro > delayAggro)
                    {
                        running = true;
                        Aggro = false;
                        elapsedAggro = 0;
                    }
                }
                else if (running)
                {
                    elapsedMaxRunning += (float)gameTime.ElapsedGameTime.TotalMilliseconds;    
                    this.CurrentAnimationName = "Glide";
                    speed = 5.0f;
                    Position += AttackersDirection * speed;
                    if (elapsedMaxRunning > delayMaxRunning)
                    {
                        running = false;
                    }
                }
                else if (HitFlag)
                {
                    if (leftOrRight)
                        this.CurrentAnimationName = "HitRight";
                    else
                        this.CurrentAnimationName = "HitLeft";
                    this.CurrentAnimation.FramesPerSeconds = .35f;
                    MovementAfterBeingHit(gameTime);
                }
                else
                {

                    speed = 0.5f;
                    running = false;
                    Aggro = false;
                    Position += speed * motion;
                }
            }
            else
            {
                ElapsedRespawn += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                this.CurrentAnimationName = "Nothing";
                if(ElapsedRespawn > DelayRespawn)
                {
                    this.Life = this.FullHp;
                    this.Dead = false;
                    this.Aggro = false;
                    this.Running = false;
                    this.HitFlag = false;
                    this.DirtPileCreated = false;
                    this.Position = StartingPosition;
                    this.ElapsedRespawn = 0.0f;
                }
            }
            base.Update(gameTime);
        }

        private void UpdateSpriteAnimation(Vector2 motion)
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
