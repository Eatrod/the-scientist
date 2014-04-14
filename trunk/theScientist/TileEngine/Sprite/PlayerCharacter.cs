using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Sprite
{
     public class PlayerCharacter : CharacterSprite
     {
         public Texture2D portrait;

         public PlayerCharacter(Texture2D texture, Texture2D portrait)
             : base(texture)
         {
             this.portrait = portrait;
         }
    }
}
