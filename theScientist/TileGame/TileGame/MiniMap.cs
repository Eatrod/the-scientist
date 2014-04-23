using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;

using XtheSmithLibrary;
using XtheSmithLibrary.Controls;

using TileEngine;
using TileEngine.Tiles;

namespace TileGame
{
    class MiniMap
    {
        #region Field Region
        public TileMap tileMap = new TileMap();
        #endregion

        public MiniMap()
        {
            //spriteBatch = new SpriteBatch(GraphicsDevice);
            //Content = Game.Content;

            //tileMap.Layers.Add(TileLayer.FromFile(Content, "Content/Layers/testGround.layer"));
            //tileMap.Layers.Add(TileLayer.FromFile(Content, "Content/Layers/testBack.layer"));
            //tileMap.Layers.Add(TileLayer.FromFile(Content, "Content/Layers/testMiddle.layer"));
            //tileMap.Layers.Add(TileLayer.FromFile(Content, "Content/Layers/testFront.layer"));
        }
    }
}
