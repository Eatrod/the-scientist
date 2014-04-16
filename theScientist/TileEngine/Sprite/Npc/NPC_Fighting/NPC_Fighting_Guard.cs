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
        private Vector2 startingPosition;
        private Vector2 vectorTowardsTarget;
        private Vector2 vectorTowardsStart;
        private bool aggro;
        private bool startingFlag;
        private bool goingHome;
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
            //this.startingPosition = Position;
            this.vectorTowardsTarget = Vector2.Zero;
            this.vectorTowardsStart = Vector2.Zero;
            this.Aggro = false;
            this.GoingHome = false;
            this.speed = 1.0f;
        }
        public void SetVectorTowardsTargetAndStartAndCheckAggro(AnimatedSprite player)
        {
            vectorTowardsTarget = player.Position - Position; 
            if (Vector2.Distance(player.Position, startingPosition) > 500)
            {
                Aggro = false;
                goingHome = true;
            }
            else if (Vector2.Distance(player.Position, Position) < 100)
            {
                Aggro = true;
            }
            else if(Vector2.Distance(startingPosition,Position) < 10 && goingHome)
            {
                goingHome = false;
            }
            

        }
        public override void Update(GameTime gameTime)
        {
            if (startingFlag)
            {
                this.startingPosition = Position;
                startingFlag = false;
            }
            vectorTowardsStart = startingPosition - Position;
            if (Aggro)
            {
                Position += VectorTowardsTarget * speed;
            }
            else if (goingHome)
            {
                Position += VectorTowardsStart * speed;
            }
            else
                Position = startingPosition;
            base.Update(gameTime);
        }
    }
}
