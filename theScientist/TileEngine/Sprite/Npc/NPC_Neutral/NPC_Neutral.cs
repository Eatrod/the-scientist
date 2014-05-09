﻿#region Using fält

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

#region Klass fält
namespace TileEngine.Sprite.Npc.NPC_Neutral
{
    public class NPC_Neutral : NPC
    {
        #region Field Region
        public Conversation text;
        private const int SpeakingRadius = 40;
        public bool ShowingBubble { get; set; }
        protected bool startingFlag;
        private Random random;
        bool StartingFlag = true;
        private Vector2 motion;
        public bool changeSpeed = true;

        private int direction;
        private bool collided;
        protected float delayDirection;
        protected float elapsedDirection;
        protected float WalkingCircle;
        #endregion

        #region Properties Region
        public bool Collided
        {
            get { return collided; }
            set { this.collided = value; }
        }
        
        public Vector2 Motion
        {
            get { return motion; }
            set { motion = value; }

        }
        #endregion

        protected NPC_Neutral(Texture2D texture, Script script) : base(texture,script)
        {
            FrameAnimation down = new FrameAnimation(1, 50, 80, 0, 0);
            FrameAnimation left = new FrameAnimation(1, 50, 80, 0, 80);
            FrameAnimation right = new FrameAnimation(1, 50, 80, 0, 160);
            FrameAnimation up = new FrameAnimation(1, 50, 80, 0, 240);


            FrameAnimation walkDown = new FrameAnimation(2, 50, 80, 50, 0);
            FrameAnimation walkLeft = new FrameAnimation(2, 50, 80, 50, 80);
            FrameAnimation walkRight = new FrameAnimation(2, 50, 80, 50, 160);
            FrameAnimation walkUp = new FrameAnimation(2, 50, 80, 50, 240);


            this.Animations.Add("Right", right);
            this.Animations.Add("Left", left);
            this.Animations.Add("Up", up);
            this.Animations.Add("Down", down);
            this.Animations.Add("WalkRight", walkRight);
            this.Animations.Add("WalkLeft", walkLeft);
            this.Animations.Add("WalkUp", walkUp);
            this.Animations.Add("WalkDown", walkDown);

            this.elapsedDirection = 0.0f;
            this.delayDirection = 10000f;
            this.WalkingCircle = 500;
            this.direction = 0;
            this.random = new Random();
            this.collided = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (changeSpeed == true)
            {
                if (StartingFlag)
                {
                    this.StartingPosition = Position;
                    StartingFlag = false;
                }

                GetRandomDirection(gameTime);

                if(collided)
                {
                    direction += 110;
                    direction = direction % 360;
                    collided = false;
                }

                this.motion = new Vector2(
                    (float)Math.Cos(MathHelper.ToRadians(direction)),
                    (float)Math.Sin(MathHelper.ToRadians(-direction)));

                UpdateSpriteAnimation(motion);
                this.speed = 0.5f;
                Position += motion * speed;
                base.Update(gameTime);
            }
            else
            {
                CurrentAnimationName = "Down";
            }
        }

        public bool InHearingRange(AnimatedSprite sprite)
        {
            Vector2 d = Origin - sprite.Origin;

            return (d.Length() < SpeakingRadius);
        }

        /// <summary>
        /// Ska generera ett för tillfället relevant utrop som NPCn säger.
        /// Typ: "Potato town is really nice during summer"
        /// </summary>
        public void TextBubble()
        {
            if (script == null)
                return;
            text = script["random"];
        }

        public void GetRandomDirection(GameTime gameTime)
        {
            elapsedDirection += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Vector2.Distance(StartingPosition, Position) > WalkingCircle)
            {
                direction += 180;
                direction = direction % 360;
            }
            if (elapsedDirection > delayDirection)
            {
                direction = random.Next(0, 200);
                elapsedDirection = 0;
            }


        }

    }
}
#endregion
