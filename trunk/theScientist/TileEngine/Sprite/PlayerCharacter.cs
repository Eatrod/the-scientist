using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Sprite
{
     public class PlayerCharacter : CharacterSprite
    {
         public PlayerCharacter(Texture2D texture)
             : base(texture)
         { }
    }
}
