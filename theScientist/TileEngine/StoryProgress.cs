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
        public static Dictionary<string, string> ProgressType = new Dictionary<string, string>();
        public static List<string> CurrentTasks = new List<string>();
        static public Dictionary<string, Keys> activeItemsDict = new Dictionary<string, Keys>();
        static public Dictionary<string, int> collectedAmountDict = new Dictionary<string, int>();

        //Item string names
        public static string Axe = "Axe";
        public static string Sword = "Sword";
        public static string Crossbow = "Crossbow";
        public static string Spear = "Spear";
        public static string DOOMerang = "DOOM-erang";
        public static string Hammer = "Hammer";
        public static string MetalBladeCrossbow = "MetalBladeCrossbow";
        public static string Hookshot = "Hookshot";
        public static string Immortui = "Immortui";
        public static string Permit = "Permit";
        public static string Alcohol = "Alcohol";
        public static string Belladonna = "Belladonna";
        public static string Fish = "Fish";

        //Quests string names
        public static string treeIsDown = "treeisDown";
        public static string contestAgainstJohnnyFinished = "contestAgainstJohnnyFinished";
        public static string contestAgainstJackFinished = "contestAgainstJackFinished";
        public static string contestAgainstJohnFinished = "contestAgainstJohnFinished";
        public static string asterixTalkedTo = "asterixTalkedTo";

        //Flags string names
        public static string lumberjacksTalkedTo = "lumberjacksTalkedTo";

        //Flags string names

        public StoryProgress()
        {
            ProgressLine.Clear();
            activeItemsDict.Clear();
            collectedAmountDict.Clear();
            //items
            AddItemProgressLine(Axe, false, "Item");
            AddItemProgressLine(Sword, true, "Item");
            AddItemProgressLine(Crossbow, true, "Item");
            AddItemProgressLine(Spear, false, "Item");
            AddItemProgressLine(DOOMerang, false, "Item");
            AddItemProgressLine(Hammer, false, "Item");
            AddItemProgressLine(MetalBladeCrossbow, false, "Item");
            AddItemProgressLine(Hookshot, false, "Item");
            AddItemProgressLine(Immortui, false, "Item");
            AddItemProgressLine(Permit, false, "Item");
            AddItemProgressLine(Alcohol, false, "Item");
            AddItemProgressLine(Belladonna, false, "Item");
            AddItemProgressLine(Fish, false, "Item");
            //Quests
            AddItemProgressLine("treeIsDown", false, "Quest:Cut down the tree");
            AddItemProgressLine("contestAgainstJohnnyFinished", false, "Quest:Beat Johnnys challenge");
            AddItemProgressLine("contestAgainstJackFinished", false, "Quest:Do Jacks errand");
            AddItemProgressLine("contestAgainstJohnFinished", false, "Quest:Solve Johns riddle");
            AddItemProgressLine("asterixTalkedTo", false, "Quest:Talk to Asterix");
            //Flaggor
            AddItemProgressLine("lumberjacksTalkedTo", false, "Flag");
            AddItemProgressLine("gateGuardsTalkedTo", false, "Flag");
            AddItemProgressLine("leavingTown", false, "Flag");

            AddItemProgressLine("CollectMinigame", false, "Minigame");

            AddItemCollectedAmountDict("IronOre", 0);
            AddItemCollectedAmountDict("Money", 0);
            CurrentTasks.Add("Talk to Asterix");
            //AddItemCollectedAmountDict("Fish", 0);
        }


        #region Methods Region
        public void SetToTrue(string key)
        {
            if(ProgressLine.ContainsKey(key))
            ProgressLine[key] = true;
        }

        public void SetToFalse(string key)
        {
            if (ProgressLine.ContainsKey(key))
                ProgressLine[key] = false;
        }

        private void AddItemProgressLine(string key, bool value, string type)
        {
            if (!ProgressLine.ContainsKey(key))
            {
                ProgressLine.Add(key, value);
                AddItemProgressType(key, type);
            }
            else
                ProgressLine[key] = value;
        }

        private void AddItemProgressType(string key, string type)
        {
            if (!ProgressType.ContainsKey(key))
                ProgressType.Add(key, type);
            else
                ProgressType[key] = type;
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

        public void AddCurrentTask(string task)
        {
            CurrentTasks.Add(task);
        }

        public void ChangeScriptsForNPCs(NPC npc)
        {
            //Story NPCs
            if (npc.NPCName == "Bibitur" && ProgressLine["Alcohol"])
                npc.ChangeScript("Alcohol");
            if (StoryProgress.ProgressLine["Permit"] && npc.NPCName == "Guard")
                npc.ChangeScript("Permit");
            if (StoryProgress.ProgressLine["Permit"] && npc.NPCName == "Bibitur")
                npc.script = null;
            if (StoryProgress.ProgressLine["Fish"] && npc.NPCName == "Fisherman") //(StoryProgress.collectedAmountDict["Fish"] > 0 && npc.NPCName == "Fisherman")
            {
                npc.ChangeScript("fishHave");
            }
            else 
            {
                if (npc.NPCName == "Fisherman")
                    npc.ChangeScript("default");
            }
            //Lumberjacks
            if (StoryProgress.ProgressLine["CollectMinigame"] && npc.NPCName == "Jack")
                npc.RemoveHandler("Potato!?");
            if (StoryProgress.collectedAmountDict["IronOre"] >= 200 && npc.NPCName == "Jack")
                npc.ChangeScript("IronOre");
            if (StoryProgress.ProgressLine["Axe"] && npc.NPCName == "Jack")
                npc.script = null;
            //Innkeeper
            if (StoryProgress.collectedAmountDict["Money"] >= 5 && StoryProgress.ProgressLine["Belladonna"] && npc.NPCName == "Innkeeper")
            {
                npc.ChangeScript("moneyHave");
            }
            else if (StoryProgress.collectedAmountDict["Money"] >= 5 && npc.NPCName == "Innkeeper")
            {
                npc.ChangeScript("belladonnaHave");
            }
            else
            {
                if (npc.NPCName == "Innkeeper")
                    npc.ChangeScript("default");
            }

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
