#region Using fält

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace TileEngine.Sprite.Npc.NPC_Story
{
    public class NPC_Story : NPC
    {
        protected bool Immortal { get; set; }
        protected float speakingRadius = 20f;
        public bool canTalk = false;
        public Conversation text;

        public NPC_Story(Texture2D texture, Script script) : base(texture,script)
        {
            this.Immortal = true;
        }

        public float SpeakingRadius
        {
            get { return speakingRadius; }
            set { speakingRadius = (float) Math.Max(value, CollisionRadius); }
        }

        public bool InSpeakingRange(AnimatedSprite sprite)
        {
            Vector2 d = Origin - sprite.Origin;

            return (d.Length() < SpeakingRadius);
        }

        public void StartConversation(string conversationName)
        {
            if (script == null)
                return;
            text = script[conversationName];
            
        }

        public void EndConversation()
        {
            /*if (script == null || dialog == null)
                return;
            dialog.Enabled = false;
            dialog.Visible = false;*/
        }
    }
}
