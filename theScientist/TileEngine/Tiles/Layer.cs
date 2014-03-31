using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine.Tiles
{
    public class Layer
    {
        protected int[,] map;

        public int Width { get { return map.GetLength(1); } }
        public int Height { get { return map.GetLength(0); } }

    }
}
