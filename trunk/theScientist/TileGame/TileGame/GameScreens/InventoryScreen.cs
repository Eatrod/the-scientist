using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using XtheSmithLibrary;
using XtheSmithLibrary.Controls;
using TileGame;
using TileEngine;

namespace TileGame.GameScreens
{
    public class InventoryScreen : BaseGameState
    {
        #region Field region

        Texture2D inventoryBackground;
        Texture2D axeImage, swordImage, crossbowImage, BelladonnaImage, ImmortuiImage, lifePotatoImage;
        Texture2D inventoryCursorImage;
        int cursor_X, cursor_Y;
        int cursor_item_number;
        Label numberOfLifePotatoes;
        Label[] activeWeaponNumbers;

        #endregion

        #region Property Region
        #endregion

        #region Constructor region
        public InventoryScreen(Game game, GameStateManager manager) 
            : base(game, manager)
        {
            
        }
        #endregion

        #region XNA Method Region
        public override void Initialize()
        {
            cursor_X = GameRef.ScreenRectangle.Width / 15; //GameRef.ScreenRectangle.Width / 7, GameRef.ScreenRectangle.Height / 3,
            cursor_Y = GameRef.ScreenRectangle.Height / 5;
            cursor_item_number = 0;

            base.Initialize();
        }
        protected override void LoadContent()
        {
            ContentManager Content = GameRef.Content;
            base.LoadContent();

            inventoryBackground = Content.Load<Texture2D>(@"Backgrounds\Inventory test");
            axeImage = Content.Load<Texture2D>(@"Sprite\Axe");
            swordImage = Content.Load<Texture2D>(@"Sprite\Inv Sword test");
            crossbowImage = Content.Load<Texture2D>(@"Sprite\Bow");
            inventoryCursorImage = Content.Load<Texture2D>(@"Sprite\Inventory Cursor test");
            BelladonnaImage = Content.Load<Texture2D>(@"Sprite\Belladonna");
            ImmortuiImage = Content.Load<Texture2D>(@"Sprite\Immortui big");
            lifePotatoImage = Content.Load<Texture2D>(@"Sprite\LifePotato");

            numberOfLifePotatoes = new Label();
            //GameRef.ScreenRectangle.Width / 8, GameRef.ScreenRectangle.Height / 6
            numberOfLifePotatoes.Position = new Vector2(GameRef.ScreenRectangle.Width / 2 + (GameRef.ScreenRectangle.Width / 2) / 10 + GameRef.ScreenRectangle.Width / 8, GameRef.ScreenRectangle.Height / 5 + ((GameRef.ScreenRectangle.Height / 6 + (GameRef.ScreenRectangle.Height / 6) / 10) * 3) + (GameRef.ScreenRectangle.Height / 6) / 3); //new Vector2(860, 200 + (140 * 3)); //1110 //860
            numberOfLifePotatoes.Text = "";
            numberOfLifePotatoes.Color = Color.Black;
        
            ControlManager.Add(numberOfLifePotatoes);

            activeWeaponNumbers = new Label[5];
            for (int i = 0; i < 5; i++)
            {
                activeWeaponNumbers[i] = new Label();
                activeWeaponNumbers[i].Position = new Vector2(GameRef.ScreenRectangle.Width / 15 + ((GameRef.ScreenRectangle.Width / 15 + GameRef.ScreenRectangle.Width / 15) * i) - (GameRef.ScreenRectangle.Width / 15)/4, GameRef.ScreenRectangle.Height / 12);
                activeWeaponNumbers[i].Text = (i + 1).ToString();
                activeWeaponNumbers[i].Color = Color.Black;
                ControlManager.Add(activeWeaponNumbers[i]);
            }
            
        }
        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, PlayerIndex.One);
            base.Update(gameTime);

            if (InputHandler.KeyReleased(Keys.I))
                StateManager.PopState();

            if (InputHandler.KeyReleased(Keys.Right))
            {
                if (cursor_X != GameRef.ScreenRectangle.Width / 15 + GameRef.ScreenRectangle.Width / 6 + (GameRef.ScreenRectangle.Width / 6) / 5)
                {
                    cursor_X += GameRef.ScreenRectangle.Width / 6 + (GameRef.ScreenRectangle.Width / 6)/5;//200;
                    cursor_item_number += 1;
                }
            }

