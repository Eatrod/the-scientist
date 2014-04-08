using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FishingGame
{
    class FishingRod
    {
        public Vector2 PositionRodLine;
        public Vector2 PositionHook;
        public int number;
        public Texture2D TextureRod;
        public int CapturedFishes;
        public int CapturedSpecialFishes;
        public Texture2D TextureLine;
        public Fish CurrentFish;
        public Texture2D TextureHook;
        public bool Captured;
        public Rectangle BoundingBox;
        public Rectangle LineSize;
        public FishingRod(Vector2 PositionRodLine, Vector2 PositionHook, Texture2D Rod, Texture2D Line, Texture2D Hook)
        {
            this.PositionRodLine = PositionRodLine;
            this.PositionHook = PositionHook;
            this.TextureRod = Rod;
            this.TextureLine = Line;
            this.number = 0;
            this.TextureHook = Hook;
            this.CapturedFishes = 0;
            this.CapturedSpecialFishes = 0;
            this.Captured = false;
            this.LineSize = new Rectangle((int)this.PositionRodLine.X,(int)this.PositionRodLine.Y,2,(int)Vector2.Distance(this.PositionRodLine,this.PositionHook));
            this.BoundingBox = new Rectangle((int)this.PositionHook.X, (int)this.PositionHook.Y, TextureHook.Width, TextureHook.Height);
        }
        public void UpdateFishingRod(FishingBoat fishingboat, KeyboardState ks, List<Fish> fishlist)
        {
            this.LineSize = new Rectangle((int)fishingboat.Position.X + fishingboat.Texture.Width/2 + TextureHook.Width /2,
                (int)this.PositionRodLine.Y,
                2,
                (int)Vector2.Distance(this.PositionRodLine, this.PositionHook));
            this.PositionRodLine = new Vector2(fishingboat.Position.X + fishingboat.Texture.Width / 2,
                fishingboat.Position.Y + fishingboat.Texture.Height / 2);
            this.PositionHook = new Vector2(fishingboat.Position.X + fishingboat.Texture.Width / 2,
                 fishingboat.Position.Y + fishingboat.Texture.Height + number);
            if (ks.IsKeyDown(Keys.Up) && this.PositionHook.Y > fishingboat.Texture.Height && number > 0)
            {
                if (CurrentFish == null)
                    number -= 3;
                if (CurrentFish != null)
                    number -= 2;
            }
            if (ks.IsKeyDown(Keys.Down) && this.PositionHook.Y < (650 - TextureHook.Height))
            {
                number += 3;
            }
            this.PositionHook.Y = fishingboat.Position.Y + fishingboat.Texture.Height + number;
            if (this.number < 1 && Captured)
            {
                this.Captured = false;
                if (CurrentFish.Special)
                    this.CapturedSpecialFishes += 1;
                else if (CurrentFish.Dangerous)
                    this.CapturedFishes -= 1;
                else
                    this.CapturedFishes += 1;
                fishlist.Remove(CurrentFish);
                CurrentFish = null;
            }
            this.BoundingBox = new Rectangle((int)this.PositionHook.X, (int)this.PositionHook.Y, TextureHook.Width, TextureHook.Height);
        }
        public void CheckIfFishCaptured(List<Fish> fishlist)
        {
            int Limit = fishlist.Count;
            for (int i = 0; i < Limit; i++)
            {
                if (this.BoundingBox.Intersects(fishlist[i].BoundingBox))
                {
                    if (fishlist[i].Dangerous)
                    {
                        this.number = 0;
                    }
                    if (!Captured && !fishlist[i].Dangerous)
                    {
                        fishlist[i].Speed = 0;
                        fishlist[i].Hooked = true;
                        Captured = true;
                        CurrentFish = fishlist[i];
                        break;
                    }
                    else if (Captured && fishlist[i].Dangerous)
                    {
                        
                        CurrentFish.Hooked = false;
                        fishlist.Remove(CurrentFish);
                        CurrentFish = null;
                        this.Captured = false;
                        this.number = 0;
                        break;
                    }
                }
            }
        }
        public void DrawFishingRod(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureHook,
                new Rectangle((int)this.PositionHook.X,(int)this.PositionHook.Y,TextureHook.Width,TextureHook.Height),
                Color.White);
            spriteBatch.Draw(TextureLine, LineSize, Color.White);
        }
    }
}
