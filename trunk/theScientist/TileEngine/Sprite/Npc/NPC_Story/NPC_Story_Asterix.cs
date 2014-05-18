using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace TileEngine.Sprite.Npc.NPC_Story
{
    public class NPC_Story_Asterix : NPC_Story
    {
        private float delayStand;
        private float elapsedStand;
        private float elapsedPocket;
        private float delayPocket;
        public NPC_Story_Asterix(Texture2D texture, Script script, Texture2D picture, string name) : base(texture, script, picture, name)
        {
            this.elapsedPocket = 0.0f;
            this.delayPocket = 4000f; 
            this.elapsedStand = 0.0f;
            this.delayStand = 6000f;
            FrameAnimation standing = new FrameAnimation(1, 50, 80, 0, 320);
            FrameAnimation move = new FrameAnimation(3, 50, 80, 0, 320);
            FrameAnimation handInPocket = new FrameAnimation(1, 50, 80, 100, 320);
            FrameAnimation moveDown = new FrameAnimation(2, 50, 80, 150, 320);
            this.Animations.Add("MoveDown", moveDown);
            this.Animations.Add("HandInPocket", handInPocket);
            this.Animations.Add("Standing", standing);
            this.Animations.Add("Move", move);
            this.CurrentAnimationName = "Standing";
            this.Animations["Move"].FramesPerSeconds = 0.1f;
            this.Animations["MoveDown"].FramesPerSeconds = 0.1f;
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if(CurrentAnimationName == "Standing" || CurrentAnimationName == "Down")
            {
                elapsedStand += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                this.CurrentAnimationName = "Standing";
                if(elapsedStand > delayStand)
                {
                    this.CurrentAnimationName = "Move";
                    elapsedStand = 0.0f;
                    this.CurrentAnimation.CurrentFrame = 0;
                }
            }
            if(CurrentAnimationName == "Move" && CurrentAnimation.CurrentFrame >=2)
            {
                this.CurrentAnimation.CurrentFrame = 0;
                this.CurrentAnimationName = "HandInPocket";
            }
            if(CurrentAnimationName == "HandInPocket")
            {
                elapsedPocket += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if(elapsedPocket > delayPocket)
                {
                    this.CurrentAnimationName = "MoveDown";
                    this.CurrentAnimation.CurrentFrame = 0;
                    this.elapsedPocket = 0.0f;
                }
            }
            if(CurrentAnimationName == "MoveDown" && CurrentAnimation.CurrentFrame >= 1)
            {
                this.CurrentAnimationName = "Standing";
            }
            base.Update(gameTime);
        }
    }
}
