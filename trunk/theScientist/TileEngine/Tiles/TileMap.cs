using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Tiles
{
    public class TileMap
    {
        public List<TileLayer> Layers = new List<TileLayer>();
        public CollisionLayer CollisionLayer;

        public int GetWidthInPixels() 
        {
            return GetWidth() * Engine.TileWidth; 
        }

        public int GetHeightInPixels()
        {
            return GetHeight() * Engine.TileHeight;
        }

        public int GetWidth()
        {
            int width = -10000;

            foreach (TileLayer layer in Layers)
                width = (int)Math.Max(width, layer.Width);

            return width;
        }

        public int GetHeight()
        {
            int height = -10000;

            foreach (TileLayer layer in Layers)
                height = (int)Math.Max(height, layer.Height);

            return height;
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {

            Point min = Engine.ConvertPostionToCell(camera.Position);
            Point max = Engine.ConvertPostionToCell(
                camera.Position + new Vector2(
                    (spriteBatch.GraphicsDevice.Viewport.Width + Engine.TileWidth) -1,
                    (spriteBatch.GraphicsDevice.Viewport.Height + Engine.TileHeight) -1));

            foreach (TileLayer layer in Layers)
            {
                layer.Draw(spriteBatch, camera, min,max);
            }
        }

        public void DrawToShadowMap(SpriteBatch spriteBatch, Vector2 place)
        {
            //Beräkning görs utifrån att man ska rita ut 4096, 4096 på en texture 2048,2048
            Point min = Engine.ConvertPostionToCell(
                (place - new Vector2(2048, 2048)));
            //Point min = (Point)(place - new Vector2((2048 / 32), (2048 / 32)));
            Point max = new Point(0,0);

            foreach (TileLayer layer in Layers)
            {
                max = Engine.ConvertPostionToCell(
                    (place + new Vector2(2048, 2048)));
                //layer.DrawToShadowMap(spriteBatch, min, max, place);
            }
        }
    }
}
