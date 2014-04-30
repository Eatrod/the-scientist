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
        

        public NPC_Fighting_Stationary(Texture2D texture, Script script, Random random, int[,] Map)
            : base(texture, script,Map)
        {

            this.DelayHitByArrow = 300f;
            this.ElapsedHitByArrow = 0.0f;
            this.DelayRespawn = 10000f;
            this.ElapsedRespawn = 0.0f;
            this.Dead = false;
            this.OldPosition = Vector2.Zero;
            this.EndPosition = Vector2.Zero;
            this.ElapsedSearch = 10001.0f;
            this.DelaySearch = 1000f;
            this.Time2 = 0.0f;
            this.StrikeForce = 5.0f;
            this.startingFlag = true;
            this.VectorTowardsTarget = Vector2.Zero;
            this.VectorTowardsStart = Vector2.Zero;
            this.AggroStartingPosition = Vector2.Zero;
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
        
        
        public override void Update(GameTime gameTime)
        {
            if (this.Life <= 0)
                Dead = true;
            if (!Dead)
            {
                this.ElapsedSearch += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (startingFlag)
                {
                    this.StartingPosition = Position;
                    startingFlag = false;
                }

                if (Aggro && !StrikeMode)
                {
                    this.speed = 1.5f;
                    this.Position += VectorTowardsTarget * speed;
                    //if (Vector2.Distance(this.Origin, this.EndPosition) < 10)
                    //{
                    //    this.ElapsedSearch = 10001f;
                    //}
                    //if (this.ElapsedSearch > this.DelaySearch)
                    //{
                    //    this.ElapsedSearch = 0.0f;
                    //    this.UsingAIAndSearchForTarget();
                    //}
                    //this.Time2 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    //Vector3 tempPos = this.Curve.GetPointOnCurve(this.Time2);
                    //this.Position = new Vector2(tempPos.X - 25, tempPos.Y - 65);

                    UpdateSpriteAnimation(VectorTowardsTarget);
                }
                else if (StrikeMode)
                {

                }
                else if (goingHome)
                {
                    this.speed = 3.0f;
                    Position += VectorTowardsStart * speed;
                    UpdateSpriteAnimation(VectorTowardsStart);
                }
                else
                {
                    goingHome = false;
                    this.Position = startingPosition;
                    UpdateSpriteAnimation(new Vector2(0, 1));
                }
            }
            else
            {
                ElapsedRespawn += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if(ElapsedRespawn > DelayRespawn)
                {
                    this.Life = this.FullHp;
                    this.Dead = false;
                    this.Aggro = false;
                    this.GoingHome = true;
                    this.ElapsedRespawn = 0.0f;
                }
            }
            base.Update(gameTime);
        }
    }
}
