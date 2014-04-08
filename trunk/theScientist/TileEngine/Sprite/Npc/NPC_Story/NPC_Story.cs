#region Using fält
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TileEngine.Sprite.Npc
{
    public class NPC_Story : NPC
    {
        protected bool Immortal { get; set; }

        protected NPC_Story(Texture2D texture, Script script) : base(texture,script)
        {
            this.Immortal = true;
        }

        public void StartConversation(string conversationName)
        {

        }

        public void EndConversation()
        {
        }
    }
}
