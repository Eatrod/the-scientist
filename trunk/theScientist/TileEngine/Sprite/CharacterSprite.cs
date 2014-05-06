using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;

namespace TileEngine.Sprite
{
    public class CharacterSprite : AnimatedSprite
    {
        private bool hitFlag;
        private float hitDelay;
        private float elapsedHit;
        
        public bool HitFlag
        {
            get { return hitFlag; }
            set { hitFlag = value; }
        }
        public float HitDelay
        {
            get { return hitDelay; }
            set { hitDelay = value; }
        }
        public float ElapsedHit
        {
            get { return elapsedHit; }
            set { elapsedHit = value; }
        }
        

        public CharacterSprite(Texture2D texture) : base(texture)
        {
            this.hitFlag = false;
            this.hitDelay = 1000f;
            this.elapsedHit = 0.0f;
            this.AttackersDirection = Vector2.Zero;
        }
        public void MovementAfterBeingHit(GameTime gameTime)
        {
            AttackersDirection.Normalize();      
            elapsedHit += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Position -= AttackersDirection * 1.0f;
            if(elapsedHit > hitDelay)
            {
                hitFlag = false;
                elapsedHit = 0.0f;
            }
            
        }
    }
}
