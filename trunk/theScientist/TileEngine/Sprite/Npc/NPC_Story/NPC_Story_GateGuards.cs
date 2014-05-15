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
        public NPC_Story_GateGuards(Texture2D texture, Script script, Texture2D picture, string name, bool Up)
            : base(texture, script, picture, name)
        {
            UpFlag = Up;
            FrameAnimation up = new FrameAnimation(1, 50, 80, 150, 240);
            FrameAnimation down = new FrameAnimation(1, 50, 80, 150, 0);
            FrameAnimation left = new FrameAnimation(1, 50, 80, 150, 80);
            this.Animations.Add("Left", left);
            this.Animations.Add("Up", up);
            this.Animations.Add("Down", down);
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
                this.CurrentAnimationName = "Up";
            }
            else
                this.CurrentAnimationName = "Down";
            
            base.Update(gameTime);
        }
    }
}
