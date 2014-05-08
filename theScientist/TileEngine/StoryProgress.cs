using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine.Sprite.Npc;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TileEngine.Sprite.Npc.NPC_Neutral;
using TileEngine.Sprite.Npc.NPC_Story;

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
            AddItemProgressLine("asterixTalkedTo", false);
            AddItemProgressLine("lumberjacksTalkedTo", false);
            AddItemProgressLine("Axe", false);
            AddItemProgressLine("Sword", false);
            AddItemProgressLine("Crossbow", true);
            AddItemProgressLine("Spear", false);
            AddItemProgressLine("DOOM-erang", false);
            AddItemProgressLine("Hammer", false);
            AddItemProgressLine("MetalBladeCrossbow", false);
            AddItemProgressLine("Hookshot", false);
            AddItemProgressLine("treeIsDown", false);
            AddItemProgressLine("contestAgainstJonnyFinished", false);
            AddItemProgressLine("contestAgainstJackFinished", false);
            AddItemProgressLine("contestAgainstJohnFinished", false);
            //Have sektion
            AddItemProgressLine("immortuiHave", false);
            AddItemProgressLine("permitHave", false);
            AddItemProgressLine("alcoholHave", false);
            AddItemProgressLine("belladonnaHave", false);

            AddItemCollectedAmountDict("IronOre", 0);
            AddItemCollectedAmountDict("Money", 0);
            AddItemCollectedAmountDict("Fish", 5);
        }


        #region Methods Region
        public void SetToTrue(string key)
        {
            if(ProgressLine.ContainsKey(key))
            ProgressLine[key] = true;
        }

        private void AddItemProgressLine(string key, bool value)
        {
            if (!ProgressLine.ContainsKey(key))
                ProgressLine.Add(key, value);
            else
                ProgressLine[key] = value;
        }

        private void AddItemCollectedAmountDict(string key, int value)
        {
            if (!collectedAmountDict.ContainsKey(key))
                collectedAmountDict.Add(key, value);
            else
                collectedAmountDict[key] = value;
        }

        public void AddItemCollectedAmount(string key, string value)
        {
            collectedAmountDict[key] += Convert.ToInt32(value);
        }

        public void ChangeScriptsForNPCs(NPC npc)
        {
            //Story NPCs
            if (npc.NPCName == "Bibitur" && ProgressLine["alcoholHave"])
                npc.ChangeScript("alcoholHave");
            if (StoryProgress.ProgressLine["permitHave"] && npc.NPCName == "Guard")
                npc.ChangeScript("permitHave");
            if (StoryProgress.ProgressLine["permitHave"] && npc.NPCName == "Bibitur")
                npc.script = null;
            if (StoryProgress.collectedAmountDict["Fish"] > 0 && npc.NPCName == "Fisherman")
                npc.ChangeScript("fishHave");
            if (StoryProgress.collectedAmountDict["Money"] >= 5 && npc.NPCName == "Innkeeper")
                npc.ChangeScript("moneyHave");

            //Neutrala NPCs
            if (StoryProgress.ProgressLine["asterixTalkedTo"] && npc.GetType() == typeof(NPC_Neutral))
                npc.ChangeScript("asterixTalkedTo");
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
