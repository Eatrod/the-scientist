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
    public class NPC_Story_Innkeeper:NPC_Story
    {
        public NPC_Story_Innkeeper(Texture2D texture, Script script, Texture2D picture, string name):base(texture,script,picture,name)
        {
            FrameAnimation cointoss = new FrameAnimation(20, 52, 80, 0, 0);
            this.Animations.Add("Cointoss", cointoss);
        }
        public override void Update(GameTime gameTime)
        {
            this.CurrentAnimationName = "Cointoss";
            this.CurrentAnimation.FramesPerSeconds = 0.1f;
            base.Update(gameTime);
        }
    }
}
