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

        #region Loading from File code

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

        #endregion
  
    }
}
