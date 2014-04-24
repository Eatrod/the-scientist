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
        public Conversation text;
        private const int SpeakingRadius = 40;
        protected Curve curve;

        public NPC_Neutral_Townsfolk(Texture2D texture, Script script) : base(texture, script)
        {
            this.ElapsedSearch = 10001.0f;
        }

        public override void Update(GameTime gameTime)
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