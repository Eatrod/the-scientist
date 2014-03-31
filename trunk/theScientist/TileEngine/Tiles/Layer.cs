using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Tiles
{
    public class Layer
    {
        protected int[,] map;
        public int Width { get { return map.GetLength(1); } }
        public int Height { get { return map.GetLength(0); } }

        float alpha = 1f;

        public float Alpha
        {
            get { return alpha; }
            set { alpha = MathHelper.Clamp(value, 0f, 1f); }
        }

        public virtual int IsUsingTexture(Texture2D texture)
        {
            return 0;
        }

        public virtual void AddTexture(Texture2D texture)
        {
            
        }

        #region Get/set cellIndex

        public int GetCellIndex(int x, int y)
        {
            return map[y, x];
        }

        public int GetCellIndex(Point point)
        {
            return map[point.Y, point.X];
        }

        public void SetCellIndex(int x, int y, int cellIndex)
        {
            map[y, x] = cellIndex;
        }

        public void SetCellIndex(Point point, int cellIndex)
        {
            map[point.Y, point.Y] = cellIndex;
        }


        public void RemoveIndex(int existingIndex)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (map[y, x] == existingIndex)
                        map[y, x] = -1;
                    else if (map[y, x] > existingIndex)
                        map[y, x]--;
                }
            }


        }

        public void ReplaceIndex(int existingIndex, int newIndex)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (map[y, x] == existingIndex)
                        map[y, x] = newIndex;
                }
            }


        }

        #endregion

    }
}
