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
    public class NPC_Neutral : NPC
    {
        protected NPC_Neutral(Texture2D texture, Script script) : base(texture,script)
        {
            
        }

    }
}
#endregion
