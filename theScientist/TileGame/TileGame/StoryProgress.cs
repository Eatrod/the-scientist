using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using XtheSmithLibrary;
using XtheSmithLibrary.Controls;
using TileGame;

namespace TileGame
{
    public class StoryProgress
    {
        #region GameState Region
        public static string lastGameScreen;
        #endregion

        public static Dictionary<string, bool> ProgressLine = new Dictionary<string, bool>();
        static public Dictionary<string, Keys> activeItemsDict = new Dictionary<string, Keys>();

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
            ProgressLine.Add("contestAgainstJonnyFinnished", false);
            ProgressLine.Add("contestAgainstJackFinnished", false);
            ProgressLine.Add("contestAgainstJohnFinnished", false);
        }


        #region Methods Region
        public void SetToTrue(string key)
        {
            if(ProgressLine.ContainsKey(key))
            ProgressLine[key] = true;
        }

        #endregion

        #region GetAll/SetAll Region
        public static string GetAll()
        {
            string text = "";
            foreach (var item in ProgressLine)
            {
                text += item.Key + ":" + item.Value + ";";
            }
            return text;
        }

        public static void SetAll(string text)
        {
            string[] rows;
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
        #endregion
    }
}
