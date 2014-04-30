using System;
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
using TileEngine.Sprite;
using TileEngine.Sprite.Npc;
using TileEngine.Sprite.Npc.NPC_Neutral;
using TileEngine.Sprite.Npc.NPC_Story;
using TileEngine.Sprite.Npc.NPC_Fighting;
using XtheSmithLibrary;
using TileEngine;
using TileEngine.Tiles;
using TileEngine.AI;
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

        //private MultiIronSprite multiIronOre;
        public Sprite treeStanding;
        public Sprite treeBridge;
        public Sprite treeStubbe;

        #endregion

        #region Property Region

        //GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
                
        Sprite sprite, sprite1;
        AnimatedSprite NPC1, NPC2;
        public NPC_Story npcstory, npcStoryAsterix;//npcstory2;
        NPC_Fighting_Stationary NPC_Guard_1;
        NPC_Fighting_Stationary NPC_Guard_2;
   
        public NPC_Story npc;
        public NPC_Story_GuardCaptain guard;
        public NPC_Neutral_Townsfolk npcNeutral;

        public List<NPC_Neutral_Townsfolk> NpcNeutralList = new List<NPC_Neutral_Townsfolk>(); 
        public List<NPC_Story> NpcStoryList = new List<NPC_Story>(); 
        protected Rectangle rectangle;
        private GraphicsDeviceManager graphics;

        CollisionWithCharacter CollisionWithCharacter = new CollisionWithCharacter();
       
        List<BaseSprite> SpriteObject = new List<BaseSprite>();
        List<BaseSprite> AnimatedSpriteObject = new List<BaseSprite>();
        public List<BaseSprite> SpriteObjectInGameWorld = new List<BaseSprite>();
        List<AnimatedProjectile> NPCProjectile = new List<AnimatedProjectile>();
        List<AnimatedSprite> NPCFightingFarmers = new List<AnimatedSprite>();
        List<AnimatedSprite> NPCPatrollingGuards = new List<AnimatedSprite>();
        List<AnimatedSprite> NPCStationaryGuards = new List<AnimatedSprite>();
        

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
            
            foreach(var s in AnimatedSpriteObject)
            {
                if (s is MultiIronSprite)
                {
                    MultiIronSprite mis = (MultiIronSprite)s;
            FrameAnimation all = new FrameAnimation(1, 32, 32, 0, 0);
                    if (!mis.Animations.ContainsKey("all"))
                        mis.Animations.Add("all", all);
            FrameAnimation half = new FrameAnimation(1, 32, 32, 32, 0);
                    if (!mis.Animations.ContainsKey("half"))
                        mis.Animations.Add("half", half);
                    mis.CurrentAnimationName = "all";
                }
            }
            
            SpriteObjectInGameWorld.Clear();
            renderList.Clear();
            renderList.Add(player);
            SpriteObjectInGameWorld.AddRange(AnimatedSpriteObject);
            SpriteObjectInGameWorld.AddRange(SpriteObject);            
            //SpriteObjectInGameWorld.AddRange(NPCFightingFarmers);
            //SpriteObjectInGameWorld.AddRange(NPCPatrollingGuards);
            renderList.AddRange(SpriteObject);
            renderList.AddRange(AnimatedSpriteObject);
            //renderList.AddRange(NPCFightingFarmers);
            //renderList.AddRange(NPCPatrollingGuards);
            if (StoryProgress.ProgressLine["treeIsDown"])
            {
                this.SpriteObjectInGameWorld.Remove(GameRef.GamePlayScreen.treeStanding);
                this.renderList.Remove(GameRef.GamePlayScreen.treeStanding);
                this.renderList.Add(GameRef.GamePlayScreen.treeBridge);
                this.renderList.Add(GameRef.GamePlayScreen.treeStubbe);
        }
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

            NPC_Guard_1 = new NPC_Fighting_Stationary(Content.Load<Texture2D>("Sprite/Bjorn_Try_Farmer"), null,GameRef.random, this.tileMap.CollisionLayer.Map);
            NPC_Guard_1.Origionoffset = new Vector2(25, 65);
            NPC_Guard_1.SetSpritePositionInGameWorld(new Vector2(20, 10));
            NPC_Guard_1.Life = 100;
            NPC_Guard_1.FullHp = 100;
            NPC_Guard_1.AI.GenerateTileNodes(new Vector2(20, 10));
            NPC_Guard_1.AI.GenerateNeighboursForTileNodes();
            AnimatedSpriteObject.Add(NPC_Guard_1);

            NPC_Guard_2 = new NPC_Fighting_Stationary(Content.Load<Texture2D>("Sprite/Bjorn_Try_WitchDoctor"), null, GameRef.random, this.tileMap.CollisionLayer.Map);
            NPC_Guard_2.Origionoffset = new Vector2(25, 65);
            NPC_Guard_2.SetSpritePositionInGameWorld(new Vector2(30, 11));
            NPC_Guard_2.Life = 100;
            NPC_Guard_2.FullHp = 100;
            NPC_Guard_2.AI.GenerateTileNodes(new Vector2(30, 11));
            NPC_Guard_2.AI.GenerateNeighboursForTileNodes();
            AnimatedSpriteObject.Add(NPC_Guard_2);


            for (int i = 0; i < 15; i++ )
            {
                NPC_Fighting_Patrolling NPC_Patroller = new NPC_Fighting_Patrolling(Content.Load<Texture2D>("Sprite/Bjorn_Try_Soldier"), null, GameRef.random,this.tileMap.CollisionLayer.Map);
                NPC_Patroller.Origionoffset = new Vector2(25, 65);
                NPC_Patroller.SetSpritePositionInGameWorld(new Vector2(22 + i, 50));
                NPC_Patroller.Life = 100;
                NPC_Patroller.FullHp = 100;
                //NPC_Patroller.AI.GenerateTileNodes(new Vector2(22 + i, 50));
                //NPC_Patroller.AI.GenerateNeighboursForTileNodes();
                AnimatedSpriteObject.Add(NPC_Patroller);
                NPCPatrollingGuards.Add(NPC_Patroller);
            }
            for (int i = 0; i < 15; i++)
            {
                NPC_Fighting_Farmer NPC_Farmer = new NPC_Fighting_Farmer(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"), null, GameRef.random);
                NPC_Farmer.Origionoffset = new Vector2(25, 65);
                NPC_Farmer.SetSpritePositionInGameWorld(new Vector2(5 + i, 22));
                NPC_Farmer.Life = 100;
                NPC_Farmer.FullHp = 100;
                AnimatedSpriteObject.Add(NPC_Farmer);
                NPCFightingFarmers.Add(NPC_Farmer);

            }

            for (int i = 0; i < 10; i++)
            {
                //Detta är NPC_Banditer, ska bli en egen klass senare
                NPC_Fighting_Patrolling NPC_Bandit = new NPC_Fighting_Patrolling(Content.Load<Texture2D>("Sprite/HumanNPCBandit"), null, GameRef.random, this.tileMap.CollisionLayer.Map);
                NPC_Bandit.Origionoffset = new Vector2(25, 65);
                float x = GameRef.random.Next(102, 137);
                float y = GameRef.random.Next(88, 100);
                NPC_Bandit.SetSpritePositionInGameWorld(new Vector2(x, y));
                NPC_Bandit.Life = 100;
                NPC_Bandit.FullHp = 100;
                NPC_Bandit.AggroCircle = 600; //Hur långt han följer
                NPC_Bandit.AggroRange = 300; //Hur långt ifrån du blir upptäckt
                NPC_Bandit.PatrollingCircle = 100;
                NPC_Bandit.StrikeForce = 10;
                NPC_Bandit.AggroSpeed = 1.8f;
                AnimatedSpriteObject.Add(NPC_Bandit);
                NPCPatrollingGuards.Add(NPC_Bandit);

            }

            #region Story NPCs
            npcStoryAsterix = new NPC_Story(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"), Content.Load<Script>("Scripts/AsterixDialog"), Content.Load<Texture2D>("CharacterPotraits/asterix"), "Asterix");
            npcStoryAsterix.Origionoffset = new Vector2(25, 65);
            npcStoryAsterix.SetSpritePositionInGameWorld(new Vector2(64, 11));//(20, 17));
            AnimatedSpriteObject.Add(npcStoryAsterix);
            NpcStoryList.Add(npcStoryAsterix);

            npcstory = new NPC_Story(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"), Content.Load<Script>("Scripts/LumberJacksDialog"), Content.Load<Texture2D>("CharacterPotraits/Jackie"), "Jack");
            npcstory.Origionoffset = new Vector2(25, 65);
            npcstory.SetSpritePositionInGameWorld(new Vector2(138, 15));
            AnimatedSpriteObject.Add(npcstory);
            NpcStoryList.Add(npcstory);

            npcstory = new NPC_Story(Content.Load<Texture2D>("Sprite/HumanNPCDartagnan"), Content.Load<Script>("Scripts/LumberJacksDialog"), Content.Load<Texture2D>("CharacterPotraits/john"), "John");
            npcstory.Origionoffset = new Vector2(25,65);
            npcstory.SetSpritePositionInGameWorld(new Vector2(136, 15));
            AnimatedSpriteObject.Add(npcstory);
            NpcStoryList.Add(npcstory);

            npcstory = new NPC_Story(Content.Load<Texture2D>("Sprite/HumanNPCDartagnan"), Content.Load<Script>("Scripts/LumberJacksDialog"), Content.Load<Texture2D>("CharacterPotraits/johnny"), "Johnny");
            npcstory.Origionoffset = new Vector2(25, 65);
            npcstory.SetSpritePositionInGameWorld(new Vector2(140, 15));
            AnimatedSpriteObject.Add(npcstory);
            NpcStoryList.Add(npcstory);

            guard = new NPC_Story_GuardCaptain(Content.Load<Texture2D>("Sprite/NPCGuardCaptain"), Content.Load<Script>("Scripts/DrunkGuard"), Content.Load<Texture2D>("CharacterPotraits/pimp-bender"), "Bibitur");
            guard.scriptList.Add(Content.Load<Script>("Scripts/DrunkGuardHaveAlcohol"));
            guard.Origionoffset = new Vector2(25, 65);
            guard.SetSpritePositionInGameWorld(new Vector2(64, 13));
            AnimatedSpriteObject.Add(guard);
            NpcStoryList.Add(guard);

            npcstory = new NPC_Story(Content.Load<Texture2D>("Sprite/Bjorn_Try_Soldier"), Content.Load<Script>("Scripts/GateGuards"), Content.Load<Texture2D>("CharacterPotraits/Jackie"), "Guard");
            npcstory.scriptList.Add(Content.Load<Script>("Scripts/GateGuardsHavePermit"));
            npcstory.Origionoffset = new Vector2(25, 65);
            npcstory.SetSpritePositionInGameWorld(new Vector2(145, 32));
            AnimatedSpriteObject.Add(npcstory);
            NpcStoryList.Add(npcstory);

            npcstory = new NPC_Story(Content.Load<Texture2D>("Sprite/Bjorn_Try_Soldier"), Content.Load<Script>("Scripts/GateGuards"), Content.Load<Texture2D>("CharacterPotraits/Jackie"), "Guard");
            npcstory.scriptList.Add(Content.Load<Script>("Scripts/GateGuardsHavePermit"));
            npcstory.Origionoffset = new Vector2(25, 65);
            npcstory.SetSpritePositionInGameWorld(new Vector2(145, 30));
            AnimatedSpriteObject.Add(npcstory);
            NpcStoryList.Add(npcstory);

            npcstory = new NPC_Story(Content.Load<Texture2D>("Sprite/Human"), Content.Load<Script>("Scripts/Innkeeper"), Content.Load<Texture2D>("CharacterPotraits/asterix"), "Innkeeper");
            npcstory.Origionoffset = new Vector2(25, 65);
            npcstory.SetSpritePositionInGameWorld(new Vector2(62, 13));
            AnimatedSpriteObject.Add(npcstory);
            NpcStoryList.Add(npcstory);
            #endregion

            #region Neutrala NPCs
            npcNeutral = new NPC_Neutral_Townsfolk(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"), Content.Load<Script>("Scripts/PotatotownTownsfolk"));
            npcNeutral.Origionoffset = new Vector2(25, 65);
            npcNeutral.SetSpritePositionInGameWorld(new Vector2(65, 42));
            AnimatedSpriteObject.Add(npcNeutral);
            NpcNeutralList.Add(npcNeutral);

            npcNeutral = new NPC_Neutral_Townsfolk(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"), Content.Load<Script>("Scripts/PotatotownTownsfolk"));
            npcNeutral.Origionoffset = new Vector2(25, 65);
            npcNeutral.SetSpritePositionInGameWorld(new Vector2(55, 32));
            AnimatedSpriteObject.Add(npcNeutral);
            NpcNeutralList.Add(npcNeutral);

            npcNeutral = new NPC_Neutral_Townsfolk(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"), Content.Load<Script>("Scripts/PotatotownTownsfolk"));
            npcNeutral.Origionoffset = new Vector2(25, 65);
            npcNeutral.SetSpritePositionInGameWorld(new Vector2(140, 44));
            AnimatedSpriteObject.Add(npcNeutral);
            NpcNeutralList.Add(npcNeutral);

            npcNeutral = new NPC_Neutral_Townsfolk(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"), Content.Load<Script>("Scripts/PotatotownTownsfolk"));
            npcNeutral.Origionoffset = new Vector2(25, 65);
            npcNeutral.SetSpritePositionInGameWorld(new Vector2(30,30));
            AnimatedSpriteObject.Add(npcNeutral);
            NpcNeutralList.Add(npcNeutral);

            npcNeutral = new NPC_Neutral_Townsfolk(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"), Content.Load<Script>("Scripts/PotatotownTownsfolk"));
            npcNeutral.Origionoffset = new Vector2(25, 65);
            npcNeutral.SetSpritePositionInGameWorld(new Vector2(25, 20));
            AnimatedSpriteObject.Add(npcNeutral);
            NpcNeutralList.Add(npcNeutral);

            npcNeutral = new NPC_Neutral_Townsfolk(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"), Content.Load<Script>("Scripts/PotatotownTownsfolk"));
            npcNeutral.Origionoffset = new Vector2(25, 65);
            npcNeutral.SetSpritePositionInGameWorld(new Vector2(55,56));
            AnimatedSpriteObject.Add(npcNeutral);
            NpcNeutralList.Add(npcNeutral);

            npcNeutral = new NPC_Neutral_Townsfolk(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"), Content.Load<Script>("Scripts/PotatotownTownsfolk"));
            npcNeutral.Origionoffset = new Vector2(25, 65);
            npcNeutral.SetSpritePositionInGameWorld(new Vector2(110,80));
            AnimatedSpriteObject.Add(npcNeutral);
            NpcNeutralList.Add(npcNeutral);

            npcNeutral = new NPC_Neutral_Townsfolk(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"), Content.Load<Script>("Scripts/PotatotownTownsfolk"));
            npcNeutral.Origionoffset = new Vector2(25, 65);
            npcNeutral.SetSpritePositionInGameWorld(new Vector2(12,80));
            AnimatedSpriteObject.Add(npcNeutral);
            NpcNeutralList.Add(npcNeutral);

            npcNeutral = new NPC_Neutral_Townsfolk(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"), Content.Load<Script>("Scripts/PotatotownTownsfolk"));
            npcNeutral.Origionoffset = new Vector2(25, 65);
            npcNeutral.SetSpritePositionInGameWorld(new Vector2(24,55));
            AnimatedSpriteObject.Add(npcNeutral);
            NpcNeutralList.Add(npcNeutral);

            npcNeutral = new NPC_Neutral_Townsfolk(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"), Content.Load<Script>("Scripts/PotatotownTownsfolk"));
            npcNeutral.Origionoffset = new Vector2(25, 65);
            npcNeutral.SetSpritePositionInGameWorld(new Vector2(55,24));
            AnimatedSpriteObject.Add(npcNeutral);
            NpcNeutralList.Add(npcNeutral);

            npcNeutral = new NPC_Neutral_Townsfolk(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"), Content.Load<Script>("Scripts/PotatotownTownsfolk"));
            npcNeutral.Origionoffset = new Vector2(25, 65);
            npcNeutral.SetSpritePositionInGameWorld(new Vector2(70,100));
            AnimatedSpriteObject.Add(npcNeutral);
            NpcNeutralList.Add(npcNeutral);

            npcNeutral = new NPC_Neutral_Townsfolk(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"), Content.Load<Script>("Scripts/PotatotownTownsfolk"));
            npcNeutral.Origionoffset = new Vector2(25, 65);
            npcNeutral.SetSpritePositionInGameWorld(new Vector2(111,49));
            AnimatedSpriteObject.Add(npcNeutral);
            NpcNeutralList.Add(npcNeutral);
            #endregion

            SpriteObject.Add(new LifePotatoSprite(Content.Load<Texture2D>("Sprite/LifePotato"),
                new Vector2(90, 40)));
            SpriteObject.Add(new LifePotatoSprite(Content.Load<Texture2D>("Sprite/LifePotato"),
                new Vector2(90, 43)));
            SpriteObject.Add(new BelladonnaSprite(Content.Load<Texture2D>("Sprite/Belladonna"),
                new Vector2(85, 43))); //new Vector2(7, 33)));
            SpriteObject.Add(new ImmortuiSprite(Content.Load<Texture2D>("Sprite/Immortui"),
                new Vector2(87, 45))); //just testing the immortui mushroom
            SpriteObject.Add(new ImmortuiSprite(Content.Load<Texture2D>("Sprite/Immortui big"),
                new Vector2(85, 45))); //just testing the big immortui mushroom

            //SpriteObject.Add(new LifePotatoSprite(Content.Load<Texture2D>("Sprite/multi_iron_ore"),
            //    new Vector2(92, 43)));

            for (int i = 0; i < 30; i++)
            {
                int x = GameRef.random.Next(102, 137);
                int y = GameRef.random.Next(88, 100);
                MultiIronSprite multiIronOre = new MultiIronSprite(Content.Load<Texture2D>("Sprite/multi_iron_ore"));
                multiIronOre.SetSpritePositionInGameWorld(new Vector2(x, y));
                AnimatedSpriteObject.Add(multiIronOre);
            }
            

            treeStanding = new Sprite(Content.Load<Texture2D>("Sprite/BridgeTreeStanding"));
            treeStanding.SetSpritePositionInGameWorld(new Vector2(35, 15));
            treeStanding.Origionoffset = new Vector2(48, 80);
            treeStanding.CollisionRadius = 48.0f;
            SpriteObject.Add(treeStanding);

            treeBridge = new Sprite(Content.Load<Texture2D>("Sprite/BridgeTreeFallen"));
            treeBridge.SetSpritePositionInGameWorld(new Vector2(33, 17));

            treeStubbe = new Sprite(Content.Load<Texture2D>("Sprite/TreeStubbe"));
            treeStubbe.SetSpritePositionInGameWorld(new Vector2(36, 18));

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
            NPC_Guard_1.SetVectorTowardsTargetAndStartAndCheckAggro(gameTime,player);
            NPC_Guard_2.SetVectorTowardsTargetAndStartAndCheckAggro(gameTime,player);
            if(NPC_Guard_1.Aggro)
            {
                NPC_Guard_1.PlayerPosition = player.Origin;           
            }
            if(NPC_Guard_2.Aggro)
            {
                NPC_Guard_2.PlayerPosition = player.Origin;
            }

            foreach(NPC_Fighting_Patrolling npc in NPCPatrollingGuards)
            {
                if (!npc.Dead)
                {
                    npc.SetVectorTowardsTargetAndStartAndCheckAggro(gameTime, player);
                    if (npc.Motion != Vector2.Zero)
                    {
                        npc.Motion.Normalize();
                        npc.Collided = CollisionWithTerrain.CheckForCollisionAroundSprite(npc, npc.Motion, this);

                    }
                    if (npc.Aggro)
                        npc.PlayerPosition = player.Origin;
                }
            }
            
            foreach(NPC_Fighting_Farmer npc in NPCFightingFarmers)
            {
                //CollisionWithCharacter.UpdateCollisionForCharacters(
                //    gameTime, SpriteObjectInGameWorld, npc,
                //    SpriteObject, playerprojectiles, renderList, AnimatedSpriteObject);
                if (!npc.Dead)
                {
                    if (npc.Motion != Vector2.Zero)
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
                    if (npc.Running)
                        npc.CheckForCollisionWithOtherNPCs(NPCFightingFarmers, player);
                }

            }

            foreach (var npc in NpcStoryList)
            {
                if (npc.InSpeakingRange(player))
                {
                    if (StoryProgress.ProgressLine["permitHave"] && npc.NPCName == "Guard")
                        npc.ChangeScript(1);
                    if (StoryProgress.ProgressLine["alcoholHave"] && npc.NPCName == "Bibitur")
                        npc.ChangeScript(1);
                    if (npc.canTalk == true)
                    {

                        if (InputHandler.KeyReleased(Keys.Space))
                        {
                            if (ActiveConversation == false)
                            {
                                PlayerStartConversation(npc, NpcStoryList);
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


            foreach (var sprite in SpriteObjectInGameWorld)
            {
                if (PlayerInTriggerRange(sprite))
                {
                    if (showingThinkingBox == false)
                        if (StoryProgress.ProgressLine["Axe"] && sprite == treeStanding)
                            PlayerShowThinkingBox("(Jag vet han har redan yxan) I will need an axe for this");
                }
            }


            //Kontroller för kod som rör Neutrala NPCs
            foreach (var npc in NpcNeutralList)
            {
                if (StoryProgress.ProgressLine["asterixTalkedTo"])
                    npc.script = Content.Load<Script>("Scripts/PotatotownTownsfolk2");
                if (npc.InHearingRange(player))
                {
                    if (npc.ShowingBubble == false)
                    {
                        PlayerShowTextBubble(npc);
                    }
                }

                else
                {
                    if (npc.ShowingBubble == true)
                    {
                        PlayerHideTextBubble(npc);
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
