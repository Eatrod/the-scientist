#region Using fält
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TileEngine.Sprite.Npc
{
    class NPC_Story : NPC
    {
        private bool Immortal { get; set; }

        NPC_Story(Texture2D texture, Script script) : base(texture,script)
        {
            Immortal = true;
        }

        public void StartConversation(string conversationName)
        {
        }

        public void EndConversation()
        {
        }
    }
}
