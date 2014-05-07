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
    public class BombSprite: Sprite
    {
       
        private Vector2 goalTarget;
        private Vector2 goalTargetVector;
        private bool boom;
        private float speedOfBomb;
        private float elapsedFly;
        private float delayFly;
        private float maxSpeedOfBomb;
    
        public bool Boom
        {
            get { return boom; }
            set { boom = value; }
        }
        
        public BombSprite(Texture2D texture, Vector2 goalTarget, Vector2 Position): base(texture)
        {
            this.maxSpeedOfBomb = 5.0f;
            this.speedOfBomb = 0.0f;
            this.goalTarget = goalTarget;
            this.Position = Position;
            this.elapsedFly = 0.0f;
            this.delayFly = 1500f;
            this.boom = false;
            this.goalTargetVector = goalTarget - this.Position;
            this.goalTargetVector.Normalize();
        }
        public void UpdateBomb(GameTime gameTime)
        {
            if(this.speedOfBomb < maxSpeedOfBomb)
            {
                this.speedOfBomb += 0.1f;
            }
            this.Position += goalTargetVector * this.speedOfBomb;
            elapsedFly +=(float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(elapsedFly > delayFly)
            {
                boom = true;
            }
        }
    }
}
