using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine.AI;
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
        protected Vector2 vectorTowardsTarget;
        protected Vector2 vectorTowardsStart;
        protected Vector2 aggroStartingPosition;
        protected bool aggro;
        protected bool startingFlag;
        protected bool goingHome;
        protected float aggroRange;
        protected float aggroCircle;
        private float time2;
        private float delaySearch;
        private float elapsedSearch;
        private Vector2 playerPosition;
        private Vector2 endPosition;
        private Curve2D curve;
        private AIsearch ai;
        private Vector2 oldPosition;
        public Vector2 PlayerPosition
        {
            get { return playerPosition; }
            set { playerPosition = value; }
        }
        public Vector2 OldPosition
        {
            get { return oldPosition; }
            set { oldPosition = value; }
        }

        public Vector2 EndPosition
        {
            get { return endPosition; }
            set { endPosition = value; }
        }
        public float ElapsedSearch
        {
            get { return elapsedSearch; }
            set { elapsedSearch = value; }
        }
        public float DelaySearch
        {
            get { return delaySearch; }
            set { delaySearch = value; }
        }
        public float Time2
        {
            get { return time2; }
            set { time2 = value; }
        }
        public Curve2D Curve
        {
            get { return curve; }
            set { curve = value; }
        }
        public AIsearch AI
        {
            get { return ai; }
            set { ai = value; }
        }
        
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

        public NPC_Fighting_Guard(Texture2D texture, Script script, int[,] Map):base(texture,script)
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
            this.AI = new AIsearch(Map);
        }
        public void InitCurve(Dictionary<TileNode, TileNode> WayToGo)
        {

            Curve = new Curve2D();
            float time = 0;

            List<Vector2> Positions = new List<Vector2>();

            foreach (var obj in WayToGo)
            {
                Positions.Add(obj.Key.PositionInGrid * 32);
            }
            Positions.Add(new Vector2(this.Origin.X, this.Origin.Y));
            int max = Positions.Count - 1;
            endPosition = Positions[0];
            for (int i = max; i > -1; i--)
            {
                float distance;
                if (i > 0)
                    distance = Vector2.Distance(Positions[i], Positions[i - 1]);
                else
                    distance = Vector2.Distance(Positions[i], Positions[i + 1]);
                Curve.AddPoint(new Vector3(Positions[i].X, Positions[i].Y, 0), time);
                time += 1f;
                Curve.AddPoint(new Vector3(Positions[i].X, Positions[i].Y, 0), time);
                time += distance * 8;

            }
            Curve.SetTangents();
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
        public void UsingAIAndSearchForTarget()
        {
            this.time2 = 0.0f;
            AI.SearchForShortestPath(
                AI.GetNodeFromNodes(new Vector2(((int)this.Origin.X) / 32, ((int)this.Origin.Y) / 32)),
                AI.GetNodeFromNodes(new Vector2((int)this.PlayerPosition.X / 32, (int)this.PlayerPosition.Y / 32)));
            int steps = AI.FinalPath.Count;
            if (steps == 0 || steps > 6)
            {
                GoingHome = true;
                Aggro = false;
            }
            else
                InitCurve(AI.FinalPath);
        }
        public void UpdateSpriteAnimation(Vector2 motion)
        {

            float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

            if (motionAngle >= -MathHelper.PiOver4 && motionAngle <= MathHelper.PiOver4)
            {
                CurrentAnimationName = "WalkRight"; //Right
                //motion = new Vector2(1f, 0f);
            }
            else if (motionAngle >= MathHelper.PiOver4 && motionAngle <= 3f * MathHelper.PiOver4)
            {
                CurrentAnimationName = "WalkDown"; //Down
                //motion = new Vector2(0f, 1f);
            }
            else if (motionAngle <= -MathHelper.PiOver4 && motionAngle >= -3f * MathHelper.PiOver4)
            {
                CurrentAnimationName = "WalkUp"; // Up
                //motion = new Vector2(0f, -1f);
            }
            else
            {
                CurrentAnimationName = "WalkLeft"; //Left
                //motion = new Vector2(-1f, 0f);
            }
        }
    }
}
