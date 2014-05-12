#region Using fält

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
        protected int SpeakingRadius = 40;
        public bool ShowingBubble { get; set; }
        protected bool startingFlag;
        protected Random random;
        protected bool StartingFlag = true;
        protected Vector2 motion;
        public bool changeSpeed = true;

        protected int direction;
        protected bool collided;
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
