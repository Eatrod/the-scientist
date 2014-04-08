using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TileEngine.Sprite;

namespace TileEngine.Sprite.Npc
{
    public class NPC : CharacterSprite
    {
        private String NPCName { get; set; }
        private String Location { get; set; }
        Script script;

        public NPC(Texture2D texture, Script script)
            : base(texture)
        {
            this.script = script;
        }

        /*Flyttad till NPC_Story.cs
        public void StartConversation(string conversationName)
        { 
        }

        public void EndConversation()
        { 
        }
         */
    }
}
