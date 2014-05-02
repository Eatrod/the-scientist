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
    public class NPC_Fighting_Ranged: NPC_Fighting_Guard
    {
        private bool leftOrRight;
        private float elapsedThrowBomb;
        private float delayThrowBomb;
        private bool bombThrow;
        private bool lockAndLoad;
        private bool animationTime;
        private float elapsedAnimationTime;
        private float delayAnimationTime;
        

        public bool BombThrow
        {
            get { return bombThrow; }
            set { bombThrow = value; }
        }
        public NPC_Fighting_Ranged(Texture2D texture, Script script, Random random): base(texture,null)
        {
            this.elapsedThrowBomb = 0.0f;
            this.delayThrowBomb = 3000f;
            this.animationTime = false;
            this.elapsedAnimationTime = 0.0f;
            this.delayAnimationTime = 500f;
            this.bombThrow = false;
            this.lockAndLoad = false;
            this.animationTime = false;
            this.AggroRange = 800f;

            FrameAnimation down = new FrameAnimation(1, 50, 80, 0, 0);
            FrameAnimation left = new FrameAnimation(1, 50, 80, 0, 80);
            FrameAnimation right = new FrameAnimation(1, 50, 80, 0, 160);
            FrameAnimation up = new FrameAnimation(1, 50, 80, 0, 240);

            FrameAnimation walkDown = new FrameAnimation(2, 50, 80, 50, 0);
            FrameAnimation walkLeft = new FrameAnimation(2, 50, 80, 50, 80);
            FrameAnimation walkRight = new FrameAnimation(2, 50, 80, 50, 160);
            FrameAnimation walkUp = new FrameAnimation(2, 50, 80, 50, 240);

            FrameAnimation throwLeft = new FrameAnimation(2, 50, 80, 200, 80);
            FrameAnimation throwRight = new FrameAnimation(2, 50, 80, 200, 160);

            this.Animations.Add("ThrowLeft", throwLeft);
            this.Animations.Add("ThrowRight", throwRight);

            this.Animations.Add("Right", right);
            this.Animations.Add("Left", left);
            this.Animations.Add("Up", up);
            this.Animations.Add("Down", down);
            this.Animations.Add("WalkRight", walkRight);
            this.Animations.Add("WalkLeft", walkLeft);
            this.Animations.Add("WalkUp", walkUp);
            this.Animations.Add("WalkDown", walkDown);
        }
        public void UpdateRangedFighter(GameTime gameTime, AnimatedSprite player)
        {
            if (Vector2.Distance(player.Origin, this.Origin) < AggroRange && !animationTime)
            {
                Aggro = true;
                lockAndLoad = true;
            }
            else if (Vector2.Distance(player.Origin,this.Origin) > AggroRange)
            {
                Aggro = false;
                lockAndLoad = false;
                animationTime = false;
                bombThrow = false;
            }
            if(Aggro && lockAndLoad)
            {
                elapsedThrowBomb += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if(elapsedThrowBomb > delayThrowBomb)
                {
                    if (player.Position.X < this.Position.X)
                        leftOrRight = false;
                    else
                        leftOrRight = true;
                    this.animationTime = true;
                    this.lockAndLoad = false;
                    elapsedThrowBomb = 0.0f;
                }
            }
            if(animationTime)
            {
                elapsedAnimationTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (leftOrRight)
                    this.CurrentAnimationName = "ThrowRight";
                else
                    this.CurrentAnimationName = "ThrowLeft";
                if(elapsedAnimationTime > delayAnimationTime)
                {
                    this.bombThrow = true;
                    this.animationTime = false;
                    this.CurrentAnimation.CurrentFrame = 0;
                    elapsedAnimationTime = 0.0f;
                }
            }
        }
        public override void Update(GameTime gameTime)
        {
            if (StartingFlag)
            {
                this.StartingPosition = Position;
                StartingFlag = false;
            }
            if (!animationTime)
            {
                this.CurrentAnimationName = "Down";
            }
            base.Update(gameTime);
        }
    }
}
