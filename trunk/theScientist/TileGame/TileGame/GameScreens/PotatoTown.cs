﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
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
using TileEngine.Sprite.Npc;
using TileEngine.Sprite.Npc.NPC_Story;
using TileEngine.Sprite.Npc.NPC_Fighting;
using XtheSmithLibrary;
using TileEngine;
using TileEngine.Tiles;
using TileGame.Collision;
using XtheSmithLibrary.Controls;

namespace TileGame.GameScreens
{
    public class PotatoTown : PlayerScreen
    {
        
        #region Field Region

        string name;
        bool gate1Locked = true;
        bool gate2Locked = true;
        private ContentManager Content;

        #endregion
        #region Property Region

        //GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
                
        Sprite sprite, sprite1;
        AnimatedSprite NPC1, NPC2;
        public NPC_Story npcstory, npcstory2;
        AnimatedSprite NPC_Farmer_1;
        public NPC_Story npc;
        public List<NPC_Story> NpcStoryList = new List<NPC_Story>(); 
        protected Rectangle rectangle;
        protected Dialog dialog ;
        private GraphicsDeviceManager graphics;

        CollisionWithCharacter CollisionWithCharacter = new CollisionWithCharacter();
       
        List<BaseSprite> SpriteObject = new List<BaseSprite>();
        List<BaseSprite> AnimatedSpriteObject = new List<BaseSprite>();
        List<BaseSprite> SpriteObjectInGameWorld = new List<BaseSprite>();
        List<AnimatedProjectile> NPCProjectile = new List<AnimatedProjectile>();
        List<AnimatedSprite> NPCFightingFarmer = new List<AnimatedSprite>();
        

        public string Name { get { return name; } }

        public bool Gate1Locked { get { return gate1Locked; } set { this.gate1Locked = value; } }
        public bool Gate2Locked { get { return gate2Locked; } set { this.gate2Locked = value; } }

        #endregion


        #region Constructor Region
        public PotatoTown(Game game, GameStateManager manager, string name)
            : base(game, manager)
        {
            //graphics = new GraphicsDeviceManager(Game);
            //Microsoft.Xna.Framework.Content.RootDirectory = "Content";
            this.name = name;
        }

        #endregion 

