
#region Using fält

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

#endregion

#region Klass fält
namespace TileEngine.Sprite.Npc.Neutral_NPC
{
    class NPC_Neutral_Townsfolk : NPC_Neutral
    {
        NPC_Neutral_Townsfolk(Texture2D texture, Script script) : base(texture, script)
        {
        }
    }
}
#endregion