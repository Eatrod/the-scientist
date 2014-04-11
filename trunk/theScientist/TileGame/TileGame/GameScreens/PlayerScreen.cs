using System;
using System.Collections.Generic;
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
        PlayerScreen screen;
        protected DialogBox dialogBox;
        

        #endregion
        #region Property Region

        //GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public TileMap tileMap = new TileMap();
        protected Camera camera = new Camera();
        private Dialog dialog;

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
            player = new PlayerCharacter(Content.Load<Texture2D>("Sprite/playerboxAnimation"));
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
            Rectangle rectangle = new Rectangle(0,0,300,100);
            dialog = new Dialog();

            
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


            if (InputHandler.KeyDown(Keys.Up))
                motion.Y-=2;
            if (InputHandler.KeyDown(Keys.Down))
                motion.Y+=2;
            if (InputHandler.KeyDown(Keys.Left))
                motion.X-=2;
            if (InputHandler.KeyDown(Keys.Right))
                motion.X+=2;
            
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

            if (InputHandler.KeyReleased(Keys.Space))
            {
                dialog.NextText(GameRef.GamePlayScreen.npc, GameRef.GamePlayScreen.npc.text);
                dialogBox.Text = dialog.conversation.Text;
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

            // lägg tillbaka hop mellan tilemaps.

            //Point cell = Engine.ConvertPostionToCell(player.Origin);
            //if ((cell.X == 17 && cell.Y == 14) && !gate2Locked)
            //{
            //    StateManager.ChangeState(GameRef.GamePlayScreen2);
            //    GameRef.GamePlayScreen2.SetPlayerPosition(1, 2);
            //    GameRef.GamePlayScreen2.Gate1Locked = true;
            //}
            //if (cell.X != 17 || cell.Y != 14)
            //    gate2Locked = false;

            //if ((cell.X == 4 && cell.Y == 3) && !gate1Locked)
            //{
            //    StateManager.ChangeState(GameRef.GamePlayScreen2);
            //    GameRef.GamePlayScreen2.SetPlayerPosition(28, 28);
            //    GameRef.GamePlayScreen2.Gate2Locked = true;
            //}
            //if (cell.X != 4 || cell.Y != 3)
            //    gate1Locked = false;


            base.Update(gameTime);

                
            
        }

      

      
        private void UpdateHealthBarAnimation()
        {
            life = player.Life;

            if (lifeRect.Width > (int)life)
                lifeRect.Width -= 1;

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

            if (staminaRect.Width > (int)stamina)
                staminaRect.Width = (int)stamina;
            if (staminaRect.Width < (int)stamina)
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

            if (chargeRect.Width > (int)charge)
                chargeRect.Width = (int)charge;
            if (chargeRect.Width < (int)charge)
                chargeRect.Width = (int)charge;

            chargeRect = new Rectangle(
                chargeanimation.CurrentAnimation.CurrentRectangle.Location.X,
                chargeanimation.CurrentAnimation.CurrentRectangle.Location.Y,
                chargeRect.Width, chargeRect.Height);
            chargeanimation.CurrentAnimation.CurrentRectangle = chargeRect;
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

            foreach (AnimatedSprite sprite in playerprojectiles)
            {
                sprite.Draw(spriteBatch);
            }

            spriteBatch.End();

            spriteBatch.Begin();
         
            lifemeteranimation.Draw(spriteBatch);
            staminaanimation.Draw(spriteBatch);
            chargeanimation.Draw(spriteBatch);

            ControlManager.Draw(spriteBatch);
            
            spriteBatch.End();
            base.Draw(gameTime);
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

       

        public void SetPlayerPosition(int x, int y)
        {
            player.SetSpritePositionInGameWorld(new Vector2(x, y));
        }
    }
}
