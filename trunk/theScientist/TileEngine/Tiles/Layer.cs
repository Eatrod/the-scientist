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

        protected List<Texture2D> tileTextures = new List<Texture2D>();

        float alpha = 1f;
        public int[,] Map
        {
            get { return map; }
        }
        public float Alpha
        {
            get { return alpha; }
            set { alpha = MathHelper.Clamp(value, 0f, 1f); }
        }

        public  int IsUsingTexture(Texture2D texture)
        {
            if (tileTextures.Contains(texture))
                return tileTextures.IndexOf(texture);
            else
                return -1;
        }

        public  void AddTexture(Texture2D texture)
        {
            tileTextures.Add(texture);
        }

        public void LoadTileTexture(ContentManager content, params string[] textureNames)
        {
            Texture2D texture;

            foreach (string textureName in textureNames)
            {
                texture = content.Load<Texture2D>(textureName);
                tileTextures.Add(texture);
            }

        }

        public void Draw(SpriteBatch batch, Camera camera)
        {
            batch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend,
                null, null, null, null, camera.TransforMatrix);


            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int textureIndex = map[y, x];

                    if (textureIndex == -1)
                        continue;

                    else
                    {
                        Texture2D texture = tileTextures[textureIndex];

                        batch.Draw(
                            texture,
                            new Rectangle(
                                x * Engine.TileWidth,
                                y * Engine.TileHeight,
                                Engine.TileWidth,
                                Engine.TileHeight),
                            new Color(new Vector4(1f, 1f, 1f, Alpha)));
                    }
                }
            }

            batch.End();

        }

        public void DrawToShadpwMap(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend,
                null, null, null, null, Matrix.Identity);


            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int textureIndex = map[y, x];

                    if (textureIndex == -1)
                        continue;

                    else
                    {
                        Texture2D texture = tileTextures[textureIndex];

                        batch.Draw(
                            texture,
                            new Rectangle(
                                x * Engine.TileWidth,
                                y * Engine.TileHeight,
                                Engine.TileWidth,
                                Engine.TileHeight),
                            new Color(new Vector4(1f, 1f, 1f, Alpha)));
                    }
                }
            }
            batch.End();
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
