#region Using fält

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
#endregion

#region Klass fält
namespace TileEngine.Sprite.Npc
{
    //Todo: Lägg till en konstruktor
    class NPC_Neutral : NPC
    {
        protected NPC_Neutral(Texture2D texture, Script script) : base(texture,script)
        {
            
        }

        /// <summary>
        /// Ska generera ett för tillfället relevant utrop som NPCn säger.
        /// Typ: "Potato town is really nice during summer"
        /// </summary>
        /// <param name="Exclamation">Någon form av lista med aktuella utrop?</param>
        public void RandomRelevantExclamation(String Exclamation)
        {
            
        }
    }
}
#endregion
