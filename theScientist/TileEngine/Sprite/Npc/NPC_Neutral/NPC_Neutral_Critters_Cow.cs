using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TileEngine.Sprite.Npc.NPC_Neutral 
{
    public class NPC_Neutral_Critters_Cow : NPC_Neutral_Critters
    {
        int randomDirection;

        public NPC_Neutral_Critters_Cow(Texture2D texture, Script script, Random random) : base(texture, script)
        {
            FrameAnimation down = new FrameAnimation(1, 32, 32, 32, 64);
            FrameAnimation left = new FrameAnimation(1, 32, 32, 32, 96);
            FrameAnimation right = new FrameAnimation(1, 32, 32, 32, 32);
            FrameAnimation up = new FrameAnimation(1, 32, 32, 32, 0);


            FrameAnimation walkDown = new FrameAnimation(3, 32, 32, 0, 64);
            FrameAnimation walkLeft = new FrameAnimation(3, 32, 32, 0, 96);
            FrameAnimation walkRight = new FrameAnimation(3, 32, 32, 0, 32);
            FrameAnimation walkUp = new FrameAnimation(3, 32, 32, 0, 0);


            if (!this.Animations.ContainsKey("Right"))
                this.Animations.Add("Right", right);
            if (!this.Animations.ContainsKey("Left"))
                this.Animations.Add("Left", left);
            if (!this.Animations.ContainsKey("Up"))
                this.Animations.Add("Up", up);
            if (!this.Animations.ContainsKey("Down"))
                this.Animations.Add("Down", down);
            if (!this.Animations.ContainsKey("walkRight"))
                this.Animations.Add("WalkRight", walkRight);
            if (!this.Animations.ContainsKey("walkLeft"))
                this.Animations.Add("WalkLeft", walkLeft);
            if (!this.Animations.ContainsKey("walkUp"))
                this.Animations.Add("WalkUp", walkUp);
            if (!this.Animations.ContainsKey("walkDown"))
                this.Animations.Add("WalkDown", walkDown);
            //this.Animations.Add("Right", right);
            //this.Animations.Add("Left", left);
            //this.Animations.Add("Up", up);
            //this.Animations.Add("Down", down);
            //this.Animations.Add("WalkRight", walkRight);
            //this.Animations.Add("WalkLeft", walkLeft);
            //this.Animations.Add("WalkUp", walkUp);
            //this.Animations.Add("WalkDown", walkDown);

            SpeakingRadius = 80;
            this.elapsedDirection = 0.0f;
            this.delayDirection = 10000f;
            this.WalkingCircle = 500;
            this.direction = 0;
            this.random = random;
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
                this.speed = 0.01f;
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
