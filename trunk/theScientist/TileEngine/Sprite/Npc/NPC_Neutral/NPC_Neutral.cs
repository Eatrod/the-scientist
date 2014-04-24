#region Using fält

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

#region Klass fält
namespace TileEngine.Sprite.Npc.NPC_Neutral
{
    public class NPC_Neutral : NPC
    {
        public Conversation text;
        private const int SpeakingRadius = 40;
        public bool ShowingBubble { get; set; }
        protected float ElapsedSearch;
        protected bool startingFlag;

        protected NPC_Neutral(Texture2D texture, Script script) : base(texture,script)
        {
            
        }

        public bool InHearingRange(AnimatedSprite sprite)
        {
            Vector2 d = Origin - sprite.Origin;

            return (d.Length() < SpeakingRadius);
        }

        /// <summary>
        /// Ska generera ett för tillfället relevant utrop som NPCn säger.
        /// Typ: "Potato town is really nice during summer"
        /// </summary>
        public void TextBubble()
        {
            if (script == null)
                return;
            text = script["random"];
        }

    }
}
#endregion
