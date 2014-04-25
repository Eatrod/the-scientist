using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileEngine
{
    public class StoryProgress
    {
        #region GameState Region
        public static string lastGameScreen;
        #endregion

        public static Dictionary<string, bool> ProgressLine = new Dictionary<string, bool>();
        static public Dictionary<string, Keys> activeItemsDict = new Dictionary<string, Keys>();
        static public Dictionary<string, int> collectedAmountDict = new Dictionary<string, int>();

        public StoryProgress()
        {
            ProgressLine.Add("asterixTalkedTo", false);
            ProgressLine.Add("lumberjacksTalkedTo", false);
            ProgressLine.Add("belladonnaHave", false);
            ProgressLine.Add("Axe", true);
            ProgressLine.Add("Sword", false);
            ProgressLine.Add("Crossbow", true);
            ProgressLine.Add("Spear", false);
            ProgressLine.Add("DOOM-erang", false);
            ProgressLine.Add("Hammer", false);
            ProgressLine.Add("MetalBladeCrossbow", false);
            ProgressLine.Add("Hookshot", false);
            ProgressLine.Add("treeIsDown", false);
            ProgressLine.Add("contestAgainstJonnyFinished", false);
            ProgressLine.Add("contestAgainstJackFinished", false);
            ProgressLine.Add("contestAgainstJohnFinished", false);
            ProgressLine.Add("immortuiHave", false);

            collectedAmountDict.Add("IronOre", 0);
        }


        #region Methods Region
        public void SetToTrue(string key)
        {
            if(ProgressLine.ContainsKey(key))
            ProgressLine[key] = true;
        }

        #endregion

        #region GetAll/SetAll Region
        public static string GetAllProperties()
        {
            string text = "";
            foreach (var item in ProgressLine)
            {
                text += item.Key + ":" + item.Value + ";";
            }
            text = RemoveLastCharacter(text);
            return text;
        }

        public static string GetAllKeys()
        {
            string text = "";
            foreach (var item in activeItemsDict)
            {
                text += item.Key + ":" + item.Value + ";";
            }
            text = RemoveLastCharacter(text);
            return text;
        }

        public static string RemoveLastCharacter(string text)
        {
            try
            {
                text = text.Remove(text.Count() - 1);
            }
            catch
            {

            }
            return text;
        }

        public static void SetAllProperties(string text)
        {
            string[] rows;
            //text text.Count
            rows = text.Split(';');
            string[] items;
            foreach (var row in rows)
            {
                items = row.Split(':');
                if (ProgressLine.ContainsKey(items[0]))
                    ProgressLine[items[0]] = Convert.ToBoolean(items[1]);
                else
                    ProgressLine.Add(items[0], Convert.ToBoolean(items[1]));
            }
        }

        //CompetitionType Event = (CompetitionType)Enum.Parse(typeof(CompetitionType), sdr["Event"].ToString());
        public static void SetAllKeys(string text)
        {
            string[] rows;
            rows = text.Split(';');
            string[] items;
            foreach (var row in rows)
            {
                items = row.Split(':');
                if (activeItemsDict.ContainsKey(items[0]))
                    activeItemsDict[items[0]] =
                        (Keys)Enum.Parse(typeof(Keys), items[1]);
                else
                {
                    try
                    {
                        activeItemsDict.Add(items[0], (Keys)Enum.Parse(typeof(Keys), items[1]));
                    }
                    catch
                    {

                    }
                }
            }
        }
        #endregion
    }
}
