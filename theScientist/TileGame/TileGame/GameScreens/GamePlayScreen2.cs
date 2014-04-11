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
using TileEngine.Sprite;
using XtheSmithLibrary;
using TileEngine;
using TileEngine.Tiles;

namespace TileGame.GameScreens
{
    public class GamePlayScreen2 : PlayerScreen
    {
        
        #region Field Region

        string name;
        bool gate1Locked = true;
        bool gate2Locked = true;

        #endregion
        #region Property Region

        //GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //TileMap tileMap = new TileMap();
        //Camera camera = new Camera();

        Sprite sprite;
        //PlayerCharacter player;

        List<BaseSprite> SpriteObjects = new List<BaseSprite>();
        List<BaseSprite> SpriteObjectInGameWorld = new List<BaseSprite>();
        //List<BaseSprite> renderList = new List<BaseSprite>();

        //Comparison<BaseSprite> renderSort = new Comparison<BaseSprite>(renderSpriteCompare);

        public string Name { get { return name; } }
        public bool Gate1Locked { get { return gate1Locked; } set { this.gate1Locked = value; } }
        public bool Gate2Locked { get { return gate2Locked; } set { this.gate2Locked = value; } }

        #endregion

        //static int renderSpriteCompare(BaseSprite a, BaseSprite b)
        //{
            //return a.Origin.Y.CompareTo(b.Origin.Y);
        //}

        #region Constructor Region
        public GamePlayScreen2(Game game, GameStateManager manager, String name)
            : base(game, manager)
        {
            
            //graphics = new GraphicsDeviceManager(this);
            //Microsoft.Xna.Framework.Content.RootDirectory = "Content";
            this.name = name;
        }

        #endregion 

        #region XNA Method Region
        public override void Initialize()
        {
            base.Initialize();

            //FrameAnimation down = new FrameAnimation(1, 32, 32, 0, 0);
            //if (!player.Animations.ContainsKey("Down"))
            //    player.Animations.Add("Down", down);

            //FrameAnimation right = new FrameAnimation(1, 32, 32, 32, 0);
            //if (!player.Animations.ContainsKey("Right"))
            //    player.Animations.Add("Right", right);

            //FrameAnimation up = new FrameAnimation(1, 32, 32, 64, 0);
            //if (!player.Animations.ContainsKey("Up"))
            //    player.Animations.Add("Up", up);

            //FrameAnimation left = new FrameAnimation(1, 32, 32, 96, 0);
            //if (player.Animations.ContainsKey("Left"))
            //    player.Animations.Add("Left", left);

            //player.CurrentAnimationName = "Down";
            //renderList.Add(player);
            
            SpriteObjectInGameWorld.Clear();
            renderList.Clear();
            renderList.Add(player);
            SpriteObjectInGameWorld.AddRange(SpriteObjects);
            renderList.AddRange(SpriteObjects);
            
            
            
            
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ContentManager Content = Game.Content;
            
            tileMap.Layers.Add(TileLayer.FromFile(Content, "Content/Layers/SecondLand.layer"));
            tileMap.CollisionLayer = CollisionLayer.ProcessFile("Content/Layers/SecondLandCollision.layer");

            sprite = new Sprite(Content.Load<Texture2D>("Sprite/playerbox"));
            sprite.Origionoffset = new Vector2(15, 15);
            sprite.SetSpritePositionInGameWorld(new Vector2(10, 10));
            SpriteObjects.Add(sprite);
            

            //player = new PlayerCharacter(Content.Load<Texture2D>("Sprite/playerboxAnimation"));
            //player.Origionoffset = new Vector2(15, 15);
            //player.SetSpritePositionInGameWorld(new Vector2(1, 2));
            //player.Life = 100;

            

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
           

            foreach (BaseSprite s in SpriteObjectInGameWorld)
            {
                s.Update(gameTime);

                if (BaseSprite.AreColliding(player, s))
                {
                    Vector2 d = Vector2.Normalize(s.Origin - player.Origin);
                    player.Position =
                        s.Position - (d * (player.CollisionRadius + s.CollisionRadius));
                }
            }


            for (int Sprite = 0; Sprite < SpriteObjectInGameWorld.Count(); Sprite++)
            {
                for (int Projectile = 0; Projectile < playerprojectiles.Count(); Projectile++)
                {
                    if (BaseSprite.AreColliding(playerprojectiles[Projectile], SpriteObjectInGameWorld[Sprite]))
                    {
                        renderList.Remove(SpriteObjectInGameWorld[Sprite]);
                        SpriteObjectInGameWorld.RemoveAt(Sprite);
                        playerprojectiles.RemoveAt(Projectile);

                    }
                }
            }

            Point cell = Engine.ConvertPostionToCell(player.Origin);
            if ((cell.X == 28 && cell.Y == 28) && !gate2Locked)
            {
                GameRef.GamePlayScreen.SetPlayerPosition(4, 3);
                GameRef.GamePlayScreen.Gate1Locked = true;
                StateManager.ChangeState(GameRef.GamePlayScreen);

            }
            if (cell.X != 28 || cell.Y != 28)
                gate2Locked = false;

            if ((cell.X == 1 && cell.Y == 2) && !gate1Locked)
            {
                GameRef.GamePlayScreen.SetPlayerPosition(17, 14);
                GameRef.GamePlayScreen.Gate2Locked = true;
                StateManager.ChangeState(GameRef.GamePlayScreen);

            }
            if (cell.X != 1 || cell.Y != 2)
                gate1Locked = false;

            //base.Update(gameTime);

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            tileMap.Draw(spriteBatch, camera);

            //spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
            //    null, null, null, null, camera.TransforMatrix);

            //renderList.Sort(renderSort);

            //foreach (BaseSprite sprite in renderList)
            //    sprite.Draw(spriteBatch);

            //spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion

        #region Abstract Method Region
        #endregion

       
    }
}
