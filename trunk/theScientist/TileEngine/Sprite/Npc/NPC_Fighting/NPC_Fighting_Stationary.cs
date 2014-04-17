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
    public class NPC_Fighting_Stationary : NPC_Fighting_Guard
    {
        public NPC_Fighting_Stationary(Texture2D texture, Script script, Random random)
            : base(texture, script)
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
            if (startingFlag)
            {
                this.startingPosition = Position;
                startingFlag = false;
            }

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
