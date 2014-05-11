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
        public bool danceFlag;
        public float delayDance;
        public float elapsedDance;
        public bool angry;
        public float acc;
        
        public NPC_Neutral_Bjorn(Texture2D texture): base(texture)
        {
            this.angry = false;
            this.acc = 1f;
            this.delayDance = 5000f;
            this.elapsedDance = 0.0f;
            this.danceFlag = false;
            this.delayWalk = 8000f;
            this.elapsedWalk = 0.0f;
            this.walkingright = true;

            FrameAnimation angry = new FrameAnimation(1, 50, 80, 0, 320);
            FrameAnimation dance = new FrameAnimation(7, 50, 80, 0, 240);
            FrameAnimation moonwalkRight = new FrameAnimation(8, 50, 80, 0, 80); 
            FrameAnimation moonwalkLeft = new FrameAnimation(8, 50, 80, 0, 160);

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
            else
                angry = false;
        }
        public override void Update(GameTime gameTime)
        {
            if (angry)
                this.CurrentAnimationName = "Angry";
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