        #region XNA Method Region
        public override void Initialize()
        {
            base.Initialize();
            
            FrameAnimation down = new FrameAnimation(1, 50, 80, 0, 0);
            FrameAnimation right = new FrameAnimation(1, 50, 80, 0, 160);
            FrameAnimation up = new FrameAnimation(1, 50, 80, 0, 240);
            FrameAnimation left = new FrameAnimation(1, 50, 80, 0, 80);
            
            foreach(AnimatedSprite s in AnimatedSpriteObject)
            {
                if (!s.Animations.ContainsKey("Up"))
                    s.Animations.Add("Up", (FrameAnimation)up.Clone());
                if (!s.Animations.ContainsKey("Down"))
                    s.Animations.Add("Down", (FrameAnimation)down.Clone());
                if (!s.Animations.ContainsKey("Left"))
                    s.Animations.Add("Left", (FrameAnimation)left.Clone());
                if (!s.Animations.ContainsKey("Right"))
                    s.Animations.Add("Right", (FrameAnimation)right.Clone());

                s.CurrentAnimationName = "Down";
            }
            
            SpriteObjectInGameWorld.Clear();
            renderList.Clear();
            renderList.Add(player);
            SpriteObjectInGameWorld.AddRange(AnimatedSpriteObject);
            SpriteObjectInGameWorld.AddRange(SpriteObject);            
            SpriteObjectInGameWorld.AddRange(NPCFightingFarmer);
            renderList.AddRange(SpriteObject);
            renderList.AddRange(AnimatedSpriteObject);
            renderList.AddRange(NPCFightingFarmer);
            
            
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Content = Game.Content;
            
            
            tileMap.Layers.Add(TileLayer.FromFile(Content, "Content/Layers/testGround.layer"));
            tileMap.Layers.Add(TileLayer.FromFile(Content, "Content/Layers/testBack.layer"));
            tileMap.Layers.Add(TileLayer.FromFile(Content, "Content/Layers/testMiddle.layer"));
            tileMap.Layers.Add(TileLayer.FromFile(Content, "Content/Layers/testFront.layer"));
            tileMap.CollisionLayer = CollisionLayer.ProcessFile("Content/Layers/testCollision.layer");

            //En Sprite i världen som återskapas vid entry.
            sprite = new Sprite(Content.Load<Texture2D>("Sprite/playerbox"));
            sprite.Origionoffset = new Vector2(15, 15);
            sprite.SetSpritePositionInGameWorld(new Vector2(5, 5));
            SpriteObject.Add(sprite);
            
            sprite1 = new Sprite(Content.Load<Texture2D>("Sprite/playerbox"));
            sprite1.Origionoffset = new Vector2(15, 15);
            sprite1.SetSpritePositionInGameWorld(new Vector2(10, 10));
            SpriteObject.Add(sprite1);

            NPC1 = new AnimatedSprite(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"));           
            NPC1.Origionoffset = new Vector2(25,65);
            NPC1.SetSpritePositionInGameWorld(new Vector2(9,9));
            NPC1.Life = 30;
            NPC1.FullHp = 30;
            AnimatedSpriteObject.Add(NPC1);

            NPC2 = new AnimatedSprite(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"));
            NPC2.Origionoffset = new Vector2(25, 65);
            NPC2.SetSpritePositionInGameWorld(new Vector2(12, 12));
            NPC2.Life = 5;
            NPC2.FullHp = 5;
            AnimatedSpriteObject.Add(NPC2);

            for (int i = 0; i < 15; i++)
            {
                NPC_Fighting_Farmer NPC_Farmer = new NPC_Fighting_Farmer(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"), null, GameRef.random);
                NPC_Farmer.Origionoffset = new Vector2(25, 65);
                NPC_Farmer.SetSpritePositionInGameWorld(new Vector2(5 + i, 22));
                NPC_Farmer.Life = 5;
                NPC_Farmer.FullHp = 5;
                AnimatedSpriteObject.Add(NPC_Farmer);
                NPCFightingFarmer.Add(NPC_Farmer);

            }

            npcstory = new NPC_Story(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"), Content.Load<Script>("Scripts/npc1"), Content.Load<Texture2D>("CharacterPotraits/pimp-bender"), "Jack");
            npcstory.Origionoffset = new Vector2(25, 65);
            npcstory.SetSpritePositionInGameWorld(new Vector2(16, 16));
            AnimatedSpriteObject.Add(npcstory);
            NpcStoryList.Add(npcstory);

            npcstory2 = new NPC_Story(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"), Content.Load<Script>("Scripts/AsterixDialog"), Content.Load<Texture2D>("CharacterPotraits/asterix"), "Asterix");
            npcstory2.Origionoffset = new Vector2(25, 65);
            npcstory2.SetSpritePositionInGameWorld(new Vector2(20, 17));
            AnimatedSpriteObject.Add(npcstory2);
            NpcStoryList.Add(npcstory2);

            //--
            lockedGateDict = new Dictionary<int,bool>();
            lockedGateDict[40] = true;
            lockedGateDict[41] = true;
            //--

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            CollisionWithCharacter.UpdateCollisionForCharacters(gameTime, SpriteObjectInGameWorld,  player,  SpriteObject,  playerprojectiles,  renderList,  AnimatedSpriteObject);
            foreach(NPC_Fighting_Farmer npc in NPCFightingFarmer)
            {
                //CollisionWithCharacter.UpdateCollisionForCharacters(
                //    gameTime, SpriteObjectInGameWorld, npc,
                //    SpriteObject, playerprojectiles, renderList, AnimatedSpriteObject);
                if(npc.Motion != Vector2.Zero)
                {
                    npc.Motion.Normalize();
                    npc.Collided = CollisionWithTerrain.CheckForCollisionAroundSprite(npc, npc.Motion, this);

                }
                //Aggro range
                if (Vector2.Distance(npc.Position, player.Position) < 100 && !npc.Aggro && !npc.Running && !npc.HitFlag)
                {
                    npc.Aggro = true;
                    npc.AttackersDirection = player.Position - npc.Position; 
                }
                if(npc.Running)
                    npc.CheckForCollisionWithOtherNPCs(NPCFightingFarmer,player);

            }
            foreach (var npc in NpcStoryList)
            {
            if (npc.InSpeakingRange(player))
            {
                if (npc.canTalk == true)
                {
                    if (InputHandler.KeyReleased(Keys.Space))
                    {
                        if (ActiveConversation == false)
                        {
                                PlayerStartConversation(npc);
                        }
                    }
                }
                else
                {
                    if (InputHandler.KeyReleased(Keys.Space))
                    {
                            PlayerEndConversation(npc);
                        }
                    }
                }
            }
            
            Point cell = Engine.ConvertPostionToCell(player.Origin);
            int cellIndex = tileMap.CollisionLayer.GetCellIndex(cell);
            if (cellIndex >= 40 && cellIndex < 50)
            {
                GateToNextScreen(cellIndex, GameRef.GamePlayScreen2, "G0");
                
                GateToNextScreen(cellIndex, GameRef.GamePlayScreen2, "G1");  
            }
            UnlockGate(cellIndex);


            base.Update(gameTime);
        }

        
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            tileMap.Draw(spriteBatch, camera);


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
            //if (sprite.CurrentAnimationName == "Down")
            //    sprite.CurrentAnimationName = "Down";
            //if (sprite.CurrentAnimationName == "Left")
            //    sprite.CurrentAnimationName = "Left";
            //if (sprite.CurrentAnimationName == "Right")
            //    sprite.CurrentAnimationName = "Right";
            //if (sprite.CurrentAnimationName == "Up")
            //    sprite.CurrentAnimationName = "Up";
        }

        private void UpdateSpriteAnimation(Vector2 motion)
        {
            //float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

            //if (motionAngle >= -MathHelper.PiOver4 && motionAngle <= MathHelper.PiOver4)
            //{
            //    player.CurrentAnimationName = "Right"; //Right
            //    //motion = new Vector2(1f, 0f);
            //}
            //else if (motionAngle >= MathHelper.PiOver4 && motionAngle <= 3f * MathHelper.PiOver4)
            //{
            //    player.CurrentAnimationName = "Down"; //Down
            //    //motion = new Vector2(0f, 1f);
            //}
            //else if (motionAngle <= -MathHelper.PiOver4 && motionAngle >= -3f * MathHelper.PiOver4)
            //{
            //    player.CurrentAnimationName = "Up"; // Up
            //    //motion = new Vector2(0f, -1f);
            //}
            //else
            //{
            //    player.CurrentAnimationName = "Left"; //Left
            //    //motion = new Vector2(-1f, 0f);
            //}
        }
        #endregion

        
    }
}
