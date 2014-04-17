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
            activeItem_textures = new Texture2D[5];
            for (int i = 0; i < activeItem_textures.Count(); i++)
            {
                activeItem_textures[i] = Content.Load<Texture2D>(@"Sprite\Inventory Empty test");
            }
            activeItems_helper = new string[5];

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

                if (cursor_item_number == 0 && StoryProgress.ProgressLine["Axe"] == true)
                {
                    activeItem_textures[0] = Content.Load<Texture2D>(@"Sprite\Axe");
                    activeItems_helper[0] = "Axe";
                    StoryProgress.activeItemsDict["Axe"] = Keys.D1;
                }
                if (cursor_item_number == 1 && StoryProgress.ProgressLine["Sword"] == true)
                {
                    activeItem_textures[0] = Content.Load<Texture2D>(@"Sprite\Inv Sword test");
                    activeItems_helper[0] = "Sword";
                    StoryProgress.activeItemsDict["Sword"] = Keys.D1;
                }
                if (cursor_item_number == 2 && StoryProgress.ProgressLine["Crossbow"] == true)
                {
                    activeItem_textures[0] = Content.Load<Texture2D>(@"Sprite\Bow");
                    activeItems_helper[0] = "Crossbow";
                    StoryProgress.activeItemsDict["Crossbow"] = Keys.D1;
                }
                if (cursor_item_number == 3 && StoryProgress.ProgressLine["Spear"] == true)
                {
                    activeItem_textures[0] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[0] = "Spear";
                    StoryProgress.activeItemsDict["Spear"] = Keys.D1;
                }
                if (cursor_item_number == 4 && StoryProgress.ProgressLine["DOOM-erang"] == true)
                {
                    activeItem_textures[0] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[0] = "DOOM-erang";
                    StoryProgress.activeItemsDict["DOOM-erang"] = Keys.D1;
                }
                if (cursor_item_number == 5 && StoryProgress.ProgressLine["Hammer"] == true)
                {
                    activeItem_textures[0] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[0] = "Hammer";
                    StoryProgress.activeItemsDict["Hammer"] = Keys.D1;
                }
                if (cursor_item_number == 6 && StoryProgress.ProgressLine["MetalBladeCrossbow"] == true)
                {
                    activeItem_textures[0] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[0] = "MetalBladeCrossbow";
                    StoryProgress.activeItemsDict["MetalBladeCrossbow"] = Keys.D1;
                }
                if (cursor_item_number == 7 && StoryProgress.ProgressLine["Hookshot"] == true)
                {
                    activeItem_textures[0] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[0] = "Hookshot";
                    StoryProgress.activeItemsDict["Hookshot"] = Keys.D1;
                }
            }

            if (InputHandler.KeyReleased(Keys.D2))
            {
                ResetKey(Keys.D2);
                ResetSelectedActiveItemTextures();

                if (cursor_item_number == 0 && StoryProgress.ProgressLine["Axe"] == true)
                {
                    activeItem_textures[1] = Content.Load<Texture2D>(@"Sprite\Axe");
                    activeItems_helper[1] = "Axe";
                    StoryProgress.activeItemsDict["Axe"] = Keys.D2;
                }
                if (cursor_item_number == 1 && StoryProgress.ProgressLine["Sword"] == true)
                {
                    activeItem_textures[1] = Content.Load<Texture2D>(@"Sprite\Inv Sword test");
                    activeItems_helper[1] = "Sword";
                    StoryProgress.activeItemsDict["Sword"] = Keys.D2;
                }
                if (cursor_item_number == 2 && StoryProgress.ProgressLine["Crossbow"] == true)
                {
                    activeItem_textures[1] = Content.Load<Texture2D>(@"Sprite\Bow");
                    activeItems_helper[1] = "Crossbow";
                    StoryProgress.activeItemsDict["Crossbow"] = Keys.D2;
                }
                if (cursor_item_number == 3 && StoryProgress.ProgressLine["Spear"] == true)
                {
                    activeItem_textures[1] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[1] = "Spear";
                    StoryProgress.activeItemsDict["Spear"] = Keys.D2;
                }
                if (cursor_item_number == 4 && StoryProgress.ProgressLine["DOOM-erang"] == true)
                {
                    activeItem_textures[1] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[1] = "DOOM-erang";
                    StoryProgress.activeItemsDict["DOOM-erang"] = Keys.D2;
                }
                if (cursor_item_number == 5 && StoryProgress.ProgressLine["Hammer"] == true)
                {
                    activeItem_textures[1] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[1] = "Hammer";
                    StoryProgress.activeItemsDict["Hammer"] = Keys.D2;
                }
                if (cursor_item_number == 6 && StoryProgress.ProgressLine["MetalBladeCrossbow"] == true)
                {
                    activeItem_textures[1] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[1] = "MetalBladeCrossbow";
                    StoryProgress.activeItemsDict["MetalBladeCrossbow"] = Keys.D2;
                }
                if (cursor_item_number == 7 && StoryProgress.ProgressLine["Hookshot"] == true)
                {
                    activeItem_textures[1] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[1] = "Hookshot";
                    StoryProgress.activeItemsDict["Hookshot"] = Keys.D2;
                }
            }
            if (InputHandler.KeyReleased(Keys.D3))
            {
                ResetKey(Keys.D3);
                ResetSelectedActiveItemTextures();

                if (cursor_item_number == 0 && StoryProgress.ProgressLine["Axe"] == true)
                {
                    activeItem_textures[2] = Content.Load<Texture2D>(@"Sprite\Axe"); 
                    activeItems_helper[2] = "Axe";
                    StoryProgress.activeItemsDict["Axe"] = Keys.D3;
                }
                if (cursor_item_number == 1 && StoryProgress.ProgressLine["Sword"] == true)
                {
                    activeItem_textures[2] = Content.Load<Texture2D>(@"Sprite\Inv Sword test");
                    activeItems_helper[2] = "Sword";
                    StoryProgress.activeItemsDict["Sword"] = Keys.D3;
                }
                if (cursor_item_number == 2 && StoryProgress.ProgressLine["Crossbow"] == true)
                {
                    activeItem_textures[2] = Content.Load<Texture2D>(@"Sprite\Bow");
                    activeItems_helper[2] = "Crossbow";
                    StoryProgress.activeItemsDict["Crossbow"] = Keys.D3;
                }
                if (cursor_item_number == 3 && StoryProgress.ProgressLine["Spear"] == true)
                {
                    activeItem_textures[2] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[2] = "Spear";
                    StoryProgress.activeItemsDict["Spear"] = Keys.D3;
                }
                if (cursor_item_number == 4 && StoryProgress.ProgressLine["DOOM-erang"] == true)
                {
                    activeItem_textures[2] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[2] = "DOOM-erang";
                    StoryProgress.activeItemsDict["DOOM-erang"] = Keys.D3;
                }
                if (cursor_item_number == 5 && StoryProgress.ProgressLine["Hammer"] == true)
                {
                    activeItem_textures[2] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[2] = "Hammer";
                    StoryProgress.activeItemsDict["Hammer"] = Keys.D3;
                }
                if (cursor_item_number == 6 && StoryProgress.ProgressLine["MetalBladeCrossbow"] == true)
                {
                    activeItem_textures[2] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[2] = "MetalBladeCrossbow";
                    StoryProgress.activeItemsDict["MetalBladeCrossbow"] = Keys.D3;
                }
                if (cursor_item_number == 7 && StoryProgress.ProgressLine["Hookshot"] == true)
                {
                    activeItem_textures[2] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[2] = "Hookshot";
                    StoryProgress.activeItemsDict["Hookshot"] = Keys.D3;
                }
            }
            if (InputHandler.KeyReleased(Keys.D4))
            {
                ResetKey(Keys.D4);
                ResetSelectedActiveItemTextures();

                if (cursor_item_number == 0 && StoryProgress.ProgressLine["Axe"] == true)
                {
                    activeItem_textures[3] = Content.Load<Texture2D>(@"Sprite\Axe");
                    activeItems_helper[3] = "Axe";
                    StoryProgress.activeItemsDict["Axe"] = Keys.D4;
                }
                if (cursor_item_number == 1 && StoryProgress.ProgressLine["Sword"] == true)
                {
                    activeItem_textures[3] = Content.Load<Texture2D>(@"Sprite\Inv Sword test");
                    activeItems_helper[3] = "Sword";
                    StoryProgress.activeItemsDict["Sword"] = Keys.D4;
                }
                if (cursor_item_number == 2 && StoryProgress.ProgressLine["Crossbow"] == true)
                {
                    activeItem_textures[3] = Content.Load<Texture2D>(@"Sprite\Bow");
                    activeItems_helper[3] = "Crossbow";
                    StoryProgress.activeItemsDict["Crossbow"] = Keys.D4;
                }
                if (cursor_item_number == 3 && StoryProgress.ProgressLine["Spear"] == true)
                {
                    activeItem_textures[3] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[3] = "Spear";
                    StoryProgress.activeItemsDict["Spear"] = Keys.D4;
                }
                if (cursor_item_number == 4 && StoryProgress.ProgressLine["DOOM-erang"] == true)
                {
                    activeItem_textures[3] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[3] = "DOOM-erang";
                    StoryProgress.activeItemsDict["DOOM-erang"] = Keys.D4;
                }
                if (cursor_item_number == 5 && StoryProgress.ProgressLine["Hammer"] == true)
                {
                    activeItem_textures[3] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[3] = "Hammer";
                    StoryProgress.activeItemsDict["Hammer"] = Keys.D4;
                }
                if (cursor_item_number == 6 && StoryProgress.ProgressLine["MetalBladeCrossbow"] == true)
                {
                    activeItem_textures[3] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[3] = "MetalBladeCrossbow";
                    StoryProgress.activeItemsDict["MetalBladeCrossbow"] = Keys.D4;
                }
                if (cursor_item_number == 7 && StoryProgress.ProgressLine["Hookshot"] == true)
                {
                    activeItem_textures[3] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[3] = "Hookshot";
                    StoryProgress.activeItemsDict["Hookshot"] = Keys.D4;
                }
            }
            if (InputHandler.KeyReleased(Keys.D5))
            {
                ResetKey(Keys.D5);
                ResetSelectedActiveItemTextures();

                if (cursor_item_number == 0 && StoryProgress.ProgressLine["Axe"] == true)
                {
                    activeItem_textures[4] = Content.Load<Texture2D>(@"Sprite\Axe");
                    activeItems_helper[4] = "Axe";
                    StoryProgress.activeItemsDict["Axe"] = Keys.D5;
                }
                if (cursor_item_number == 1 && StoryProgress.ProgressLine["Sword"] == true)
                {
                    activeItem_textures[4] = Content.Load<Texture2D>(@"Sprite\Inv Sword test");
                    activeItems_helper[4] = "Sword";
                    StoryProgress.activeItemsDict["Sword"] = Keys.D5;
                }
                if (cursor_item_number == 2 && StoryProgress.ProgressLine["Crossbow"] == true)
                {
                    activeItem_textures[4] = Content.Load<Texture2D>(@"Sprite\Bow");
                    activeItems_helper[4] = "Crossbow";
                    StoryProgress.activeItemsDict["Crossbow"] = Keys.D5;
                }
                if (cursor_item_number == 3 && StoryProgress.ProgressLine["Spear"] == true)
                {
                    activeItem_textures[4] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[4] = "Spear";
                    StoryProgress.activeItemsDict["Spear"] = Keys.D5;
                }
                if (cursor_item_number == 4 && StoryProgress.ProgressLine["DOOM-erang"] == true)
                {
                    activeItem_textures[4] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[4] = "DOOM-erang";
                    StoryProgress.activeItemsDict["DOOM-erang"] = Keys.D5;
                }
                if (cursor_item_number == 5 && StoryProgress.ProgressLine["Hammer"] == true)
                {
                    activeItem_textures[4] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[4] = "Hammer";
                    StoryProgress.activeItemsDict["Hammer"] = Keys.D5;
                }
                if (cursor_item_number == 6 && StoryProgress.ProgressLine["MetalBladeCrossbow"] == true)
                {
                    activeItem_textures[4] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[4] = "MetalBladeCrossbow";
                    StoryProgress.activeItemsDict["MetalBladeCrossbow"] = Keys.D5;
                }
                if (cursor_item_number == 7 && StoryProgress.ProgressLine["Hookshot"] == true)
                {
                    activeItem_textures[4] = Content.Load<Texture2D>(@"Sprite\Inv Crossbow test");
                    activeItems_helper[4] = "Hookshot";
                    StoryProgress.activeItemsDict["Hookshot"] = Keys.D5;
                }
            }
        }

        private void ResetKey(Keys Key)
        {
            foreach (var item in StoryProgress.activeItemsDict.Where(dictkey => dictkey.Value == Key).ToList())
            {
                StoryProgress.activeItemsDict.Remove(item.Key);
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
                    if (activeItems_helper[i] == "DOOM-erang")
                    {
                        activeItem_textures[i] = Content.Load<Texture2D>(@"Sprite\Inventory Empty test");
                    }
                }
            }
            if (cursor_item_number == 5)
            {
                for (int i = 0; i < activeItems_helper.Count(); i++)
                {
                    if (activeItems_helper[i] == "Hammer")
                    {
                        activeItem_textures[i] = Content.Load<Texture2D>(@"Sprite\Inventory Empty test");
                    }
                }
            }
            if (cursor_item_number == 6)
            {
                for (int i = 0; i < activeItems_helper.Count(); i++)
                {
                    if (activeItems_helper[i] == "MetalBladeCrossbow")
                    {
                        activeItem_textures[i] = Content.Load<Texture2D>(@"Sprite\Inventory Empty test");
                    }
                }
            }
            if (cursor_item_number == 7)
            {
                for (int i = 0; i < activeItems_helper.Count(); i++)
                {
                    if (activeItems_helper[i] == "Hookshot")
                    {
                        activeItem_textures[i] = Content.Load<Texture2D>(@"Sprite\Inventory Empty test");
                    }
                }
            }
        }

        #endregion
    }
}
