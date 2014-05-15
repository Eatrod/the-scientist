using Microsoft.Xna.Framework.Graphics;
using TileEngine.Sprite;

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine;
//using TileEngine.Sprite;

namespace TileEngine.Sprite
{
     public class PlayerCharacter : CharacterSprite
     {
         public Texture2D portrait;
         private bool hasAxe = false;
         private bool hasBow = false;
         public string oldAnimation { get; set; }
         public bool shotFired { get; set; }


         public bool multishotArrow { get; set; }
         public bool multishotFireArrow { get; set; }
         public bool fireArrow { get; set; }
         public bool normalArrow { get; set; }

         public bool meleeAttackPossible { get; set; }
         public bool meleeAttackStart { get; set; }
         public bool meleeAttackFinish { get; set; }
         public bool meleeAttackSpinAxe { get; set; }

         public bool spinAxeAway { get; set; }

         public bool MiniGameWait { get; set; }

         public float delayAttack = 300f;
         public float elapsedAttack = 0.0f;
         public float delayShot = 500f;
         public float elapsedShot = 0.0f;
         
         public float pick_up_radius = 30f;
         public bool pickingup;

         public PlayerCharacter(Texture2D texture, Texture2D portrait)
             : base(texture)
         {
             this.portrait = portrait;
             this.MiniGameWait = false;
         }

         public void UpdateAxeStatus(string status)
         {
             if (status == "true")
                this.hasAxe = true;
             else
             {
                 this.hasAxe = false;
             }
         }

         public void UpdateBowStatus(string status)
         {
             if (status == "true")
                 this.hasBow = true;
             else
             {
                 this.hasBow = false;
             }
         }
         public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
         {
             FrameAnimation animation = CurrentAnimation;
             if (animation == null)
             {
                 if (Animations.Count > 0)
                 {
                     string[] keys = new string[Animations.Count];
                     Animations.Keys.CopyTo(keys, 0);

                     CurrentAnimationName = keys[0];
                     animation = CurrentAnimation;
                 }
                 else
                     return;
             }
             SettingSpriteBlink(gameTime);
             ReducingHealth();
             if (HitFlag)
                 MovementAfterBeingHit(gameTime);
             animation.Update(gameTime);

             //base.Update(gameTime);
         }
    }
}
