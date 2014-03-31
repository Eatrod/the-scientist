using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine
{
    public static class Engine
    {
        public const int TileWidth = 32;
        public const int TileHeight = 32;


        public static Point ConvertPostionToCell(Vector2 position)
        {
            return new Point(
                (int)(position.X / (float)TileWidth),
                (int)(position.Y / (float)TileHeight));
        }

        public static Rectangle CreateRectForCell(Point cell)
        {
            return new Rectangle(
                cell.X * TileWidth,
                cell.Y * TileHeight,
                TileWidth,
                TileHeight);
        }

        //static int tileWidth = 64;
        //static int tileHeight = 64;

        //public static int TileWidth
        //{
        //    get { return tileWidth; }
        //    set { tileWidth = (int)MathHelper.Clamp(value, 20f, 100f); }
        //}

        //public static int TileHeight
        //{
        //    get { return tileHeight; }
        //    set { tileHeight = (int)MathHelper.Clamp(value, 20f, 100f); }
        //}

    }
}
