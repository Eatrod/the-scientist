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
        private float damage;
        private Vector2 goalTarget;
        private Vector2 goalTargetVector;
        private bool boom;
        private float speedOfBomb;
        private float maxSpeedOfBomb;
        public float Damage
        {
            get { return damage; }
            set { damage = value; }
        }
        public bool Boom
        {
            get { return boom; }
            set { boom = value; }
        }
        
        public BombSprite(Texture2D texture, Vector2 goalTarget, Vector2 Position): base(texture)
        {
            this.maxSpeedOfBomb = 3.0f;
            this.speedOfBomb = 0.0f;
            this.goalTarget = goalTarget;
            this.Position = Position;
            this.boom = false;
            this.goalTargetVector = goalTarget - this.Position;
            this.goalTargetVector.Normalize();
            this.damage = 5.0f;
        }
        public void UpdateBomb(GameTime gameTime)
        {
            if(this.speedOfBomb < maxSpeedOfBomb)
            {
                this.speedOfBomb += 0.3f;
            }
            this.Position += goalTargetVector * this.speedOfBomb;
            if(Vector2.Distance(this.Position,this.goalTarget) < 5)
            {
                boom = true;
            }
        }
    }
}
