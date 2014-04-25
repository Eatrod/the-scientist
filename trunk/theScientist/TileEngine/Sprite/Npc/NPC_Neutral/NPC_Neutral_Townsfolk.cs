#region Using fält

using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

#endregion

#region Klass fält
namespace TileEngine.Sprite.Npc.NPC_Neutral
{
    public class NPC_Neutral_Townsfolk : NPC_Neutral
    {
        private const int SpeakingRadius = 40;
        protected Curve curve;

        public NPC_Neutral_Townsfolk(Texture2D texture, Script script) : base(texture, script)
        {
            
        }
    }
}
#endregion