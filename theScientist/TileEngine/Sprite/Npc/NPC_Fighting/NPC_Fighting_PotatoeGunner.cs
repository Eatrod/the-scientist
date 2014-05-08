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
    public class NPC_Fighting_PotatoeGunner: NPC_Fighting_Guard
    {
        public float ElapsedShot;
        public float DelayShot;
        public float ElapsedFirstShot;
        public float DelayFirstShot;
        public bool FireTime;
        public int ShotTicker;
        public NPC_Fighting_PotatoeGunner(Texture2D texture, Script script): base(texture,script)
        {
            this.ShotTicker = 0;
            this.FireTime = false;
            this.ElapsedFirstShot = 0.0f;
            this.DelayFirstShot = 2000f;
            this.DelayShot = 500f;
            this.ElapsedShot = 0.0f;
            FrameAnimation standingStill = new FrameAnimation(8, 50, 80, 0, 0);
            FrameAnimation playerClose = new FrameAnimation(2, 50, 80, 0, 160);
            FrameAnimation attackPlayerRight = new FrameAnimation(2, 50, 80, 0, 80);
            FrameAnimation attackPlayerLeft = new FrameAnimation(2, 50, 80, 100, 80);
            FrameAnimation reload = new FrameAnimation(4, 50, 80, 200, 80);

            this.AggroRange = 450f;
            this.Animations.Add("Reload", reload);
            this.Animations.Add("PlayerClose", playerClose);
            this.Animations.Add("StandingStill", standingStill);
            this.Animations.Add("AttackPlayerRight", attackPlayerRight);
            this.Animations.Add("AttackPlayerLeft", attackPlayerLeft);
        }
        public void UpdateWithPlayer(GameTime gameTime, AnimatedSprite player)
        {
            if(Vector2.Distance(this.Origin,player.Origin) < AggroRange * 0.65)
            {
                Aggro = true;
            }
            else if (Vector2.Distance(this.Origin, player.Origin) < AggroRange)
            {
                this.CurrentAnimationName = "PlayerClose";
                this.CurrentAnimation.FramesPerSeconds = 0.40f;
                Aggro = false;
            }
            else
            {
                this.CurrentAnimationName = "StandingStill";
                this.CurrentAnimation.FramesPerSeconds = 0.55f;
                Aggro = false;
            }
            if (Aggro && !TimeToStrike)
            {
                ElapsedFirstShot += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                this.CurrentAnimationName = "Reload";
                this.CurrentAnimation.FramesPerSeconds = 0.25f;
                if(ElapsedFirstShot > DelayFirstShot)
                {
                    TimeToStrike = true;
                    ElapsedShot = 0.0f;
                }
            }
            if(TimeToStrike)
            {
                ElapsedShot += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (player.Origin.X < this.Origin.X)
                    this.CurrentAnimationName = "AttackPlayerLeft";
                else
                    this.CurrentAnimationName = "AttackPlayerRight";
                this.CurrentAnimation.FramesPerSeconds = 0.6f;
                if(ElapsedShot > DelayShot)
                {
                    ShotTicker++;
                    ElapsedShot = 0.0f;
                    FireTime = true;
                }
                if(ShotTicker >= 3)
                {
                    ShotTicker = 0;
                    ElapsedShot = 0.0f;
                    ElapsedFirstShot = 0.0f;
                    TimeToStrike = false;
                }
            }

            base.Update(gameTime);
        }
    }
}
