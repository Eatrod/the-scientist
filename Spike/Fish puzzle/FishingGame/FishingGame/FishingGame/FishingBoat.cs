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
    class FishingBoat
    {
        public Texture2D Texture;
        public Vector2 Position;
        public bool UpAndDown;
        public Rectangle BoundingBox;
        public FishingBoat(Texture2D Texture, Vector2 Position)
        {
            this.Texture = Texture;
            this.Position = Position;
            this.UpAndDown = true;
            this.BoundingBox = new Rectangle(
                (int)this.Position.X,
                (int)this.Position.Y,
                this.Texture.Width,
                this.Texture.Height);
        }
        public void UpdatePosition(KeyboardState ks)
        {
            if (ks.IsKeyDown(Keys.Right) && Position.X < (800 - Texture.Width))
                    Position.X += 1.5f;
            else if (ks.IsKeyDown(Keys.Left) && Position.X > 0)       
                Position.X -= 1.5f;
            BoundingBox = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                Texture.Width,
                Texture.Height);
            if (Position.Y < 0)
                UpAndDown = true;
            if (Position.Y > 40)
                UpAndDown = false;
            if (UpAndDown)
                Position.Y += 0.8f;
            if(!UpAndDown)
                 Position.Y -= 0.8f;
               
        }
        public void DrawBoat(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
