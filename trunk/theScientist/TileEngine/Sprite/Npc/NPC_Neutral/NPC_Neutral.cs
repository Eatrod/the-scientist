#region Using fält

using Microsoft.Xna.Framework.Graphics;

#endregion

#region Klass fält
namespace TileEngine.Sprite.Npc.NPC_Neutral
{
    public class NPC_Neutral : NPC
    {
        public bool ShowingBubble { get; set; }

        protected NPC_Neutral(Texture2D texture, Script script) : base(texture,script)
        {
            
        }

    }
}
#endregion
