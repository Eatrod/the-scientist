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
    class Stim
    {
        public Random rand;
        public Texture2D TextureFishLeft;
        public Texture2D TextureFishRight;
        public Texture2D TextureCapturedFish;
        public Texture2D TextureSharkLeft;
        public Texture2D TextureSharkRight;
        public Texture2D TextureSpecialLeft;
        public Texture2D TextureSpecialRight;
        public Texture2D TextureSpecialCaptured;
        public List<Fish> Fishes;
        public int WantedFishes;
        public int Unique;
        public Stim(Texture2D TextureSpecialLeft, Texture2D TextureSpecialRight, Texture2D TextureSpecialCaptured,Texture2D TextureSharkLeft, Texture2D TextureSharkRight, Texture2D TextureCapturedFish, Texture2D TextureFishLeft, Texture2D TextureFishRight, int NumberOfFishesWanted)
        {
            this.TextureSpecialLeft = TextureSpecialLeft;
            this.TextureSpecialRight = TextureSpecialRight;
            this.TextureSpecialCaptured = TextureSpecialCaptured;
            this.TextureSharkLeft = TextureSharkLeft;
            this.TextureSharkRight = TextureSharkRight;
            this.TextureCapturedFish = TextureCapturedFish;
            this.rand = new Random();
            this.TextureFishLeft = TextureFishLeft;
            this.TextureFishRight = TextureFishRight;
            this.Fishes = new List<Fish>();
            this.WantedFishes = NumberOfFishesWanted;
            GenerateFishes(NumberOfFishesWanted);
            
        }
        public void GenerateFishes(int NumberOfFishesWanted)
        {
            for (int i = 0; i < NumberOfFishesWanted; i++)
            {
                Unique = rand.Next(1, 21);
                Color ColorFish = new Color();
                int ColorNumber = rand.Next(0, 6);
                if (ColorNumber == 0)
                    ColorFish = Color.Yellow;
                if (ColorNumber == 1)
                    ColorFish = Color.Blue;
                if (ColorNumber == 2)
                    ColorFish = Color.Green;
                if (ColorNumber == 3)
                    ColorFish = Color.Turquoise;
                if (ColorNumber == 4)
                    ColorFish = Color.PowderBlue;
                if (ColorNumber == 5)
                    ColorFish = Color.Pink;

                int side = rand.Next(0, 2);
                if (Unique < 20)
                {
                    if (side == 0)
                    {
                        Fish fish = new Fish(TextureSpecialLeft, TextureSpecialRight, TextureSpecialCaptured,
                            TextureSharkLeft, TextureSharkRight,TextureCapturedFish,
                            TextureFishLeft, TextureFishRight, new Vector2(0, rand.Next(200, 650)),
                            ColorFish, rand.Next(50, 300) / 100f, 0);
                        Fishes.Add(fish);
                    }
                    if (side == 1)
                    {
                        Fish fish = new Fish(TextureSpecialLeft, TextureSpecialRight, TextureSpecialCaptured, 
                            TextureSharkLeft, TextureSharkRight, TextureCapturedFish,
                            TextureFishLeft, TextureFishRight, new Vector2(800, rand.Next(200, 650)),
                            ColorFish, rand.Next(50, 300) / 100f, 180);
                        Fishes.Add(fish);
                    }
                }
                else
                {
                    if (side == 0)
                    {
                        Fish fish = new Fish(TextureSpecialLeft, TextureSpecialRight, TextureSpecialCaptured, 
                            TextureSharkLeft, TextureSharkRight, TextureCapturedFish,
                            TextureFishLeft, TextureFishRight, new Vector2(-20, rand.Next(400, 550)),
                            Color.Black, rand.Next(200, 400) / 100f, 0);
                        Fishes.Add(fish);
                    }
                    if (side == 1)
                    {
                        Fish fish = new Fish(TextureSpecialLeft, TextureSpecialRight, TextureSpecialCaptured, 
                            TextureSharkLeft, TextureSharkRight, TextureCapturedFish,
                            TextureFishLeft, TextureFishRight, new Vector2(850, rand.Next(400, 550)),
                            Color.Black, rand.Next(200, 400) / 100f, 180);
                        Fishes.Add(fish);
                    }
                }
            }
        }
        public void UpdateFishes(FishingRod fishingrod)
        {
            int Limit = Fishes.Count;
            if (Limit != WantedFishes)
                GenerateFishes(1);
            Limit = Fishes.Count;
            for (int i = 0; i < Limit; i++)
            {
                Fishes[i].UpdateFish(fishingrod);
                if (!Fishes[i].Alive)
                {
                    Fishes.Remove(Fishes[i]);
                    break;
                }
            }
        }
        public void DrawFishes(SpriteBatch spriteBatch)
        {
            int Limit = Fishes.Count;
            for (int i = 0; i < Limit; i++)
                Fishes[i].DrawFish(spriteBatch);
        }
    }
}
