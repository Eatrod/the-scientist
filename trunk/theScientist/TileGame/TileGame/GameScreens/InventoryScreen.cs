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

namespace TileGame.GameScreens
{
    public class InventoryScreen : BaseGameState
    {
        #region Field region

        Texture2D inventoryBackground;
        String[] activeItems_helper;
        Texture2D axeImage, swordImage, crossbowImage;
        Texture2D inventoryCursorImage;
        int cursor_X, cursor_Y;
        int cursor_item_number;
        Texture2D[] activeItem_textures;
        static public Dictionary<string, Keys> activeItemsDict;

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
            axeImage = Content.Load<Texture2D>(@"Sprite\Inv Axe test");
            swordImage = Content.Load<Texture2D>(@"Sprite\Inv Sword test");
            crossbowImage = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
            inventoryCursorImage = Content.Load<Texture2D>(@"Sprite\Inventory Cursor test");
            activeItem_textures = new Texture2D[5];
            for (int i = 0; i < activeItem_textures.Count(); i++)
            {
                activeItem_textures[i] = Content.Load<Texture2D>(@"Sprite\Inventory Empty test");
            }
            activeItems_helper = new string[5];

            activeItemsDict = new Dictionary<string, Keys>();

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

            //Active items ------------------------
            for (int i = 0; i < activeItem_textures.Count(); i++)
            {
                GameRef.spriteBatch.Draw(
                activeItem_textures[i],
                new Rectangle(95 + (160 * i), 70, 50, 45),
                Color.White);
            }
            //-----------------------------

            //Cursor
            GameRef.spriteBatch.Draw(
                inventoryCursorImage,
                new Rectangle(cursor_X, cursor_Y, 150, 100),
                Color.White);
            //------

            //Inventory Weapons------
            if( PlayerScreen.inventoryDict["Axe"] == true)
            {
                GameRef.spriteBatch.Draw(
                    axeImage,
                    new Rectangle(100, 180, 100, 90),
                    Color.White);
            }

            if (PlayerScreen.inventoryDict["Sword"] == true)
            {
                GameRef.spriteBatch.Draw(
                    swordImage,
                    new Rectangle(300, 180, 100, 90),
                    Color.White);
            }

            if (PlayerScreen.inventoryDict["Crossbow"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(100, 180 + (140 * 1), 100, 90),
                    Color.White);
            }

            if (PlayerScreen.inventoryDict["Spear"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(300, 180 + (140 * 1), 100, 90),
                    Color.White);
            }

            if (PlayerScreen.inventoryDict["BOOM-erang"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(100, 180 + (140 * 2), 100, 90),
                    Color.White);
            }

            if (PlayerScreen.inventoryDict["Hammer?"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(300, 180 + (140 * 2), 100, 90),
                    Color.White);
            }

            if (PlayerScreen.inventoryDict["MetalBladeCrossbow?"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(100, 180 + (140 * 3), 100, 90),
                    Color.White);
            }

            if (PlayerScreen.inventoryDict["Hookshot?"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(300, 180 + (140 * 3), 100, 90),
                    Color.White);
            }
            //-------

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
                ResetSelectedActiveItemTextures();

                if (cursor_item_number == 0 && PlayerScreen.inventoryDict["Axe"] == true)
                {
                    activeItem_textures[0] = Content.Load<Texture2D>(@"Sprite\Inv Axe test");
                    activeItems_helper[0] = "Axe";
                    activeItemsDict["Axe"] = Keys.D1;
                }
                if (cursor_item_number == 1 && PlayerScreen.inventoryDict["Sword"] == true)
                {
                    activeItem_textures[0] = Content.Load<Texture2D>(@"Sprite\Inv Sword test");
                    activeItems_helper[0] = "Sword";
                    activeItemsDict["Sword"] = Keys.D1;
                }
                if (cursor_item_number == 2 && PlayerScreen.inventoryDict["Crossbow"] == true)
                {
                    activeItem_textures[0] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[0] = "Crossbow";
                    activeItemsDict["Crossbow"] = Keys.D1;
                }
                if (cursor_item_number == 3 && PlayerScreen.inventoryDict["Spear"] == true)
                {
                    activeItem_textures[0] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[0] = "Spear";
                    activeItemsDict["Spear"] = Keys.D1;
                }
                if (cursor_item_number == 4 && PlayerScreen.inventoryDict["BOOM-erang"] == true)
                {
                    activeItem_textures[0] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[0] = "BOOM-erang";
                    activeItemsDict["BOOM-erang"] = Keys.D1;
                }
                if (cursor_item_number == 5 && PlayerScreen.inventoryDict["Hammer?"] == true)
                {
                    activeItem_textures[0] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[0] = "Hammer?";
                    activeItemsDict["Hammer?"] = Keys.D1;
                }
                if (cursor_item_number == 6 && PlayerScreen.inventoryDict["MetalBladeCrossbow?"] == true)
                {
                    activeItem_textures[0] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[0] = "MetalBladeCrossbow?";
                    activeItemsDict["MetalBladeCrossbow?"] = Keys.D1;
                }
                if (cursor_item_number == 7 && PlayerScreen.inventoryDict["Hookshot?"] == true)
                {
                    activeItem_textures[0] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[0] = "Hookshot?";
                    activeItemsDict["Hookshot?"] = Keys.D1;
                }
            }

