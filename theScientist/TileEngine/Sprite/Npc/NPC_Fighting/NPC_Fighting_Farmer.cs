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

namespace TileEngine.Sprite.Npc.NPC_Fighting
{
    public class NPC_Fighting_Farmer: NPC_Fighting
    {
        private Random random;
        private float delayDirection;
        private float elapsedDirection;
        private int direction;
        private Vector2 motion;
        private Vector2 oldMotion;
        private bool collided;
        public bool Collided
        {
            get { return collided; }
            set { collided = value; }
        }
        public Vector2 Motion
        {
            get { return motion; }
            set { motion = value; }
        }
        public NPC_Fighting_Farmer(Texture2D texture, Script script, Random random)
            : base(texture, script)
        {
            this.collided = false;
            this.random = random;
            this.speed = 0.2f;
            this.direction = 3;
            this.delayDirection = 5000f;
            this.motion = Vector2.Zero;
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if(motion != Vector2.Zero)
            {
                oldMotion = motion;
            }
            elapsedDirection += (float)gameTime.ElapsedGameTime.TotalMilliseconds; 
            if(collided)
            {
                direction += 2;
                direction = direction % 4;
                elapsedDirection = 0.0f;
                collided = false;
            }
            else if ((elapsedDirection > delayDirection))
            {
                direction = random.Next(0, 4);
                elapsedDirection = 0.0f;
            }
            
            if (direction == 0)
            {
                this.motion = new Vector2(1, 0);
            }
            else if (direction == 1)
            {
                this.motion = new Vector2(0, -1);
            }
            else if (direction == 2)
            {
                this.motion = new Vector2(-1, 0);
            }
            else if (direction == 3)
            {
                this.motion = new Vector2(0, 1);
            }
            Position += speed * motion;
            
            //if (ElapsedRandom > DelayRandom)
            //{
            //    this.Angle = number * 90;
            //    this.ElapsedRandom = 0;
            //}
            //if (Angle == 180)
            //{
            //    if (Position.X < 0)
            //    {
            //        Position.X = 0;
            //        Angle = 0;
            //    }
            //    this.Yframe = 1;
            //}
            //else if (Angle == 0)
            //{
            //    if (Position.X + (TexturePlayer.Width / 4) > graphic.Viewport.Width)
            //    {
            //        Position.X = graphic.Viewport.Width - (TexturePlayer.Width / 4);
            //        Angle = 180;
            //    }
            //    Yframe = 2;
            //}
            //else if (Angle == 90)
            //{
            //    if (Position.Y < 0)
            //    {
            //        Position.Y = 0;
            //        Angle = 270;
            //    }
            //    Yframe = 3;
            //}
            //else if (Angle == 270)
            //{
            //    if (Position.Y + (TexturePlayer.Height / 4) > graphic.Viewport.Height)
            //    {
            //        Position.Y = graphic.Viewport.Height - (TexturePlayer.Height / 4);
            //        Angle = 90;
            //    }
            //    Yframe = 0;
            //}
            //this.Direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(Angle)), (float)Math.Sin(MathHelper.ToRadians(-Angle)));
            //this.Position += this.Direction * 0.5f;
            base.Update(gameTime);
        }
    }
}
