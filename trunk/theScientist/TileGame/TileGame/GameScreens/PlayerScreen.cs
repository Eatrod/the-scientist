using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;
using TileEngine.Dialog;
using TileEngine.Sprite;
using TileEngine.Sprite.Npc.NPC_Neutral;
using TileEngine.Sprite.Npc.NPC_Story;
using TileEngine.Sprite.Projectiles;
using XtheSmithLibrary;
using TileEngine;
using TileEngine.Tiles;
using TileGame.Collision;
using XtheSmithLibrary.Controls;

namespace TileGame.GameScreens
{
    public class PlayerScreen : BaseGameState
    {
        
        #region Field Region

        //bool gate1Locked = true;
        //bool gate2Locked = true;
        protected PlayerScreen screen;
        protected DialogBox dialogBox;
        protected TextBubble textBubble;
        protected NPC_Story talksTo;
        public StoryProgress storyProgress;
        
        public Dictionary<int, bool> lockedGateDict;
        protected Dictionary<string, int> gateDict;

        //--     
        Texture2D activeItemBackground, abilityBackground, axeImage, swordImage, crossbowImage;
        Texture2D[] activeItem_textures;
        Color[] activeItemBackgroundColor;
        Color[] abilityBackgroundColor;

        bool[] flashing_abilityBackground, flashOn;
        float[] flashTime, flashLength;
        const float flashTimeTotal = 1.0f;
        //--

        #endregion

        #region Property Region

        //GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public TileMap tileMap = new TileMap();
        protected Camera camera = new Camera();
        private Dialog dialog;
        protected bool ActiveConversation = false;
        private Rectangle rectangle;

        //Sprite sprite;
        static public PlayerCharacter player;

        //Stamina & Healthbar
        static protected AnimatedSprite lifemeteranimation;
        static protected AnimatedSprite staminaanimation;
        static protected AnimatedSprite chargeanimation;
        Rectangle lifeRect, staminaRect, chargeRect;
        float life, stamina, charge;


        //player Collision
        protected CollisionWithTerrain CollisionWithTerrain = new CollisionWithTerrain();

        //Player Projectiles code
        static protected AnimatedSprite arrowprojectile;
        protected List<AnimatedProjectile> playerprojectiles = new List<AnimatedProjectile>();

        //List<AnimatedSprite> npcs = new List<AnimatedSprite>();
        protected List<BaseSprite> renderList = new List<BaseSprite>();
        protected List <TextBubble> bubbleList = new List<TextBubble>();

        Comparison<BaseSprite> renderSort = new Comparison<BaseSprite>(renderSpriteCompare);

        //public bool Gate1Locked { get { return gate1Locked; } set { this.gate1Locked = value; } }
        //public bool Gate2Locked { get { return gate2Locked; } set { this.gate2Locked = value; } }

        #endregion

        static int renderSpriteCompare(BaseSprite a, BaseSprite b)
        {
            return a.Origin.Y.CompareTo(b.Origin.Y);
        }

        #region Constructor Region
        public PlayerScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {
            
            //graphics = new GraphicsDeviceManager(this);
            //Microsoft.Xna.Framework.Content.RootDirectory = "Content";
            
        }

        #endregion 

