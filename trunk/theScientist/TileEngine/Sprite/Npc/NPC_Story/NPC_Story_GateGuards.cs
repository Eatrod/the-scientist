using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace TileEngine.Sprite.Npc.NPC_Story
{
    public class NPC_Story_GateGuards:NPC_Story
    {
        private bool UpFlag;
        private float elapsedStickMove;
        private float delayStickMove;
        private float elapsedStanding;
        private float delayStanding;
        public NPC_Story_GateGuards(Texture2D texture, Script script, Texture2D picture, string name, bool Up, Random random)
            : base(texture, script, picture, name)
        {
            delayStanding = random.Next(5000,12000);
            delayStickMove = 1500f;
            elapsedStickMove = 0.0f;
            elapsedStanding = 00.0f;
            UpFlag = Up;
            FrameAnimation up = new FrameAnimation(1, 50, 80, 0, 240);
            FrameAnimation down = new FrameAnimation(1, 50, 80, 150, 0);
            FrameAnimation stickMoveDown = new FrameAnimation(4, 50, 80, 150, 0);
            FrameAnimation stickMoveUp = new FrameAnimation(4, 50, 80, 0, 240);
            this.Animations.Add("StickMoveUp", stickMoveUp);
            this.Animations.Add("StickMoveDown", stickMoveDown);
            this.Animations.Add("Up", up);
            this.Animations.Add("Down", down);
            this.Animations["StickMoveUp"].FramesPerSeconds = 0.15f;
            this.Animations["StickMoveDown"].FramesPerSeconds = 0.15f;
            if (UpFlag)
            {
                this.CurrentAnimationName = "Up";
            }
            else
                this.CurrentAnimationName = "Down";
        }
        public override void Update(GameTime gameTime)
        {
            if (UpFlag)
            {
                if (this.CurrentAnimationName == "Up" || this.CurrentAnimationName == "Down")
                {
                    this.CurrentAnimationName = "Up";
                    elapsedStanding += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (elapsedStanding > delayStanding)
                    {
                        this.CurrentAnimation.CurrentFrame = 0;
                        this.CurrentAnimationName = "StickMoveUp";
                        elapsedStanding = 0.0f;
                    }
                }
                if (this.CurrentAnimationName == "StickMoveUp")
                {
                    elapsedStickMove += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    this.CurrentAnimationName = "StickMoveUp";
                    if (elapsedStickMove > delayStickMove)
                    {
                        this.CurrentAnimation.CurrentFrame = 0;
                        this.CurrentAnimationName = "Up";
                        elapsedStickMove = 0.0f;
                    }
                }
            }
            else if(!UpFlag)
            {
                if(this.CurrentAnimationName == "Down")
                {
                    elapsedStanding += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if(elapsedStanding > delayStanding)
                    {
                        this.CurrentAnimation.CurrentFrame = 0;
                        this.CurrentAnimationName = "StickMoveDown";
                        elapsedStanding = 0.0f;
                    }
                }
                if(this.CurrentAnimationName == "StickMoveDown")
                {
                    elapsedStickMove += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (elapsedStickMove > delayStickMove)
                    {
                        this.CurrentAnimation.CurrentFrame = 0;
                        this.CurrentAnimationName = "Down";
                        elapsedStickMove = 0.0f;
                    }
                }
            }
                
            
            base.Update(gameTime);
        }
    }
}
