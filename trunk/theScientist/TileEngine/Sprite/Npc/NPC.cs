using System;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Sprite.Npc
{
    public class NPC : CharacterSprite
    {
        public String NPCName { get; set; }
        protected String Location { get; set; }
        protected Script script;

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
