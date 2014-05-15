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
    public class NPC_Story_Fisherman:NPC_Story
    {
        public NPC_Story_Fisherman(Texture2D texture, Script script, Texture2D picture, string name):base(texture,script,picture,name)
        {
            FrameAnimation dance = new FrameAnimation(4, 52, 80, 0, 0);
            this.Animations.Add("Dance", dance);
        }
        public override void Update(GameTime gameTime)
        {
            this.CurrentAnimationName = "Dance";
            this.CurrentAnimation.FramesPerSeconds = 5f;
            base.Update(gameTime);
        }
    }
}
