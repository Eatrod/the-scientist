using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Sprite.Projectiles
{
    public class NormalArrowProjectile : AnimatedProjectile
    {

        //float damageofprojectile = 5;

        string currentAnimation = null;
       
        bool takingDamage = false;      //kanske flyttas till bas sprite 

        

        double oldTime = 0;  //taking damage see kommentar.

        public float DamageOfProjectile
        { 
            get{return damageofprojectile;}
        }

        public override Vector2 Center
        {
            get 
            {
                return Position + new Vector2(
                    CurrentAnimation.CurrentRectangle.Width /2 ,
                    CurrentAnimation.CurrentRectangle.Height /2 );
            }
        } 

        public override Rectangle Bounds
        {
            get
            {
                Rectangle rect = CurrentAnimation.CurrentRectangle;
                rect.X = (int)Position.X;
                rect.Y = (int)Position.Y;
                return rect;
            }
        } 

        
       

        public override void ClampToArea(int width, int height)
        {
            if (Position.X < 0)
                Position.X = 0;

            if (Position.Y < 0)
                Position.Y = 0;

            if (Position.X > width - CurrentAnimation.CurrentRectangle.Width)
                Position.X = width - CurrentAnimation.CurrentRectangle.Width;

            if (Position.Y > height - CurrentAnimation.CurrentRectangle.Height)
                Position.Y = height - CurrentAnimation.CurrentRectangle.Height;
        }  

        public override void Update(GameTime gameTime)
        {
            FrameAnimation animation = CurrentAnimation;

            if (animation == null)
            {
                if (Animations.Count > 0)
                {
                    string[] keys = new string[Animations.Count];
                    Animations.Keys.CopyTo(keys, 0);

                    currentAnimation = keys[0];
                    animation = CurrentAnimation;
                }
                else
                    return;
            }
            SettingSpriteBlink(gameTime);
            ReducingHealth();
            animation.Update(gameTime);
        }

        private void ReducingHealth()
        {
            if (takingDamage)
            {
                life -= damage;
                if (life < 0)
                    life = 0;
            }
        }

        private void SettingSpriteBlink(GameTime gameTime)
        {
            if ((gameTime.TotalGameTime.TotalMilliseconds - oldTime) > 150)
            {
                if (takingDamage)
                {
                    if (faded)
                    {
                        Alpha = 255.0f;
                        faded = false;
                    }
                    else
                    {
                        Alpha = 0.0f;
                        faded = true;
                    }
                }
                else
                {
                    Alpha = 255.0f;
                    faded = false;
                }
                oldTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            FrameAnimation animation = CurrentAnimation;

            if (animation != null)
            {
                spriteBatch.Draw(
                    texture,
                    Position,
                    animation.CurrentRectangle,
                    new Color(255, 255, 255, (byte)MathHelper.Clamp(Alpha, 0, 255)));
            }
        }

   
        public NormalArrowProjectile(Texture2D texture, float life, float timetolive, float speed, Vector2 position)
            : base(texture) 
        {
            this.damageofprojectile = 20;
            this.life = life;
            this.timetolive = timetolive;
            this.speed = speed;
            this.Position = position;
            this.continueafterHit = false;
            this.spinaxe = false;
            FrameAnimation right = new FrameAnimation(1, 25, 25, 0, 0);
            this.Animations.Add("right", right);
            FrameAnimation left = new FrameAnimation(1, 25, 25, 25, 0);
            this.Animations.Add("left", left);
            FrameAnimation down = new FrameAnimation(1, 25, 25, 50, 0);
            this.Animations.Add("down", down);
            FrameAnimation up = new FrameAnimation(1, 25, 25, 75, 0);
            this.Animations.Add("up", up);
        }  //konstruktor 
    }
}
