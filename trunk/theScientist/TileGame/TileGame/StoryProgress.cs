using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileGame
{
    public class StoryProgress
    {
        #region GameState Region
        public static string lastGameScreen;
        #endregion

        #region Talk To Region
        public static bool asterixTalkedTo = false;
        #endregion

        #region
        public static bool belladonnaHave = false;
        #endregion

        #region Methods Region
        public void SetAsterixTalkedTo()
        {
            asterixTalkedTo = true;
        }
        #endregion

        #region GetAll/SetAll Region
        public static string GetAll()
        {
            string text = "";
            text += "asterixTalkedTo" + ":" + asterixTalkedTo.ToString() + ";";
            text += "belladonnaHave" + ":" + belladonnaHave.ToString();
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
                if (items[0].Equals("asterixTalkedTo"))
                {
                    if (items[1].Equals("True"))
                        asterixTalkedTo = true;
                    else
                        asterixTalkedTo = false;
                }
                if (items[0].Equals("belladonnaHave"))
                {
                    if (items[1].Equals("True"))
                        belladonnaHave = true;
                    else
                        belladonnaHave = false;
                }
            }
        }
        #endregion
    }
}
