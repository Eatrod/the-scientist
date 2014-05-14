using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

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
        Texture2D axeImage, swordImage, crossbowImage, belladonnaImage, immortuiImage, ironOreImage, permitImage, goldImage, alcoholImage, fishImage;
        Texture2D inventoryCursorImage;
        int cursor_X, cursor_Y;
        int cursor_item_number;
        Label axeLabel, bowLabel;
        Label belladonnaLabel, immortuiLabel, ironOreLabel, permitLabel, goldLabel, alcoholLabel, fishLabel;
        Label amountOfIronLabel, amountOfGoldLabel, amountOfFishLabel;
        Label[] activeWeaponNumbers;

        int item_width, item_height, active_item_width, active_item_height;
        int weapon_starting_x, nonweapon_starting_x, item_starting_y, item_x_distance, item_y_distance;
        int cursor_bracket_width, cursor_y_distance, cursor_width, cursor_height;
        int active_item_slot_width, active_item_slot_height, active_item_slot_starting_x, active_item_slot_bracket_width;
        int active_item_y, active_item_x_distance, active_item_starting_x;


        SoundEffect inventorySelectSound;

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

            cursor_bracket_width = (GameRef.ScreenRectangle.Width / 6) / 6;
            cursor_y_distance = GameRef.ScreenRectangle.Height / 6 + (GameRef.ScreenRectangle.Height / 6) / 10;
            cursor_width = GameRef.ScreenRectangle.Width / 6;
            cursor_height = GameRef.ScreenRectangle.Height / 6;

            item_width = GameRef.ScreenRectangle.Width / 8;
            item_height = GameRef.ScreenRectangle.Height / 6;

            active_item_slot_width = GameRef.ScreenRectangle.Width / 15;
            active_item_slot_height = GameRef.ScreenRectangle.Height / 15;
            active_item_slot_starting_x = GameRef.ScreenRectangle.Width / 15;
            active_item_slot_bracket_width = GameRef.ScreenRectangle.Width / 90;

            active_item_width = GameRef.ScreenRectangle.Width / 20;
            active_item_height = GameRef.ScreenRectangle.Height / 15;
            active_item_y = GameRef.ScreenRectangle.Height / 12;
            active_item_x_distance = GameRef.ScreenRectangle.Width / 15 + GameRef.ScreenRectangle.Width / 15;
            active_item_starting_x = active_item_slot_starting_x + active_item_slot_bracket_width;

            item_x_distance = GameRef.ScreenRectangle.Width / 6 + (GameRef.ScreenRectangle.Width / 6) / 5;
            weapon_starting_x = GameRef.ScreenRectangle.Width / 15 + cursor_bracket_width;
            nonweapon_starting_x = GameRef.ScreenRectangle.Width / 2 + (GameRef.ScreenRectangle.Width / 2) / 10;

            item_starting_y = GameRef.ScreenRectangle.Height / 5;
            item_y_distance = item_height + item_height / 10;

            inventoryBackground = Content.Load<Texture2D>(@"Backgrounds\Inventory test");
            axeImage = Content.Load<Texture2D>(@"Sprite\Axe");
            swordImage = Content.Load<Texture2D>(@"Sprite\Inv Sword test");
            crossbowImage = Content.Load<Texture2D>(@"Sprite\Bow");
            inventoryCursorImage = Content.Load<Texture2D>(@"Sprite\Inventory Cursor test");
            belladonnaImage = Content.Load<Texture2D>(@"Sprite\Belladonna");
            immortuiImage = Content.Load<Texture2D>(@"Sprite\Immortui big");
            ironOreImage = Content.Load<Texture2D>(@"Sprite\Iron ore inv");
            permitImage = Content.Load<Texture2D>(@"Sprite\Permit");
            goldImage = Content.Load<Texture2D>(@"Sprite\Gold");
            alcoholImage = Content.Load<Texture2D>(@"Sprite\Wine");
            fishImage = Content.Load<Texture2D>(@"Sprite\Fish");

            amountOfIronLabel = new Label();
            amountOfIronLabel.Text = "";
            amountOfIronLabel.Color = Color.Black;
            amountOfIronLabel.Position = new Vector2(nonweapon_starting_x + item_width + item_x_distance * 0, item_starting_y + item_y_distance * 3 + item_height / 3); //new Vector2(860, 200 + (140 * 3)); //1110 //860
            ControlManager.Add(amountOfIronLabel);

            amountOfGoldLabel = new Label();
            amountOfGoldLabel.Text = "";
            amountOfGoldLabel.Color = Color.Black;
            amountOfGoldLabel.Position = new Vector2(nonweapon_starting_x + item_width + item_x_distance * 1, item_starting_y + item_y_distance * 3 + item_height / 3); //new Vector2(860, 200 + (140 * 3)); //1110 //860
            ControlManager.Add(amountOfGoldLabel);

            amountOfFishLabel = new Label();
            amountOfFishLabel.Text = "";
            amountOfFishLabel.Color = Color.Black;
            amountOfFishLabel.Position = new Vector2(nonweapon_starting_x + item_width + item_x_distance * 1, item_starting_y + item_y_distance * 2 + item_height / 3);
            ControlManager.Add(amountOfFishLabel);

            belladonnaLabel = new Label();
            belladonnaLabel.Text = "";
            belladonnaLabel.Color = Color.Black;
            belladonnaLabel.Position = new Vector2(nonweapon_starting_x + item_x_distance * 0, item_starting_y + item_y_distance * 0 + item_height);
            ControlManager.Add(belladonnaLabel);
                    
            alcoholLabel = new Label();
            alcoholLabel.Text = "";
            alcoholLabel.Color = Color.Black;
            alcoholLabel.Position = new Vector2(nonweapon_starting_x + item_x_distance * 0, item_starting_y + item_y_distance * 1 + item_height);
            ControlManager.Add(alcoholLabel);

            permitLabel = new Label();
            permitLabel.Text = "";
            permitLabel.Color = Color.Black;
            permitLabel.Position = new Vector2(nonweapon_starting_x + item_x_distance * 1, item_starting_y + item_y_distance * 0 + item_height);
            ControlManager.Add(permitLabel);

            immortuiLabel = new Label();
            immortuiLabel.Text = "";
            immortuiLabel.Color = Color.Black;
            immortuiLabel.Position = new Vector2(nonweapon_starting_x + item_x_distance * 0, item_starting_y + item_y_distance * 1 + item_height);
            ControlManager.Add(immortuiLabel);

            ironOreLabel = new Label();
            ironOreLabel.Text = "Iron Ore";
            ironOreLabel.Color = Color.Black;
            ironOreLabel.Position = new Vector2(nonweapon_starting_x + item_x_distance * 0, item_starting_y + item_y_distance * 3 + item_height);
            ControlManager.Add(ironOreLabel);

            goldLabel = new Label();
            goldLabel.Text = "Gold";
            goldLabel.Color = Color.Black;
            goldLabel.Position = new Vector2(nonweapon_starting_x + item_x_distance * 1, item_starting_y + item_y_distance * 3 + item_height);
            ControlManager.Add(goldLabel);

            fishLabel = new Label();
            fishLabel.Text = "Fish";
            fishLabel.Color = Color.Black;
            fishLabel.Position = new Vector2(nonweapon_starting_x + item_x_distance * 1, item_starting_y + item_y_distance * 2 + item_height);
            ControlManager.Add(fishLabel);

            axeLabel = new Label();
            axeLabel.Text = "";
            axeLabel.Color = Color.Black;
            axeLabel.Position = new Vector2(weapon_starting_x + item_x_distance * 0, item_starting_y + item_y_distance * 0 + item_height);
            ControlManager.Add(axeLabel);

            bowLabel = new Label();
            bowLabel.Text = "";
            bowLabel.Color = Color.Black;
            bowLabel.Position = new Vector2(weapon_starting_x + item_x_distance * 0, item_starting_y + item_y_distance * 1 + item_height);
            ControlManager.Add(bowLabel);

            activeWeaponNumbers = new Label[5];
            for (int i = 0; i < 5; i++)
            {
                activeWeaponNumbers[i] = new Label();
                activeWeaponNumbers[i].Position = new Vector2(active_item_slot_starting_x + active_item_x_distance * i - active_item_slot_width / 4, active_item_y);
                activeWeaponNumbers[i].Text = (i + 1).ToString();
                activeWeaponNumbers[i].Color = Color.Black;
                ControlManager.Add(activeWeaponNumbers[i]);
            }


            inventorySelectSound = Content.Load<SoundEffect>(@"Sounds/Effects/inventory_pickup");

        }
        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, PlayerIndex.One);
            base.Update(gameTime);

            if (InputHandler.KeyReleased(Keys.I))
                StateManager.PopState();

            if (InputHandler.KeyReleased(Keys.Right))
            {
                if (cursor_X != weapon_starting_x - cursor_bracket_width + item_x_distance)
                {
                    cursor_X += item_x_distance;
                    cursor_item_number += 1;
                }
            }

            if (InputHandler.KeyReleased(Keys.Left))
            {
                if (cursor_X != weapon_starting_x - cursor_bracket_width)
                {
                    cursor_X -= item_x_distance;
                    cursor_item_number -= 1;
                }
            }

            if (InputHandler.KeyReleased(Keys.Up))
            {
                if (cursor_Y != item_starting_y)
                {
                    cursor_Y -= cursor_y_distance;
                    cursor_item_number -= 2;
                }
            }

            if (InputHandler.KeyReleased(Keys.Down))
            {
                if (cursor_Y != item_starting_y + cursor_y_distance * 3)
                {
                    cursor_Y += cursor_y_distance;
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
                new Rectangle(cursor_X, cursor_Y, cursor_width, cursor_height),
                Color.White);
            //------

            //Inventory Weapons------
            if(StoryProgress.ProgressLine["Axe"] == true)
            {
                GameRef.spriteBatch.Draw(
                    axeImage,
                    new Rectangle(weapon_starting_x + item_x_distance * 0, item_starting_y + (item_y_distance * 0), item_width, item_height),
                    Color.White);
                axeLabel.Text = "Axe";
            }

            if (StoryProgress.ProgressLine["Sword"] == true)
            {
                GameRef.spriteBatch.Draw(
                    swordImage,
                    new Rectangle(weapon_starting_x + item_x_distance * 1, item_starting_y + (item_y_distance * 0), item_width, item_height), 
                    Color.White);
            }

            if (StoryProgress.ProgressLine["Crossbow"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(weapon_starting_x + item_x_distance * 0, item_starting_y + (item_y_distance * 1), item_width, item_height),
                    Color.White);
                bowLabel.Text = "Bow & Arrows";
            }

            if (StoryProgress.ProgressLine["Spear"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(weapon_starting_x + item_x_distance * 1, item_starting_y + (item_y_distance * 1), item_width, item_height),
                    Color.White);
            }

            if (StoryProgress.ProgressLine["DOOM-erang"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(weapon_starting_x + item_x_distance * 0, item_starting_y + (item_y_distance * 2), item_width, item_height),
                    Color.White);
            }

            if (StoryProgress.ProgressLine["Hammer"] == true)
            {
                GameRef.spriteBatch.Draw(crossbowImage,
                    new Rectangle(weapon_starting_x + item_x_distance * 1, item_starting_y + (item_y_distance * 2), item_width, item_height),
                    Color.White);
            }

            if (StoryProgress.ProgressLine["MetalBladeCrossbow"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(weapon_starting_x + item_x_distance * 0, item_starting_y + (item_y_distance * 3), item_width, item_height),
                    Color.White);
            }

            if (StoryProgress.ProgressLine["Hookshot"] == true)
            {
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(weapon_starting_x + item_x_distance * 1, item_starting_y + (item_y_distance * 3), item_width, item_height),
                    Color.White);
            }
            //-------

            //Active item slots
            for (int i = 0; i < 5; i++)
            {
                GameRef.spriteBatch.Draw(
                    inventoryCursorImage,
                    new Rectangle(active_item_slot_starting_x + active_item_x_distance * i, active_item_y, active_item_slot_width, active_item_slot_height), 
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
                    new Rectangle(active_item_starting_x + active_item_x_distance * (key_number - 1), active_item_y, active_item_width, active_item_height),
                    Color.White); 
            }

            if (StoryProgress.activeItemsDict.ContainsKey("Sword"))
            {
                key_string = StoryProgress.activeItemsDict["Sword"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(
                    swordImage,
                    new Rectangle(active_item_starting_x + active_item_x_distance * (key_number - 1), active_item_y, active_item_width, active_item_height),
                    Color.White);
            }

            if (StoryProgress.activeItemsDict.ContainsKey("Crossbow"))
            {
                key_string = StoryProgress.activeItemsDict["Crossbow"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(active_item_starting_x + active_item_x_distance * (key_number - 1), active_item_y, active_item_width, active_item_height),
                    Color.White);
            }

            if (StoryProgress.activeItemsDict.ContainsKey("Spear"))
            {
                key_string = StoryProgress.activeItemsDict["Spear"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(active_item_starting_x + active_item_x_distance * (key_number - 1), active_item_y, active_item_width, active_item_height),
                    Color.White);
            }

            if (StoryProgress.activeItemsDict.ContainsKey("DOOM-erang"))
            {
                key_string = StoryProgress.activeItemsDict["DOOM-erang"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(active_item_starting_x + active_item_x_distance * (key_number - 1), active_item_y, active_item_width, active_item_height),
                    Color.White);
            }

            if (StoryProgress.activeItemsDict.ContainsKey("Hammer"))
            {
                key_string = StoryProgress.activeItemsDict["Hammer"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(active_item_starting_x + active_item_x_distance * (key_number - 1), active_item_y, active_item_width, active_item_height),
                    Color.White);
            }

            if (StoryProgress.activeItemsDict.ContainsKey("MetalBladeCrossbow"))
            {
                key_string = StoryProgress.activeItemsDict["MetalBladeCrossbow"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(active_item_starting_x + active_item_x_distance * (key_number - 1), active_item_y, active_item_width, active_item_height),
                    Color.White);
            }

            if (StoryProgress.activeItemsDict.ContainsKey("Hookshot"))
            {
                key_string = StoryProgress.activeItemsDict["Hookshot"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                GameRef.spriteBatch.Draw(
                    crossbowImage,
                    new Rectangle(active_item_starting_x + active_item_x_distance * (key_number - 1), active_item_y, active_item_width, active_item_height),
                    Color.White);
            }

            // Non Weapon Items -------------------------
            if (StoryProgress.ProgressLine["Belladonna"] == true)
            {
                GameRef.spriteBatch.Draw(
                        belladonnaImage,
                        new Rectangle(nonweapon_starting_x + item_x_distance * 0, item_starting_y + (item_y_distance * 0), item_width, item_height),
                        Color.White);

                belladonnaLabel.Text = "Belladonna";
            }

            if (StoryProgress.ProgressLine["Immortui"] == true)
            {
                GameRef.spriteBatch.Draw(
                            immortuiImage,
                            new Rectangle(nonweapon_starting_x + item_x_distance * 1, item_starting_y + (item_y_distance * 1), item_width, item_height),
                            Color.White);
                immortuiLabel.Text = "Immortui";
            }

            if (StoryProgress.ProgressLine["Permit"] == true)
            {
                GameRef.spriteBatch.Draw(
                            permitImage,
                            new Rectangle(nonweapon_starting_x + item_x_distance * 1, item_starting_y + (item_y_distance * 0), item_width, item_height),
                            Color.White);
                permitLabel.Text = "Permit";
            }          

            if (StoryProgress.ProgressLine["Alcohol"] == true)
            {
                GameRef.spriteBatch.Draw(
                            alcoholImage,
                            new Rectangle(nonweapon_starting_x + item_x_distance * 0, item_starting_y + (item_y_distance * 1), item_width, item_height),
                            Color.White);
                alcoholLabel.Text = "Alcohol";
            }

            GameRef.spriteBatch.Draw(
                        fishImage,
                        new Rectangle(nonweapon_starting_x + item_x_distance * 1, item_starting_y + (item_y_distance * 2), item_width, item_height),
                        Color.White);

            GameRef.spriteBatch.Draw(
                    ironOreImage,
                    new Rectangle(nonweapon_starting_x + item_x_distance * 0, item_starting_y + (item_y_distance * 3), item_width, item_height),
                    Color.White);
         
            GameRef.spriteBatch.Draw(
                        goldImage,
                        new Rectangle(nonweapon_starting_x + item_x_distance * 1, item_starting_y + (item_y_distance * 3), item_width, item_height),
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
                inventorySelectSound.Play();
                ResetKey(Keys.D1);
                AssignKeyToItem(Keys.D1);
            }

            if (InputHandler.KeyReleased(Keys.D2))
            {
                inventorySelectSound.Play();
                ResetKey(Keys.D2);
                AssignKeyToItem(Keys.D2);
            }

            if (InputHandler.KeyReleased(Keys.D3))
            {
                inventorySelectSound.Play();
                ResetKey(Keys.D3);
                AssignKeyToItem(Keys.D3);
            }

            if (InputHandler.KeyReleased(Keys.D4))
            {
                inventorySelectSound.Play();
                ResetKey(Keys.D4);
                AssignKeyToItem(Keys.D4);
            }

            if (InputHandler.KeyReleased(Keys.D5))
            {
                inventorySelectSound.Play();
                ResetKey(Keys.D5);
                AssignKeyToItem(Keys.D5);
            }

            amountOfIronLabel.Text = "x " + StoryProgress.collectedAmountDict["IronOre"].ToString();
            amountOfGoldLabel.Text = "x " + StoryProgress.collectedAmountDict["Money"].ToString();
            amountOfFishLabel.Text = "x " + StoryProgress.collectedAmountDict["Fish"].ToString();
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
