#region Using fält
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TileEngine.Sprite.Npc
{
    public class NPC_Story : NPC
    {
        protected bool Immortal { get; set; }
        protected float speakingRadius = 20f;
        Dialog dialog;

        public NPC_Story(Texture2D texture, Dialog dialog, Script script) : base(texture,script)
        {
            this.Immortal = true;
            this.dialog = dialog;
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
            if (script == null || dialog == null)
                return;
            dialog.Enabled = true;
            dialog.Visible = true;
            dialog.Npc = this;
            dialog.conversation = script[conversationName];
        }

        public void EndConversation()
        {
            if (script == null || dialog == null)
                return;
            dialog.Enabled = false;
            dialog.Visible = false;
        }
    }
}