            if (InputHandler.KeyReleased(Keys.Left))
            {
                if (cursor_X != GameRef.ScreenRectangle.Width / 15)
                {
                    cursor_X -= GameRef.ScreenRectangle.Width / 6 + (GameRef.ScreenRectangle.Width / 6) / 5;//200;
                    cursor_item_number -= 1;
                }
            }

            if (InputHandler.KeyReleased(Keys.Up))
            {
                if (cursor_Y != GameRef.ScreenRectangle.Height / 5)
                {
                    cursor_Y -= GameRef.ScreenRectangle.Height / 6 + (GameRef.ScreenRectangle.Height / 6) / 10;//140;
                    cursor_item_number -= 2;
                }
            }

            if (InputHandler.KeyReleased(Keys.Down))
            {
                if (cursor_Y != GameRef.ScreenRectangle.Height / 5 + ((GameRef.ScreenRectangle.Height / 6 + (GameRef.ScreenRectangle.Height / 6) / 10) * 3))
                {
                    cursor_Y += GameRef.ScreenRectangle.Height / 6 + (GameRef.ScreenRectangle.Height / 6)/10;//140;
                    cursor_item_number += 2;
                }
            }

            UpdateActiveItems();
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.spriteBatch.Begin();
            base.Draw(gameTime);

            GameRef.spriteBatch.Draw(
                inventoryBackground,
                GameRef.ScreenRectangle,
                Color.White);

            //Cursor
            GameRef.spriteBatch.Draw(
                inventoryCursorImage,
                new Rectangle(cursor_X, cursor_Y, GameRef.ScreenRectangle.Width / 6, GameRef.ScreenRectangle.Height / 6), //150, 100),
                Color.White);
            //------

            //Inventory Weapons------
            if(StoryProgress.ProgressLine["Axe"] == true)
            {
                GameRef.spriteBatch.Draw(
                    axeImage,
                    new Rectangle(GameRef.ScreenRectangle.Width / 15 + (GameRef.ScreenRectangle.Width / 6) / 6 + (GameRef.ScreenRectangle.Width / 6 + (GameRef.ScreenRectangle.Width / 6) / 5) * 0, GameRef.ScreenRectangle.Height / 5 + ((GameRef.ScreenRectangle.Height / 6 + (GameRef.ScreenRectangle.Height / 6) / 10) * 0), GameRef.ScreenRectangle.Width / 8, GameRef.ScreenRectangle.Height / 6),
                    Color.White);
            }

            if (StoryProgress.ProgressLine["Sword"] == true)
            {
                GameRef.spriteBatch.Draw(
                    swordImage,
                    new Rectangle(GameRef.ScreenRectangle.Width / 15 + (GameRef.ScreenRectangle.Width / 6) / 6 + (GameRef.ScreenRectangle.Width / 6 + (GameRef.ScreenRectangle.Width / 6) / 5) * 1, GameRef.ScreenRectangle.Height / 5 + ((GameRef.ScreenRectangle.Height / 6 + (GameRef.ScreenRectangle.Height / 6) / 10) * 0), GameRef.ScreenRectangle.Width / 8, GameRef.ScreenRectangle.Height / 6), 
                    Color.White);
            }

            if (StoryProgress.ProgressLine["Crossbow"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(GameRef.ScreenRectangle.Width / 15 + (GameRef.ScreenRectangle.Width / 6) / 6 + (GameRef.ScreenRectangle.Width / 6 + (GameRef.ScreenRectangle.Width / 6) / 5) * 0, GameRef.ScreenRectangle.Height / 5 + ((GameRef.ScreenRectangle.Height / 6 + (GameRef.ScreenRectangle.Height / 6) / 10) * 1), GameRef.ScreenRectangle.Width / 8, GameRef.ScreenRectangle.Height / 6),
                    Color.White);
            }

            if (StoryProgress.ProgressLine["Spear"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(GameRef.ScreenRectangle.Width / 15 + (GameRef.ScreenRectangle.Width / 6) / 6 + (GameRef.ScreenRectangle.Width / 6 + (GameRef.ScreenRectangle.Width / 6) / 5) * 1, GameRef.ScreenRectangle.Height / 5 + ((GameRef.ScreenRectangle.Height / 6 + (GameRef.ScreenRectangle.Height / 6) / 10) * 1), GameRef.ScreenRectangle.Width / 8, GameRef.ScreenRectangle.Height / 6),
                    Color.White);
            }

