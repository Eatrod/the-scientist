using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TileEngine.Sprite;
using TileEngine.Sprite.Npc;
using TileEngine.Sprite.Npc.NPC_Story;

namespace TileEngine.Dialog
{
    public class Dialog
    {
        public Conversation conversation = null;
        public NPC_Story Npc;
        public PlayerCharacter player;

        public Rectangle area = new Rectangle(0,0,500,500);

        public void NextText(NPC_Story npc, Conversation conversation, PlayerCharacter player, StoryProgress story)
        {
            this.Npc = npc;
            this.conversation = conversation;
        }
    }
}
