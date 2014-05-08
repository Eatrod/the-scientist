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

        //BF
        protected Texture2D shadowMap;
        PictureBox miniMap;
        PictureBox backgroundToMinimap;
        Label coordX, coordY;
        bool coordVisible = true;
        RenderTarget2D renderTarget;
        //BF

        //bool gate1Locked = true;
        //bool gate2Locked = true;
        protected PlayerScreen screen;
        protected DialogBox dialogBox;
        protected TextBubble textBubble;
        protected NPC_Story talksTo;
        protected bool showingThinkingBox = false;
        
        public Dictionary<int, bool> lockedGateDict;
        protected Dictionary<string, int> gateDict;

        //--     
        Texture2D activeItemBackground, abilityBackground, axeImage, swordImage, crossbowImage;
        Texture2D[] activeItem_textures, axe_ability_textures, crossbow_ability_textures;
        Color[] activeItemBackgroundColor;
        Color[] abilityBackgroundColor;

        bool[] flashing_abilityBackground, flashOn;
        float[] flashTime, flashLength;
        const float flashTimeTotal = 1.0f;

        int HUD_size_ref;
        

        Texture2D Bar_BG, Bar_overlay, Charge_Bar, HP_Bar, Stamina_Bar;
        Color Charge_Bar_Color;
        //--

        int fire_arrow_counter = 0;

        #endregion

        #region Property Region

        //GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public TileMap tileMap = new TileMap();
        protected Camera camera = new Camera();
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

            HUD_size_ref = GameRef.ScreenRectangle.Width / 20;

            lifeRect = new Rectangle(0, 0, 100, 20);//(0, 0, 100, 20);//lifebarRectangle
            staminaRect = new Rectangle(0, 0, 100, 20);//staminabarRectangle
            chargeRect = new Rectangle(0, 0, 100, 20); //chargeRectangle

            FrameAnimation down = new FrameAnimation(4, 50, 80, 0, 0);
            if(!player.Animations.ContainsKey("Down"))
                player.Animations.Add("Down", down);

            FrameAnimation left = new FrameAnimation(4, 50, 80, 0, 80);
            if (!player.Animations.ContainsKey("Left"))
                player.Animations.Add("Left", left);

            FrameAnimation right = new FrameAnimation(4, 50, 80, 0, 160);
            if(!player.Animations.ContainsKey("Right"))
                player.Animations.Add("Right", right);

            FrameAnimation up = new FrameAnimation(4, 50, 80, 0, 240);
            if(!player.Animations.ContainsKey("Up"))
                player.Animations.Add("Up", up);         

            FrameAnimation idledown = new FrameAnimation(1, 50, 80, 200, 0);
            if (!player.Animations.ContainsKey("IdleDown"))
                player.Animations.Add("IdleDown", idledown);
 
            FrameAnimation idleleft = new FrameAnimation(1, 50, 80, 200, 80);
            if (!player.Animations.ContainsKey("IdleLeft"))
                player.Animations.Add("IdleLeft", idleleft);

            FrameAnimation idleright = new FrameAnimation(1, 50, 80, 200, 160);
            if (!player.Animations.ContainsKey("IdleRight"))
                player.Animations.Add("IdleRight", idleright);        

            FrameAnimation idleup = new FrameAnimation(1, 50, 80, 200, 240);
            if (!player.Animations.ContainsKey("IdleUp"))
                player.Animations.Add("IdleUp", idleup);



            FrameAnimation bowright = new FrameAnimation(2, 50, 80, 0, 480);
            if (!player.Animations.ContainsKey("BowRight"))
                player.Animations.Add("BowRight", bowright);

            FrameAnimation bowleft = new FrameAnimation(2, 50, 80, 100, 480);
            if (!player.Animations.ContainsKey("BowLeft"))
                player.Animations.Add("BowLeft", bowleft);

            FrameAnimation bowdown = new FrameAnimation(2, 50, 80, 200, 480);
            if (!player.Animations.ContainsKey("BowDown"))
                player.Animations.Add("BowDown", bowdown);

            FrameAnimation bowup = new FrameAnimation(2, 50, 80, 300, 480);
            if (!player.Animations.ContainsKey("BowUp"))
                player.Animations.Add("BowUp", bowup);


            FrameAnimation axestartright = new FrameAnimation(1, 50, 80, 0, 560);
            if (!player.Animations.ContainsKey("AxeStartRight"))
                player.Animations.Add("AxeStartRight", axestartright);

            FrameAnimation axestartleft = new FrameAnimation(1, 50, 80, 250, 560);
            if (!player.Animations.ContainsKey("AxeStartLeft"))
                player.Animations.Add("AxeStartLeft", axestartleft);

            FrameAnimation axestartdown = new FrameAnimation(1, 50, 80, 150, 560);
            if (!player.Animations.ContainsKey("AxeStartDown"))
                player.Animations.Add("AxeStartDown", axestartdown);

            FrameAnimation axestartup = new FrameAnimation(1, 50, 80, 400, 560);
            if (!player.Animations.ContainsKey("AxeStartUp"))
                player.Animations.Add("AxeStartUp", axestartup);

            
            FrameAnimation axefinishright = new FrameAnimation(1, 100, 80, 50, 560);
            if (!player.Animations.ContainsKey("AxeFinishRight"))
                player.Animations.Add("AxeFinishRight", axefinishright);

            FrameAnimation axefinishleft = new FrameAnimation(1, 100, 80, 300, 560);
            if (!player.Animations.ContainsKey("AxeFinishLeft"))
                player.Animations.Add("AxeFinishLeft", axefinishleft);

            FrameAnimation axefinishdown = new FrameAnimation(1, 50, 80, 200, 560);
            if (!player.Animations.ContainsKey("AxeFinishDown"))
                player.Animations.Add("AxeFinishDown", axefinishdown);

            FrameAnimation axefinishup = new FrameAnimation(1, 50, 80, 450, 560);
            if (!player.Animations.ContainsKey("AxeFinishUp"))
                player.Animations.Add("AxeFinishUp", axefinishup);


            player.CurrentAnimationName = "Down";
            player.oldAnimation = player.CurrentAnimationName;
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
                Point startCell;
                startCell = GameRef.BaseGamePlayScreen.FindCellWithIndexInCurrentTilemap(
                50,
                GameRef.PotatoTown);
                player = new PlayerCharacter(Content.Load<Texture2D>("Sprite/Player"), Content.Load<Texture2D>("CharacterPotraits/DiaPortPlcholder"));
            player.Origionoffset = new Vector2(25, 65);
                player.SetSpritePositionInGameWorld(new Vector2(startCell.X, startCell.Y));
            player.Life = 100;
            player.Stamina = 100;
            }

            //BF
            coordX = new Label();
            coordX.Text = "";
            coordY = new Label();
            coordY.Text = "";
            coordX.Visible = coordVisible;
            coordY.Visible = coordVisible;
            coordX.Position = new Vector2(GraphicsDevice.Viewport.Width - 200, GraphicsDevice.Viewport.Height - 250);
            coordY.Position = new Vector2(GraphicsDevice.Viewport.Width - 100, GraphicsDevice.Viewport.Height - 250);
            
            ControlManager.Add(coordX);
            ControlManager.Add(coordY);
            //BF

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
            //TODO: Fixa det här
            List<SpriteFont> fonts = new List<SpriteFont>(); 
            fonts.Add(Content.Load<SpriteFont>("Fonts/VeniceClassic"));
            fonts.Add(Content.Load<SpriteFont>("Fonts/pixel_pirate"));
            rectangle = new Rectangle(GraphicsDevice.Viewport.Width/2-350, GraphicsDevice.Viewport.Height - 175, 700, 175);
            dialogBox = new DialogBox(fonts,Content.Load<Texture2D>("GUI/DialogBox"), rectangle, "", Content.Load<Texture2D>("GUI/DialogArrow"));
            textBubble = new TextBubble(Content.Load<Texture2D>("GUI/SpeechBubble"), rectangle, "", Content.Load<SpriteFont>("Fonts/BubbleFont"));

            gateDict = new Dictionary<string, int>();
            for (int i = 0; i < 10; i++)
            {
                gateDict["G" + i.ToString()] = 40 + i;
            }
            activeItemBackground = Content.Load<Texture2D>(@"Sprite\activeItemBackground test");
            abilityBackground = Content.Load<Texture2D>(@"Sprite\abilityBackground test2");
            axeImage = Content.Load<Texture2D>(@"Sprite\Axe");
            swordImage = Content.Load<Texture2D>(@"Sprite\Inv Sword test");
            crossbowImage = Content.Load<Texture2D>(@"Sprite\Bow");
            activeItem_textures = new Texture2D[5];

            axe_ability_textures = new Texture2D[2];
            for (int i = 0; i < axe_ability_textures.Count(); i++)
            {
                axe_ability_textures[i] = Content.Load<Texture2D>(@"Sprite\axe ability " + (i + 1).ToString());
            }

            crossbow_ability_textures = new Texture2D[4];
            for (int i = 0; i < crossbow_ability_textures.Count(); i++)
            {
                crossbow_ability_textures[i] = Content.Load<Texture2D>(@"Sprite\arrow ability " + (i+1).ToString());
            }

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

            Bar_BG = Content.Load<Texture2D>(@"Sprite\Bar-BG");
            Bar_overlay = Content.Load<Texture2D>(@"Sprite\Bar-overlay");
            Charge_Bar = Content.Load<Texture2D>(@"Sprite\Charge-Bar");
            HP_Bar = Content.Load<Texture2D>(@"Sprite\HP-Bar");
            Stamina_Bar = Content.Load<Texture2D>(@"Sprite\Stamina-Bar");

            Charge_Bar_Color = Color.White;
            //--

            //BF
            renderTarget = new RenderTarget2D(GraphicsDevice,
                2048, 2048, false,
                GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer,
                Color.DarkSlateBlue, 1.0f, 0);
            tileMap.DrawToShadowMap(spriteBatch, player.Position);
            GraphicsDevice.SetRenderTarget(null);
            shadowMap = (Texture2D)renderTarget;
            
            miniMap = new PictureBox(shadowMap, new Rectangle(
                GraphicsDevice.Viewport.Width - 205, GraphicsDevice.Viewport.Height - 205,
                200, 200));
            backgroundToMinimap = new PictureBox(Content.Load<Texture2D>(@"Sprite\backgroundToMinimap"),
                new Rectangle( GraphicsDevice.Viewport.Width - 210, GraphicsDevice.Viewport.Height - 210,
                    210, 210));
            miniMap.Visible = false;
            backgroundToMinimap.Visible = false;
            ControlManager.Add(backgroundToMinimap);
            ControlManager.Add(miniMap);
            
            //BF
            
        }
        public override void Update(GameTime gameTime)
        {
            
            screen = (PlayerScreen)StateManager.CurrentState;
           
            ContentManager Content = Game.Content;

            //BF
            coordX.Text = ((int)(player.Origin.X / 32)).ToString();
            coordY.Text = ((int)(player.Origin.Y / 32)).ToString();
            //BF

            foreach (AnimatedProjectile sprite in playerprojectiles)
            {
                sprite.updateprojectileposition();            

                if (sprite.GetType() == typeof(FlamingArrowProjectile))
                {
                    
                    FlamingArrowProjectile FireArrow = (FlamingArrowProjectile)sprite;
                    if ((FireArrow.CurrentAnimationName == "right") || (FireArrow.CurrentAnimationName == "right2") || (FireArrow.CurrentAnimationName == "right3"))
                    {
                        if (fire_arrow_counter == 3)//(FireArrow.CurrentAnimationName == "right")
                            FireArrow.CurrentAnimationName = "right2";
                        else if (fire_arrow_counter == 6) //(FireArrow.CurrentAnimationName == "right2")
                            FireArrow.CurrentAnimationName = "right3";
                        else if (fire_arrow_counter == 9)//(FireArrow.CurrentAnimationName == "right3")
                            FireArrow.CurrentAnimationName = "right";
            }
                    if ((FireArrow.CurrentAnimationName == "up") || (FireArrow.CurrentAnimationName == "up2") || (FireArrow.CurrentAnimationName == "up3"))
                    {
                        if (fire_arrow_counter == 3)
                            FireArrow.CurrentAnimationName = "up2";
                        else if (fire_arrow_counter == 6) 
                            FireArrow.CurrentAnimationName = "up3";
                        else if (fire_arrow_counter == 9)
                            FireArrow.CurrentAnimationName = "up";
                    }
                    if ((FireArrow.CurrentAnimationName == "left") || (FireArrow.CurrentAnimationName == "left2") || (FireArrow.CurrentAnimationName == "left3"))
                    {
                        if (fire_arrow_counter == 3)
                            FireArrow.CurrentAnimationName = "left2";
                        else if (fire_arrow_counter == 6)
                            FireArrow.CurrentAnimationName = "left3";
                        else if (fire_arrow_counter == 9)
                            FireArrow.CurrentAnimationName = "left";
                    }
                    if ((FireArrow.CurrentAnimationName == "down") || (FireArrow.CurrentAnimationName == "down2") || (FireArrow.CurrentAnimationName == "down3"))
                    {
                        if (fire_arrow_counter == 3)
                            FireArrow.CurrentAnimationName = "down2";
                        else if (fire_arrow_counter == 6)
                            FireArrow.CurrentAnimationName = "down3";
                        else if (fire_arrow_counter == 9)
                            FireArrow.CurrentAnimationName = "down";
                    }

                    fire_arrow_counter += 1;
                    if (fire_arrow_counter > 9)
                        fire_arrow_counter = 0;
                }
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


            //BF
            if(InputHandler.KeyReleased(Keys.M))
            {
                GraphicsDevice.SetRenderTarget(renderTarget);
                GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer,
                    Color.Black, 1.0f, 0);
                tileMap.DrawToShadowMap(spriteBatch, player.Position);
                GraphicsDevice.SetRenderTarget(null);
                shadowMap = (Texture2D)renderTarget;
                miniMap.Image = shadowMap;

                if (miniMap.Visible == false)
                {
                    miniMap.Visible = true;
                    backgroundToMinimap.Visible = true;
                }
                else
                {
                    miniMap.Visible = false;
                    backgroundToMinimap.Visible = false;
                }
            }
            //BF

            string key_string;
            int key_number;
            if (StoryProgress.activeItemsDict.ContainsKey("Crossbow"))
            {
                key_string = StoryProgress.activeItemsDict["Crossbow"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                if (activeItemBackgroundColor[key_number - 1] == Color.White)
                {
                    if (InputHandler.KeyReleased(Keys.Q) && (player.Stamina - 20 >= 0) && !player.shotFired)
                    {
                        player.oldAnimation = player.CurrentAnimationName;
                        player.shotFired = true;
                        player.normalArrow = true;
                        UpdateBowAttackAnimaition();                                     
                    }

                    if (InputHandler.KeyReleased(Keys.W) && (player.Stamina - 50 > 0) && !player.shotFired)
                    {
                        player.oldAnimation = player.CurrentAnimationName;
                        player.shotFired = true;
                        player.fireArrow = true;
                        UpdateBowAttackAnimaition();                                                               
                    }

                    if (InputHandler.KeyReleased(Keys.E) && (player.Stamina - 40 > 0) && !player.shotFired)
                    {
                        player.oldAnimation = player.CurrentAnimationName;
                        player.shotFired = true;
                        player.multishotArrow = true;
                        UpdateBowAttackAnimaition();        
                    }
                    
                    if (InputHandler.KeyDown(Keys.R))
                    {
                        player.Charge += 0.5f;
                        if (player.Charge >= 100)
                        {
                            player.Charge = 100;           
                            Charge_Bar_Color = Color.Crimson;
                        }
                    }

                    if (InputHandler.KeyReleased(Keys.R))
                    {
                        if (player.Charge == 100 && (player.Stamina - 30) > 0 && !player.shotFired)
                        {
                            player.oldAnimation = player.CurrentAnimationName;
                            player.shotFired = true;
                            player.multishotFireArrow = true;
                            UpdateBowAttackAnimaition();
                        }

                        else if ((player.Stamina - 20) > 0 && !player.shotFired)
                        {
                            player.oldAnimation = player.CurrentAnimationName;
                            player.shotFired = true;
                            player.normalArrow = true;
                            UpdateBowAttackAnimaition();                           
                        }

                        else
                        {
                            player.Stamina = 10;
                        }
                        player.Charge = 0;
                        Charge_Bar_Color = Color.White;
                    }
                }
            }

            if (StoryProgress.activeItemsDict.ContainsKey("Axe"))
            {
                key_string = StoryProgress.activeItemsDict["Axe"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                if (activeItemBackgroundColor[key_number - 1] == Color.White)
                {                   

                    if (InputHandler.KeyDown(Keys.Q) && !player.meleeAttackStart && (player.Stamina - 20 >= 0) || InputHandler.KeyDown(Keys.W) && !player.meleeAttackStart && (player.Stamina - 50 >= 0))
                    {
                        player.oldAnimation = player.CurrentAnimationName;
                        player.meleeAttackStart = true;
                        UpdateAxeStartAttackAnimaition();
                    }
                    
                    
                    
                    
                    if (InputHandler.KeyReleased(Keys.Q) && (player.Stamina - 20 >= 0) && player.meleeAttackStart)
                    {
                        player.meleeAttackStart = false;
                        player.meleeAttackFinish = true;
                        UpdateAxeFinishAttackAnimaition();

                        if (player.CurrentAnimationName == "AxeFinishLeft")
                        {
                            player.Position.X -= 27;
                        }

                        if (player.CurrentAnimationName == "AxeFinishRight")
                        {
                            player.Position.X -= 27;
                        }

                        

                        if (Vector2.Distance(player.Origin, GameRef.PotatoTown.treeStanding.Origin) < 75)
                        {
                            if (!StoryProgress.ProgressLine["treeIsDown"])
                            {
                                GameRef.PotatoTown.SpriteObjectInGameWorld.Remove(GameRef.PotatoTown.treeStanding);
                                GameRef.PotatoTown.renderList.Remove(GameRef.PotatoTown.treeStanding);
                                GameRef.PotatoTown.renderList.Add(GameRef.PotatoTown.treeBridge);
                                GameRef.PotatoTown.renderList.Add(GameRef.PotatoTown.treeStubbe);
                                StoryProgress.ProgressLine["treeIsDown"] = true;
                            }
                        }
                        player.Stamina -= 20f;
                    }                   
                }
            }

            if (InputHandler.KeyReleased(Keys.Tab))
            {
                player.Stamina = 100;
                //player.Life -= 10;
            }
            if (InputHandler.KeyReleased(Keys.D))
                player.Life -= 10;

            if (InputHandler.KeyReleased(Keys.Escape))
            {
                GameRef.StartMenuScreen.SwitchBackToOriginalMenu();
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

            if (motion != Vector2.Zero && !player.shotFired && !player.meleeAttackStart && !player.meleeAttackFinish)
            {
                motion.Normalize();                 //Comment out to use gamePad
                motion = CollisionWithTerrain.CheckCollisionForMotion(motion, player,screen);

                //sprite.Position += motion * sprite.Speed;
                UpdateSpriteAnimation(motion);
                player.isAnimating = true;
                CollisionWithTerrain.CheckForCollisionAroundSprite(player, motion,screen);
            }
            else if (!player.shotFired && !player.meleeAttackStart && !player.meleeAttackFinish)
            {
                UpdateSpriteIdleAnimation(player);
                player.isAnimating = false;
                motion = new Vector2(0, 0);
            }

            else
            {
                player.elapsedShot += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                player.elapsedAttack += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                player.Speed = -0.1f;
                
                if (player.elapsedShot > player.delayShot && player.multishotArrow)
                {
 
                    MultiarrowFired(Content, motion);
                    player.CurrentAnimation.CurrentFrame = 0;
                    player.elapsedShot = 0.0f;
                    player.shotFired = false;
                    player.multishotArrow = false;
                    player.CurrentAnimationName = player.oldAnimation;
                    player.Speed = 2;
                }

                if (player.elapsedShot > player.delayShot && player.multishotFireArrow)
                {
                    MultiFireArrowFired(Content, motion);
                    player.CurrentAnimation.CurrentFrame = 0;
                    player.elapsedShot = 0.0f;
                    player.shotFired = false;
                    player.multishotFireArrow = false;
                    player.CurrentAnimationName = player.oldAnimation;
                    player.Speed = 2;
                }

                if (player.elapsedShot > player.delayShot && player.fireArrow)
                {
                    FirearrowFired(Content, motion);
                    player.CurrentAnimation.CurrentFrame = 0;
                    player.elapsedShot = 0.0f;
                    player.shotFired = false;
                    player.fireArrow = false;
                    player.CurrentAnimationName = player.oldAnimation;
                    player.Speed = 2;
                }

                if (player.elapsedShot > player.delayShot && player.normalArrow)
                {
                    NormalarrowFired(Content, motion);
                    player.CurrentAnimation.CurrentFrame = 0;
                    player.elapsedShot = 0.0f;
                    player.shotFired = false;
                    player.normalArrow = false;
                    player.CurrentAnimationName = player.oldAnimation;
                    player.Speed = 2;
                }

                if (player.elapsedAttack > player.delayAttack && player.meleeAttackFinish)
                {
                    player.elapsedAttack = 0.0f;
                    player.meleeAttackFinish = false;
                    if (player.CurrentAnimationName == "AxeFinishLeft")
                    {
                        player.Position.X += 27;
                    }

                    if (player.CurrentAnimationName == "AxeFinishRight")
                    {
                        player.Position.X += 27;
                    } 
                    UpdateAxeDoneAttackAnimaition();

                    
                    //player.CurrentAnimationName = player.oldAnimation;
                    player.Speed = 2;
                }
            }


            motion = CollisionWithTerrain.CheckCollisionAutomaticMotion(motion, player,screen);
            //UpdateSpriteAnimation(motion);
            player.Position += motion * player.Speed;
            player.ClampToArea(screen.tileMap.GetWidthInPixels(), screen.tileMap.GetHeightInPixels());  //Funktion för att hämta nuvarande tilemap state.
            player.Update(gameTime);


            int screenWidth = GraphicsDevice.Viewport.Width;
            int screenHeight = GraphicsDevice.Viewport.Height;

            camera.LockToTarget(player, screenWidth, screenHeight);
            camera.ClampToArea(
                screen.tileMap.GetWidthInPixels() - screenWidth,   //Funktion för att hämta nuvarande tilemap state.
                screen.tileMap.GetHeightInPixels() - screenHeight);

            if (player.Life <= 0)
            {
                //player.SetSpritePositionInGameWorld(new Vector2(0, 0));
                Point startCell;
                startCell = GameRef.BaseGamePlayScreen.FindCellWithIndexInCurrentTilemap(50, GameRef.PotatoTown);
                player.SetSpritePositionInGameWorld( new Vector2(startCell.X, startCell.Y));

                player.Life = 100;
                player.areTakingDamage = false;
                lifeRect = new Rectangle(0, 0, 100, 20);
                staminaRect = new Rectangle(0, 0, 100, 20);
                chargeRect = new Rectangle(0, 0, 100, 20);

            }

            UpdateItemHUD(gameTime);

            base.Update(gameTime);
        }

        

        private void NormalarrowFired(ContentManager Content, Vector2 motion)
        {
            NormalArrowProjectile temparrow = new NormalArrowProjectile(Content.Load<Texture2D>("Sprite/Arrow2"), 10f, 0.1f, 6f, player.Position);

            if (motion == Vector2.Zero)
            {
                if (player.CurrentAnimationName == "BowUp")
                {
                    motion = new Vector2(0, -2);
                }
                if (player.CurrentAnimationName == "BowDown")
                {
                    motion = new Vector2(0, 2);
                }
                if (player.CurrentAnimationName == "BowLeft")
                {
                    motion = new Vector2(-2, 0);
                }
                if (player.CurrentAnimationName == "BowRight")
                {
                    motion = new Vector2(2, 0);
                }

                temparrow.UpdatecurrentAnimation(motion);
                playerprojectiles.Add(temparrow);
                player.Stamina -= 20f;
                motion = Vector2.Zero;
            }

            else
            {
                temparrow.UpdatecurrentAnimation(motion);
                playerprojectiles.Add(temparrow);
                player.Stamina -= 20f;
            }
        }

        private void FirearrowFired(ContentManager Content, Vector2 motion)
        {
            if (motion == Vector2.Zero)
            {
                if (player.CurrentAnimationName == "BowUp")
                {
                    motion = new Vector2(0, -2);
                }
                if (player.CurrentAnimationName == "BowDown")
                {
                    motion = new Vector2(0, 2);
                }
                if (player.CurrentAnimationName == "BowLeft")
                {
                    motion = new Vector2(-2, 0);
                }
                if (player.CurrentAnimationName == "BowRight")
                {
                    motion = new Vector2(2, 0);
                }

                FlamingArrowProjectile temparrow = new FlamingArrowProjectile(Content.Load<Texture2D>("Sprite/FireArrow"), 10f, 0.1f, 6f, player.Position);
                temparrow.UpdatecurrentAnimation(motion);
                playerprojectiles.Add(temparrow);
                player.Stamina -= 50;
                motion = Vector2.Zero;
            }

            else
            {
                FlamingArrowProjectile temparrow = new FlamingArrowProjectile(Content.Load<Texture2D>("Sprite/FireArrow"), 10f, 0.1f, 6f, player.Position);
                temparrow.UpdatecurrentAnimation(motion);
                playerprojectiles.Add(temparrow);
                player.Stamina -= 50;
            }
        }

        private void MultiarrowFired(ContentManager Content, Vector2 motion)
        {
            if (motion == Vector2.Zero)
            {
                if (player.CurrentAnimationName == "BowUp")
                {
                    motion = new Vector2(0, -2);
                }
                if (player.CurrentAnimationName == "BowDown")
                {
                    motion = new Vector2(0, 2);
                }
                if (player.CurrentAnimationName == "BowLeft")
                {
                    motion = new Vector2(-2, 0);
                }
                if (player.CurrentAnimationName == "BowRight")
                {
                    motion = new Vector2(2, 0);
                }

                NormalArrowProjectile temparrow = new NormalArrowProjectile(Content.Load<Texture2D>("Sprite/Arrow2"), 10f, 0.1f, 6f, player.Position);             
                NormalArrowProjectile temparrow1 = new NormalArrowProjectile(Content.Load<Texture2D>("Sprite/Arrow2"), 10f, 0.1f, 6f, player.Position);            
                NormalArrowProjectile temparrow2 = new NormalArrowProjectile(Content.Load<Texture2D>("Sprite/Arrow2"), 10f, 0.1f, 6f, player.Position);
                if (player.CurrentAnimationName == "BowUp" || player.CurrentAnimationName == "BowDown")
                {
                    temparrow1.Position.X += 10;
                    temparrow2.Position.X -= 10;

                    if (player.CurrentAnimationName == "BowUp")
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


                    if (player.CurrentAnimationName == "BowLeft")
                    {
                        temparrow.Position.Y += 30;
                        temparrow1.Position.Y += 20;
                        temparrow2.Position.Y += 40;
                        temparrow.Position.X -= 10;
                        temparrow1.Position.X -= 5;
                        temparrow2.Position.X -= 5;
                    }
                    else
                    {
                        temparrow.Position.Y += 30;
                        temparrow1.Position.Y += 20;
                        temparrow2.Position.Y += 40;
                        temparrow.Position.X += 60;
                        temparrow1.Position.X += 55;
                        temparrow2.Position.X += 55;
                    }
                }
                temparrow.UpdatecurrentAnimation(motion);
                playerprojectiles.Add(temparrow);
                temparrow1.UpdatecurrentAnimation(motion);
                playerprojectiles.Add(temparrow1);
                temparrow2.UpdatecurrentAnimation(motion);
                playerprojectiles.Add(temparrow2);
                player.Stamina -= 40f;
                motion = Vector2.Zero;
            }

            else 
            {
                NormalArrowProjectile temparrow = new NormalArrowProjectile(Content.Load<Texture2D>("Sprite/Arrow2"), 10f, 0.1f, 6f, player.Position);       
                NormalArrowProjectile temparrow1 = new NormalArrowProjectile(Content.Load<Texture2D>("Sprite/Arrow2"), 10f, 0.1f, 6f, player.Position);            
                NormalArrowProjectile temparrow2 = new NormalArrowProjectile(Content.Load<Texture2D>("Sprite/Arrow2"), 10f, 0.1f, 6f, player.Position);
                if (player.CurrentAnimationName == "BowUp" || player.CurrentAnimationName == "BowDown")
                {
                    temparrow1.Position.X += 10;
                    temparrow2.Position.X -= 10;

                    if (player.CurrentAnimationName == "BowUp")
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


                    if (player.CurrentAnimationName == "BowLeft")
                    {
                        temparrow.Position.Y += 30;
                        temparrow1.Position.Y += 20;
                        temparrow2.Position.Y += 40;
                        temparrow.Position.X -= 10;
                        temparrow1.Position.X -= 5;
                        temparrow2.Position.X -= 5;
                    }
                    else
                    {
                        temparrow.Position.Y += 30;
                        temparrow1.Position.Y += 20;
                        temparrow2.Position.Y += 40;
                        temparrow.Position.X += 60;
                        temparrow1.Position.X += 55;
                        temparrow2.Position.X += 55;
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
        }

        private void MultiFireArrowFired(ContentManager Content, Vector2 motion)
        {
            if (motion == Vector2.Zero)
            {
                if (player.CurrentAnimationName == "BowUp")
                {
                    motion = new Vector2(0, -2);
                }
                if (player.CurrentAnimationName == "BowDown")
                {
                    motion = new Vector2(0, 2);
                }
                if (player.CurrentAnimationName == "BowLeft")
                {
                    motion = new Vector2(-2, 0);
                }
                if (player.CurrentAnimationName == "BowRight")
                {
                    motion = new Vector2(2, 0);
                }

                FlamingArrowProjectile temparrow = new FlamingArrowProjectile(Content.Load<Texture2D>("Sprite/FireArrow"), 10f, 0.1f, 6f, player.Position);
                FlamingArrowProjectile temparrow1 = new FlamingArrowProjectile(Content.Load<Texture2D>("Sprite/FireArrow"), 10f, 0.1f, 6f, player.Position);
                FlamingArrowProjectile temparrow2 = new FlamingArrowProjectile(Content.Load<Texture2D>("Sprite/FireArrow"), 10f, 0.1f, 6f, player.Position);
                if (player.CurrentAnimationName == "BowUp" || player.CurrentAnimationName == "BowDown")
                {
                    temparrow1.Position.X += 10;
                    temparrow2.Position.X -= 10;

                    if (player.CurrentAnimationName == "BowUp")
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


                    if (player.CurrentAnimationName == "BowLeft")
                    {
                        temparrow.Position.Y += 30;
                        temparrow1.Position.Y += 20;
                        temparrow2.Position.Y += 40;
                        temparrow.Position.X -= 10;
                        temparrow1.Position.X -= 5;
                        temparrow2.Position.X -= 5;
                    }
                    else
                    {
                        temparrow.Position.Y += 30;
                        temparrow1.Position.Y += 20;
                        temparrow2.Position.Y += 40;
                        temparrow.Position.X += 60;
                        temparrow1.Position.X += 55;
                        temparrow2.Position.X += 55;
                    }
                }
                temparrow.UpdatecurrentAnimation(motion);
                playerprojectiles.Add(temparrow);
                temparrow1.UpdatecurrentAnimation(motion);
                playerprojectiles.Add(temparrow1);
                temparrow2.UpdatecurrentAnimation(motion);
                playerprojectiles.Add(temparrow2);
                player.Stamina -= 40f;
                motion = Vector2.Zero;
            }

            else
            {
                FlamingArrowProjectile temparrow = new FlamingArrowProjectile(Content.Load<Texture2D>("Sprite/FireArrow"), 10f, 0.1f, 6f, player.Position);
                FlamingArrowProjectile temparrow1 = new FlamingArrowProjectile(Content.Load<Texture2D>("Sprite/FireArrow"), 10f, 0.1f, 6f, player.Position);
                FlamingArrowProjectile temparrow2 = new FlamingArrowProjectile(Content.Load<Texture2D>("Sprite/FireArrow"), 10f, 0.1f, 6f, player.Position);
                if (player.CurrentAnimationName == "BowUp" || player.CurrentAnimationName == "BowDown")
                {
                    temparrow1.Position.X += 10;
                    temparrow2.Position.X -= 10;

                    if (player.CurrentAnimationName == "BowUp")
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


                    if (player.CurrentAnimationName == "BowLeft")
                    {
                        temparrow.Position.Y += 30;
                        temparrow1.Position.Y += 20;
                        temparrow2.Position.Y += 40;
                        temparrow.Position.X -= 10;
                        temparrow1.Position.X -= 5;
                        temparrow2.Position.X -= 5;
                    }
                    else
                    {
                        temparrow.Position.Y += 30;
                        temparrow1.Position.Y += 20;
                        temparrow2.Position.Y += 40;
                        temparrow.Position.X += 60;
                        temparrow1.Position.X += 55;
                        temparrow2.Position.X += 55;
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

            //--
            HUD_size_ref = GameRef.ScreenRectangle.Width / 25;// /20
            //--

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
         
            for (int i = 0; i < 3; i++)
            {
                spriteBatch.Draw(
                    Bar_BG,
                    new Rectangle(0, (int)(HUD_size_ref / 1.5) * i, (HUD_size_ref * 4 / 100) * 100 + (HUD_size_ref / 8) * 2 /*(HUD_size_ref * 3) + (int)(HUD_size_ref / 2.5)*/, (int)(HUD_size_ref / 1.5)),  //new Rectangle(110 + (50 * i), 0, 42, 42),
                    Color.White);
            }

            //lifemeteranimation.Draw(spriteBatch);
            lifemeteranimation.Draw_test(spriteBatch, HUD_size_ref / 8, HUD_size_ref / 16, (HUD_size_ref * 4 / 100) * (int)player.Life, HUD_size_ref / 2, Color.White);
            //staminaanimation.Draw(spriteBatch);
            staminaanimation.Draw_test(spriteBatch, HUD_size_ref / 8, HUD_size_ref / 2 + ((HUD_size_ref / 16) * 4), (HUD_size_ref * 4 / 100) * (int)player.Stamina/*(HUD_size_ref * 4 / 100) * (int)player.Stamina*/, HUD_size_ref / 2, Color.White);  
            //chargeanimation.Draw(spriteBatch);
            chargeanimation.Draw_test(spriteBatch, HUD_size_ref / 8, (HUD_size_ref / 2) * 2 + ((HUD_size_ref / 16) * 7), (HUD_size_ref * 4 / 100) * (int)player.Charge, HUD_size_ref / 2, Charge_Bar_Color);
            DrawItemHUD();

            spriteBatch.Draw(
                HP_Bar,
                new Rectangle(0, 0, (HUD_size_ref * 4 / 100) * 100 + (HUD_size_ref / 8)*2  /*(HUD_size_ref * 3) + (int)(HUD_size_ref /2.5)*/, (int)(HUD_size_ref / 1.5)),  //new Rectangle(110 + (50 * i), 0, 42, 42),
                Color.White);

            spriteBatch.Draw(
                Stamina_Bar,
                new Rectangle(0, (int)(HUD_size_ref / 1.5), (HUD_size_ref * 4 / 100) * 100 + (HUD_size_ref / 8)*2 /*(HUD_size_ref * 3) + (int)(HUD_size_ref / 2.5)*/, (int)(HUD_size_ref / 1.5)),  //new Rectangle(110 + (50 * i), 0, 42, 42),
                Color.White);

            spriteBatch.Draw(
                Charge_Bar,
                new Rectangle(0, (int)(HUD_size_ref / 1.5) * 2, (HUD_size_ref * 4 / 100) * 100 + (HUD_size_ref / 8) * 2 /*(HUD_size_ref * 3) + (int)(HUD_size_ref / 2.5)*/, (int)(HUD_size_ref / 1.5)),  //new Rectangle(110 + (50 * i), 0, 42, 42),
                Color.White);

            for (int i = 0; i < 3; i++)
            {
                spriteBatch.Draw(
                    Bar_overlay,
                    new Rectangle(0, (int)(HUD_size_ref / 1.5) * i, (HUD_size_ref * 4 / 100) * 100 + (HUD_size_ref / 8) * 2/*(HUD_size_ref * 3) + (int)(HUD_size_ref / 2.5)*/, (int)(HUD_size_ref / 1.5)),  //new Rectangle(110 + (50 * i), 0, 42, 42),
                    Color.White * 0.3f);
            }


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
                new Rectangle(((HUD_size_ref * 4) / 100) * 100 + HUD_size_ref / 10 + ((HUD_size_ref + HUD_size_ref / 10) * i) + (HUD_size_ref / 4), 0, HUD_size_ref, HUD_size_ref),  //new Rectangle(110 + (50 * i), 0, 42, 42),
                activeItemBackgroundColor[i]); //* 0.8f);
            }         


            //Weapon Ability backgrounds
            //Bakgrundens "ram" = WAB_width / 8
            //int WAB_width = GameRef.ScreenRectangle.Width / 20;
            for (int i = 0; i < activeItem_textures.Count(); i++)
            {
                spriteBatch.Draw(
                abilityBackground,
                new Rectangle(GameRef.ScreenRectangle.Width / 2 - (HUD_size_ref * 2 + (HUD_size_ref) / 2) + ((HUD_size_ref + HUD_size_ref/10) * i), GameRef.ScreenRectangle.Height - HUD_size_ref, HUD_size_ref, HUD_size_ref),//(GameRef.ScreenRectangle.Width / 2 - (50 * 2 + 21) + (50 * i), GameRef.ScreenRectangle.Height - 50, 42, 42),
                abilityBackgroundColor[i]);
            }

            //Weapon Ability pics

            string key_string;
            int key_number;

            if (StoryProgress.activeItemsDict.ContainsKey("Axe"))
            {
                key_string = StoryProgress.activeItemsDict["Axe"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                if (activeItemBackgroundColor[key_number - 1] == Color.White)
                {
                    for (int i = 0; i < axe_ability_textures.Count(); i++)
                    {
                        spriteBatch.Draw(
                        axe_ability_textures[i],
                        new Rectangle(GameRef.ScreenRectangle.Width / 2 - (HUD_size_ref * 2 + (HUD_size_ref) / 2) + ((HUD_size_ref + HUD_size_ref / 10) * i) + HUD_size_ref / 8, GameRef.ScreenRectangle.Height - HUD_size_ref + HUD_size_ref / 8, HUD_size_ref - HUD_size_ref / 4, HUD_size_ref - HUD_size_ref / 4),
                        Color.White);
                    }
                }
            }
            if (StoryProgress.activeItemsDict.ContainsKey("Crossbow"))
            {
                key_string = StoryProgress.activeItemsDict["Crossbow"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                if (activeItemBackgroundColor[key_number - 1] == Color.White)
                {
                    for (int i = 0; i < crossbow_ability_textures.Count(); i++)
                    {
                        spriteBatch.Draw(
                        crossbow_ability_textures[i],
                        new Rectangle(GameRef.ScreenRectangle.Width / 2 - (HUD_size_ref * 2 + (HUD_size_ref) / 2) + ((HUD_size_ref + HUD_size_ref / 10) * i) + HUD_size_ref / 8, GameRef.ScreenRectangle.Height - HUD_size_ref + HUD_size_ref / 8, HUD_size_ref - HUD_size_ref / 4, HUD_size_ref - HUD_size_ref / 4), //(GameRef.ScreenRectangle.Width / 2 - (50 * 2 + 16) + (50 * i), GameRef.ScreenRectangle.Height - 45, 32, 32),
                        Color.White);
                    }
                }
            }


            if (StoryProgress.activeItemsDict.ContainsKey("Axe"))
            {
                key_string = StoryProgress.activeItemsDict["Axe"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                spriteBatch.Draw(axeImage, new Rectangle(((HUD_size_ref * 4) / 100) * 100 + HUD_size_ref / 10 + ((HUD_size_ref + HUD_size_ref / 10) * (key_number - 1)) + HUD_size_ref / 8 + (HUD_size_ref / 4), HUD_size_ref / 8, HUD_size_ref - HUD_size_ref / 4, HUD_size_ref - HUD_size_ref / 4), Color.White);
            }
           
            if (StoryProgress.activeItemsDict.ContainsKey("Sword"))
            {
                key_string = StoryProgress.activeItemsDict["Sword"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                spriteBatch.Draw(swordImage, new Rectangle(((HUD_size_ref * 4) / 100) * 100 + HUD_size_ref / 10 + ((HUD_size_ref + HUD_size_ref / 10) * (key_number - 1)) + HUD_size_ref / 8 + (HUD_size_ref / 4), HUD_size_ref / 8, HUD_size_ref - HUD_size_ref / 4, HUD_size_ref - HUD_size_ref / 4), Color.White);
            }
            
            if (StoryProgress.activeItemsDict.ContainsKey("Crossbow"))
            {
                key_string = StoryProgress.activeItemsDict["Crossbow"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                spriteBatch.Draw(crossbowImage, new Rectangle(((HUD_size_ref * 4) / 100) * 100 + HUD_size_ref / 10 + ((HUD_size_ref + HUD_size_ref / 10) * (key_number - 1)) + HUD_size_ref / 8 + (HUD_size_ref / 4), HUD_size_ref / 8, HUD_size_ref - HUD_size_ref / 4, HUD_size_ref - HUD_size_ref / 4), Color.White);
            }
            
            if (StoryProgress.activeItemsDict.ContainsKey("Spear"))
            {
                key_string = StoryProgress.activeItemsDict["Spear"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                spriteBatch.Draw(crossbowImage, new Rectangle(((HUD_size_ref * 4) / 100) * 100 + HUD_size_ref / 10 + ((HUD_size_ref + HUD_size_ref / 10) * (key_number - 1)) + HUD_size_ref / 8 + (HUD_size_ref / 4), HUD_size_ref / 8, HUD_size_ref - HUD_size_ref / 4, HUD_size_ref - HUD_size_ref / 4), Color.White);
            }
            
            if (StoryProgress.activeItemsDict.ContainsKey("DOOM-erang"))
            {
                key_string = StoryProgress.activeItemsDict["DOOM-erang"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                spriteBatch.Draw(crossbowImage, new Rectangle(((HUD_size_ref * 4) / 100) * 100 + HUD_size_ref / 10 + ((HUD_size_ref + HUD_size_ref / 10) * (key_number - 1)) + HUD_size_ref / 8 + (HUD_size_ref / 4), HUD_size_ref / 8, HUD_size_ref - HUD_size_ref / 4, HUD_size_ref - HUD_size_ref / 4), Color.White);
            }
            
            if (StoryProgress.activeItemsDict.ContainsKey("Hammer"))
            {
                key_string = StoryProgress.activeItemsDict["Hammer"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                spriteBatch.Draw(crossbowImage, new Rectangle(((HUD_size_ref * 4) / 100) * 100 + HUD_size_ref / 10 + ((HUD_size_ref + HUD_size_ref / 10) * (key_number - 1)) + HUD_size_ref / 8 + (HUD_size_ref / 4), HUD_size_ref / 8, HUD_size_ref - HUD_size_ref / 4, HUD_size_ref - HUD_size_ref / 4), Color.White);
            }
            
            if (StoryProgress.activeItemsDict.ContainsKey("MetalBladeCrossbow"))
            {
                key_string = StoryProgress.activeItemsDict["MetalBladeCrossbow"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                spriteBatch.Draw(crossbowImage, new Rectangle(((HUD_size_ref * 4) / 100) * 100 + HUD_size_ref / 10 + ((HUD_size_ref + HUD_size_ref / 10) * (key_number - 1)) + HUD_size_ref / 8 + (HUD_size_ref / 4), HUD_size_ref / 8, HUD_size_ref - HUD_size_ref / 4, HUD_size_ref - HUD_size_ref / 4), Color.White);
            }
            
            if (StoryProgress.activeItemsDict.ContainsKey("Hookshot"))
            {
                key_string = StoryProgress.activeItemsDict["Hookshot"].ToString();
                key_string = key_string.Replace('D', ' ');
                key_number = Convert.ToInt32(key_string);
                spriteBatch.Draw(crossbowImage, new Rectangle(((HUD_size_ref * 4) / 100) * 100 + HUD_size_ref / 10 + ((HUD_size_ref + HUD_size_ref / 10) * (key_number - 1)) + HUD_size_ref / 8 + (HUD_size_ref / 4), HUD_size_ref / 8, HUD_size_ref - HUD_size_ref / 4, HUD_size_ref - HUD_size_ref / 4), Color.White);
            }
            
        }

        #endregion

        #region Abstract Method Region
        #endregion

        #region Sprite Animation Code

        private static void UpdateBowAttackAnimaition()
        {
            if (player.CurrentAnimationName == "Up" || player.CurrentAnimationName == "Down" || player.CurrentAnimationName == "IdleUp" || player.CurrentAnimationName == "IdleDown")
            {

                if (player.CurrentAnimationName == "Up" || player.CurrentAnimationName == "IdleUp")
                {
                    player.CurrentAnimationName = "BowUp";
                }
                else
                {
                    player.CurrentAnimationName = "BowDown";
                }

            }
            else
            {
                if (player.CurrentAnimationName == "Left" || player.CurrentAnimationName == "IdleLeft")
                {
                    player.CurrentAnimationName = "BowLeft";

                }
                else
                {
                    player.CurrentAnimationName = "BowRight";
                }
            }
        }

        private static void UpdateAxeStartAttackAnimaition()
        {
            if (player.CurrentAnimationName == "Up" || player.CurrentAnimationName == "Down" || player.CurrentAnimationName == "IdleUp" || player.CurrentAnimationName == "IdleDown")
            {

                if (player.CurrentAnimationName == "Up" || player.CurrentAnimationName == "IdleUp")
                {
                    player.CurrentAnimationName = "AxeStartUp";
                }
                else
                {
                    player.CurrentAnimationName = "AxeStartDown";
                }

            }
            else
            {
                if (player.CurrentAnimationName == "Left" || player.CurrentAnimationName == "IdleLeft")
                {
                    player.CurrentAnimationName = "AxeStartLeft";

                }
                else
                {
                    player.CurrentAnimationName = "AxeStartRight";
                }
            }
        }

        private static void UpdateAxeFinishAttackAnimaition()
        {
            if (player.CurrentAnimationName == "AxeStartUp" || player.CurrentAnimationName == "AxeStartDown" )
            {

                if (player.CurrentAnimationName == "AxeStartUp" )
                {
                    player.CurrentAnimationName = "AxeFinishUp";
                }
                else
                {
                    player.CurrentAnimationName = "AxeFinishDown";
                }

            }
            else
            {
                if (player.CurrentAnimationName == "AxeStartLeft")
                {
                    player.CurrentAnimationName = "AxeFinishLeft";

                }
                else
                {
                    player.CurrentAnimationName = "AxeFinishRight";
                }
            }
        }

        private static void UpdateAxeDoneAttackAnimaition()
        {
            if (player.CurrentAnimationName == "AxeFinishUp" || player.CurrentAnimationName == "AxeFinishDown")
            {

                if (player.CurrentAnimationName == "AxeFinishUp")
                {
                    player.CurrentAnimationName = "IdleUp";
                }
                else
                {
                    player.CurrentAnimationName = "IdleDown";
                }

            }
            else
            {
                if (player.CurrentAnimationName == "AxeFinishLeft")
                {
                    player.CurrentAnimationName = "IdleLeft";

                }
                else
                {
                    player.CurrentAnimationName = "IdleRight";
                }
            }
        }

        private void UpdateSpriteIdleAnimation(AnimatedSprite sprite)
        {
            if (sprite.CurrentAnimationName == "Up")
                sprite.CurrentAnimationName = "IdleUp";
            if (sprite.CurrentAnimationName == "Down")
                sprite.CurrentAnimationName = "IdleDown";
            if (sprite.CurrentAnimationName == "Right")
                sprite.CurrentAnimationName = "IdleRight";
            if (sprite.CurrentAnimationName == "Left")
                sprite.CurrentAnimationName = "IdleLeft";           
        }

        private void UpdateSpriteAnimation(Vector2 motion)
        {
            float motionAngle = (float)Math.Atan2(motion.Y, motion.X);
            player.CurrentAnimation.FramesPerSeconds = 0.20f;

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
            dialogBox.story = GameRef.storyProgress;
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

        public bool PlayerInTriggerRange(BaseSprite sprite)
        {
            Vector2 d = player.Origin - sprite.Origin;
            return (d.Length() < 70);
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
