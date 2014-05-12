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

namespace TileEngine.Sprite.Npc.NPC_Neutral
{
    
    public class NPC_Neutral_Bjorn:AnimatedSprite
    {
        public bool walkingright;
        public float delayWalk;
        public float elapsedWalk;
        public float delayFly;
        public float elapsedFly;
        public bool danceFlag;
        public float delayDance;
        public float elapsedDance;
        public bool angry;
        public float acc;
        public Vector2 startPosition;
        
        public NPC_Neutral_Bjorn(Texture2D texture,Vector2 startPosition): base(texture)
        {
            this.startPosition = startPosition;
            this.elapsedFly = 0.0f;
            this.delayFly = 10000f;
            this.angry = false;
            this.acc = 1f;
            this.delayDance = 3000f;
            this.elapsedDance = 0.0f;
            this.danceFlag = false;
            this.delayWalk = 8000f;
            this.elapsedWalk = 0.0f;
            this.walkingright = true;

            FrameAnimation redbull = new FrameAnimation(16, 50, 80, 0, 0);
            FrameAnimation wings = new FrameAnimation(4, 75, 80, 0, 400);
            FrameAnimation fly = new FrameAnimation(6, 75, 80, 300, 400);
            FrameAnimation angry = new FrameAnimation(1, 50, 80, 0, 320);
            FrameAnimation dance = new FrameAnimation(7, 50, 80, 0, 240);
            FrameAnimation moonwalkRight = new FrameAnimation(8, 50, 80, 0, 80); 
            FrameAnimation moonwalkLeft = new FrameAnimation(8, 50, 80, 0, 160);

            this.Animations.Add("Redbull", redbull);
            this.Animations.Add("Wings", wings);
            this.Animations.Add("Fly", fly);
            this.Animations.Add("Angry", angry);
            this.Animations.Add("MoonwalkRight", moonwalkRight);
            this.Animations.Add("MoonwalkLeft", moonwalkLeft);
            this.Animations.Add("Dance", dance);
        }
        public void AngryCheck(AnimatedSprite player)
        {
            if (Vector2.Distance(player.Position, this.Position) < 75)
            {
                angry = true;
            }
        }
        public override void Update(GameTime gameTime)
        {

            if (angry)
            {
                if (this.CurrentAnimationName != "Wings" && this.CurrentAnimationName != "Fly")
                {
                    this.CurrentAnimationName = "Redbull";
                    this.CurrentAnimation.FramesPerSeconds = 0.20f;
                }
                if(this.CurrentAnimation.CurrentFrame >= 15 && this.CurrentAnimationName == "Redbull")
                {
                    this.Position.X -= 9;
                    this.CurrentAnimationName = "Wings";
                    this.CurrentAnimation.FramesPerSeconds = 0.20f;
                }
                if(this.CurrentAnimationName == "Wings" && this.CurrentAnimation.CurrentFrame >= 3)
                {
                    this.CurrentAnimationName = "Fly";
                    this.CurrentAnimation.FramesPerSeconds = 0.20f;
                }
                if(this.CurrentAnimationName == "Fly")
                {
                    this.elapsedFly +=(float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    this.Position -= new Vector2(1,2);
                    if(elapsedFly > delayFly)
                    {
                        angry = false;
                        this.Position = this.startPosition;
                        this.elapsedFly = 0.0f;
                        this.Animations["Redbull"].CurrentFrame = 0;
                        this.Animations["Wings"].CurrentFrame = 0;
                        this.Animations["Fly"].CurrentFrame = 0;
                    }
                }
            }
            else
            {
                if (danceFlag)
                {
                    elapsedDance += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    this.CurrentAnimationName = "Dance";
                    this.CurrentAnimation.FramesPerSeconds = 0.15f;
                    if (elapsedDance > delayDance)
                    {
                        danceFlag = false;
                        this.walkingright = !walkingright;
                        this.elapsedDance = 0.0f;
                    }
                }
                if (!danceFlag)
                {
                    if (this.CurrentAnimation.CurrentFrame == 3 || this.CurrentAnimation.CurrentFrame == 7)
                    {
                        this.acc = 0;
                    }
                    else if (this.acc < 1)
                        this.acc += 0.1f;

                    elapsedWalk += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (walkingright)
                    {
                        this.CurrentAnimationName = "MoonwalkRight";
                        this.CurrentAnimation.FramesPerSeconds = 0.15f;
                        this.Position.X += this.acc;
                    }
                    else if (!walkingright)
                    {
                        this.CurrentAnimationName = "MoonwalkLeft";
                        this.CurrentAnimation.FramesPerSeconds = 0.15f;
                        this.Position.X -= this.acc;
                    }

                    if (elapsedWalk > delayWalk)
                    {
                        danceFlag = true;
                        elapsedWalk = 0.0f;
                    }
                }
            }
            base.Update(gameTime);
        }
    }
}
