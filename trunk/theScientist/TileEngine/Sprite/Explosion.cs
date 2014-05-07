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
    public class Explosion:AnimatedSprite
    {
        private float timeToLive;
        private float elapsedTime;
        private bool finished;
        private float damage;
        private Vector2 direction;
        public float Damage
        {
            get { return damage; }
            set { damage = value; }
        }
        public Vector2 Direction
        {
            get { return direction; }
            set 
            {
                direction.Normalize();
                direction = value; 
            }
        }
        public bool Finished
        {
            get { return finished; }
            set { finished = value; }
        }
        public Explosion(Texture2D texture, Vector2 Position, Vector2 Direction): base(texture)
        {
            this.Position = Position;
            this.elapsedTime = 0.0f;
            this.timeToLive = 700f;
            this.finished = false;
            this.damage = 2.0f;
            this.direction = Direction;
            
            FrameAnimation burn = new FrameAnimation(1,12, 12, 0, 0);
            this.Animations.Add("Burn", burn);
            this.CurrentAnimationName = "Burn";
        }
        public void UpdateExplosion(GameTime gameTime)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            this.Position += this.Direction * 5.0f;
            if (elapsedTime > timeToLive)
                finished = true;
            base.Update(gameTime);
        }
        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
