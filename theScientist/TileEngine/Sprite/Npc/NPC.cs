using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TileEngine.Sprite.Npc
{
    public class NPC : CharacterSprite
    {
        public String NPCName { get; set; }
        protected String Location { get; set; }
        protected Vector2 startingPosition;
        protected Script script;

        public Vector2 StartingPosition
        {
            get { return startingPosition; }
            set { startingPosition = value; }
        }

        public NPC(Texture2D texture, Script script)
            : base(texture)
        {
            this.script = script;
        }

        protected void UpdateSpriteAnimation(Vector2 motion)
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


        /*Flyttad till NPC_Story.cs
        public void StartConversation(string conversationName)
        { 
        }

        public void EndConversation()
        { 
        }
         */
    }
}
