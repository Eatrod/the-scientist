using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine.Tiles;
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
        private float elapsedAggro;
        private float delayAggro;
        private float elapsedHitWall;
        private float delayHitWall;
        private bool running;
        private int direction;
        private Vector2 motion;
        private Vector2 attackDirection;
        private Point cellPosition;
        private bool collided;
        private bool aggro;
        private bool hitwall;
        public bool HitWall
        {
            get { return hitwall; }
            set { hitwall = value; }
        }
        public bool Running
        {
            get { return running; }
            set { running = value; }
        }
        public Vector2 AttackDirection
        {
            get { return attackDirection; }
            set { attackDirection = value; }
        }
        public bool Aggro
        {
            get { return aggro; }
            set { aggro = value; }
        }
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
            this.elapsedHitWall = 0;
            this.delayHitWall = 1000f;
            this.hitwall = false;
            this.elapsedAggro = 0;
            this.elapsedDirection = 0;
            this.delayAggro = 2000f;
            this.aggro = false;
            this.collided = false;
            this.random = random;
            this.speed = 3.0f;
            this.direction = 0;
            this.cellPosition = Engine.ConvertPostionToCell(Position);
            this.delayDirection = 5000f;
            this.motion = Vector2.Zero;
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            cellPosition = Engine.ConvertPostionToCell(Position);
            if (cellPosition.X >= 20)
            {
                running = false;
                Position.X -= 10f;
                direction += 180;
                direction = direction % 360;
            }
            elapsedDirection += (float)gameTime.ElapsedGameTime.TotalMilliseconds; 
            if(collided && !hitwall)
            {
                if (running)
                {
                    hitwall = true;
                    elapsedHitWall = 0;
                }
                direction += 180;
                direction = direction % 360;
                elapsedDirection = 0.0f;
                running = false;
                aggro = false;
                collided = false;
                
            }
            else if ((elapsedDirection > delayDirection) && !aggro && !running && !hitwall)
            {
                direction = random.Next(0, 360);
                elapsedDirection = 0.0f;
            }
            
            this.motion = new Vector2(
                (float)Math.Cos(MathHelper.ToRadians(direction)),
                (float)Math.Sin(MathHelper.ToRadians(-direction)));
            if(aggro)
            {
                elapsedAggro += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if(elapsedAggro > delayAggro)
                {
                    running = true;
                    aggro = false;
                    elapsedAggro = 0;
                }
            }
            else if (running)
            {
                attackDirection.Normalize();
                speed = 3.0f;
                Position += attackDirection * speed;
            }
            else if(hitwall)
            {
                elapsedHitWall += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                speed = 0.5f;
                Position -= attackDirection * speed;
                if (elapsedHitWall > delayHitWall)
                {
                    hitwall = false;          
                }
            }
            else
            {
                speed = 3.0f;
                running = false;
                aggro = false;
                Position += speed * motion;
            }
            
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
