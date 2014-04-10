using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileGame
{
    class Message
    {
        #region Field Region
        string text;
        bool unlocked;
        #endregion

        #region Property Region
        public string Text
        {
            get { return text; }
            set { this.text = value; }
        }
        public bool Unlocked
        {
            get { return unlocked; }
            set { this.unlocked = value; }
        }
        #endregion

        #region Constructor Region
        public Message()
        {
            this.text = "";
            this.unlocked = false;
        }

        public Message(string text)
        {
            this.text = text;
            this.unlocked = false;
        }

        public Message(string text, bool unlocked)
        {
            this.text = text;
            this.unlocked = unlocked;
        }
        #endregion

        #region Method Region
        #endregion
    }
}
