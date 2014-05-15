#region Using fält

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

#endregion

namespace TileEngine.Sprite.Npc.NPC_Story
{
    public class NPC_Story_GuardCaptain : NPC_Story
    {
        public NPC_Story_GuardCaptain(Texture2D texture, Script script, Texture2D picture, string name)
            : base(texture, script, picture, name)
        {
            FrameAnimation down = new FrameAnimation(9, 50, 80, 150, 0);
            FrameAnimation sleeping = new FrameAnimation(1, 100, 160, 100, 80);
            this.Animations.Add("Sleeping", sleeping);
            this.Animations.Add("Down", down);
            this.Animations["Down"].FramesPerSeconds = 1f;
        }
    }
}
