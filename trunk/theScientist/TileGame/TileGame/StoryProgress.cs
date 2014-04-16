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
            text += "asterixTalkedTo" + ":" + asterixTalkedTo.ToString();
            text += "belladonnaHave" + ":" + belladonnaHave.ToString();
            return text;
        }
        #endregion
    }
}
