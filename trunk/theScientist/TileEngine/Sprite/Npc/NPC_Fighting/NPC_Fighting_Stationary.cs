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
    public class NPC_Fighting_Stationary : NPC_Fighting_Guard
    {
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
        public NPC_Fighting_Stationary(Texture2D texture, Script script, Random random, int[,] Map)
            : base(texture, script)
        {
            this.oldPosition = Vector2.Zero;
            this.endPosition = Vector2.Zero;
            this.elapsedSearch = 10001.0f;
            this.delaySearch = 1000f;
            this.time2 = 0.0f;
            this.ai = new AIsearch(Map);
           
            this.ai.GenerateNeighboursForTileNodes();
            this.startingFlag = true;
            this.vectorTowardsTarget = Vector2.Zero;
            this.vectorTowardsStart = Vector2.Zero;
            this.aggroStartingPosition = Vector2.Zero;
            this.Aggro = false;
            this.GoingHome = false;
            this.speed = 3.0f;
            this.aggroRange = 100;
            this.aggroCircle = 500;

            FrameAnimation down = new FrameAnimation(1, 50, 80, 0, 0);
            FrameAnimation left = new FrameAnimation(1, 50, 80, 0, 80);
            FrameAnimation right = new FrameAnimation(1, 50, 80, 0, 160);
            FrameAnimation up = new FrameAnimation(1, 50, 80, 0, 240);


            FrameAnimation walkDown = new FrameAnimation(2, 50, 80, 50, 0);
            FrameAnimation walkLeft = new FrameAnimation(2, 50, 80, 50, 80);
            FrameAnimation walkRight = new FrameAnimation(2, 50, 80, 50, 160);
            FrameAnimation walkUp = new FrameAnimation(2, 50, 80, 50, 240);


            this.Animations.Add("Right", right);
            this.Animations.Add("Left", left);
            this.Animations.Add("Up", up);
            this.Animations.Add("Down", down);
            this.Animations.Add("WalkRight", walkRight);
            this.Animations.Add("WalkLeft", walkLeft);
            this.Animations.Add("WalkUp", walkUp);
            this.Animations.Add("WalkDown", walkDown);
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
        public void UsingAIAndSearchForTarget()
        {
            this.time2 = 0.0f;
            //Vector2 tempshit = new Vector2((int)(this.Position.X + 25) / 32, (int)(this.Position.Y + 65) / 32); 
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
            {
                InitCurve(AI.FinalPath);
            }

        }
        public override void Update(GameTime gameTime)
        {
            this.ElapsedSearch += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (startingFlag)
            {
                this.startingPosition = Position;
                startingFlag = false;
            }

            if (Aggro)
            {
                if (Vector2.Distance(this.Origin,this.EndPosition) < 10)
                {
                    this.ElapsedSearch = 10001f;
                }
                if (this.ElapsedSearch > this.DelaySearch)
                {
                    this.ElapsedSearch = 0.0f;
                    this.UsingAIAndSearchForTarget();
                }
                this.Time2 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                Vector3 tempPos = this.Curve.GetPointOnCurve(this.Time2);
                this.Position = new Vector2(tempPos.X - 25, tempPos.Y - 65);

                this.UpdateSpriteAnimation(this.Position - this.OldPosition);
                this.OldPosition = this.Position;
            }
            else if (goingHome)
            {
                Position += VectorTowardsStart * speed;
                UpdateSpriteAnimation(VectorTowardsStart);
            }
            else
            {
                goingHome = false;
                this.Position = startingPosition;
                UpdateSpriteAnimation(new Vector2(0, 1));
            }
            base.Update(gameTime);
        }
    }
}
