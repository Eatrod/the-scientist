using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Sprite
{
    public class AnimatedSprite : BaseSprite
    {
        public Dictionary<string, FrameAnimation> Animations =
            new Dictionary<string, FrameAnimation>();

        private bool aggro;
        private Vector2 aggroStartingPosition;
        private bool hitByArrow;
        private float elapsedHitByArrow;
        private float delayHitByArrow;
        private Vector2 arrowDirection;


        string currentAnimation = null;
        bool animating = true;
        bool takingDamage = false; //kanske flyttas till bas sprite 

        protected float fullhp;

        protected float damage = 0;
        protected float life = 0;
        protected float stamina = 0;
        protected float charge = 0;

        double oldTime = 0;  //taking damage see kommentar.
        public Vector2 ArrowDirection
        {
            get 
            {
                arrowDirection.Normalize();
                return arrowDirection; 
            }
            set { arrowDirection = value; }
        }
        public bool HitByArrow
        {
            get { return hitByArrow; }
            set { hitByArrow = value; }
        }
        public float ElapsedHitByArrow
        {
            get { return elapsedHitByArrow; }
            set { elapsedHitByArrow = value; }
        }
        public float DelayHitByArrow
        {
            get { return delayHitByArrow; }
            set { delayHitByArrow = value; }
        }
        public Vector2 AggroStartingPosition
        {
            get { return aggroStartingPosition; }
            set { aggroStartingPosition = value; }
        }
        public bool Aggro
        {
            get { return aggro; }
            set { aggro = value; }
        }
        public bool Animating
        {
            get { return animating; }
            set { }
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

        public bool isAnimating
        {
            get { return animating; }
            set { animating = value; }
        }

        public bool areTakingDamage
        {
            get { return takingDamage; }
            set { takingDamage = value; }
        }  //kanske flyttas

        public float Damage
        {
            get { return damage; }
            set { damage = value; }
        }
        public float Life
        {
            get { return life; }
            set { life = value; }
        }

        public float FullHp
        {
            get { return fullhp; }
            set { fullhp = value; }
        }

        public float Stamina
        {
            get { return stamina; }
            set { stamina = value; }
        }

        public float Charge
        {
            get { return charge; }
            set { charge = value; }
        }
      
        public FrameAnimation CurrentAnimation
        {
            get
            {
                if (!string.IsNullOrEmpty(currentAnimation))
                    return Animations[currentAnimation];
                else
                    return null;
            }
        }

        public string CurrentAnimationName
        {
            get { return currentAnimation; }
            set 
            {
                if (Animations.ContainsKey(value))
                    currentAnimation = value;
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

        public void ReducingHealth()
        {
            if (takingDamage)
            {
                life -= damage;
                if (life < 0)
                    life = 0;
            }
        }

        public void SettingSpriteBlink(GameTime gameTime)
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
        public void HitByArrowMethod(AnimatedProjectile sprite)
        {
            Aggro = true;
            HitByArrow = true;
            AggroStartingPosition = this.Position;
            if (sprite.CurrentAnimationName == "right" ||
                sprite.CurrentAnimationName == "right2" ||
                sprite.CurrentAnimationName == "right3")
                ArrowDirection = new Vector2(1, 0);
            if (sprite.CurrentAnimationName == "left" ||
                sprite.CurrentAnimationName == "left2" ||
                sprite.CurrentAnimationName == "left3")
                ArrowDirection = new Vector2(-1, 0);
            if (sprite.CurrentAnimationName == "up" ||
                sprite.CurrentAnimationName == "up2" ||
                sprite.CurrentAnimationName == "up3")
                ArrowDirection = new Vector2(0, -1);
            if (sprite.CurrentAnimationName == "down" ||
                sprite.CurrentAnimationName == "down2" ||
                sprite.CurrentAnimationName == "down3")
                ArrowDirection = new Vector2(0, 1);
            
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

        public void Draw_test(SpriteBatch spriteBatch, int x, int y, int width, int height)
        {
            FrameAnimation animation = CurrentAnimation;

            if (animation != null)
            {
                spriteBatch.Draw(
                    texture,
                    new Rectangle(x, y, width, height),
                    animation.CurrentRectangle,
                    Color.White);
            }
        }

        public AnimatedSprite(Texture2D texture) : base(texture)
        {
            //this.texture = texture;
        }  //konstruktor 
    }
}
