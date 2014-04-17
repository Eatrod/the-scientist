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
    public class NPC_Fighting_Guard : NPC_Fighting
    {
        protected Vector2 startingPosition;
        protected Vector2 vectorTowardsTarget;
        protected Vector2 vectorTowardsStart;
        protected Vector2 aggroStartingPosition;
        protected bool aggro;
        protected bool startingFlag;
        protected bool goingHome;
        protected float aggroRange;
        protected float aggroCircle;
        public Vector2 AggroStartingPosition
        {
            get { return aggroStartingPosition; }
            set { aggroStartingPosition = value; }
        }
        public float AggroCircle
        {
            get { return aggroCircle; }
            set { aggroCircle = value; }
        }
        public float AggroRange
        {
            get { return aggroRange; }
            set { aggroRange = value; }
        }
        public bool StartingFlag
        {
            get { return startingFlag; }
            set { startingFlag = value; }
        }
        public bool GoingHome
        {
            get { return goingHome; }
            set { goingHome = value; }
        }
        public Vector2 VectorTowardsStart
        {
            get 
            { 
                vectorTowardsStart.Normalize();
                return vectorTowardsStart;
            }
            set
            {
                vectorTowardsStart = value;
            }
        }
        public bool Aggro
        {
            get { return aggro; }
            set { aggro = value; }
        }
        public Vector2 VectorTowardsTarget
        {
            get
            {
                vectorTowardsTarget.Normalize();
                return vectorTowardsTarget; 
            }
            set { vectorTowardsTarget = value; }

        }
        public Vector2 StartingPosition
        {
            get { return startingPosition; }
            set { startingPosition = value; }
        }
        public NPC_Fighting_Guard(Texture2D texture, Script script):base(texture,script)
        {
            this.startingFlag = true;
            this.vectorTowardsTarget = Vector2.Zero;
            this.vectorTowardsStart = Vector2.Zero;
            this.aggroStartingPosition = Vector2.Zero;
            this.Aggro = false;
            this.GoingHome = false;
            this.speed = 1.0f;
            this.aggroRange = 100;
            this.aggroCircle = 500;
        }
        public void SetVectorTowardsTargetAndStartAndCheckAggro(AnimatedSprite player)
        {
            vectorTowardsTarget = player.Position - Position;
            vectorTowardsStart = startingPosition - Position;
            if (Vector2.Distance(Position, AggroStartingPosition) > AggroCircle && AggroStartingPosition != Vector2.Zero)
            {
                Aggro = false;
                GoingHome = true;
                AggroStartingPosition = Vector2.Zero;
            }
            else if (Vector2.Distance(player.Position, Position) < AggroRange && !GoingHome &&!Aggro)
            {
                AggroStartingPosition = Position;
                Aggro = true;
            }
            else if(Vector2.Distance(startingPosition,Position) < 10 && GoingHome)
            {
                GoingHome = false;
            }
            

        }
        public override void Update(GameTime gameTime)
        {
            //if (startingFlag)
            //{
            //    this.startingPosition = Position;
            //    startingFlag = false;
            //}
            
            //if (Aggro)
            //{
            //    Position += VectorTowardsTarget * speed;
            //}
            //else if (goingHome)
            //{
            //    Position += VectorTowardsStart * speed;
            //}
            //else
            //    Position = startingPosition;
            base.Update(gameTime);
        }
    }
}
