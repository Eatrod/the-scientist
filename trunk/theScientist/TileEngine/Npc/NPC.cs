using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TileEngine.Sprite;

namespace TileEngine.Npc
{
    public class NPC : AnimatedSprite
    {
        Script scipt;

        public NPC(Texture2D texture, Script script)
            : base(texture)
        { 
        }

        public void StartConversation(string conversationName)
        { 
        }

        public void EndConversation()
        { 
        }
    }
}
