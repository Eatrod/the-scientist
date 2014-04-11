using System;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Sprite.Npc.NPC_Neutral
{
    class NPC_Neutral_Critters_Goat : NPC_Neutral_Critters
    {
        NPC_Neutral_Critters_Goat(Texture2D texture, Script script) : base(texture, script)
        {
            
        }

        /// <summary>
        /// Får geten att göra ett get läte
        /// </summary>
        /// <param name="Sound">Vilket läte</param>
        public void RandomRelevantGoatSound(String Sound)
        {

        }
    }
}
