using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FishingGame
{
    class Fish
    {
        public bool Special;
        public bool Dangerous;
        public Texture2D TextureLeft;
        public Texture2D TextureRight;
        public Texture2D TextureCapturedFish;
        public Texture2D Texture;
        public Vector2 Position;
        public Color ColorFish;
        public Vector2 Direction;
        public float Speed;
        public int Angle;
        public bool Hooked;
        public bool Alive;
        public Rectangle BoundingBox;
        Random rand;
        public Fish(Texture2D SpecialLeft, Texture2D SpecialRight, Texture2D SpecialCaptured,
            Texture2D TextureSharkLeft, Texture2D TextureSharkRight, Texture2D TextureCapturedFish,
            Texture2D TextureLeft, Texture2D TextureRight,
            Vector2 Position, Color Color, float Speed, int Angle)
        {
            if (Color == Color.Pink)
                this.Dangerous = true;
            else
                this.Dangerous = false;
            if (Color == Color.Black)
                Special = true;
            else
                this.Special = false;
            if (Dangerous)
            {
                this.TextureRight = TextureSharkRight;
                this.TextureLeft = TextureSharkLeft;
                this.TextureCapturedFish = TextureCapturedFish;
            }
            else if (Special)
            {
                this.TextureLeft = SpecialLeft;
                this.TextureRight = SpecialRight;
                this.TextureCapturedFish = SpecialCaptured;
            }
            else if (!Dangerous && !Special)
            {
                this.TextureRight = TextureRight;
                this.TextureLeft = TextureLeft;
                this.TextureCapturedFish = TextureCapturedFish;
            }
            if (Angle == 0)
            {
                Texture = this.TextureRight;
            }
            else
                Texture = this.TextureLeft;    
            this.Position = Position;
            this.ColorFish = Color;
            this.Direction = Vector2.Zero;
            this.Speed = Speed;
            this.Angle = Angle;
            this.Alive = true;
            this.Hooked = false;
            this.BoundingBox = new Rectangle((int)this.Position.X,(int) this.Position.Y, Texture.Width, Texture.Height);
            this.rand = new Random();
        }
        public void UpdateFish(FishingRod fishingrod)
        {
            if (fishingrod.CurrentFish != null &&
                this.Dangerous &&
                Vector2.Distance(this.Position, fishingrod.PositionHook) < 100 &&
                Angle == 180)
                    this.Direction = new Vector2(this.Position.X - fishingrod.PositionHook.X,
                        this.Position.Y - fishingrod.PositionHook.Y);
            else if (fishingrod.CurrentFish != null &&
                this.Dangerous &&
                Vector2.Distance(new Vector2(this.Position.X + this.Texture.Width, this.Position.Y), fishingrod.PositionHook) < 100 &&
                Angle == 0)
                this.Direction = new Vector2(fishingrod.PositionHook.X - (this.Position.X + this.Texture.Width - 10),
                    fishingrod.PositionHook.Y - this.Position.Y);
            if (!Hooked)
            {
                if (Vector2.Distance(this.Position, fishingrod.PositionHook) < 100 && this.Dangerous && fishingrod.CurrentFish != null && Angle == 180)
                {
                    this.Position -= (Direction * 0.1f);
                }
                else if (Vector2.Distance(new Vector2(this.Position.X + this.Texture.Width, this.Position.Y), fishingrod.PositionHook) < 100 && this.Dangerous && fishingrod.CurrentFish != null && Angle == 0)
                {
                    this.Position += (Direction * 0.1f);
                }
                else
                {
                    this.Direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(Angle)), (float)Math.Sin(MathHelper.ToRadians(Angle)));
                    Position.X += Speed * Direction.X;
                    Position.Y += Speed * Direction.Y;
                    if (Angle == 180 && Position.X < -50)
                        Alive = false;
                    else if (Angle == 0 && Position.X > 850)
                        Alive = false;
                }
                
            }
            else if (Hooked)
            {
                Texture = TextureCapturedFish;
                this.Position = fishingrod.PositionHook;
            }
            this.BoundingBox = new Rectangle((int)this.Position.X, (int)this.Position.Y, Texture.Width, Texture.Height);
            
        }
        public void DrawFish(SpriteBatch spriteBatch)
        {
            if (Special)
            {
                if (!Hooked)
                    spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height), Color.White);
                else
                    spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height), Color.White);
            }
            else if (Dangerous)
            {
                if (!Hooked)
                    spriteBatch.Draw(Texture, BoundingBox, Color.White);
                else
                    spriteBatch.Draw(Texture, BoundingBox, Color.White);
            }
            else if (!Special)
                spriteBatch.Draw(Texture, Position, ColorFish * 0.8f);
            
        }

    }
}
