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
        Texture2D axeImage, swordImage, crossbowImage;
        Texture2D inventoryCursorImage;
        int cursor_X, cursor_Y;
        int cursor_item_number;

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
            cursor_X = 75; 
            cursor_Y = 175;
            cursor_item_number = 0;

            base.Initialize();
        }
        protected override void LoadContent()
        {
            ContentManager Content = GameRef.Content;

            inventoryBackground = Content.Load<Texture2D>(@"Backgrounds\Inventory test");
            axeImage = Content.Load<Texture2D>(@"Sprite\Axe");
            swordImage = Content.Load<Texture2D>(@"Sprite\Inv Sword test");
            crossbowImage = Content.Load<Texture2D>(@"Sprite\Bow");
            inventoryCursorImage = Content.Load<Texture2D>(@"Sprite\Inventory Cursor test");

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, PlayerIndex.One);
            base.Update(gameTime);

            if (InputHandler.KeyReleased(Keys.I))
                StateManager.PopState();

            if (InputHandler.KeyReleased(Keys.Right))
            {
                if (cursor_X != 275)
                {
                    cursor_X += 200;
                    cursor_item_number += 1;
                }
            }

            if (InputHandler.KeyReleased(Keys.Left))
            {
                if (cursor_X != 75)
                {
                    cursor_X -= 200;
                    cursor_item_number -= 1;
                }
            }

            if (InputHandler.KeyReleased(Keys.Up))
            {
                if (cursor_Y != 175)
                {
                    cursor_Y -= 140;
                    cursor_item_number -= 2;
                }
            }

            if (InputHandler.KeyReleased(Keys.Down))
            {
                if (cursor_Y != 175 + (140 * 3))
                {
                    cursor_Y += 140;
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
                new Rectangle(cursor_X, cursor_Y, 150, 100),
                Color.White);
            //------

            //Inventory Weapons------
            if(StoryProgress.ProgressLine["Axe"] == true)
            {
                GameRef.spriteBatch.Draw(
                    axeImage,
                    new Rectangle(100, 180, 100, 90),
                    Color.White);
            }

            if (StoryProgress.ProgressLine["Sword"] == true)
            {
                GameRef.spriteBatch.Draw(
                    swordImage,
                    new Rectangle(300, 180, 100, 90),
                    Color.White);
            }

            if (StoryProgress.ProgressLine["Crossbow"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(100, 180 + (140 * 1), 100, 90),
                    Color.White);
            }

            if (StoryProgress.ProgressLine["Spear"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(300, 180 + (140 * 1), 100, 90),
                    Color.White);
            }

            if (StoryProgress.ProgressLine["DOOM-erang"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(100, 180 + (140 * 2), 100, 90),
                    Color.White);
            }

            if (StoryProgress.ProgressLine["Hammer"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(300, 180 + (140 * 2), 100, 90),
                    Color.White);
            }

            if (StoryProgress.ProgressLine["MetalBladeCrossbow"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(100, 180 + (140 * 3), 100, 90),
                    Color.White);
            }

            if (StoryProgress.ProgressLine["Hookshot"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(300, 180 + (140 * 3), 100, 90),
                    Color.White);
            }
            //-------

            //Active items ------------------------
            string key_string;
            int key_number;
            if (StoryProgress.activeItemsDict.ContainsKey("Axe"))
            {
                key_string = StoryProgress.activeItemsDict["Axe"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(axeImage, new Rectangle(95 + (160 * (key_number - 1)), 70, 50, 45), Color.White);
            }

            if (StoryProgress.activeItemsDict.ContainsKey("Sword"))
            {
                key_string = StoryProgress.activeItemsDict["Sword"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(swordImage, new Rectangle(95 + (160 * (key_number - 1)), 70, 50, 45), Color.White);
            }

            if (StoryProgress.activeItemsDict.ContainsKey("Crossbow"))
            {
                key_string = StoryProgress.activeItemsDict["Crossbow"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(crossbowImage, new Rectangle(95 + (160 * (key_number - 1)), 70, 50, 45), Color.White);
            }

            if (StoryProgress.activeItemsDict.ContainsKey("Spear"))
            {
                key_string = StoryProgress.activeItemsDict["Spear"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(crossbowImage, new Rectangle(95 + (160 * (key_number - 1)), 70, 50, 45), Color.White);
            }

            if (StoryProgress.activeItemsDict.ContainsKey("DOOM-erang"))
            {
                key_string = StoryProgress.activeItemsDict["DOOM-erang"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(crossbowImage, new Rectangle(95 + (160 * (key_number - 1)), 70, 50, 45), Color.White);
            }

            if (StoryProgress.activeItemsDict.ContainsKey("Hammer"))
            {
                key_string = StoryProgress.activeItemsDict["Hammer"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(crossbowImage, new Rectangle(95 + (160 * (key_number - 1)), 70, 50, 45), Color.White);
            }

            if (StoryProgress.activeItemsDict.ContainsKey("MetalBladeCrossbow"))
            {
                key_string = StoryProgress.activeItemsDict["MetalBladeCrossbow"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(crossbowImage, new Rectangle(95 + (160 * (key_number - 1)), 70, 50, 45), Color.White);
            }

            if (StoryProgress.activeItemsDict.ContainsKey("Hookshot"))
            {
                key_string = StoryProgress.activeItemsDict["Hookshot"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(crossbowImage, new Rectangle(95 + (160 * (key_number - 1)), 70, 50, 45), Color.White);
            }
            //-------------------------

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
