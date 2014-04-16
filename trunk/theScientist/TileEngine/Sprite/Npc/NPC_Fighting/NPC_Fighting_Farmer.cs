using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine.Tiles;
using TileEngine.Sprite;
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
        private float chargeDamage;
        private bool running;
        private int direction;
        private Vector2 motion;
        private Vector2 attackDirection;
        private Point cellPosition;
        private bool collided;
        private bool aggro;

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
            this.chargeDamage = 5;
            this.elapsedAggro = 0;
            this.elapsedDirection = 0;
            this.delayAggro = 2000f;
            this.aggro = false;
            this.collided = false;
            this.random = random;
            this.speed = 0.5f;
            this.direction = 0;
            this.cellPosition = Engine.ConvertPostionToCell(Position);
            this.delayDirection = 5000f;
            this.motion = Vector2.Zero;
        }
        public void CheckForCollisionWithOtherNPCs(List<AnimatedSprite> Npcs, CharacterSprite player)
        {
            foreach (NPC_Fighting_Farmer npc in Npcs)
            {
                if(npc.Bounds.Intersects(this.Bounds) && npc.Bounds != this.Bounds && !npc.Running && !npc.Aggro)
                {
                    this.running = false;
                    this.aggro = false;
                    this.HitFlag = true;
                    npc.AttackersDirection = -this.AttackersDirection;
                    npc.HitFlag = true;
                    this.elapsedAggro = 0;
                    break;
                }
                if (player.Bounds.Intersects(this.Bounds))
                {
                    this.running = false;
                    this.aggro = false;
                    this.HitFlag = true;
                    player.Life -= chargeDamage;
                    player.AttackersDirection = -this.AttackersDirection;
                    player.HitFlag = true;
                    this.elapsedAggro = 0;
                    break;
                }
                
            }
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            cellPosition = Engine.ConvertPostionToCell(Position);
            if (cellPosition.X >= 20 && cellPosition.Y <= 15)
            {
                Position.X -= 5f;
                collided = true;
            }
            elapsedDirection += (float)gameTime.ElapsedGameTime.TotalMilliseconds; 
            if(collided && !HitFlag)
            {
                if (running)
                {
                    HitFlag = true;
                    ElapsedHit = 0;
                }
                direction += 180;
                direction = direction % 360;
                running = false;
                aggro = false;
                collided = false;
                
            }
            else if ((elapsedDirection > delayDirection) && !aggro && !running && !HitFlag)
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
                speed = 5.0f;
                Position += AttackersDirection * speed;//speed;
            }
            else if(HitFlag)
            {
                 MovementAfterBeingHit(gameTime);
            }
            else
            {
                speed = 0.5f;
                running = false;
                aggro = false;
                Position += speed * motion;
            }
            base.Update(gameTime);
        }
    }
}