            if (InputHandler.KeyReleased(Keys.D2))
            {
                ResetKey(Keys.D2);
                ResetSelectedActiveItemTextures();

                if (cursor_item_number == 0 && PlayerScreen.inventoryDict["Axe"] == true)
                {
                    activeItem_textures[1] = Content.Load<Texture2D>(@"Sprite\Inv Axe test");
                    activeItems_helper[1] = "Axe";
                    activeItemsDict["Axe"] = Keys.D2;
                }
                if (cursor_item_number == 1 && PlayerScreen.inventoryDict["Sword"] == true)
                {
                    activeItem_textures[1] = Content.Load<Texture2D>(@"Sprite\Inv Sword test");
                    activeItems_helper[1] = "Sword";
                    activeItemsDict["Sword"] = Keys.D2;
                }
                if (cursor_item_number == 2 && PlayerScreen.inventoryDict["Crossbow"] == true)
                {
                    activeItem_textures[1] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[1] = "Crossbow";
                    activeItemsDict["Crossbow"] = Keys.D2;
                }
                if (cursor_item_number == 3 && PlayerScreen.inventoryDict["Spear"] == true)
                {
                    activeItem_textures[1] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[1] = "Spear";
                    activeItemsDict["Spear"] = Keys.D2;
                }
                if (cursor_item_number == 4 && PlayerScreen.inventoryDict["BOOM-erang"] == true)
                {
                    activeItem_textures[1] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[1] = "BOOM-erang";
                    activeItemsDict["BOOM-erang"] = Keys.D2;
                }
                if (cursor_item_number == 5 && PlayerScreen.inventoryDict["Hammer?"] == true)
                {
                    activeItem_textures[1] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[1] = "Hammer?";
                    activeItemsDict["Hammer?"] = Keys.D2;
                }
                if (cursor_item_number == 6 && PlayerScreen.inventoryDict["MetalBladeCrossbow?"] == true)
                {
                    activeItem_textures[1] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[1] = "MetalBladeCrossbow?";
                    activeItemsDict["MetalBladeCrossbow?"] = Keys.D2;
                }
                if (cursor_item_number == 7 && PlayerScreen.inventoryDict["Hookshot?"] == true)
                {
                    activeItem_textures[1] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[1] = "Hookshot?";
                    activeItemsDict["Hookshot?"] = Keys.D2;
                }
            }
            if (InputHandler.KeyReleased(Keys.D3))
            {
                ResetKey(Keys.D3);
                ResetSelectedActiveItemTextures();

                if (cursor_item_number == 0 && PlayerScreen.inventoryDict["Axe"] == true)
                {
                    activeItem_textures[2] = Content.Load<Texture2D>(@"Sprite\Inv Axe test"); 
                    activeItems_helper[2] = "Axe";
                    activeItemsDict["Axe"] = Keys.D3;
                }
                if (cursor_item_number == 1 && PlayerScreen.inventoryDict["Sword"] == true)
                {
                    activeItem_textures[2] = Content.Load<Texture2D>(@"Sprite\Inv Sword test");
                    activeItems_helper[2] = "Sword";
                    activeItemsDict["Sword"] = Keys.D3;
                }
                if (cursor_item_number == 2 && PlayerScreen.inventoryDict["Crossbow"] == true)
                {
                    activeItem_textures[2] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[2] = "Crossbow";
                    activeItemsDict["Crossbow"] = Keys.D3;
                }
                if (cursor_item_number == 3 && PlayerScreen.inventoryDict["Spear"] == true)
                {
                    activeItem_textures[2] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[2] = "Spear";
                    activeItemsDict["Spear"] = Keys.D3;
                }
                if (cursor_item_number == 4 && PlayerScreen.inventoryDict["BOOM-erang"] == true)
                {
                    activeItem_textures[2] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[2] = "BOOM-erang";
                    activeItemsDict["BOOM-erang"] = Keys.D3;
                }
                if (cursor_item_number == 5 && PlayerScreen.inventoryDict["Hammer?"] == true)
                {
                    activeItem_textures[2] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[2] = "Hammer?";
                    activeItemsDict["Hammer?"] = Keys.D3;
                }
                if (cursor_item_number == 6 && PlayerScreen.inventoryDict["MetalBladeCrossbow?"] == true)
                {
                    activeItem_textures[2] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[2] = "MetalBladeCrossbow?";
                    activeItemsDict["MetalBladeCrossbow?"] = Keys.D3;
                }
                if (cursor_item_number == 7 && PlayerScreen.inventoryDict["Hookshot?"] == true)
                {
                    activeItem_textures[2] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[2] = "Hookshot?";
                    activeItemsDict["Hookshot?"] = Keys.D3;
                }
            }
            if (InputHandler.KeyReleased(Keys.D4))
            {
                ResetKey(Keys.D4);
                ResetSelectedActiveItemTextures();

                if (cursor_item_number == 0 && PlayerScreen.inventoryDict["Axe"] == true)
                {
                    activeItem_textures[3] = Content.Load<Texture2D>(@"Sprite\Inv Axe test");
                    activeItems_helper[3] = "Axe";
                    activeItemsDict["Axe"] = Keys.D4;
                }
                if (cursor_item_number == 1 && PlayerScreen.inventoryDict["Sword"] == true)
                {
                    activeItem_textures[3] = Content.Load<Texture2D>(@"Sprite\Inv Sword test");
                    activeItems_helper[3] = "Sword";
                    activeItemsDict["Sword"] = Keys.D4;
                }
                if (cursor_item_number == 2 && PlayerScreen.inventoryDict["Crossbow"] == true)
                {
                    activeItem_textures[3] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[3] = "Crossbow";
                    activeItemsDict["Crossbow"] = Keys.D4;
                }
                if (cursor_item_number == 3 && PlayerScreen.inventoryDict["Spear"] == true)
                {
                    activeItem_textures[3] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[3] = "Spear";
                    activeItemsDict["Spear"] = Keys.D4;
                }
                if (cursor_item_number == 4 && PlayerScreen.inventoryDict["BOOM-erang"] == true)
                {
                    activeItem_textures[3] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[3] = "BOOM-erang";
                    activeItemsDict["BOOM-erang"] = Keys.D4;
                }
                if (cursor_item_number == 5 && PlayerScreen.inventoryDict["Hammer?"] == true)
                {
                    activeItem_textures[3] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[3] = "Hammer?";
                    activeItemsDict["Hammer?"] = Keys.D4;
                }
                if (cursor_item_number == 6 && PlayerScreen.inventoryDict["MetalBladeCrossbow?"] == true)
                {
                    activeItem_textures[3] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[3] = "MetalBladeCrossbow?";
                    activeItemsDict["MetalBladeCrossbow?"] = Keys.D4;
                }
                if (cursor_item_number == 7 && PlayerScreen.inventoryDict["Hookshot?"] == true)
                {
                    activeItem_textures[3] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[3] = "Hookshot?";
                    activeItemsDict["Hookshot?"] = Keys.D4;
                }
            }
            if (InputHandler.KeyReleased(Keys.D5))
            {
                ResetKey(Keys.D5);
                ResetSelectedActiveItemTextures();

                if (cursor_item_number == 0 && PlayerScreen.inventoryDict["Axe"] == true)
                {
                    activeItem_textures[4] = Content.Load<Texture2D>(@"Sprite\Inv Axe test");
                    activeItems_helper[4] = "Axe";
                    activeItemsDict["Axe"] = Keys.D5;
                }
                if (cursor_item_number == 1 && PlayerScreen.inventoryDict["Sword"] == true)
                {
                    activeItem_textures[4] = Content.Load<Texture2D>(@"Sprite\Inv Sword test");
                    activeItems_helper[4] = "Sword";
                    activeItemsDict["Sword"] = Keys.D5;
                }
                if (cursor_item_number == 2 && PlayerScreen.inventoryDict["Crossbow"] == true)
                {
                    activeItem_textures[4] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[4] = "Crossbow";
                    activeItemsDict["Crossbow"] = Keys.D5;
                }
                if (cursor_item_number == 3 && PlayerScreen.inventoryDict["Spear"] == true)
                {
                    activeItem_textures[4] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[4] = "Spear";
                    activeItemsDict["Spear"] = Keys.D5;
                }
                if (cursor_item_number == 4 && PlayerScreen.inventoryDict["BOOM-erang"] == true)
                {
                    activeItem_textures[4] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[4] = "BOOM-erang";
                    activeItemsDict["BOOM-erang"] = Keys.D5;
                }
                if (cursor_item_number == 5 && PlayerScreen.inventoryDict["Hammer?"] == true)
                {
                    activeItem_textures[4] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[4] = "Hammer?";
                    activeItemsDict["Hammer?"] = Keys.D5;
                }
                if (cursor_item_number == 6 && PlayerScreen.inventoryDict["MetalBladeCrossbow?"] == true)
                {
                    activeItem_textures[4] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[4] = "MetalBladeCrossbow?";
                    activeItemsDict["MetalBladeCrossbow?"] = Keys.D5;
                }
                if (cursor_item_number == 7 && PlayerScreen.inventoryDict["Hookshot?"] == true)
                {
                    activeItem_textures[4] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[4] = "Hookshot?";
                    activeItemsDict["Hookshot?"] = Keys.D5;
                }
            }
        }

        private void ResetKey(Keys Key)
        {
            foreach (var item in activeItemsDict.Where(dictkey => dictkey.Value == Key).ToList())
            {
                activeItemsDict.Remove(item.Key);
            }
        }

        private void ResetSelectedActiveItemTextures()
        {
            ContentManager Content = GameRef.Content;

            if (cursor_item_number == 0)
            {
                for (int i = 0; i < activeItems_helper.Count(); i++)
                {
                    if (activeItems_helper[i] == "Axe")
                    {
                        activeItem_textures[i] = Content.Load<Texture2D>(@"Sprite\Inventory Empty test");
                    }
                }
            }
            if (cursor_item_number == 1)
            {
                for (int i = 0; i < activeItems_helper.Count(); i++)
                {
                    if (activeItems_helper[i] == "Sword")
                    {
                        activeItem_textures[i] = Content.Load<Texture2D>(@"Sprite\Inventory Empty test");
                    }
                }
            }
            if (cursor_item_number == 2)
            {
                for (int i = 0; i < activeItems_helper.Count(); i++)
                {
                    if (activeItems_helper[i] == "Crossbow")
                    {
                        activeItem_textures[i] = Content.Load<Texture2D>(@"Sprite\Inventory Empty test");
                    }
                }
            }
            if (cursor_item_number == 3)
            {
                for (int i = 0; i < activeItems_helper.Count(); i++)
                {
                    if (activeItems_helper[i] == "Spear")
                    {
                        activeItem_textures[i] = Content.Load<Texture2D>(@"Sprite\Inventory Empty test");
                    }
                }
            }
            if (cursor_item_number == 4)
            {
                for (int i = 0; i < activeItems_helper.Count(); i++)
                {
                    if (activeItems_helper[i] == "BOOM-erang")
                    {
                        activeItem_textures[i] = Content.Load<Texture2D>(@"Sprite\Inventory Empty test");
                    }
                }
            }
            if (cursor_item_number == 5)
            {
                for (int i = 0; i < activeItems_helper.Count(); i++)
                {
                    if (activeItems_helper[i] == "Hammer?")
                    {
                        activeItem_textures[i] = Content.Load<Texture2D>(@"Sprite\Inventory Empty test");
                    }
                }
            }
            if (cursor_item_number == 6)
            {
                for (int i = 0; i < activeItems_helper.Count(); i++)
                {
                    if (activeItems_helper[i] == "MetalBladeCrossbow?")
                    {
                        activeItem_textures[i] = Content.Load<Texture2D>(@"Sprite\Inventory Empty test");
                    }
                }
            }
            if (cursor_item_number == 7)
            {
                for (int i = 0; i < activeItems_helper.Count(); i++)
                {
                    if (activeItems_helper[i] == "Hookshot?")
                    {
                        activeItem_textures[i] = Content.Load<Texture2D>(@"Sprite\Inventory Empty test");
                    }
                }
            }
        }

        #endregion
    }
}