        #region XNA Method Region
        public override void Initialize()
        {
            base.Initialize();

            lifeRect = new Rectangle(0, 0, 100, 20);//lifebarRectangle
            staminaRect = new Rectangle(0, 0, 100, 20);//staminabarRectangle
            chargeRect = new Rectangle(0, 0, 100, 20); //chargeRectangle

            FrameAnimation down = new FrameAnimation(1, 32, 32, 0, 0);
            if(!player.Animations.ContainsKey("Down"))
                player.Animations.Add("Down", down);

            FrameAnimation right = new FrameAnimation(1, 32, 32, 32, 0);
            if(!player.Animations.ContainsKey("Right"))
                player.Animations.Add("Right", right);

            FrameAnimation up = new FrameAnimation(1, 32, 32, 64, 0);
            if(!player.Animations.ContainsKey("Up"))
                player.Animations.Add("Up", up);

            FrameAnimation left = new FrameAnimation(1, 32, 32, 96, 0);
            if(!player.Animations.ContainsKey("Left"))
                player.Animations.Add("Left", left);

            player.CurrentAnimationName = "Down";
            renderList.Add(player);
            //renderList.Add(sprite);

            //LifeMeter Animation Code load
            FrameAnimation fullHp = new FrameAnimation(1, 100, 20, 0, 0);
            if(!lifemeteranimation.Animations.ContainsKey("FullHp"))
                lifemeteranimation.Animations.Add("FullHp", fullHp);

            FrameAnimation seventyfivehp = new FrameAnimation(1, 100, 20, 0, 20);
            if (!lifemeteranimation.Animations.ContainsKey("SeventtyFiveHp"))
                lifemeteranimation.Animations.Add("SeventtyFiveHp", seventyfivehp);

            FrameAnimation fiftyhp = new FrameAnimation(1, 100, 20, 0, 40);
            if (!lifemeteranimation.Animations.ContainsKey("FiftyHp"))
                lifemeteranimation.Animations.Add("FiftyHp", fiftyhp);

            FrameAnimation twentyfivehp = new FrameAnimation(1, 100, 20, 0, 60);
            if (!lifemeteranimation.Animations.ContainsKey("TwentyFiveHp"))
                lifemeteranimation.Animations.Add("TwentyFiveHp", twentyfivehp);

            //StaminaMeter Animation Code load
            FrameAnimation staminafull = new FrameAnimation(1, 100, 20, 0, 0);
            if (!staminaanimation.Animations.ContainsKey("StaminaFull"))
                staminaanimation.Animations.Add("StaminaFull", staminafull);            
            staminaanimation.CurrentAnimationName = "StaminaFull";

            //ChargeMeter Animation Code.
            FrameAnimation chargebar = new FrameAnimation(1, 100, 20, 0, 0);
            if (!chargeanimation.Animations.ContainsKey("ChargeBar"))
                chargeanimation.Animations.Add("ChargeBar", chargebar);
            chargeanimation.CurrentAnimationName = "ChargeBar";

        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ContentManager Content = Game.Content;

            base.LoadContent();
                       
            if(player == null)
            {
                player = new PlayerCharacter(Content.Load<Texture2D>("Sprite/playerboxAnimation"), Content.Load<Texture2D>("CharacterPotraits/Assassins-Creed-4"));
                player.Origionoffset = new Vector2(15, 15);
                player.SetSpritePositionInGameWorld(new Vector2(4, 3));
                player.Life = 100;
                player.Stamina = 100;
            }

            if (lifemeteranimation == null)
            {
                lifemeteranimation = new AnimatedSprite(Content.Load<Texture2D>("Sprite/HealthBar"));
                lifemeteranimation.SetSpritePositionInGameWorld(new Vector2(0, 0));
            }

            if (staminaanimation == null)
            {
                staminaanimation = new AnimatedSprite(Content.Load<Texture2D>("Sprite/StaminaBar"));
                staminaanimation.SetSpritePositionInGameWorld(new Vector2(0, 0.7f));
            }

            if (chargeanimation == null)
            {
                chargeanimation = new AnimatedSprite(Content.Load<Texture2D>("Sprite/ChargeBar"));
                chargeanimation.SetSpritePositionInGameWorld(new Vector2(0, 1.4f));
            }
            dialog = new Dialog();
            rectangle = new Rectangle(0, GraphicsDevice.Viewport.Height - 100, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            dialogBox = new DialogBox(Content.Load<Texture2D>("GUI/DialogPlaceholder"), rectangle, "");

            textBubble = new TextBubble(Content.Load<Texture2D>("GUI/SpeechBubble"), rectangle, "", Content.Load<SpriteFont>("Fonts/BubbleFont"));

            gateDict = new Dictionary<string, int>();
            for (int i = 0; i < 10; i++)
            {
                gateDict["G" + i.ToString()] = 40 + i;
            }
            storyProgress = new StoryProgress();
            activeItemBackground = Content.Load<Texture2D>(@"Sprite\activeItemBackground test");
            abilityBackground = Content.Load<Texture2D>(@"Sprite\abilityBackground test2");
            axeImage = Content.Load<Texture2D>(@"Sprite\Axe");
            swordImage = Content.Load<Texture2D>(@"Sprite\Inv Sword test");
            crossbowImage = Content.Load<Texture2D>(@"Sprite\Bow");
            activeItem_textures = new Texture2D[5];
            activeItemBackgroundColor = new Color[5];
            for (int i = 0; i < activeItemBackgroundColor.Count(); i++)
            {
                activeItemBackgroundColor[i] = Color.LightSlateGray;
            }
            abilityBackgroundColor = new Color[5];
            for (int i = 0; i < abilityBackgroundColor.Count(); i++)
            {
                abilityBackgroundColor[i] = Color.LightSlateGray;
            }
            flashing_abilityBackground = flashOn = new bool[5];//false;
            for (int i = 0; i < flashing_abilityBackground.Count(); i++)
            {
                flashing_abilityBackground[i] = false;
                flashOn[i] = false;
            }
            flashTime = flashLength = new float[5];
            for (int i = 0; i < flashTime.Count(); i++)
            {
                flashTime[i] = 0f;
                flashLength[i] = 0f;
            }
            //--
        }
        public override void Update(GameTime gameTime)
        {
            
            screen = (PlayerScreen)StateManager.CurrentState;
           
            ContentManager Content = Game.Content;

            foreach (AnimatedProjectile sprite in playerprojectiles)
            {
                sprite.updateprojectileposition();            
            }

            for (int i = 0; i < playerprojectiles.Count; i++ )
            {
                if (playerprojectiles[i].Life <= 0)
                {
                    playerprojectiles.RemoveAt(i);
                }
            }
            

            player.Stamina += 0.1f;
            if (player.Stamina >= 100)
                player.Stamina = 100;
            UpdateHealthBarAnimation();
            UpdateStaminaBarAnimation();
            UpdateChargeBarAnimation();
            life = player.Life;
            Vector2 motion = Vector2.Zero;

            if (!ActiveConversation)
            {
                if (InputHandler.KeyDown(Keys.Up))
                    motion.Y -= 2;
                if (InputHandler.KeyDown(Keys.Down))
                    motion.Y += 2;
                if (InputHandler.KeyDown(Keys.Left))
                    motion.X -= 2;
                if (InputHandler.KeyDown(Keys.Right))
                    motion.X += 2;
            }


            if (InputHandler.KeyReleased(Keys.Q) && (player.Stamina - 20 >=0))
            {

                NormalArrowProjectile temparrow = new NormalArrowProjectile(Content.Load<Texture2D>("Sprite/Arrow"), 10f, 0.1f, 3f, player.Position);
                temparrow.UpdatecurrentAnimation(motion);        
                playerprojectiles.Add(temparrow);
                player.Stamina -= 20f;
            }

            if (InputHandler.KeyReleased(Keys.W) && (player.Stamina - 50 > 0))
            {

                FlamingArrowProjectile temparrow = new FlamingArrowProjectile(Content.Load<Texture2D>("Sprite/FlamingArrow"), 10f, 0.1f, 3f, player.Position);
                temparrow.UpdatecurrentAnimation(motion);
                playerprojectiles.Add(temparrow);
                player.Stamina -= 50;                                    
            }

            if (InputHandler.KeyReleased(Keys.E) && (player.Stamina - 40 > 0)) //delay. fixas.
            {
                NormalArrowProjectile temparrow = new NormalArrowProjectile(Content.Load<Texture2D>("Sprite/Arrow"), 10f, 0.1f, 3f, player.Position);               
                NormalArrowProjectile temparrow1 = new NormalArrowProjectile(Content.Load<Texture2D>("Sprite/Arrow"), 10f, 0.1f, 3f, player.Position);                
                NormalArrowProjectile temparrow2 = new NormalArrowProjectile(Content.Load<Texture2D>("Sprite/Arrow"), 10f, 0.1f, 3f, player.Position);



                if (player.CurrentAnimationName == "Up" || player.CurrentAnimationName == "Down")
                {
                    temparrow1.Position.X += 10 ;                   
                    temparrow2.Position.X -= 10 ;




                    if (player.CurrentAnimationName == "Up")
                    {
                        temparrow1.Position.Y += 5;
                        temparrow2.Position.Y += 5;
                    }
                    else
                    {
                        temparrow1.Position.Y -= 5;
                        temparrow2.Position.Y -= 5;
                    }
                    
                }                
                else
                {
                    temparrow1.Position.Y += 10;
                    temparrow2.Position.Y -= 10;

                    if (player.CurrentAnimationName == "Left")
                    {
                        temparrow1.Position.X += 5;
                        temparrow2.Position.X += 5;
                    }
                    else 
                    {
                        temparrow1.Position.X -= 5;
                        temparrow2.Position.X -= 5;
                    }
                    
                
                }
                temparrow.UpdatecurrentAnimation(motion);
                playerprojectiles.Add(temparrow);
                temparrow1.UpdatecurrentAnimation(motion);
                playerprojectiles.Add(temparrow1);
                temparrow2.UpdatecurrentAnimation(motion);
                playerprojectiles.Add(temparrow2);
                player.Stamina -= 40f;
            
            }

            if (InputHandler.KeyDown(Keys.R))
            {
                player.Charge += 0.5f;
                if(player.Charge >= 100)
                    player.Charge = 100;           
            }

            if (InputHandler.KeyReleased(Keys.R))
            {
                if (player.Charge == 100 && (player.Stamina - 30) > 0)
                {
                    FlamingArrowProjectile temparrow = new FlamingArrowProjectile(Content.Load<Texture2D>("Sprite/FlamingArrow"), 10f, 0.1f, 3f, player.Position);
                    temparrow.UpdatecurrentAnimation(motion);
                    playerprojectiles.Add(temparrow);
                    player.Stamina -= 30;
                }

                else if ((player.Stamina - 20) > 0)
                {
                    NormalArrowProjectile temparrow = new NormalArrowProjectile(Content.Load<Texture2D>("Sprite/Arrow"), 10f, 0.1f, 3f, player.Position);
                    temparrow.UpdatecurrentAnimation(motion);
                    playerprojectiles.Add(temparrow);
                player.Stamina -= 20f;
                }

                else
                {
                    player.Stamina = 10;
                }

                player.Charge = 0;
            }

            if (InputHandler.KeyReleased(Keys.Tab))
                player.Stamina = 100;

            if (InputHandler.KeyReleased(Keys.Escape))
            {
                GameRef.lastGameScreen = GameRef.stateManager.CurrentState.Tag.ToString();
                GameRef.playerPosition = player.Position;
                GameRef.playerLife = player.Life;
                GameRef.playerStamina = player.Stamina;
                StateManager.PushState(GameRef.StartMenuScreen);
            }
            if (InputHandler.KeyReleased(Keys.N))
                StateManager.PushState(GameRef.NotebookScreen);


            if (InputHandler.KeyReleased(Keys.I))
                StateManager.PushState(GameRef.InventoryScreen);

            dialogBox.Update(gameTime);
            if (InputHandler.KeyReleased(Keys.Space))
            {
                if (ActiveConversation == true)
                {
                    dialog.NextText(this.talksTo, this.talksTo.text,player, storyProgress);
                    dialogBox.Text = dialog.conversation.Text;
                    dialogBox.conversation = dialog.conversation;
                }
            }

            if (motion != Vector2.Zero)
            {
                motion.Normalize();                 //Comment out to use gamePad
                motion = CollisionWithTerrain.CheckCollisionForMotion(motion, player,screen);

                //sprite.Position += motion * sprite.Speed;
                //UpdateSpriteAnimation(motion);
                player.isAnimating = true;
                CollisionWithTerrain.CheckForCollisionAroundSprite(player, motion,screen);
            }
            else
            {
                //UpdateSpriteIdleAnimation(sprite);
                player.isAnimating = false;
                motion = new Vector2(0, 0);
            }


            motion = CollisionWithTerrain.CheckCollisionAutomaticMotion(motion, player,screen);
            UpdateSpriteAnimation(motion);
            player.Position += motion * player.Speed;
            player.ClampToArea(screen.tileMap.GetWidthInPixels(), screen.tileMap.GetHeightInPixels());  //Funktion för att hämta nuvarande tilemap state.
            player.Update(gameTime);


            int screenWidth = GraphicsDevice.Viewport.Width;
            int screenHeight = GraphicsDevice.Viewport.Height;

            camera.LockToTarget(player, screenWidth, screenHeight);
            camera.ClampToArea(
                screen.tileMap.GetWidthInPixels() - screenWidth,   //Funktion för att hämta nuvarande tilemap state.
                screen.tileMap.GetHeightInPixels() - screenHeight);

            if (player.Life == 0)
            {
                player.SetSpritePositionInGameWorld(new Vector2(0, 0));
                player.Life = 100;
                player.areTakingDamage = false;
                lifeRect = new Rectangle(0, 0, 100, 20);
                staminaRect = new Rectangle(0, 0, 100, 20);
                chargeRect = new Rectangle(0, 0, 100, 20);

            }

            UpdateItemHUD(gameTime);

            base.Update(gameTime);


        }

      
        private void UpdateHealthBarAnimation()
        {
            life = player.Life;

            lifeRect.Width = (int)life;

            if (life >= 75f)
                lifemeteranimation.CurrentAnimationName = "FullHp";
            if (life < 75f)
                lifemeteranimation.CurrentAnimationName = "SeventtyFiveHp";
            if (life < 50f)
                lifemeteranimation.CurrentAnimationName = "FiftyHp";
            if (life < 25f)
                lifemeteranimation.CurrentAnimationName = "TwentyFiveHp";

            lifeRect = new Rectangle(lifemeteranimation.CurrentAnimation.CurrentRectangle.Location.X, lifemeteranimation.CurrentAnimation.CurrentRectangle.Location.Y, lifeRect.Width, lifeRect.Height);
            lifemeteranimation.CurrentAnimation.CurrentRectangle = lifeRect;
        } //Code for healthbar

        private void UpdateStaminaBarAnimation() //Code for Staminabar
        {
            stamina = player.Stamina;

            staminaRect.Width = (int)stamina;

            staminaRect = new Rectangle(
                staminaanimation.CurrentAnimation.CurrentRectangle.Location.X, 
                staminaanimation.CurrentAnimation.CurrentRectangle.Location.Y, 
                staminaRect.Width, staminaRect.Height);
            staminaanimation.CurrentAnimation.CurrentRectangle = staminaRect;
        }

        private void UpdateChargeBarAnimation() //Code for Chargebar
        {
            charge = player.Charge;

            chargeRect.Width = (int)charge;

            chargeRect = new Rectangle(
                chargeanimation.CurrentAnimation.CurrentRectangle.Location.X,
                chargeanimation.CurrentAnimation.CurrentRectangle.Location.Y,
                chargeRect.Width, chargeRect.Height);
            chargeanimation.CurrentAnimation.CurrentRectangle = chargeRect;
        }

        private void UpdateItemHUD(GameTime gameTime)
        {
            if (InputHandler.KeyReleased(Keys.D1))
            {
                for (int i = 0; i < activeItemBackgroundColor.Count(); i++)
                {
                    activeItemBackgroundColor[i] = Color.LightSlateGray;
                }
                activeItemBackgroundColor[0] = Color.White;
            }
            if (InputHandler.KeyReleased(Keys.D2))
            {
                for (int i = 0; i < activeItemBackgroundColor.Count(); i++)
                {
                    activeItemBackgroundColor[i] = Color.LightSlateGray;
                }
                activeItemBackgroundColor[1] = Color.White;
            }
            if (InputHandler.KeyReleased(Keys.D3))
            {
                for (int i = 0; i < activeItemBackgroundColor.Count(); i++)
                {
                    activeItemBackgroundColor[i] = Color.LightSlateGray;
                }
                activeItemBackgroundColor[2] = Color.White;
            }
            if (InputHandler.KeyReleased(Keys.D4))
            {
                for (int i = 0; i < activeItemBackgroundColor.Count(); i++)
                {
                    activeItemBackgroundColor[i] = Color.LightSlateGray;
                }
                activeItemBackgroundColor[3] = Color.White;
            }
            if (InputHandler.KeyReleased(Keys.D5))
            {
                for (int i = 0; i < activeItemBackgroundColor.Count(); i++)
                {
                    activeItemBackgroundColor[i] = Color.LightSlateGray;
                }
                activeItemBackgroundColor[4] = Color.White;
            }

            if (InputHandler.KeyReleased(Keys.Q))
            {
                flashing_abilityBackground[0] = true;
            }
            if (InputHandler.KeyReleased(Keys.W))
            {
                flashing_abilityBackground[1] = true;
            }
            if (InputHandler.KeyReleased(Keys.E))
            {
                flashing_abilityBackground[2] = true;
            }
            if (InputHandler.KeyReleased(Keys.R))
            {
                flashing_abilityBackground[3] = true;
            }
            if (InputHandler.KeyReleased(Keys.T))
            {
                flashing_abilityBackground[4] = true;
            }

            for (int i = 0; i < flashing_abilityBackground.Count(); i++)
            {
                flashingUsedAbility(gameTime, i);
            }

        }

        private void flashingUsedAbility(GameTime gameTime, int Index)
        {
            if (flashing_abilityBackground[Index])
            {
                const float flashTimeSlice = 1.0f / 2.0f;
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                flashLength[Index] += elapsed;
                flashTime[Index] += elapsed;
                if (flashTime[Index] > flashTimeTotal)
                {
                    flashing_abilityBackground[Index] = false;
                }
                else
                {
                    if (flashLength[Index] > flashTimeSlice)
                    {
                        flashOn[Index] = !flashOn[Index];
                        flashLength[Index] = 0f;
                    }
                }
            }

            if (!flashing_abilityBackground[Index] || (flashing_abilityBackground[Index] && !flashOn[Index]))
            {
                abilityBackgroundColor[Index] = Color.LightSlateGray;
            }
            else
            {
                abilityBackgroundColor[Index] = Color.White;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            //tileMap.Draw(spriteBatch, camera); 

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                null, null, null, null, camera.TransforMatrix);

            renderList.Sort(renderSort);

            foreach (BaseSprite sprite in renderList)
                sprite.Draw(spriteBatch);

            foreach (var textBubble in bubbleList)
            {
                textBubble.Draw(spriteBatch);
            }

            foreach (AnimatedSprite sprite in playerprojectiles)
            {
                sprite.Draw(spriteBatch);
            }

            spriteBatch.End();

            spriteBatch.Begin();
         
            lifemeteranimation.Draw(spriteBatch);
            staminaanimation.Draw(spriteBatch);
            chargeanimation.Draw(spriteBatch);
            DrawItemHUD();
            //Draws active conversation
            ControlManager.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawItemHUD()
        {
            //Weapon backgrounds
            for (int i = 0; i < activeItem_textures.Count(); i++)
            {
                spriteBatch.Draw( 
                abilityBackground, 
                new Rectangle(110 + (50 * i), 0, 42, 42),
                activeItemBackgroundColor[i]);
            }         

            //Weapon Ability backgrounds
            for (int i = 0; i < activeItem_textures.Count(); i++)
            {
                spriteBatch.Draw(
                abilityBackground,
                new Rectangle(GameRef.ScreenRectangle.Width / 2 - (50 * 2 + 21) + (50 * i), GameRef.ScreenRectangle.Height - 50, 42, 42),
                abilityBackgroundColor[i]);
            }

            //Weapon Ability pics
            if (StoryProgress.activeItemsDict.ContainsKey("Axe"))
            {
                if (StoryProgress.activeItemsDict["Axe"] == Keys.D1 && activeItemBackgroundColor[0] == Color.White)
                {
                    for (int i = 0; i < activeItem_textures.Count(); i++)
                    {
                        spriteBatch.Draw(
                        swordImage,
                        new Rectangle(GameRef.ScreenRectangle.Width / 2 - (50 * 2 + 16) + (50 * i), GameRef.ScreenRectangle.Height - 45, 32, 32),
                        Color.White);
                    }
                }
            }

            string key_string;
            int key_number;

            if (StoryProgress.activeItemsDict.ContainsKey("Axe"))
            {
                key_string = StoryProgress.activeItemsDict["Axe"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                spriteBatch.Draw(axeImage, new Rectangle(115 + (50 * (key_number - 1)), 5, 32, 32), Color.White);
            }
           
            if (StoryProgress.activeItemsDict.ContainsKey("Sword"))
            {
                key_string = StoryProgress.activeItemsDict["Sword"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                spriteBatch.Draw(swordImage, new Rectangle(115 + (50 * (key_number - 1)), 5, 32, 32), Color.White);
            }
            
            if (StoryProgress.activeItemsDict.ContainsKey("Crossbow"))
            {
                key_string = StoryProgress.activeItemsDict["Crossbow"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                spriteBatch.Draw(crossbowImage, new Rectangle(115 + (50 * (key_number - 1)), 5, 32, 32), Color.White);
            }
            
            if (StoryProgress.activeItemsDict.ContainsKey("Spear"))
            {
                key_string = StoryProgress.activeItemsDict["Spear"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                spriteBatch.Draw(crossbowImage, new Rectangle(115 + (50 * (key_number - 1)), 5, 32, 32), Color.White);
            }
            
            if (StoryProgress.activeItemsDict.ContainsKey("DOOM-erang"))
            {
                key_string = StoryProgress.activeItemsDict["DOOM-erang"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                spriteBatch.Draw(crossbowImage, new Rectangle(115 + (50 * (key_number - 1)), 5, 32, 32), Color.White);
            }
            
            if (StoryProgress.activeItemsDict.ContainsKey("Hammer"))
            {
                key_string = StoryProgress.activeItemsDict["Hammer"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                spriteBatch.Draw(crossbowImage, new Rectangle(115 + (50 * (key_number - 1)), 5, 32, 32), Color.White);
            }
            
            if (StoryProgress.activeItemsDict.ContainsKey("MetalBladeCrossbow"))
            {
                key_string = StoryProgress.activeItemsDict["MetalBladeCrossbow"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                spriteBatch.Draw(crossbowImage, new Rectangle(115 + (50 * (key_number - 1)), 5, 32, 32), Color.White);
            }
            
            if (StoryProgress.activeItemsDict.ContainsKey("Hookshot"))
            {
                key_string = StoryProgress.activeItemsDict["Hookshot"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                spriteBatch.Draw(crossbowImage, new Rectangle(115 + (50 * (key_number - 1)), 5, 32, 32), Color.White);
            }
            
        }

        #endregion

        #region Abstract Method Region
        #endregion

        #region Sprite Animation Code
        private void UpdateSpriteIdleAnimation(AnimatedSprite sprite)
        {
            //if (sprite.CurrentAnimationName == "Up")
            //    sprite.CurrentAnimationName = "IdleUp";
            //if (sprite.CurrentAnimationName == "Down")
            //    sprite.CurrentAnimationName = "IdleDown";
            //if (sprite.CurrentAnimationName == "Right")
            //    sprite.CurrentAnimationName = "IdleRight";
            //if (sprite.CurrentAnimationName == "Left")
            //    sprite.CurrentAnimationName = "IdleLeft";
            if (sprite.CurrentAnimationName == "Down")
                sprite.CurrentAnimationName = "Down";
            if (sprite.CurrentAnimationName == "Left")
                sprite.CurrentAnimationName = "Left";
            if (sprite.CurrentAnimationName == "Right")
                sprite.CurrentAnimationName = "Right";
            if (sprite.CurrentAnimationName == "Up")
                sprite.CurrentAnimationName = "Up";
        }

        private void UpdateSpriteAnimation(Vector2 motion)
        {
            float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

            if (motionAngle >= -MathHelper.PiOver4 && motionAngle <= MathHelper.PiOver4)
            {
                player.CurrentAnimationName = "Right"; //Right
                //motion = new Vector2(1f, 0f);
            }
            else if (motionAngle >= MathHelper.PiOver4 && motionAngle <= 3f * MathHelper.PiOver4)
            {
                player.CurrentAnimationName = "Down"; //Down
                //motion = new Vector2(0f, 1f);
            }
            else if (motionAngle <= -MathHelper.PiOver4 && motionAngle >= -3f * MathHelper.PiOver4)
            {
                player.CurrentAnimationName = "Up"; // Up
                //motion = new Vector2(0f, -1f);
            }
            else
            {
                player.CurrentAnimationName = "Left"; //Left
                //motion = new Vector2(-1f, 0f);
            }
        }

        
        #endregion

        
        #region Method Region

        public void PlayerShowTextBubble(NPC_Neutral_Townsfolk npc)
        {
            npc.TextBubble();
            textBubble.conversation = npc.text;
            textBubble.Text = npc.text.Text;
            textBubble.npcList.Add(npc);
            textBubble.Enabled = true;
            textBubble.Visible = true;
            bubbleList.Add(textBubble);
            npc.ShowingBubble = true;
        }

        public void PlayerHideTextBubble(NPC_Neutral_Townsfolk npc)
        {
            textBubble.Visible = false;
            textBubble.Enabled = false;
            textBubble.npcList.Remove(npc);
            bubbleList.Remove(textBubble);
            npc.ShowingBubble = false;
        }

        public void PlayerStartConversation(NPC_Story npc, List<NPC_Story> npclist)
        {
            this.talksTo = npc;
            dialogBox.player = player;
            dialogBox.npc = npc;
            dialogBox.npcStoryList = npclist;
            ActiveConversation = true;
            dialogBox.Enabled = true;
            dialogBox.Visible = true;
            ControlManager.Add(dialogBox);
            npc.StartConversation("Greeting");
            dialogBox.conversation = npc.text;
            dialogBox.Text = npc.text.Text;
        }

        public void PlayerEndConversation(NPC_Story npc)
        {
            dialogBox.Visible = false;
            dialogBox.Enabled = false;
            dialogBox.npc = null;
            ControlManager.Remove(dialogBox);
            npc.canTalk = true;
            ActiveConversation = false;
        }

        public void SetPlayerPosition(int x, int y)
        {
            player.SetSpritePositionInGameWorld(new Vector2(x, y));
        }
       
        protected void GateToNextScreen(int CellIndex, PlayerScreen screen, string GateName)
        {
            int GateNumber = gateDict[GateName];
            if (CellIndex == GateNumber && !lockedGateDict[GateNumber])
            {
                StateManager.ChangeState(screen);
                Point next_position;
                next_position.X = 0;
                next_position.Y = 0;
                for (int i = 0; i < screen.tileMap.CollisionLayer.Width; i++)
                {
                    for (int j = 0; j < screen.tileMap.CollisionLayer.Height; j++)
                    {
                        Point temp = new Point(i, j);
                        if (screen.tileMap.CollisionLayer.GetCellIndex(temp) == GateNumber)
                        {
                            next_position.X = i;
                            next_position.Y = j;
                            break;
                        }
                    }
                }
                screen.SetPlayerPosition(next_position.X, next_position.Y);
                screen.lockedGateDict[CellIndex] = true;
            }
        }

        protected void UnlockGate(int cellIndex)
        {
            for (int i = 40; i <= 49; i++)
            {
                if (cellIndex != i)
        {
                    if (lockedGateDict.ContainsKey(i))
                        lockedGateDict[i] = false;
                }
            }
        }

        public Point FindCellWithIndexInCurrentTilemap(int index, PlayerScreen screen)
        {
            //StateManager.ChangeState(screen);
            Point next_position;
            next_position.X = 0;
            next_position.Y = 0;
            for (int i = 0; i < screen.tileMap.CollisionLayer.Width; i++)
            {
                for (int j = 0; j < screen.tileMap.CollisionLayer.Height; j++)
                {
                    Point temp = new Point(i, j);
                    if (screen.tileMap.CollisionLayer.GetCellIndex(temp) == index)
                    {
                        next_position.X = i;
                        next_position.Y = j;
                        break;
                    }
                }
            }
            return next_position;
        }

        #endregion
    }
}
