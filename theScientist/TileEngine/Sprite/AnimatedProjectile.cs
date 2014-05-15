using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Sprite
{
    public class AnimatedProjectile : AnimatedSprite
    {

        public float damageofprojectile;

        public bool continueafterHit { get; set; }
        public bool spinaxe { get; set; }
        
        string currentAnimation = null;
       
        bool takingDamage = false;      //kanske flyttas till bas sprite 

        protected float timetolive = 0;

        double oldTime = 0;  //taking damage see kommentar.

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

        public void updateprojectileposition()
        {
            this.Life -= this.timetolive;
            if (this.Life > 0)
            {
                if ((this.CurrentAnimationName == "right") || (this.CurrentAnimationName == "right2") || (this.CurrentAnimationName == "right3"))
                    this.Position.X += 1 * this.Speed;
                if ((this.CurrentAnimationName == "left") || (this.CurrentAnimationName == "left2") || (this.CurrentAnimationName == "left3"))
                    this.Position.X -= 1 * this.Speed;
                if ((this.CurrentAnimationName == "down") || (this.CurrentAnimationName == "down2") || (this.CurrentAnimationName == "down3"))
                    this.Position.Y += 1 * this.Speed;
                if ((this.CurrentAnimationName == "up") || (this.CurrentAnimationName == "up2") || (this.CurrentAnimationName == "up3"))
                    this.Position.Y -= 1 * this.Speed;
            }

            if (this.continueafterHit && this.Life <= 0)
            {
                if ((this.CurrentAnimationName == "right") || (this.CurrentAnimationName == "right2") || (this.CurrentAnimationName == "right3"))
                    this.Position.X -= 1 * this.Speed;
                if ((this.CurrentAnimationName == "left") || (this.CurrentAnimationName == "left2") || (this.CurrentAnimationName == "left3"))
                    this.Position.X += 1 * this.Speed;
                if ((this.CurrentAnimationName == "down") || (this.CurrentAnimationName == "down2") || (this.CurrentAnimationName == "down3"))
                    this.Position.Y -= 1 * this.Speed;
                if ((this.CurrentAnimationName == "up") || (this.CurrentAnimationName == "up2") || (this.CurrentAnimationName == "up3"))
                    this.Position.Y += 1 * this.Speed;
            }
        }

        public void UpdatecurrentAnimation(Vector2 motion)
        {
            float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

            if (motionAngle >= -MathHelper.PiOver4 && motionAngle <= MathHelper.PiOver4)
            {
                this.CurrentAnimationName = "right"; //Right

            }
            else if (motionAngle >= MathHelper.PiOver4 && motionAngle <= 3f * MathHelper.PiOver4)
            {
                this.CurrentAnimationName = "down"; //Down

            }
            else if (motionAngle <= -MathHelper.PiOver4 && motionAngle >= -3f * MathHelper.PiOver4)
            {
                this.CurrentAnimationName = "up"; // Up

            }
            else
            {
                this.CurrentAnimationName = "left"; //Left

            }
        }
        

        public AnimatedProjectile(Texture2D texture) : base(texture)
        {

        }  //konstruktor 

       
    }
}
