using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace TileEngine.Sprite.Npc.NPC_Story
{
    class NPC_Story_DonPotato : NPC_Story
    {
        protected NPC_Story_DonPotato(Texture2D texture, Dialog.Dialog dialog, Script script, Texture2D picture) : base(texture, script,picture, "Don Potato")
        {
        }
    }
}
