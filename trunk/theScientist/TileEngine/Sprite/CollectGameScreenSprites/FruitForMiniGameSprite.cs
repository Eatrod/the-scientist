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

namespace TileEngine.Sprite
{
    public class FruitForMiniGameSprite:AnimatedSprite
    {
        private Random random;
        private float delayLifeTime;
        private float elapsedLifeTime;
        private bool alive;
        static public float playerPoints;
        static public float npcPoints;

        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }
        public FruitForMiniGameSprite(Texture2D texture, Random random):base(texture)
        {
            npcPoints = 0.0f;
            playerPoints = 0.0f;
            this.alive = true;
            this.random = random;
            this.delayLifeTime = random.Next(15000, 25000);
            this.elapsedLifeTime = 0.0f;
            this.Position = new Vector2(random.Next(10, 55) * 32, random.Next(10, 40) * 32);

            FrameAnimation freshFruit = new FrameAnimation(1, 32, 32, 64, 0);
            FrameAnimation middleFruit = new FrameAnimation(1, 32, 32, 32, 0);
            FrameAnimation rottenFruit = new FrameAnimation(1, 32, 32, 0, 0);

            this.Animations.Add("FreshFruit", freshFruit);
            this.Animations.Add("MiddleFruit", middleFruit);
            this.Animations.Add("RottenFruit", rottenFruit);
            this.CurrentAnimationName = "FreshFruit";
        }
        public void CheckForContactWithPlayerOrNPC(AnimatedSprite player, LumberJackJohnny npc)
        {
            if(playerPoints >= 800)
            {
                npc.AngryFlag = true;
                npc.delayTripped = 500f;
            }
            if(player.MovementBounds().Intersects(this.Bounds))
            {
                this.alive = false;
                if (this.CurrentAnimationName == "FreshFruit")
                    playerPoints += 100;
                else if (this.CurrentAnimationName == "MiddleFruit")
                    playerPoints += 75;
                else if (this.CurrentAnimationName == "RottenFruit")
                    playerPoints += 50;
            }
            else if (npc.MovementBounds().Intersects(this.Bounds))
            {
                this.alive = false;
                npc.GotIT = true;
                if (this.CurrentAnimationName == "FreshFruit")
                    npcPoints += 100;
                else if (this.CurrentAnimationName == "MiddleFruit")
                    npcPoints += 75;
                else if (this.CurrentAnimationName == "RottenFruit")
                    npcPoints += 50;
            }
        }
        public override void Update(GameTime gameTime)
        {
            elapsedLifeTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsedLifeTime < delayLifeTime / 2)
            {
                this.CurrentAnimationName = "FreshFruit";
            }
            else if (elapsedLifeTime < (delayLifeTime / 4) * 3 && elapsedLifeTime > delayLifeTime / 2)
            {
                this.CurrentAnimationName = "MiddleFruit";
            }
            else
                this.CurrentAnimationName = "RottenFruit";
            if(elapsedLifeTime > delayLifeTime)
                alive = false;  
            if(!alive)
            {
                this.elapsedLifeTime = 0.0f;
                this.Position = new Vector2(random.Next(10, 55) *32, random.Next(10, 40) * 32);
                this.CurrentAnimationName = "FreshFruit";
                this.alive = true;
            }
            //if(npcPoints > 1000)
            //{
                
            //}
            //if(playerPoints > 1000)
            //{

            //}
 	        base.Update(gameTime);
        }
    }
}
