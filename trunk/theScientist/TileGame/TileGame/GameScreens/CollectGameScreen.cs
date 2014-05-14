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
    public class CollectGameScreen : PlayerScreen
    {
        #region Field Region

        string name;
        bool gate1Locked = true;
        bool gate2Locked = true;

        //static public float playerPoints;
        //static public float npcPoints;

        #endregion
        #region Property Region

        SpriteBatch spriteBatch;
        private ContentManager Content;
        public FruitForMiniGameSprite fruit;
        LumberJackJohnny johnny;
        MoleHandlerSprite molehandler;
        private float elapsedSlow;
        private float delaySlow;
        private bool SlowFlag;

        private float elapsedStart;
        private float delayStart;
        private bool StartFlag;
        Sprite sprite;

        List<BaseSprite> SpriteObjects = new List<BaseSprite>();
        List<BaseSprite> SpriteObjectInGameWorld = new List<BaseSprite>();
        List<Sprite> BombSprites = new List<Sprite>();
        List<AnimatedSprite> Explosions = new List<AnimatedSprite>();
        
        public string Name { get { return name; } }
        public bool Gate1Locked { get { return gate1Locked; } set { this.gate1Locked = value; } }
        public bool Gate2Locked { get { return gate2Locked; } set { this.gate2Locked = value; } }

        #endregion

        #region Constructor Region
        public CollectGameScreen(Game game, GameStateManager manager, String name)
            : base(game, manager)
        {
            this.name = name;
        }

        #endregion 

        #region XNA Method Region
        public override void Initialize()
        {
            base.Initialize();

            player.SetSpritePositionInGameWorld(new Vector2(32, 28));

            SlowFlag = false;
            elapsedSlow = 0.0f;
            delaySlow = 2000f;
            StartFlag = true;
            delayStart = 10000f;
            elapsedStart = 0.0f;

            SpriteObjectInGameWorld.Clear();
            renderList.Clear();
            renderList.Add(player);
            renderList.Add(fruit);
            renderList.Add(johnny);
            renderList.Add(molehandler);
            SpriteObjectInGameWorld.Add(molehandler);
            SpriteObjectInGameWorld.Add(johnny);
            SpriteObjectInGameWorld.Add(fruit);
            SpriteObjectInGameWorld.AddRange(SpriteObjects);
            renderList.AddRange(SpriteObjects);


            //StoryProgress.ProgressLine["CollectMinigame"] = false;
            
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Content = Game.Content;

            tileMap.Layers.Add(TileLayer.FromFile(Content, "Content/Layers/CollectMinigameGround.layer"));
            tileMap.Layers.Add(TileLayer.FromFile(Content, "Content/Layers/CollectMinigameBack.layer"));
            tileMap.Layers.Add(TileLayer.FromFile(Content, "Content/Layers/CollectMinigameMiddle.layer"));
            tileMap.Layers.Add(TileLayer.FromFile(Content, "Content/Layers/CollectMinigameFront.layer"));
            tileMap.CollisionLayer = CollisionLayer.ProcessFile("Content/Layers/CollectMinigameCollision.layer");
            fruit = new FruitForMiniGameSprite(Content.Load<Texture2D>("Sprite/Bjorn_Try_Fruit"),GameRef.random);
            fruit.Origionoffset = new Vector2(16, 16);
            johnny = new LumberJackJohnny(Content.Load<Texture2D>("Sprite/Bjorn_Try_Johnny"), GameRef.random);
            johnny.Origionoffset = new Vector2(25, 65);
            molehandler = new MoleHandlerSprite(null, GameRef.random);
            
            //--
            //lockedGateDict = new Dictionary<int,bool>();
            ////lockedGateDict[40] = true;
            ////lockedGateDict[41] = true;
            //lockedGateDict[42] = true;
            //--

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            if (StartFlag)
            {
                elapsedStart += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                player.Speed = 0.0f;
                if (elapsedStart > delayStart)
                {
                    johnny.StopSearch = false;
                    this.StartFlag = false;
                }
            }
            else
            {
                if (SlowFlag)
                {
                    elapsedSlow += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    player.Speed = 1.0f;
                    if (elapsedSlow > delaySlow)
                    {
                        SlowFlag = false;
                        elapsedSlow = 0.0f;
                    }
                }
                else
                    player.Speed = 3.5f;
            }
        
                if (!StartFlag)
                {
                    if (molehandler.SpawnFlag)
                    {
                        MoleSprite mole = new MoleSprite(Content.Load<Texture2D>("Sprite/Bjorn_Try_Mole"),
                            new Vector2(GameRef.random.Next(10, 55) * 32, GameRef.random.Next(10, 40) * 32));
                        mole.Origionoffset = new Vector2(16, 40);
                        molehandler.SpawnFlag = false;
                        molehandler.Moles.Add(mole);
                        SpriteObjectInGameWorld.Add(mole);
                        renderList.Add(mole);
                    }
                }
                foreach (Explosion explosion in Explosions)
                {
                    explosion.UpdateExplosion(gameTime);
                    if (explosion.Bounds.Intersects(new Rectangle(player.Bounds.X + 10, player.Bounds.Y + 10, 30, 50)))
                    {
                        SlowFlag = true;
                        explosion.Finished = true;
                    }
                    else if (explosion.Bounds.Intersects(new Rectangle(johnny.Bounds.X + 10, johnny.Bounds.Y + 10, 30, 50)))
                    {
                        johnny.HitFlag = true;
                        explosion.Finished = true;
                    }
                    if (explosion.Finished)
                    {
                        renderList.Remove(explosion);
                        Explosions.Remove(explosion);
                        break;
                    }
                }
                foreach (BombSprite bomb in BombSprites)
                {
                    bomb.UpdateBomb(gameTime);
                    if (bomb.Bounds.Intersects(new Rectangle(player.Bounds.X + 10, player.Bounds.Y + 10, 30, 50)))
                    {
                        bomb.Boom = true;
                    }
                    if (bomb.Bounds.Intersects(new Rectangle(johnny.Bounds.X + 10, johnny.Bounds.Y + 10, 30, 50)))
                    {
                        johnny.HitFlag = true;
                        bomb.Boom = true;
                    }
                    if (bomb.Boom)
                    {

                        for (int i = 0; i < 4; i++)
                        {
                            Explosion explosion = new Explosion(Content.Load<Texture2D>("Sprite/Bjorn_Try_Explosion"),
                                bomb.Position, new Vector2(GameRef.random.Next(-100, 100) / 100f, GameRef.random.Next(-100, 100) / 100f));
                            Explosions.Add(explosion);
                            renderList.Add(explosion);
                        }
                        int randomnumber = GameRef.random.Next(1, 11);
                        if (randomnumber <= 5)
                        {
                            MoleSprite mole = new MoleSprite(Content.Load<Texture2D>("Sprite/Bjorn_Try_Mole"),
                                bomb.Position);
                            mole.Origionoffset = new Vector2(16, 40);
                            molehandler.Moles.Add(mole);
                            SpriteObjectInGameWorld.Add(mole);
                            renderList.Add(mole);
                        }
                        BombSprites.Remove(bomb);
                        renderList.Remove(bomb);
                        break;
                    }

                }
                foreach (MoleSprite mole in molehandler.Moles)
                {
                    mole.SearchTowardsTarget(player);
                    if (mole.ThrowBomb)
                    {
                        BombSprite bomb;
                        int randomnumber = GameRef.random.Next(1, 11);
                        if(randomnumber <= 8)
                            bomb = new BombSprite(Content.Load<Texture2D>("Sprite/Bjorn_Try_Bomb"),
                                        new Vector2(player.Origin.X, player.Origin.Y - 40), new Vector2(mole.Position.X + 15, mole.Position.Y + 23));
                        else
                            bomb = new BombSprite(Content.Load<Texture2D>("Sprite/Bjorn_Try_Bomb"),
                                        new Vector2(johnny.Origin.X, johnny.Origin.Y - 40), new Vector2(mole.Position.X + 15, mole.Position.Y + 23));
                        bomb.potatoExplosionSound = Content.Load<SoundEffect>(@"Sounds/Effects/explosion");
                        BombSprites.Add(bomb);
                        renderList.Add(bomb);

                        mole.ThrowBomb = false;
                    }
                    if (mole.Finished)
                    {
                        molehandler.Moles.Remove(mole);
                        SpriteObjectInGameWorld.Remove(mole);
                        renderList.Remove(mole);
                        break;
                    }
                }

                fruit.CheckForContactWithPlayerOrNPC(player, johnny);
                johnny.SearchTowardsTarget(gameTime, fruit);
                CollisionWithTerrain.CheckForCollisionAroundSprite(johnny, johnny.TargetedPosition, this);
                johnny.SlowWalk = CollisionWithTerrain.CheckCollisionForMotionBool(johnny, this);
            
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
            //int cellIndex = tileMap.CollisionLayer.GetCellIndex(cell);
            //if (cellIndex >= 40 && cellIndex < 50)
            //{
            //    GateToNextScreen(cellIndex, GameRef.PotatoTown, "G2");
            //}
            //UnlockGate(cellIndex);

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
            //spriteBatch.End();
        }

        #endregion

        #region Method Region

        private void CheckGameFinished()
        {
            if (FruitForMiniGameSprite.npcPoints > 1000)
            {
                
            }
            if (FruitForMiniGameSprite.playerPoints > 1000)
            {

            }
        }

        #endregion
    }
}
