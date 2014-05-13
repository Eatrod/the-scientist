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
    public class MoleHandlerSprite:AnimatedSprite
    {
        private List<MoleSprite> moles;
        private Random random;
        private float delaySpawnMole;
        private float elapsedSpawnMole;
        private bool spawnFlag;
        public List<MoleSprite> Moles
        {
            get { return moles; }
        }
        public bool SpawnFlag
        {
            get { return spawnFlag; }
            set { spawnFlag = value; }
        }


        public MoleHandlerSprite(Texture2D texture, Random random):base(texture)
        {
            this.spawnFlag = false;
            this.random = random;
            this.elapsedSpawnMole = 0.0f;
            this.delaySpawnMole = random.Next(500, 3500);
            this.moles = new List<MoleSprite>();
        }
        public override void Update(GameTime gameTime)
        {
            elapsedSpawnMole += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(elapsedSpawnMole > delaySpawnMole)
            {
                this.spawnFlag = true;
                this.elapsedSpawnMole = 0.0f;
                this.delaySpawnMole = random.Next(500, 3500);
                
            }
            base.Update(gameTime);
        }
    }
}
