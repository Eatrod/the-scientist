using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using XtheSmithLibrary;
using TileGame.GameScreens;

namespace TileGame.Music
{
    public class BackgroundMusic
    {

        protected Song song;
        protected bool songStart;

        public BackgroundMusic(Song song)
        {
            songStart = false;
            this.song = song;
        }

        public Song Song
        {
            get {return song;}
            set { song = value; }
        }

        public bool SongStart
        {
            get { return songStart; }
            set { songStart = value; }
        }
    }
}
