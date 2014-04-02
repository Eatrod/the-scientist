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
    public class CollisionLayer : Layer
    {
        

        public CollisionLayer(int width, int height)
        {
            map = new int[height, width];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    map[y, x] = -1;
        }



        #region Saving to file code
        public void SaveLayerToFile(string filename,Dictionary<string,int> collDict, List<string> collList)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                
                writer.WriteLine("[Layout]");

                for (int y = 0; y < Height; y++)
                {

                    string line = string.Empty;
                    for (int x = 0; x < Width; x++)
                    {
                        int collisionNumber;
                        if (map[y, x] == -1)
                        {
                            collisionNumber = map[y, x];
                        }
                        else
                        {
                            int index = map[y, x];
                            string name = collList[index];
                            name = name.Replace("CollisionTiles\\", "");
                            collisionNumber = collDict[name + ".png"];
                        }
                        line += collisionNumber.ToString() + " ";
                       
                    }

                    writer.WriteLine(line);
                }
            }
        }
        #endregion

        #region Loading from File code


        //public static CollisionLayer FromFile(ContentManager content, string filename)
        //{
        //    CollisionLayer collisionLayer;
        //    List<string> textureNames = new List<string>();

        //    using (StreamReader reader = new StreamReader("../../../../TileEditor/Content/CollisionTiles.txt"))
        //    {
        //        while (!reader.EndOfStream)
        //        {
        //            string line = reader.ReadLine();

        //            string[] items = line.Split(';');
        //            int number = Convert.ToInt32(items[0]);
        //            string name = items[1];

        //            textureNames.Add(name);
        //        }
        //    }

        //    collisionLayer = ProcessFile(filename, null, null);
        //    collisionLayer.LoadTileTexture(content, textureNames.ToArray());

        //    return collisionLayer;
        //}

        public static CollisionLayer ProcessFile(string filename, Dictionary<string, int> collDict, List<string> collList)
        {
            CollisionLayer CollisionLayer;
            List<List<int>> tempLayout = new List<List<int>>();

            using (StreamReader reader = new StreamReader(filename))
            {
                bool readingLayout = false;


                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;

                    if (line.Contains("[Layout]"))
                    {
                        readingLayout = true;
                    }

                    else if (readingLayout)
                    {
                        List<int> row = new List<int>();

                        string[] cells = line.Split(' ');

                        foreach (string c in cells)
                        {
                            if (!string.IsNullOrEmpty(c))
                            {
                                int index = 0;
                                if (c != "-1")
                                {
                                    string name = "";
                                    foreach (var temp in collDict)
                                    {
                                        if (temp.Value == int.Parse(c))
                                        {
                                            name = temp.Key;
                                            break;
                                        }
                                    }
                                    name = name.Replace(".png", "");
                                    name = "CollisionTiles\\" + name;
                                    
                                    foreach (string listName in collList)
                                    {
                                        if (listName == name)
                                            break;
                                        index++;
                                    }

                                }
                                else
                                    index = int.Parse(c); 
                                row.Add(index);
                            }
                        }

                        tempLayout.Add(row);
                    }
                }
            }

            int width = tempLayout[0].Count;
            int height = tempLayout.Count;

            CollisionLayer = new CollisionLayer(width, height);

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    CollisionLayer.SetCellIndex(x, y, tempLayout[y][x]);
            return CollisionLayer;
        }

        public static CollisionLayer ProcessFile(string filename)
        {
            CollisionLayer CollisionLayer;
            List<List<int>> tempLayout = new List<List<int>>();

            using (StreamReader reader = new StreamReader(filename))
            {
                bool readingLayout = false;


                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;

                    if (line.Contains("[Layout]"))
                    {
                        readingLayout = true;
                    }

                    else if (readingLayout)
                    {
                        List<int> row = new List<int>();

                        string[] cells = line.Split(' ');

                        foreach (string c in cells)
                        {
                            if (!string.IsNullOrEmpty(c))
                            {
                                row.Add(int.Parse(c));
                            }
                        }

                        tempLayout.Add(row);
                    }
                }
            }

            int width = tempLayout[0].Count;
            int height = tempLayout.Count;

            CollisionLayer = new CollisionLayer(width, height);

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    CollisionLayer.SetCellIndex(x, y, tempLayout[y][x]);
            return CollisionLayer;
        }

        //public override int IsUsingTexture(Texture2D texture)
        //{
        //    if (tileTextures.Contains(texture))
        //        return tileTextures.IndexOf(texture);
        //    else
        //        return -1;
        //}

        #endregion
  
        //#region Get/set cellIndex

        //public int GetCellIndex(int x, int y)
        //{
        //    return map[y, x];
        //}

        //public int GetCellIndex(Point point)
        //{
        //    return map[point.Y, point.X];
        //}
        
        
        //public void SetCellIndex(int x, int y, int cellIndex)
        //{
        //    map[y, x] = cellIndex;
        //}

        //public void SetCellIndex(Point point, int cellIndex)
        //{
        //    map[point.Y,point.Y] = cellIndex;
        //}


        //public void RemoveIndex(int existingIndex)
        //{
        //    for (int x = 0; x < Width; x++)
        //    {
        //        for (int y = 0; y < Height; y++)
        //        {
        //            if (map[y, x] == existingIndex)
        //                map[y, x] = -1;
        //            else if (map[y, x] > existingIndex)
        //                map[y, x]--;
        //        }
        //    }
        
        
        //}

        //public void ReplaceIndex(int existingIndex, int newIndex)
        //{
        //    for (int x = 0; x < Width; x++)
        //    {
        //        for (int y = 0; y < Height; y++)
        //        {
        //            if (map[y, x] == existingIndex)
        //                map[y, x] = newIndex;
        //        }
        //    }


        //}

        //#endregion



    }
}
