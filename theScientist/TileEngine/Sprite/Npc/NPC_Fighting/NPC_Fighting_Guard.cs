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
        
        
        protected bool startingFlag;
        protected bool goingHome;
        protected float aggroRange;
        protected float aggroCircle;

        private float elapsedRespawn;
        private float delayRespawn;

        private float aggroSpeed;

        private float elapsedHitByMelee;
        private float delayHitByMelee;
        private bool meleeHit;

        private float patrollingCircle;

        private float strikeForce;
        private bool strikeMode;
        private float delayStrike;
        private float delayStruck;
        private float elapsedStruck;
        private float elapsedStrike;
        private float time2;

        private float delaySearch;
        private float elapsedSearch;
        private Vector2 playerPosition;
        private Vector2 endPosition;
        private Curve2D curve;
        private AIsearch ai;
        private Vector2 oldPosition;
        public float AggroSpeed
        {
            get { return aggroSpeed; }
            set { aggroSpeed = value; }
        }
        public float PatrollingCircle
        {
            get { return patrollingCircle; }
            set { patrollingCircle = value; }
        }
        public float DelayRespawn
        {
            get { return delayRespawn; }
            set { delayRespawn = value; }
        }
        public float ElapsedRespawn
        {
            get { return elapsedRespawn; }
            set { elapsedRespawn = value; }
        }
        public bool MeleeHit
        {
            get { return meleeHit; }
            set { meleeHit = value; }
        }
        public float ElapsedHitByMelee
        {
            get { return elapsedHitByMelee; }
            set { elapsedHitByMelee = value; }
        }
        public float DelayHitByMelee
        {
            get { return delayHitByMelee; }
            set { delayHitByMelee = value; }
        }
        public float StrikeForce
        {
            get { return strikeForce; }
            set { strikeForce = value; }
        }
        public float DelayStruck
        {
            get { return delayStruck; }
            set { delayStruck = value; }
        }
        public float ElapsedStruck
        {
            get { return elapsedStruck; }
            set { elapsedStruck = value; }
        }
        public bool StrikeMode
        {
            get { return strikeMode; }
            set { strikeMode = value; }
        }
        public float DelayStrike
        {
            get { return delayStrike; }
            set { delayStrike = value; }
        }
        public float ElapsedStrike
        {
            get { return elapsedStrike; }
            set { elapsedStrike = value; }
        }
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
        public Vector2 VectorTowardsTarget
        {
            get
            {
                vectorTowardsTarget.Normalize();
                return vectorTowardsTarget;
            }
            set { vectorTowardsTarget = value; }

        }

        public NPC_Fighting_Guard(Texture2D texture, Script script, int[,] Map)
            : base(texture, script)
        {
            this.Dead = false;
            this.ElapsedHitByMelee = 0.0f;
            this.DelayHitByMelee = 300f;
            this.strikeMode = false;
            this.startingFlag = true;
            this.vectorTowardsTarget = Vector2.Zero;
            this.vectorTowardsStart = Vector2.Zero;
            this.AggroStartingPosition = Vector2.Zero;
            this.DelayStrike = 1000f;
            this.ElapsedStrike = 0.0f;
            this.Aggro = false;
            this.GoingHome = false;
            this.speed = 1.0f;
            this.aggroRange = 150;
            this.delayStruck = 500f;
            this.elapsedStruck = 0.0f;
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
        public void SetVectorTowardsTargetAndStartAndCheckAggro(GameTime gameTime, AnimatedSprite player)
        {

            vectorTowardsTarget = player.Origin - this.Origin;
            vectorTowardsStart = startingPosition - Position;
            if (HitByArrow)
            {
                ElapsedHitByArrow += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                this.Position += this.ArrowDirection * 3;
                if (ElapsedHitByArrow > DelayHitByArrow)
                {
                    HitByArrow = false;
                    ElapsedHitByArrow = 0.0f;
                }
            }
            if (MeleeHit)
            {
                ElapsedHitByMelee += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                Vector2 HitVector = this.Origin - player.Origin;
                HitVector.Normalize();
                player.Position -= HitVector * 3;
                player.SettingSpriteBlink(gameTime);
                if (ElapsedHitByMelee > DelayHitByMelee)
                {
                    MeleeHit = false;
                    ElapsedHitByMelee = 0.0f;
                }
            }
            if (Vector2.Distance(this.Position, player.Position) < 50)
            {
                ElapsedStrike += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                strikeMode = true;
                UpdateSpriteAnimation(player.Position - this.Position);
                if (ElapsedStrike > DelayStrike)
                {
                    this.MeleeHit = true;
                    player.Life -= StrikeForce;
                    ElapsedStrike = 0.0f;
                }

            }
            else
            {
                strikeMode = false;
            }
            if (Vector2.Distance(Position, AggroStartingPosition) > AggroCircle && AggroStartingPosition != Vector2.Zero && !StrikeMode)
            {
                Aggro = false;
                GoingHome = true;
                AggroStartingPosition = Vector2.Zero;
            }
            else if (Vector2.Distance(player.Position, Position) < AggroRange && !GoingHome && !Aggro)
            {
                AggroStartingPosition = Position;
                Aggro = true;
            }
            else if (Vector2.Distance(startingPosition, Position) < 10 && GoingHome)
            {
                //this.Life = 100;
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
