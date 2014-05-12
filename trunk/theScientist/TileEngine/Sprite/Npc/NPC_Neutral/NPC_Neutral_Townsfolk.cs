#region Using fält

using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

#endregion

#region Klass fält
namespace TileEngine.Sprite.Npc.NPC_Neutral
{
    public class NPC_Neutral_Townsfolk : NPC_Neutral
    {
        private const int SpeakingRadius = 40;
        protected Curve curve;

        public NPC_Neutral_Townsfolk(Texture2D texture, Script script) : base(texture, script)
        {
            FrameAnimation down = new FrameAnimation(1, 50, 80, 0, 0);
            FrameAnimation left = new FrameAnimation(1, 50, 80, 0, 80);
            FrameAnimation right = new FrameAnimation(1, 50, 80, 0, 160);
            FrameAnimation up = new FrameAnimation(1, 50, 80, 0, 240);


            FrameAnimation walkDown = new FrameAnimation(2, 50, 80, 50, 0);
            FrameAnimation walkLeft = new FrameAnimation(2, 50, 80, 50, 80);
            FrameAnimation walkRight = new FrameAnimation(2, 50, 80, 50, 160);
            FrameAnimation walkUp = new FrameAnimation(2, 50, 80, 50, 240);

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

            this.elapsedDirection = 0.0f;
            this.delayDirection = 10000f;
            this.WalkingCircle = 500;
            this.direction = 0;
            this.random = new Random();
            this.collided = false;
        }
    }
}
#endregion