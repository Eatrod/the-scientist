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
    public class NPC_Story_Johnny : NPC_Story
    {
        public NPC_Story_Johnny(Texture2D texture, Script script, Texture2D picture, string name)
            : base(texture, script, picture, name)
        {
            FrameAnimation down = new FrameAnimation(1, 50, 80, 1900, 800);
            //FrameAnimation sleeping = new FrameAnimation(1, 100, 160, 100, 80);
            //this.Animations.Add("Sleeping", sleeping);
            this.Animations.Add("Down", down);
            this.Animations["Down"].FramesPerSeconds = 1f;
        }
        public override void Update(GameTime gameTime)
        {
            this.CurrentAnimationName = "Down";
            base.Update(gameTime);
        }
    }
}
