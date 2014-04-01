using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Tiles
{
    public class TileLayer : Layer
    {
        
        //List<Texture2D> tileTextures = new List<Texture2D>();
       

        public int WidthInPixels{get { return Width * Engine.TileWidth; }}
        public int HeightInPixels{get { return Height * Engine.TileHeight;}}

        

        public TileLayer(int width, int height)
        {
            map = new int[height, width];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    map[y, x] = -1;
        }

        public TileLayer(int[,] existingMap)
        {
                map = (int[,])existingMap.Clone();
        }


        //public override int IsUsingTexture(Texture2D texture)
        //{
        //    if ( tileTextures.Contains(texture))
        //        return tileTextures.IndexOf(texture);
        //    else
        //        return -1;
        //}

        #region Saving to file code
        public void SaveLayerToFile(string filename, string[] textureNames)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("[Texture]");
                foreach (string t in textureNames)
                    writer.WriteLine(t);

                writer.WriteLine();

                writer.WriteLine("[Properties]");
                writer.WriteLine("Alpha = " + Alpha.ToString());

                writer.WriteLine();

                writer.WriteLine("[Layout]");

                for (int y = 0; y < Height; y++)
                {

                    string line = string.Empty;
                    for (int x = 0; x < Width; x++)
                    {
                        line += map[y, x].ToString() + " ";
                    }

                    writer.WriteLine(line);
                }
            }
        }
        #endregion

        #region Loading from File code

        public static TileLayer FromFile(ContentManager content, string filename)
        { 
           TileLayer tileLayer;
           List<string> textureNames = new List<string>();

           tileLayer = ProcessFile(filename, textureNames);
           tileLayer.LoadTileTexture(content, textureNames.ToArray());

           return tileLayer;
        }

        public static TileLayer FromFile(string filename, out string[] textureNameArray)
        {
            TileLayer tileLayer;
            List<string> textureNames = new List<string>();

            tileLayer = ProcessFile(filename, textureNames);
            textureNameArray = textureNames.ToArray();

            return tileLayer;
        }

        private static TileLayer ProcessFile(string filename, List<string> textureNames)
        {
            TileLayer tileLayer;
            List<List<int>> tempLayout = new List<List<int>>();
            Dictionary<string, string> propertiesDict = new Dictionary<string, string>();

            using (StreamReader reader = new StreamReader(filename))
            {
                bool readingTextures = false;
                bool readingLayout = false;
                bool readingProperties = false;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;

                    if (line.Contains("[Texture]"))
                    {
                        readingTextures = true;
                        readingLayout = false;
                        readingProperties = false;

                    }
                    
                    else if (line.Contains("[Properties]"))
                    {
                        readingProperties = true;
                        readingLayout = false;
                        readingTextures = false;

                        
                    }

                    else if (line.Contains("[Layout]"))
                    {
                        readingLayout = true;
                        readingTextures = false;
                        readingProperties = false;
                    }

                    else if (readingTextures)
                    {
                        textureNames.Add(line);
                    }

                    else if (readingProperties)
                    {
                        string[] pair = line.Split('=');
                        string key = pair[0].Trim();
                        string value = pair[1].Trim();

                        propertiesDict.Add(key, value);
                    }

                    else if (readingLayout)
                    {
                        List<int> row = new List<int>();

                        string[] cells = line.Split(' ');

                        foreach (string c in cells)
                        {
                            if (!string.IsNullOrEmpty(c))
                                row.Add(int.Parse(c));
                        }

                        tempLayout.Add(row);
                    }
                }
            }

            int width = tempLayout[0].Count;
            int height = tempLayout.Count;

            tileLayer = new TileLayer(width, height);

            foreach (KeyValuePair<string, string> property in propertiesDict)
            {
                switch (property.Key)
                { 
                    case "Alpha":
                        tileLayer.Alpha = float.Parse(property.Value,System.Globalization.CultureInfo.InvariantCulture);
                        break;
                
                }
            
            }

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    tileLayer.SetCellIndex(x, y, tempLayout[y][x]);
            return tileLayer;
        }


        #endregion


        //public override void AddTexture(Texture2D texture)
        //{
        //    tileTextures.Add(texture);
        //}

        public void RemoveTexture(Texture2D texture)
        {
            RemoveIndex(tileTextures.IndexOf(texture));
            tileTextures.Remove(texture);
        }

        


        //public void Draw(SpriteBatch batch, Camera camera)
        //{
        //    batch.Begin(SpriteSortMode.Texture,BlendState.AlphaBlend,
        //        null,null,null,null,camera.TransforMatrix);

            
        //    for (int x = 0; x < Width; x++)
        //    {
        //        for (int y = 0; y < Height; y++)
        //        {
        //            int textureIndex = map[y, x];

        //            if (textureIndex == -1)
        //                continue;

        //            else
        //            {
        //                Texture2D texture = tileTextures[textureIndex];

        //                batch.Draw(
        //                    texture,
        //                    new Rectangle(
        //                        x * Engine.TileWidth,
        //                        y * Engine.TileHeight,
        //                        Engine.TileWidth,
        //                        Engine.TileHeight),
        //                    new Color(new Vector4(1f,1f,1f, Alpha)));
        //            }
        //        }
        //    }

        //    batch.End();

        //}

        public void Draw(SpriteBatch batch, Camera camera, Point min, Point max)
        {
            batch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend,
                null, null, null, null, camera.TransforMatrix);


            min.X = (int)Math.Max(min.X, 0);
            min.Y = (int)Math.Max(min.Y, 0);
            max.X = (int)Math.Min(max.X, Width);
            max.Y = (int)Math.Min(max.Y, Height);

            for (int x = min.X; x < max.X; x++)
            {
                for (int y = min.Y; y < max.Y; y++)
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
       

    }
}
