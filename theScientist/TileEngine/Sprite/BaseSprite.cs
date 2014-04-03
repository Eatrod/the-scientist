﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.Sprite
{
    public class BaseSprite
    {
        protected Texture2D texture;

        protected float Alpha = 255.0f;
        protected float radius = 10f;
        protected float speed = 2f;
        protected bool faded = false;

        public Vector2 Position = Vector2.Zero;
        public Vector2 Origionoffset = Vector2.Zero;

        public Vector2 Origin
        {
            get { return Position + Origionoffset; }
        }

        public virtual Vector2 Center
        {
            get;
            set;
        } 

        public virtual Rectangle Bounds
        {
            get;
            set;   
        } 

        public float Speed
        {
            get { return speed; }
            set { speed = (float)Math.Max(value, .1f); }
        }

        public float CollisionRadius
        {
            get { return radius; }
            set { radius = (float)Math.Max(value, 1f); }
        }

        public static bool AreColliding(BaseSprite a, BaseSprite b)
        {
            Vector2 d = b.Origin - a.Origin;

            return (d.Length() < b.CollisionRadius + a.CollisionRadius);
        }

        public void SetSpritePositionInGameWorld(Vector2 position)
        {
            this.Position.X = position.X * TileEngine.Engine.TileWidth;
            this.Position.Y = position.Y * TileEngine.Engine.TileHeight;
        }

        public virtual void ClampToArea(int width, int height)
        {  
        }

        public virtual void Update(GameTime gameTime)
        {       
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {           
        }

    }
}
