using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Sprite
{
     public class PlayerCharacter : CharacterSprite
     {
         public Texture2D portrait;
         public bool hasAxe = false;

         public PlayerCharacter(Texture2D texture, Texture2D portrait)
             : base(texture)
         {
             this.portrait = portrait;
         }

         public void UpdateAxeStatus(string status)
         {
             if (status == "true")
                this.hasAxe = true;
             else
             {
                 this.hasAxe = false;
             }
         }
    }
}
