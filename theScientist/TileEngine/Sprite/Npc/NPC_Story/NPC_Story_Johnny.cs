using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace TileEngine.Sprite.Npc.NPC_Story
{
    public class NPC_Story_Johnny : NPC_Story
    {
        private float delay;
        private float elapsed;
        public NPC_Story_Johnny(Texture2D texture, Script script, Texture2D picture, string name)
            : base(texture, script, picture, name)
        {
            this.elapsed = 0.0f;
            this.delay = 20000f;

            FrameAnimation down = new FrameAnimation(1, 50, 80, 0, 960);
            FrameAnimation yawn = new FrameAnimation(5, 50, 80, 0, 960);
            //FrameAnimation sleeping = new FrameAnimation(1, 100, 160, 100, 80);
            //this.Animations.Add("Sleeping", sleeping);
            this.Animations.Add("Down", down);
            this.Animations.Add("Yawn", yawn);



            this.Animations["Down"].FramesPerSeconds = 8f;
            this.Animations["Yawn"].FramesPerSeconds = 0.4f;
            this.CurrentAnimationName = "Down";
        }
        public override void Update(GameTime gameTime)
        {


                if (this.CurrentAnimationName == "Yawn" && this.CurrentAnimation.CurrentFrame >= 4)
                {
                    this.CurrentAnimationName = "Down";
                }
                if (this.CurrentAnimationName == "Down")
                {
                    elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (elapsed > delay)
                    {
                        this.CurrentAnimationName = "Yawn";
                        this.CurrentAnimation.CurrentFrame = 0;
                        this.elapsed = 0.0f;
                    }
                }


                base.Update(gameTime);
            }
        }
    }