            if (StoryProgress.ProgressLine["DOOM-erang"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(GameRef.ScreenRectangle.Width / 15 + (GameRef.ScreenRectangle.Width / 6) / 6 + (GameRef.ScreenRectangle.Width / 6 + (GameRef.ScreenRectangle.Width / 6) / 5) * 0, GameRef.ScreenRectangle.Height / 5 + ((GameRef.ScreenRectangle.Height / 6 + (GameRef.ScreenRectangle.Height / 6) / 10) * 2), GameRef.ScreenRectangle.Width / 8, GameRef.ScreenRectangle.Height / 6),
                    Color.White);
            }

            if (StoryProgress.ProgressLine["Hammer"] == true)
            {
                GameRef.spriteBatch.Draw(crossbowImage,
                    new Rectangle(GameRef.ScreenRectangle.Width / 15 + (GameRef.ScreenRectangle.Width / 6) / 6 + (GameRef.ScreenRectangle.Width / 6 + (GameRef.ScreenRectangle.Width / 6) / 5) * 1, GameRef.ScreenRectangle.Height / 5 + ((GameRef.ScreenRectangle.Height / 6 + (GameRef.ScreenRectangle.Height / 6) / 10) * 2), GameRef.ScreenRectangle.Width / 8, GameRef.ScreenRectangle.Height / 6),
                    Color.White);
            }

            if (StoryProgress.ProgressLine["MetalBladeCrossbow"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(GameRef.ScreenRectangle.Width / 15 + (GameRef.ScreenRectangle.Width / 6) / 6 + (GameRef.ScreenRectangle.Width / 6 + (GameRef.ScreenRectangle.Width / 6) / 5) * 0, GameRef.ScreenRectangle.Height / 5 + ((GameRef.ScreenRectangle.Height / 6 + (GameRef.ScreenRectangle.Height / 6) / 10) * 3), GameRef.ScreenRectangle.Width / 8, GameRef.ScreenRectangle.Height / 6),
                    Color.White);
            }

            if (StoryProgress.ProgressLine["Hookshot"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(GameRef.ScreenRectangle.Width / 15 + (GameRef.ScreenRectangle.Width / 6) / 6 + (GameRef.ScreenRectangle.Width / 6 + (GameRef.ScreenRectangle.Width / 6) / 5) * 1, GameRef.ScreenRectangle.Height / 5 + ((GameRef.ScreenRectangle.Height / 6 + (GameRef.ScreenRectangle.Height / 6) / 10) * 3), GameRef.ScreenRectangle.Width / 8, GameRef.ScreenRectangle.Height / 6),
                    Color.White);
            }
            //-------

            //Active item slots
            for (int i = 0; i < 5; i++)
            {
                GameRef.spriteBatch.Draw(
                    inventoryCursorImage,
                    new Rectangle(GameRef.ScreenRectangle.Width / 15 + ((GameRef.ScreenRectangle.Width / 15 + GameRef.ScreenRectangle.Width / 15) * i), GameRef.ScreenRectangle.Height / 12, GameRef.ScreenRectangle.Width / 15, GameRef.ScreenRectangle.Height / 15), //90, 50), //new Rectangle(95 + (160 * i), 65, 90, 50), GameRef.ScreenRectangle.Width
                    Color.White);
            }
            //--

