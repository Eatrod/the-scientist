#region Using fält

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine.Sprite.Npc;
using Microsoft.Xna.Framework.Graphics;

#endregion

#region Klass fält
namespace TileEngine.Sprite.Npc_Fighting
{
    class NPC_Fighting_Allied : NPC_Fighting
    {
        private int AggresionLevel { get; set; }
            
        NPC_Fighting_Allied(Texture2D texture, Script script) : base(texture,script)
        {
            
        }

    }
}
#endregion