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
        public TreeStandingBridge treeStanding;
        public Sprite treeBridge;
        public Sprite treeStubbe;

        #endregion

        #region Property Region

        //GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
                
        Sprite sprite, sprite1;
        AnimatedSprite NPC1, NPC2;
        public NPC_Story npcstory, npcStoryAsterix;//npcstory2;

        #region Björn NPC
        NPC_Neutral_Bjorn bjorne;
        #endregion

        public NPC_Story npc;
        public NPC_Story_GuardCaptain guard;
        public NPC_Neutral_Townsfolk npcNeutral;
        public NPC_Neutral_Critters_Cow npcCow;

        public List<NPC_Neutral_Townsfolk> NpcNeutralList = new List<NPC_Neutral_Townsfolk>();
        public List<NPC_Neutral_Critters_Cow> NpcCritters = new List<NPC_Neutral_Critters_Cow>();
        public List<NPC_Story> NpcStoryList = new List<NPC_Story>(); 
        protected Rectangle rectangle;
        private GraphicsDeviceManager graphics;

        CollisionWithCharacter CollisionWithCharacter = new CollisionWithCharacter();
       
        public List<BaseSprite> SpriteObject = new List<BaseSprite>();
        List<BaseSprite> AnimatedSpriteObject = new List<BaseSprite>();
        public List<BaseSprite> SpriteObjectInGameWorld = new List<BaseSprite>();
        List<AnimatedProjectile> NPCProjectile = new List<AnimatedProjectile>();
        List<AnimatedSprite> NPCFightingFarmers = new List<AnimatedSprite>();
        List<AnimatedSprite> NPCPatrollingGuards = new List<AnimatedSprite>();
        List<AnimatedSprite> NPCStationaryGuards = new List<AnimatedSprite>();
        List<AnimatedSprite> NPCRangedGuards = new List<AnimatedSprite>();
        List<AnimatedSprite> NPCPotatoeGunners = new List<AnimatedSprite>();
        List<Sprite> DirtPiles = new List<Sprite>();
        List<Sprite> BombSprites = new List<Sprite>();
        List<AnimatedSprite> Explosions = new List<AnimatedSprite>();
        List<BaseSprite> pickupableobjects = new List<BaseSprite>();
        
        

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
            SpriteObjectInGameWorld.AddRange(pickupableobjects);
            SpriteObjectInGameWorld.AddRange(SpriteObject);            
            //SpriteObjectInGameWorld.AddRange(NPCFightingFarmers);
            //SpriteObjectInGameWorld.AddRange(NPCPatrollingGuards);
            renderList.AddRange(SpriteObject);
            renderList.AddRange(AnimatedSpriteObject);
            renderList.AddRange(pickupableobjects);
            //renderList.AddRange(NPCFightingFarmers);
            //renderList.AddRange(NPCPatrollingGuards);
            if (StoryProgress.ProgressLine["treeIsDown"])
            {
                this.SpriteObjectInGameWorld.Remove(GameRef.PotatoTown.treeStanding);
                this.renderList.Remove(GameRef.PotatoTown.treeStanding);
                this.renderList.Add(GameRef.PotatoTown.treeBridge);
                this.renderList.Add(GameRef.PotatoTown.treeStubbe);
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
            #region Björn NPC
            bjorne = new NPC_Neutral_Bjorn(Content.Load<Texture2D>("Sprite/Bjorn_Try_Bjorn"),new Vector2(32 * 32,54 * 32));
            bjorne.Origionoffset = new Vector2(25, 65);
            bjorne.SetSpritePositionInGameWorld(new Vector2(32, 54));
            bjorne.Life = 100;
            bjorne.FullHp = 100;
            AnimatedSpriteObject.Add(bjorne);
            #endregion

            #region Potatisgunners
            for (int i = 0; i < 4; i++)
            {
                NPC_Fighting_PotatoeGunner NPC_Gunner = new NPC_Fighting_PotatoeGunner(Content.Load<Texture2D>("Sprite/Bjorn_Try_PotatoeGunner"), null);
                NPC_Gunner.Origionoffset = new Vector2(25, 65);
                NPC_Gunner.SetSpritePositionInGameWorld(new Vector2(80 + i, 70 + i));
                NPC_Gunner.Life = 100;
                NPC_Gunner.FullHp = 100;
                AnimatedSpriteObject.Add(NPC_Gunner);
                NPCPotatoeGunners.Add(NPC_Gunner);
            }
            NPCPotatoeGunners[0].SetSpritePositionInGameWorld(new Vector2(133, 90));
            NPCPotatoeGunners[1].SetSpritePositionInGameWorld(new Vector2(115, 98));
            NPCPotatoeGunners[2].SetSpritePositionInGameWorld(new Vector2(105, 95));
            NPCPotatoeGunners[3].SetSpritePositionInGameWorld(new Vector2(115, 85));
            #endregion

            #region Potatiskastare
                for (int i = 0; i < 4; i++)
                {
                    NPC_Fighting_Ranged NPC_Ranged = new NPC_Fighting_Ranged(Content.Load<Texture2D>("Sprite/Bjorn_Try_Ranged"), null, GameRef.random);
                    NPC_Ranged.Origionoffset = new Vector2(25, 65);
                    NPC_Ranged.SetSpritePositionInGameWorld(new Vector2(120 + i, 90 + i));
                    NPC_Ranged.Life = 100;
                    NPC_Ranged.FullHp = 100;
                    AnimatedSpriteObject.Add(NPC_Ranged);
                    NPCRangedGuards.Add(NPC_Ranged);
                }
#endregion

            #region Dementa bönder
            for (int i = 0; i < 10; i++)
            {
                NPC_Fighting_Farmer NPC_Farmer = new NPC_Fighting_Farmer(Content.Load<Texture2D>("Sprite/Bjorn_Try_Farmer"), null, GameRef.random, new Vector2((5+i) * 32, 22 * 32));
                NPC_Farmer.Origionoffset = new Vector2(25, 65);
                NPC_Farmer.SetSpritePositionInGameWorld(new Vector2(5 + i, 22));
                NPC_Farmer.Life = 100;
                NPC_Farmer.FullHp = 100;
                AnimatedSpriteObject.Add(NPC_Farmer);
                NPCFightingFarmers.Add(NPC_Farmer);
            }
            for (int i = 0; i < 7; i++)
            {
                NPC_Fighting_Farmer NPC_Farmer = new NPC_Fighting_Farmer(Content.Load<Texture2D>("Sprite/Bjorn_Try_Farmer"), null, GameRef.random, new Vector2((5 + i) * 32, 22 * 32));
                NPC_Farmer.Origionoffset = new Vector2(25, 65);
                NPC_Farmer.SetSpritePositionInGameWorld(new Vector2(23, 19));
                NPC_Farmer.Life = 100;
                NPC_Farmer.FullHp = 100;
                AnimatedSpriteObject.Add(NPC_Farmer);
                NPCFightingFarmers.Add(NPC_Farmer);
            }
            for (int i = 0; i < 2; i++)
            {
                NPC_Fighting_Farmer NPC_Farmer = new NPC_Fighting_Farmer(Content.Load<Texture2D>("Sprite/Bjorn_Try_Farmer"), null, GameRef.random, new Vector2((5 + i) * 32, 22 * 32));
                NPC_Farmer.Origionoffset = new Vector2(25, 65);
                NPC_Farmer.SetSpritePositionInGameWorld(new Vector2(21, 10));
                NPC_Farmer.Life = 100;
                NPC_Farmer.FullHp = 100;
                AnimatedSpriteObject.Add(NPC_Farmer);
                NPCFightingFarmers.Add(NPC_Farmer);
            }
            #endregion

            #region Patrullerande Banditer
            for (int i = 0; i < 10; i++)
            {
                //Detta är NPC_Banditer, ska bli en egen klass senare
                NPC_Fighting_Patrolling NPC_Bandit = new NPC_Fighting_Patrolling(Content.Load<Texture2D>("Sprite/HumanNPCBandit_WithAttack"), null, GameRef.random);
                NPC_Bandit.Origionoffset = new Vector2(25, 65);
                float x = GameRef.random.Next(102, 137);
                float y = GameRef.random.Next(88, 100);
                NPC_Bandit.SetSpritePositionInGameWorld(new Vector2(x, y));
                NPC_Bandit.Life = 100;
                NPC_Bandit.FullHp = 100;
                NPC_Bandit.AggroCircle = 600; //Hur långt han följer
                NPC_Bandit.AggroRange = 300; //Hur långt ifrån du blir upptäckt
                NPC_Bandit.PatrollingCircle = 100;
                NPC_Bandit.StrikeForce = 2;
                NPC_Bandit.AggroSpeed = 1.8f;
                AnimatedSpriteObject.Add(NPC_Bandit);
                NPCPatrollingGuards.Add(NPC_Bandit);

            }
            #endregion

            #region Story NPCs
            npcStoryAsterix = new NPC_Story(Content.Load<Texture2D>("Sprite/Asterix"), Content.Load<Script>("Scripts/AsterixDialog"), Content.Load<Texture2D>("CharacterPotraits/PortraitAsterix"), "Asterix");
            npcStoryAsterix.Origionoffset = new Vector2(25, 65);
            npcStoryAsterix.SetSpritePositionInGameWorld(new Vector2(64, 11));//(20, 17));
            AnimatedSpriteObject.Add(npcStoryAsterix);
            NpcStoryList.Add(npcStoryAsterix);

            npcstory = new NPC_Story(Content.Load<Texture2D>("Sprite/NPC1PotatoTown"), Content.Load<Script>("Scripts/LumberJacksDialog"), Content.Load<Texture2D>("CharacterPotraits/Jack"), "Jack");
            npcstory.Origionoffset = new Vector2(25, 65);
            npcstory.SetSpritePositionInGameWorld(new Vector2(138, 15));
            AnimatedSpriteObject.Add(npcstory);
            NpcStoryList.Add(npcstory);

            npcstory = new NPC_Story(Content.Load<Texture2D>("Sprite/HumanNPCDartagnan"), Content.Load<Script>("Scripts/LumberJacksDialog"), Content.Load<Texture2D>("CharacterPotraits/Anon"), "John");
            npcstory.Origionoffset = new Vector2(25,65);
            npcstory.SetSpritePositionInGameWorld(new Vector2(136, 15));
            AnimatedSpriteObject.Add(npcstory);
            NpcStoryList.Add(npcstory);

            npcstory = new NPC_Story(Content.Load<Texture2D>("Sprite/HumanNPCDartagnan"), Content.Load<Script>("Scripts/LumberJacksDialog"), Content.Load<Texture2D>("CharacterPotraits/Anon"), "Johnny");
            npcstory.Origionoffset = new Vector2(25, 65);
            npcstory.SetSpritePositionInGameWorld(new Vector2(140, 15));
            AnimatedSpriteObject.Add(npcstory);
            NpcStoryList.Add(npcstory);

            guard = new NPC_Story_GuardCaptain(Content.Load<Texture2D>("Sprite/NPCGuardCaptain"), Content.Load<Script>("Scripts/DrunkGuard"), Content.Load<Texture2D>("CharacterPotraits/Bibitur"), "Bibitur");
            guard.scriptDict.Add("alcoholHave", Content.Load<Script>("Scripts/DrunkGuardHaveAlcohol"));
            guard.Origionoffset = new Vector2(25, 65);
            guard.SetSpritePositionInGameWorld(new Vector2(64, 13));
            AnimatedSpriteObject.Add(guard);
            NpcStoryList.Add(guard);

            npcstory = new NPC_Story(Content.Load<Texture2D>("Sprite/Bjorn_Try_Soldier"), Content.Load<Script>("Scripts/GateGuards"), Content.Load<Texture2D>("CharacterPotraits/Anon"), "Guard");
            npcstory.scriptDict.Add("permitHave", Content.Load<Script>("Scripts/GateGuardsHavePermit"));
            npcstory.Origionoffset = new Vector2(25, 65);
            npcstory.SetSpritePositionInGameWorld(new Vector2(144, 33));
            AnimatedSpriteObject.Add(npcstory);
            NpcStoryList.Add(npcstory);

            npcstory = new NPC_Story(Content.Load<Texture2D>("Sprite/Bjorn_Try_Soldier"), Content.Load<Script>("Scripts/GateGuards"), Content.Load<Texture2D>("CharacterPotraits/Anon"), "Guard");
            npcstory.scriptDict.Add("permitHave", Content.Load<Script>("Scripts/GateGuardsHavePermit"));
            npcstory.Origionoffset = new Vector2(25, 65);
            npcstory.SetSpritePositionInGameWorld(new Vector2(144, 31));
            AnimatedSpriteObject.Add(npcstory);
            NpcStoryList.Add(npcstory);

            npcstory = new NPC_Story(Content.Load<Texture2D>("Sprite/Human"), Content.Load<Script>("Scripts/InnkeeperNoMoney"), Content.Load<Texture2D>("CharacterPotraits/Anon"), "Innkeeper");
            npcstory.scriptDict.Add("moneyHave", Content.Load<Script>("Scripts/Innkeeper"));
            npcstory.Origionoffset = new Vector2(25, 65);
            npcstory.SetSpritePositionInGameWorld(new Vector2(62, 13));
            AnimatedSpriteObject.Add(npcstory);
            NpcStoryList.Add(npcstory);

            npcstory = new NPC_Story(Content.Load<Texture2D>("Sprite/Human"), Content.Load<Script>("Scripts/FishmarketNoFish"), Content.Load<Texture2D>("CharacterPotraits/Fisherman"), "Fisherman");
            npcstory.scriptDict.Add("fishHave", Content.Load<Script>("Scripts/Fishmarket"));
            npcstory.Origionoffset = new Vector2(25, 65);
            npcstory.SetSpritePositionInGameWorld(new Vector2(84, 49));
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

            for (int i = 0; i < 9; i++)
            {
                npcCow = new NPC_Neutral_Critters_Cow(Content.Load<Texture2D>("Sprite/Cow"), Content.Load<Script>("Scripts/Cows"), GameRef.random);
                npcCow.Origionoffset = new Vector2(16, 16);
                npcCow.SetSpritePositionInGameWorld(new Vector2(112 + i, 36 + i));
                AnimatedSpriteObject.Add(npcCow);
                NpcCritters.Add(npcCow);
            }

            for (int i = 0; i < 5; i++)
            {
                npcCow = new NPC_Neutral_Critters_Cow(Content.Load<Texture2D>("Sprite/Cow"), Content.Load<Script>("Scripts/Cows"), GameRef.random);
                npcCow.Origionoffset = new Vector2(16, 16);
                npcCow.SetSpritePositionInGameWorld(new Vector2(127 + i, 39 + i));
                AnimatedSpriteObject.Add(npcCow);
                NpcCritters.Add(npcCow);
            }

            #endregion

            createLifePotatoPlant();

            pickupableobjects.Add(new BelladonnaSprite(Content.Load<Texture2D>("Sprite/Belladonna"),
                new Vector2(7, 33)));
            //SpriteObject.Add(new ImmortuiSprite(Content.Load<Texture2D>("Sprite/Immortui"),
            //    new Vector2(87, 45))); //just testing the immortui mushroom
            //SpriteObject.Add(new ImmortuiSprite(Content.Load<Texture2D>("Sprite/Immortui big"),
            //    new Vector2(85, 45))); //just testing the big immortui mushroom

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


            treeStanding = new TreeStandingBridge(Content.Load<Texture2D>("Sprite/BridgeTreeStanding"));
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
            lockedGateDict[42] = true;
            //--


            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            player.Speed = 10;
            CollisionWithCharacter.UpdateCollisionForCharacters(gameTime, SpriteObjectInGameWorld,  player,  SpriteObject,  playerprojectiles,  renderList,  AnimatedSpriteObject);
            //--
            if (InputHandler.KeyReleased(Keys.Space))
            {
                PlayerScreen.PickUp(gameTime, SpriteObjectInGameWorld, player, SpriteObject, playerprojectiles, renderList, AnimatedSpriteObject, pickupableobjects);
            }
            //--
            #region Björn NPC
            bjorne.AngryCheck(player);
            #endregion
            foreach (DirtPileSprite dirtpile in DirtPiles)
            {
                dirtpile.UpdateTheDirtPile(gameTime);
                if(dirtpile.Finished)
                {
                    DirtPiles.Remove(dirtpile);
                    renderList.Remove(dirtpile);
                    break;
            }
            }
            foreach(Explosion explosion in Explosions)
            {
                explosion.UpdateExplosion(gameTime);
                if (explosion.Bounds.Intersects(new Rectangle(player.Bounds.X + 10, player.Bounds.Y + 10, 30, 50)))
                {
                    player.Life -= explosion.Damage;
                    explosion.Finished = true;
                }
                if(explosion.Finished)
                {
                    AnimatedSpriteObject.Remove(explosion);
                    renderList.Remove(explosion);
                    Explosions.Remove(explosion);
                    break;
                }
            }
            foreach(BombSprite bomb in BombSprites)
            {
                bomb.UpdateBomb(gameTime);
                if(bomb.Bounds.Intersects(new Rectangle(player.Bounds.X + 10,player.Bounds.Y + 10,30,50)))
                {
                    bomb.Boom = true;
                }
                if(bomb.Boom)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Explosion explosion = new Explosion(Content.Load<Texture2D>("Sprite/Bjorn_Try_Explosion"),
                            bomb.Position,new Vector2(GameRef.random.Next(-100,100)/100f,GameRef.random.Next(-100,100)/100f));
                        Explosions.Add(explosion);
                        AnimatedSpriteObject.Add(explosion);
                        renderList.Add(explosion);
                    }
                    BombSprites.Remove(bomb);
                    renderList.Remove(bomb);
                    
                    break;
                }
                
            }
            foreach(NPC_Fighting_PotatoeGunner npc in NPCPotatoeGunners)
            {
                if (!npc.Dead)
                {
                    npc.UpdateWithPlayer(gameTime, player);
                    if (npc.FireTime)
                    {
                        BombSprite bomb;
                        if (npc.CurrentAnimationName == "AttackPlayerRight")
                            bomb = new BombSprite(Content.Load<Texture2D>("Sprite/Bjorn_Try_Bomb"),
                                    new Vector2(player.Origin.X, player.Origin.Y - 40), new Vector2(npc.Position.X + 50, npc.Position.Y + 20));
                        else
                            bomb = new BombSprite(Content.Load<Texture2D>("Sprite/Bjorn_Try_Bomb"),
                                   new Vector2(player.Origin.X, player.Origin.Y - 40), new Vector2(npc.Position.X, npc.Position.Y + 20));
                        BombSprites.Add(bomb);
                        renderList.Add(bomb);
                        npc.FireTime = false;

                    }
                }
                else if (!npc.DirtPileCreated)
                {
                    DirtPileSprite dirtpile = new DirtPileSprite(Content.Load<Texture2D>("Sprite/dirtpile"));
                    dirtpile.Position = npc.Position;
                    dirtpile.DelayTime = npc.DelayRespawn;
                    DirtPiles.Add(dirtpile);
                    renderList.Add(dirtpile);
                    npc.DirtPileCreated = true;
                }
            }
            foreach(NPC_Fighting_Ranged npc in NPCRangedGuards)
            {
                if (!npc.Dead)
                {
                    npc.UpdateRangedFighter(gameTime, player);
                    if (npc.BombThrow)
                    {
                        BombSprite bomb;
                        if (npc.CurrentAnimationName == "ThrowRight")
                            bomb = new BombSprite(Content.Load<Texture2D>("Sprite/Bjorn_Try_Bomb"),
                                    new Vector2(player.Origin.X, player.Origin.Y - 40), new Vector2(npc.Position.X + 50, npc.Position.Y + 20));
                        else
                            bomb = new BombSprite(Content.Load<Texture2D>("Sprite/Bjorn_Try_Bomb"),
                                   new Vector2(player.Origin.X, player.Origin.Y - 40), new Vector2(npc.Position.X, npc.Position.Y + 20));
                        BombSprites.Add(bomb);
                        renderList.Add(bomb);
                        npc.BombThrow = false;
                    }
                    if (npc.Motion != Vector2.Zero && !npc.GoingHome)
                    {
                        npc.Motion.Normalize();
                        npc.Collided = CollisionWithTerrain.CheckForCollisionAroundSprite(npc, npc.Motion, this);

                    }
                }
                else if(!npc.DirtPileCreated)
            {
                    DirtPileSprite dirtpile = new DirtPileSprite(Content.Load<Texture2D>("Sprite/dirtpile"));
                    dirtpile.Position = npc.Position;
                    dirtpile.DelayTime = npc.DelayRespawn;
                    DirtPiles.Add(dirtpile);
                    renderList.Add(dirtpile);
                    npc.DirtPileCreated = true;
            }

            }

            foreach (NPC_Neutral npc in NpcNeutralList)
            {
                if (npc.Motion != Vector2.Zero)
                {
                    npc.Motion.Normalize();
                    npc.Collided = CollisionWithTerrain.CheckForCollisionAroundSprite(npc, npc.Motion, this);
                }
                GameRef.storyProgress.ChangeScriptsForNPCs(npc);
                if (npc.InHearingRange(player))
                {
                    if (npc.ShowingBubble == false)
                    {
                        npc.changeSpeed = false;
                        PlayerShowTextBubble(npc);
                    }
                }

                else
                {
                    if (npc.ShowingBubble == true)
                    {
                        npc.changeSpeed = true;
                        PlayerHideTextBubble(npc);
                    }
                }
            }

            foreach (var npc in NpcCritters)
            {
                if (npc.Motion != Vector2.Zero)
                {
                    npc.Motion.Normalize();
                    npc.Collided = CollisionWithTerrain.CheckForCollisionAroundSprite(npc, npc.Motion, this);

                }
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

            foreach(NPC_Fighting_Patrolling npc in NPCPatrollingGuards)
            {
                if (!npc.Dead)
                {
                    npc.SetVectorTowardsTargetAndStartAndCheckAggroMelee(gameTime, player);
                    if (npc.Motion != Vector2.Zero && !npc.GoingHome)
                    {
                        npc.Motion.Normalize();
                        npc.Collided = CollisionWithTerrain.CheckForCollisionAroundSprite(npc, npc.Motion, this);

                    }
                if (npc.Aggro)
                    npc.PlayerPosition = player.Origin;
            }
                else if(!npc.DirtPileCreated)
                {
                    DirtPileSprite dirtpile = new DirtPileSprite(Content.Load<Texture2D>("Sprite/dirtpile"));
                    dirtpile.Position = npc.Position;
                    dirtpile.DelayTime = npc.DelayRespawn;
                    DirtPiles.Add(dirtpile);
                    renderList.Add(dirtpile);
                    npc.DirtPileCreated = true;
                }
            }
            foreach (NPC_Fighting_Farmer npc in NPCFightingFarmers)
            {
                if (!npc.Dead)
                {
                    if (npc.Motion != Vector2.Zero)
                    {
                        npc.Motion.Normalize();
                        npc.Collided = CollisionWithTerrain.CheckForCollisionAroundSprite(npc, npc.Motion, this);
                    }
                    //Aggro range
                    if (Vector2.Distance(npc.Position, player.Position) < 200 && !npc.Aggro && !npc.Running && !npc.HitFlag)
                    {
                        npc.Aggro = true;
                        npc.AttackersDirection = player.Position - npc.Position;
                    }
                    
                    if (npc.Running)
                        npc.CheckForCollisionWithOtherNPCs(NPCFightingFarmers, player);
                }
                else if(!npc.DirtPileCreated)
                {
                    DirtPileSprite dirtpile = new DirtPileSprite(Content.Load<Texture2D>("Sprite/dirtpile"));
                    dirtpile.Position = npc.Position;
                    dirtpile.DelayTime = npc.DelayRespawn;
                    DirtPiles.Add(dirtpile);
                    renderList.Add(dirtpile);
                    npc.DirtPileCreated = true;
                }

            }

            foreach (var npc in NpcStoryList)
            {
                if (npc.InSpeakingRange(player))
                {
                    GameRef.storyProgress.ChangeScriptsForNPCs(npc);
                    if (npc.canTalk == true)
                    {

                        if (InputHandler.KeyReleased(Keys.Space))
                        {
                            if (ActiveConversation == false && npc.script != null)
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
                  
            Point cell = Engine.ConvertPostionToCell(player.Origin);
            int cellIndex = tileMap.CollisionLayer.GetCellIndex(cell);
            if (cellIndex >= 40 && cellIndex < 50)
            {
                GateToNextScreen(cellIndex, GameRef.GamePlayScreen2, "G0");

                GateToNextScreen(cellIndex, GameRef.GamePlayScreen2, "G1");

                GateToNextScreen(cellIndex, GameRef.CollectGameScreen, "G2");  
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

        #region Method Region
        void createLifePotatoPlant()
        {
            pickupableobjects.Add(new LifePotatoSprite(Content.Load<Texture2D>("Sprite/LifePotato"),
                new Vector2(90, 40)));
            pickupableobjects.Add(new LifePotatoSprite(Content.Load<Texture2D>("Sprite/LifePotato"),
                new Vector2(90, 43)));
            pickupableobjects.Add(new LifePotatoSprite(Content.Load<Texture2D>("Sprite/LifePotato"),
                new Vector2(54, 53)));
            pickupableobjects.Add(new LifePotatoSprite(Content.Load<Texture2D>("Sprite/LifePotato"),
                new Vector2(45, 48)));
            pickupableobjects.Add(new LifePotatoSprite(Content.Load<Texture2D>("Sprite/LifePotato"),
                new Vector2(39, 62)));
            pickupableobjects.Add(new LifePotatoSprite(Content.Load<Texture2D>("Sprite/LifePotato"),
                new Vector2(39, 75)));
            pickupableobjects.Add(new LifePotatoSprite(Content.Load<Texture2D>("Sprite/LifePotato"),
                new Vector2(25, 40)));
            pickupableobjects.Add(new LifePotatoSprite(Content.Load<Texture2D>("Sprite/LifePotato"),
                new Vector2(50, 4)));
            pickupableobjects.Add(new LifePotatoSprite(Content.Load<Texture2D>("Sprite/LifePotato"),
                new Vector2(82, 5)));
            pickupableobjects.Add(new LifePotatoSprite(Content.Load<Texture2D>("Sprite/LifePotato"),
                new Vector2(133, 31)));
            pickupableobjects.Add(new LifePotatoSprite(Content.Load<Texture2D>("Sprite/LifePotato"),
                new Vector2(129, 58)));
            pickupableobjects.Add(new LifePotatoSprite(Content.Load<Texture2D>("Sprite/LifePotato"),
                new Vector2(91, 78)));
            pickupableobjects.Add(new LifePotatoSprite(Content.Load<Texture2D>("Sprite/LifePotato"),
                new Vector2(94, 99)));
            pickupableobjects.Add(new LifePotatoSprite(Content.Load<Texture2D>("Sprite/LifePotato"),
                new Vector2(64, 100)));
            pickupableobjects.Add(new LifePotatoSprite(Content.Load<Texture2D>("Sprite/LifePotato"),
                new Vector2(29, 85)));
            pickupableobjects.Add(new LifePotatoSprite(Content.Load<Texture2D>("Sprite/LifePotato"),
                new Vector2(11, 91)));
        }
        #endregion

        
    }
}