            //Active items ------------------------
            string key_string;
            int key_number;
            if (StoryProgress.activeItemsDict.ContainsKey("Axe"))
            {
                key_string = StoryProgress.activeItemsDict["Axe"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(
                    axeImage,
                    new Rectangle((GameRef.ScreenRectangle.Width / 15) + GameRef.ScreenRectangle.Width / 90 + ((GameRef.ScreenRectangle.Width / 15 + GameRef.ScreenRectangle.Width / 15) * (key_number - 1)), GameRef.ScreenRectangle.Height / 12, GameRef.ScreenRectangle.Width / (20), GameRef.ScreenRectangle.Height / 15),
                    Color.White); //new Rectangle(110 + (160 * (key_number - 1))
            }

            if (StoryProgress.activeItemsDict.ContainsKey("Sword"))
            {
                key_string = StoryProgress.activeItemsDict["Sword"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(
                    swordImage,
                    new Rectangle((GameRef.ScreenRectangle.Width / 15) + GameRef.ScreenRectangle.Width / 90 + ((GameRef.ScreenRectangle.Width / 15 + GameRef.ScreenRectangle.Width / 15) * (key_number - 1)), GameRef.ScreenRectangle.Height / 12, GameRef.ScreenRectangle.Width / (20), GameRef.ScreenRectangle.Height / 15),
                    Color.White);
            }

            if (StoryProgress.activeItemsDict.ContainsKey("Crossbow"))
            {
                key_string = StoryProgress.activeItemsDict["Crossbow"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle((GameRef.ScreenRectangle.Width / 15) + GameRef.ScreenRectangle.Width / 90 + ((GameRef.ScreenRectangle.Width / 15 + GameRef.ScreenRectangle.Width / 15) * (key_number - 1)), GameRef.ScreenRectangle.Height / 12, GameRef.ScreenRectangle.Width / (20), GameRef.ScreenRectangle.Height / 15),
                    Color.White);
            }

            if (StoryProgress.activeItemsDict.ContainsKey("Spear"))
            {
                key_string = StoryProgress.activeItemsDict["Spear"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle((GameRef.ScreenRectangle.Width / 15) + GameRef.ScreenRectangle.Width / 90 + ((GameRef.ScreenRectangle.Width / 15 + GameRef.ScreenRectangle.Width / 15) * (key_number - 1)), GameRef.ScreenRectangle.Height / 12, GameRef.ScreenRectangle.Width / (20), GameRef.ScreenRectangle.Height / 15),
                    Color.White);
            }

            if (StoryProgress.activeItemsDict.ContainsKey("DOOM-erang"))
            {
                key_string = StoryProgress.activeItemsDict["DOOM-erang"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle((GameRef.ScreenRectangle.Width / 15) + GameRef.ScreenRectangle.Width / 90 + ((GameRef.ScreenRectangle.Width / 15 + GameRef.ScreenRectangle.Width / 15) * (key_number - 1)), GameRef.ScreenRectangle.Height / 12, GameRef.ScreenRectangle.Width / (20), GameRef.ScreenRectangle.Height / 15),
                    Color.White);
            }

            if (StoryProgress.activeItemsDict.ContainsKey("Hammer"))
            {
                key_string = StoryProgress.activeItemsDict["Hammer"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle((GameRef.ScreenRectangle.Width / 15) + GameRef.ScreenRectangle.Width / 90 + ((GameRef.ScreenRectangle.Width / 15 + GameRef.ScreenRectangle.Width / 15) * (key_number - 1)), GameRef.ScreenRectangle.Height / 12, GameRef.ScreenRectangle.Width / (20), GameRef.ScreenRectangle.Height / 15),
                    Color.White);
            }

            if (StoryProgress.activeItemsDict.ContainsKey("MetalBladeCrossbow"))
            {
                key_string = StoryProgress.activeItemsDict["MetalBladeCrossbow"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle((GameRef.ScreenRectangle.Width / 15) + GameRef.ScreenRectangle.Width / 90 + ((GameRef.ScreenRectangle.Width / 15 + GameRef.ScreenRectangle.Width / 15) * (key_number - 1)), GameRef.ScreenRectangle.Height / 12, GameRef.ScreenRectangle.Width / (20), GameRef.ScreenRectangle.Height / 15),
                    Color.White);
            }

            if (StoryProgress.activeItemsDict.ContainsKey("Hookshot"))
            {
                key_string = StoryProgress.activeItemsDict["Hookshot"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle((GameRef.ScreenRectangle.Width / 15) + GameRef.ScreenRectangle.Width / 90 + ((GameRef.ScreenRectangle.Width / 15 + GameRef.ScreenRectangle.Width / 15) * (key_number - 1)), GameRef.ScreenRectangle.Height / 12, GameRef.ScreenRectangle.Width / (20), GameRef.ScreenRectangle.Height / 15),
                    Color.White);
            }
            //-------------------------
            if (StoryProgress.ProgressLine["belladonnaHave"] == true)
            {
                GameRef.spriteBatch.Draw(
                        BelladonnaImage,
                        new Rectangle(GameRef.ScreenRectangle.Width / 2 + (GameRef.ScreenRectangle.Width / 2)/10, GameRef.ScreenRectangle.Height / 5 + ((GameRef.ScreenRectangle.Height / 6 + (GameRef.ScreenRectangle.Height / 6) / 10) * 0), GameRef.ScreenRectangle.Width / 8, GameRef.ScreenRectangle.Height / 6),  //new Rectangle(750, 180 + (140 * 0), 100, 90),
                        Color.White);
            }

            if (StoryProgress.ProgressLine["immortuiHave"] == true)
            {
                GameRef.spriteBatch.Draw(
                            ImmortuiImage,
                            new Rectangle(GameRef.ScreenRectangle.Width / 2 + (GameRef.ScreenRectangle.Width / 2) / 10 + (GameRef.ScreenRectangle.Width / 6 + (GameRef.ScreenRectangle.Width / 6) / 5) * 1  /*- GameRef.ScreenRectangle.Width / 15 - (GameRef.ScreenRectangle.Width / 6) / 6 - (GameRef.ScreenRectangle.Width / 6 - (GameRef.ScreenRectangle.Width / 6) / 5) * 1*/, GameRef.ScreenRectangle.Height / 5 + ((GameRef.ScreenRectangle.Height / 6 + (GameRef.ScreenRectangle.Height / 6) / 10) * 0), GameRef.ScreenRectangle.Width / 8, GameRef.ScreenRectangle.Height / 6),
                            Color.White);
            }

            GameRef.spriteBatch.Draw(
                    lifePotatoImage,
                    new Rectangle(GameRef.ScreenRectangle.Width / 2 + (GameRef.ScreenRectangle.Width / 2) / 10, GameRef.ScreenRectangle.Height / 5 + ((GameRef.ScreenRectangle.Height / 6 + (GameRef.ScreenRectangle.Height / 6) / 10) * 3), GameRef.ScreenRectangle.Width / 8, GameRef.ScreenRectangle.Height / 6),
                    Color.White);



            ControlManager.Draw(GameRef.spriteBatch);

            GameRef.spriteBatch.End();
        }
        #endregion

        #region Game State Method Region
        #endregion

        #region Methods Region
        
        private void UpdateActiveItems()
        {
            ContentManager Content = GameRef.Content;

            if (InputHandler.KeyReleased(Keys.D1))
            {
                ResetKey(Keys.D1);
                AssignKeyToItem(Keys.D1);
            }

            if (InputHandler.KeyReleased(Keys.D2))
            {
                ResetKey(Keys.D2);
                AssignKeyToItem(Keys.D2);
            }

            if (InputHandler.KeyReleased(Keys.D3))
            {
                ResetKey(Keys.D3);
                AssignKeyToItem(Keys.D3);
            }

            if (InputHandler.KeyReleased(Keys.D4))
            {
                ResetKey(Keys.D4);
                AssignKeyToItem(Keys.D4);
            }

            if (InputHandler.KeyReleased(Keys.D5))
            {
                ResetKey(Keys.D5);
                AssignKeyToItem(Keys.D5);
            }

            numberOfLifePotatoes.Text = "x " + StoryProgress.numberOfItemsDict["LifePotato"].ToString();
        }

        private void ResetKey(Keys Key)
        {
            foreach (var item in StoryProgress.activeItemsDict.Where(dictkey => dictkey.Value == Key).ToList())
            {
                StoryProgress.activeItemsDict.Remove(item.Key);
            }
        }

        private void AssignKeyToItem(Keys Key)
        {
            if (cursor_item_number == 0 && StoryProgress.ProgressLine["Axe"] == true)
            {
                StoryProgress.activeItemsDict["Axe"] = Key;
            }
            if (cursor_item_number == 1 && StoryProgress.ProgressLine["Sword"] == true)
            {
                StoryProgress.activeItemsDict["Sword"] = Key;
            }
            if (cursor_item_number == 2 && StoryProgress.ProgressLine["Crossbow"] == true)
            {
                StoryProgress.activeItemsDict["Crossbow"] = Key;
            }
            if (cursor_item_number == 3 && StoryProgress.ProgressLine["Spear"] == true)
            {
                StoryProgress.activeItemsDict["Spear"] = Key;
            }
            if (cursor_item_number == 4 && StoryProgress.ProgressLine["DOOM-erang"] == true)
            {
                StoryProgress.activeItemsDict["DOOM-erang"] = Key;
            }
            if (cursor_item_number == 5 && StoryProgress.ProgressLine["Hammer"] == true)
            {
                StoryProgress.activeItemsDict["Hammer"] = Key;
            }
            if (cursor_item_number == 6 && StoryProgress.ProgressLine["MetalBladeCrossbow"] == true)
            {
                StoryProgress.activeItemsDict["MetalBladeCrossbow"] = Key;
            }
            if (cursor_item_number == 7 && StoryProgress.ProgressLine["Hookshot"] == true)
            {
                StoryProgress.activeItemsDict["Hookshot"] = Key;
            }
        }

        #endregion
    }
}
