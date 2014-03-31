using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using TileEngine;
using TileEngine.Tiles;

namespace TileGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        TileMap tileMap = new TileMap();
        Camera camera = new Camera();
        
        AnimatedSprite sprite;

        List<AnimatedSprite> npcs = new List<AnimatedSprite>();
        List<AnimatedSprite> renderList = new List<AnimatedSprite>();

        Comparison<AnimatedSprite> renderSort = new Comparison<AnimatedSprite>(renderSpriteCompare);
        static int renderSpriteCompare(AnimatedSprite a, AnimatedSprite b)
        {
            return a.Origin.Y.CompareTo(b.Origin.Y);
        }

        

        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.graphics.PreferredBackBufferWidth = 1600;
            this.graphics.PreferredBackBufferHeight = 900;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            FrameAnimation down = new FrameAnimation(2, 50, 80, 50, 0);
            sprite.Animations.Add("Down", down);

            FrameAnimation left = new FrameAnimation(2, 50, 80, 50, 80);
            sprite.Animations.Add("Left", left);


            FrameAnimation right = new FrameAnimation(2, 50, 80, 50, 160);
            sprite.Animations.Add("Right", right);


            FrameAnimation up = new FrameAnimation(2, 50, 80, 50, 240);
            sprite.Animations.Add("Up", up);

            FrameAnimation idledown = new FrameAnimation(1, 50, 80, 150, 0);
            sprite.Animations.Add("IdleDown", idledown);

            FrameAnimation idleleft = new FrameAnimation(1, 50, 80, 150, 80);
            sprite.Animations.Add("IdleLeft", idleleft);


            FrameAnimation idleright = new FrameAnimation(1, 50, 80, 150, 160);
            sprite.Animations.Add("IdleRight", idleright);


            FrameAnimation idleup = new FrameAnimation(1, 50, 80, 150, 240);
            sprite.Animations.Add("IdleUp", idleup);


            Random rand = new Random();

            foreach (AnimatedSprite s in npcs)
            {
                s.Origionoffset = new Vector2(25, 80);

                s.Animations.Add("IdleUp", (FrameAnimation)idleup.Clone());
                s.Animations.Add("IdleDown", (FrameAnimation)idledown.Clone());
                s.Animations.Add("IdleLeft", (FrameAnimation)idleleft.Clone());
                s.Animations.Add("IdleRight", (FrameAnimation)idleright.Clone());

                int animation = rand.Next(3);

                switch (animation)
                { 
                    case 0:
                        s.CurrentAnimationName = "IdleUp";
                        break;
                    case 1:
                        s.CurrentAnimationName = "IdleDown";
                        break;
                    case 2:
                        s.CurrentAnimationName = "IdleLeft";
                        break;
                    case 3:
                        s.CurrentAnimationName = "IdleRight";
                        break;
                }

                s.Position = new Vector2(
                    rand.Next(600),rand.Next(400));
            }

            sprite.CurrentAnimationName = "Down";

            renderList.Add(sprite);
            renderList.AddRange(npcs);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            tileMap.Layers.Add(TileLayer.FromFile(Content, "Content/Layers/Layer1.layer"));
            tileMap.CollisionLayer = CollisionLayer.FromFile("Content/Layers/Collision.layer");
            
            sprite = new AnimatedSprite(Content.Load<Texture2D>("Sprite/Human"));
            sprite.Origionoffset = new Vector2(25, 80);

            npcs.Add(new AnimatedSprite(Content.Load<Texture2D>("Sprite/human")));
            npcs.Add(new AnimatedSprite(Content.Load<Texture2D>("Sprite/human")));
            npcs.Add(new AnimatedSprite(Content.Load<Texture2D>("Sprite/human")));
            npcs.Add(new AnimatedSprite(Content.Load<Texture2D>("Sprite/human")));
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState keyState = Keyboard.GetState();
            Vector2 motion = Vector2.Zero;

            //GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            //motion = new Vector2(gamePadState.ThumbSticks.Left.X, -gamePadState.ThumbSticks.Left.Y);

            if (keyState.IsKeyDown(Keys.W))
                motion.Y--;
            if (keyState.IsKeyDown(Keys.S))
                motion.Y++;
            if (keyState.IsKeyDown(Keys.A))
                motion.X--;
            if (keyState.IsKeyDown(Keys.D))
                motion.X++;

            if (motion != Vector2.Zero)
            {
                
                motion.Normalize();                 //Comment out to use gamePad

                motion = CheckCollisionForMotion(motion,sprite);

                sprite.Position += motion * sprite.Speed;
                UpdateSpriteAnimation(motion);
                sprite.isAnimating = true;

                CheckForUnwalkebleTile(sprite);

            }

            else
            {
                UpdateSpriteIdleAnimation(sprite);
                sprite.isAnimating = false;
                motion = new Vector2(0, 0);
                
            }

            sprite.ClampToArea(tileMap.GetWidthInPixels(), tileMap.GetHeightInPixels());
            
            sprite.Update(gameTime);

            foreach (AnimatedSprite s in npcs)
            {
                s.Update(gameTime);

                if (AnimatedSprite.AreColliding(sprite, s))
                {
                    Vector2 d = Vector2.Normalize(s.Origin - sprite.Origin);
                    sprite.Position = 
                        s.Position - (d * (sprite.CollisionRadius + s.CollisionRadius)); 
                }
            }

            int screenWidth = GraphicsDevice.Viewport.Width;
            int screenHeight = GraphicsDevice.Viewport.Height;

            camera.LockToTarget(sprite, screenWidth, screenHeight);
            
            camera.ClampToArea(
                tileMap.GetWidthInPixels() - screenWidth, 
                tileMap.GetHeightInPixels() - screenHeight);

            base.Update(gameTime);
        }
        #region Sprite Animation Code
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


            if (motionAngle >= -MathHelper.PiOver4 && motionAngle <= MathHelper.PiOver4)
            {
                sprite.CurrentAnimationName = "Right";
                //motion = new Vector2(1f, 0f);
            }
            else if (motionAngle >= MathHelper.PiOver4 && motionAngle <= 3f * MathHelper.PiOver4)
            {
                sprite.CurrentAnimationName = "Down";
                //motion = new Vector2(0f, 1f);
            }
            else if (motionAngle <= -MathHelper.PiOver4 && motionAngle >= -3f * MathHelper.PiOver4)
            {
                sprite.CurrentAnimationName = "Up";
                //motion = new Vector2(0f, -1f);
            }
            else
            {
                sprite.CurrentAnimationName = "Left";
                //motion = new Vector2(-1f, 0f);
            }

        }
        #endregion

        #region Code for Collision with terrain

        #region Code for Unwalkeble Terrain
        private void CheckForUnwalkebleTile(AnimatedSprite sprite)
        {
            Point spriteCell = Engine.ConvertPostionToCell(sprite.Origin);

            Point? upLeft = null, up = null, upRight = null,
                left = null, right = null,
                downLeft = null, down = null, downRight = null;

            if (spriteCell.Y > 0)
                up = new Point(spriteCell.X, spriteCell.Y -1);
            
            if (spriteCell.Y < tileMap.CollisionLayer.Height - 1)
                down = new Point(spriteCell.X, spriteCell.Y + 1);

            if (spriteCell.X > 0)
                left = new Point(spriteCell.X - 1, spriteCell.Y);

            if (spriteCell.X < tileMap.CollisionLayer.Width - 1)
                right = new Point(spriteCell.X + 1, spriteCell.Y);

            if (spriteCell.X > 0 && spriteCell.Y > 0)
                upLeft = new Point(spriteCell.X - 1, spriteCell.Y -1);
            
            if (spriteCell.X < tileMap.CollisionLayer.Width -1 && spriteCell.Y > 0)
                upRight = new Point(spriteCell.X + 1, spriteCell.Y - 1);
            
            if (spriteCell.X > 0 && spriteCell.Y < tileMap.CollisionLayer.Height - 1)
                downLeft = new Point(spriteCell.X - 1, spriteCell.Y + 1);

            if (spriteCell.X < tileMap.CollisionLayer.Width - 1 &&
                spriteCell.Y < tileMap.CollisionLayer.Height - 1)
                downRight = new Point(spriteCell.X + 1, spriteCell.Y + 1);
                
            
            
            if (up !=null && tileMap.CollisionLayer.GetCellIndex(up.Value) == 1)
            {
                
                Rectangle cellRect = Engine.CreateRectForCell(up.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {


                    sprite.Position.Y = up.Value.Y * Engine.TileHeight + sprite.Bounds.Height ;
                
                }

            }
            if (down != null && tileMap.CollisionLayer.GetCellIndex(down.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(down.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.Y = down.Value.Y * Engine.TileHeight - sprite.Bounds.Height;

                }
            
            }
            if (left != null && tileMap.CollisionLayer.GetCellIndex(left.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(left.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.X = left.Value.X * Engine.TileWidth  ;
                    

                }

            }
            if (right != null && tileMap.CollisionLayer.GetCellIndex(right.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(right.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.X = right.Value.X * Engine.TileWidth - sprite.Bounds.Width ;


                }

            }

            if (upLeft != null && tileMap.CollisionLayer.GetCellIndex(upLeft.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(upLeft.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.X = spriteCell.X * Engine.TileWidth ;
                    sprite.Position.Y = spriteCell.Y * Engine.TileHeight - sprite.Bounds.Height;
                }
            }

            if (upRight != null && tileMap.CollisionLayer.GetCellIndex(upRight.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(upRight.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.X = spriteCell.X * Engine.TileWidth;
                    sprite.Position.Y = spriteCell.Y * Engine.TileHeight - sprite.Bounds.Height ;
                }
            }

            if (downLeft != null && tileMap.CollisionLayer.GetCellIndex(downLeft.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(downLeft.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.X = left.Value.X * Engine.TileWidth + sprite.Bounds.Width ;
                    sprite.Position.Y = down.Value.Y * Engine.TileHeight + sprite.Bounds.Height ;
                }
            }

            if (downRight != null && tileMap.CollisionLayer.GetCellIndex(downRight.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(downRight.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.X = right.Value.X * Engine.TileWidth - sprite.Bounds.Width;
                    sprite.Position.Y = down.Value.Y * Engine.TileHeight - sprite.Bounds.Height;
                }
            }
        }
        #endregion

        private Vector2 CheckCollisionForMotion(Vector2 motion, AnimatedSprite sprite)
        {
            Point cell = Engine.ConvertPostionToCell(sprite.Origin);

            int colIndex = tileMap.CollisionLayer.GetCellIndex(cell);

            if (colIndex == 2)
                return motion * .2f;

            return motion;
        }



        #endregion

        

        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            tileMap.Draw(spriteBatch, camera);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                null, null, null, null, camera.TransforMatrix);
            
            renderList.Sort(renderSort);

            foreach (AnimatedSprite sprite in renderList)
                sprite.Draw(spriteBatch);
            
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
