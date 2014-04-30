using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;

namespace TileEngine.Sprite
{
    public class DirtPileSprite: Sprite
    {
        private float elapsedTime;
        private float delayTime;
        private bool finished;
        public bool Finished
        {
            get { return finished; }
            set { finished = value; }
        }
        public float ElapsedTime
        {
            get { return elapsedTime; }
            set { elapsedTime = value; }
        }
        public float DelayTime
        {
            get { return delayTime; }
            set { delayTime = value; }
        }
        public DirtPileSprite(Texture2D texture): base(texture)
        {
            this.DelayTime = 10000f;
            this.ElapsedTime = 0.0f;
            this.Finished = false;
        }
        public void UpdateTheDirtPile(GameTime gameTime)
        {
            ElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(ElapsedTime > DelayTime)
            {
                Finished = true;
                ElapsedTime = 0.0f;
            }
        }
    }
}
