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

using XtheSmithLibrary;
using TileEngine;
using TileEngine.Tiles;
using TileEngine.Sprite;
using TileEngine.Sprite.Npc;
using TileEngine.Collision;

namespace TileGame.GameScreens
{
    public class GamePlayScreen : BaseGamePlayScreen
    {
        
        #region Field Region

        string name;
        bool gate1Locked = true;
        bool gate2Locked = true;

        #endregion
        #region Property Region

        //GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
                
        Sprite sprite, sprite1;
        AnimatedSprite NPC1, NPC2;

        CollisionWithCharacter Collision = new CollisionWithCharacter();
       

        List<BaseSprite> SpriteObject = new List<BaseSprite>();
        List<BaseSprite> AnimatedSpriteObject = new List<BaseSprite>();
        List<BaseSprite> SpriteObjectInGameWorld = new List<BaseSprite>();
        List<AnimatedProjectile> NPCProjectile = new List<AnimatedProjectile>();
        

        public string Name { get { return name; } }

        public bool Gate1Locked { get { return gate1Locked; } set { this.gate1Locked = value; } }
        public bool Gate2Locked { get { return gate2Locked; } set { this.gate2Locked = value; } }

        #endregion


        #region Constructor Region
        public GamePlayScreen(Game game, GameStateManager manager, string name)
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

            FrameAnimation down = new FrameAnimation(1, 32, 32, 0, 0);
            FrameAnimation right = new FrameAnimation(1, 32, 32, 32, 0);
            FrameAnimation up = new FrameAnimation(1, 32, 32, 64, 0);
            FrameAnimation left = new FrameAnimation(1, 32, 32, 96, 0);
            
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
            }
            
            SpriteObjectInGameWorld.Clear();
            renderList.Clear();
            renderList.Add(player);
            SpriteObjectInGameWorld.AddRange(AnimatedSpriteObject);
            SpriteObjectInGameWorld.AddRange(SpriteObject);            
            renderList.AddRange(SpriteObject);
            renderList.AddRange(AnimatedSpriteObject);

            
            
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ContentManager Content = Game.Content;
            
            
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

            NPC1 = new AnimatedSprite(Content.Load<Texture2D>("Sprite/playerboxAnimation"));           
            NPC1.Origionoffset = new Vector2(15,15);
            NPC1.SetSpritePositionInGameWorld(new Vector2(9,9));
            NPC1.Life = 30;
            NPC1.FullHp = 30;
            AnimatedSpriteObject.Add(NPC1);

            NPC2 = new AnimatedSprite(Content.Load<Texture2D>("Sprite/playerboxAnimation"));
            NPC2.Origionoffset = new Vector2(15, 15);
            NPC2.SetSpritePositionInGameWorld(new Vector2(12, 12));
            NPC2.Life = 5;
            NPC2.FullHp = 5;
            AnimatedSpriteObject.Add(NPC2);
            
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {

            Collision.UpdateCollisionForCharacters(gameTime, SpriteObjectInGameWorld,  player,  SpriteObject,  playerprojectiles,  renderList,  AnimatedSpriteObject);
            
            

            Point cell = Engine.ConvertPostionToCell(player.Origin);
            if ((cell.X == 17 && cell.Y == 14) && !gate2Locked)
            {
                GameRef.BaseGamePlayScreen.SetPlayerPosition(1, 2);
                GameRef.GamePlayScreen2.Gate1Locked = true;
                StateManager.ChangeState(GameRef.GamePlayScreen2);
                
            }
            if (cell.X != 17 || cell.Y != 14)
                gate2Locked = false;

            if ((cell.X == 4 && cell.Y == 3) && !gate1Locked)
            {
                //player.SetSpritePositionInGameWorld(new Vector2(28, 28));
                GameRef.BaseGamePlayScreen.SetPlayerPosition(28, 28);
                GameRef.GamePlayScreen2.Gate2Locked = true;
                StateManager.ChangeState(GameRef.GamePlayScreen2);
                
            }
            if (cell.X != 4 || cell.Y != 3)
                gate1Locked = false;


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

        //#region Code for Collision with terrain

        //#region Code for Collision Around Sprite

        //private void CheckForCollisionAroundSprite(AnimatedSprite sprite, Vector2 motion)
        //{
        //    Point spriteCell = Engine.ConvertPostionToCell(sprite.Origin);

        //    Point? upLeft = null, up = null, upRight = null,
        //        left = null, right = null,
        //        downLeft = null, down = null, downRight = null;

        //    if (spriteCell.Y > 0)
        //        up = new Point(spriteCell.X, spriteCell.Y - 1);

        //    if (spriteCell.Y < tileMap.CollisionLayer.Height - 1)
        //        down = new Point(spriteCell.X, spriteCell.Y + 1);

        //    if (spriteCell.X > 0)
        //        left = new Point(spriteCell.X - 1, spriteCell.Y);

        //    if (spriteCell.X < tileMap.CollisionLayer.Width - 1)
        //        right = new Point(spriteCell.X + 1, spriteCell.Y);

        //    if (spriteCell.X > 0 && spriteCell.Y > 0)
        //        upLeft = new Point(spriteCell.X - 1, spriteCell.Y - 1);

        //    if (spriteCell.X < tileMap.CollisionLayer.Width - 1 && spriteCell.Y > 0)
        //        upRight = new Point(spriteCell.X + 1, spriteCell.Y - 1);

        //    if (spriteCell.X > 0 && spriteCell.Y < tileMap.CollisionLayer.Height - 1)
        //        downLeft = new Point(spriteCell.X - 1, spriteCell.Y + 1);

        //    if (spriteCell.X < tileMap.CollisionLayer.Width - 1 &&
        //        spriteCell.Y < tileMap.CollisionLayer.Height - 1)
        //        downRight = new Point(spriteCell.X + 1, spriteCell.Y + 1);

        //    CheckNoneWalkebleArea(sprite, ref motion, ref spriteCell, upLeft, up, upRight, left, right, downLeft, down, downRight);
        //    CheckWalkebleAreaFromOneDirection(sprite, ref motion, ref spriteCell, upLeft, up, upRight, left, right, downLeft, down, downRight);
        //    CheckWalkableDamageAreaCollision(sprite, ref motion, ref spriteCell, upLeft, up, upRight, left, right, downLeft, down, downRight);
        //}

        //private void CheckNoneWalkebleArea(AnimatedSprite sprite, ref Vector2 motion, ref Point spriteCell, Point? upLeft, Point? up,
        //    Point? upRight, Point? left, Point? right, Point? downLeft, Point? down, Point? downRight)
        //{
        //    if (up != null && tileMap.CollisionLayer.GetCellIndex(up.Value) == 1)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(up.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            sprite.Position.Y = up.Value.Y * Engine.TileHeight + sprite.Bounds.Height;
        //        }
        //    }
        //    if (down != null && tileMap.CollisionLayer.GetCellIndex(down.Value) == 1)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(down.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            sprite.Position.Y = down.Value.Y * Engine.TileHeight - sprite.Bounds.Height;
        //        }
        //    }
        //    if (left != null && tileMap.CollisionLayer.GetCellIndex(left.Value) == 1)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(left.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            sprite.Position.X = left.Value.X * Engine.TileWidth + sprite.Bounds.Width;
        //        }

        //    }
        //    if (right != null && tileMap.CollisionLayer.GetCellIndex(right.Value) == 1)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(right.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            sprite.Position.X = right.Value.X * Engine.TileWidth - sprite.Bounds.Width;
        //        }

        //    }

        //    if (upLeft != null && tileMap.CollisionLayer.GetCellIndex(upLeft.Value) == 1)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(upLeft.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            if (motion.X != 0)
        //                sprite.Position.X = spriteCell.X * Engine.TileWidth;
        //            if (motion.Y != 0)
        //                sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
        //        }
        //    }

        //    if (upRight != null && tileMap.CollisionLayer.GetCellIndex(upRight.Value) == 1)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(upRight.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            if (motion.X != 0)
        //                sprite.Position.X = spriteCell.X * Engine.TileWidth;
        //            if (motion.Y != 0)
        //                sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
        //        }
        //    }

        //    if (downLeft != null && tileMap.CollisionLayer.GetCellIndex(downLeft.Value) == 1)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(downLeft.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            if (motion.X != 0)
        //                sprite.Position.X = spriteCell.X * Engine.TileWidth;
        //            if (motion.Y != 0)
        //                sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
        //        }
        //    }

        //    if (downRight != null && tileMap.CollisionLayer.GetCellIndex(downRight.Value) == 1)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(downRight.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            if (motion.X != 0)
        //                sprite.Position.X = spriteCell.X * Engine.TileWidth;
        //            if (motion.Y != 0)
        //                sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
        //        }
        //    }
        //}

        //private void CheckWalkebleAreaFromOneDirection(AnimatedSprite sprite, ref Vector2 motion, ref Point spriteCell, Point? upLeft, Point? up,
        //    Point? upRight, Point? left, Point? right, Point? downLeft, Point? down, Point? downRight)
        //{
        //    if (up != null && (tileMap.CollisionLayer.GetCellIndex(up.Value) == 14 || tileMap.CollisionLayer.GetCellIndex(up.Value) == 16 || tileMap.CollisionLayer.GetCellIndex(up.Value) == 18))
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(up.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            if (!(motion.Y >= 0))
        //                sprite.Position.Y = up.Value.Y * Engine.TileHeight + sprite.Bounds.Height;
        //        }
        //    }
        //    if (down != null && (tileMap.CollisionLayer.GetCellIndex(down.Value) == 13 || tileMap.CollisionLayer.GetCellIndex(down.Value) == 17 || tileMap.CollisionLayer.GetCellIndex(down.Value) == 15))
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(down.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            if (!(motion.Y <= 0))
        //                sprite.Position.Y = down.Value.Y * Engine.TileHeight - sprite.Bounds.Height;
        //        }
        //    }
        //    if (left != null && (tileMap.CollisionLayer.GetCellIndex(left.Value) == 11 || tileMap.CollisionLayer.GetCellIndex(left.Value) == 15 || tileMap.CollisionLayer.GetCellIndex(left.Value) == 16))
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(left.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            if (!(motion.X >= 0))
        //                sprite.Position.X = left.Value.X * Engine.TileWidth + sprite.Bounds.Width;
        //        }

        //    }
        //    if (right != null && (tileMap.CollisionLayer.GetCellIndex(right.Value) == 12 || tileMap.CollisionLayer.GetCellIndex(right.Value) == 17 || tileMap.CollisionLayer.GetCellIndex(right.Value) == 18))
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(right.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            if (!(motion.X <= 0))
        //                sprite.Position.X = right.Value.X * Engine.TileWidth - sprite.Bounds.Width;
        //        }

        //    }

        //    if (upLeft != null && tileMap.CollisionLayer.GetCellIndex(upLeft.Value) == 16)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(upLeft.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            if (!(motion.X >= 0))
        //                sprite.Position.X = spriteCell.X * Engine.TileWidth;
        //            if (!(motion.Y <= 0))
        //                sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
        //        }
        //    }

        //    if (upRight != null && tileMap.CollisionLayer.GetCellIndex(upRight.Value) == 18)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(upRight.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            if (!(motion.X <= 0))
        //                sprite.Position.X = spriteCell.X * Engine.TileWidth;
        //            if (!(motion.Y >= 0))
        //                sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
        //        }
        //    }

        //    if (downLeft != null && tileMap.CollisionLayer.GetCellIndex(downLeft.Value) == 15)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(downLeft.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            if (!(motion.X >= 0))
        //                sprite.Position.X = spriteCell.X * Engine.TileWidth;
        //            if (!(motion.Y <= 0))
        //                sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
        //        }
        //    }

        //    if (downRight != null && tileMap.CollisionLayer.GetCellIndex(downRight.Value) == 17)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(downRight.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            if (!(motion.X <= 0))
        //                sprite.Position.X = spriteCell.X * Engine.TileWidth;
        //            if (!(motion.Y <= 0))
        //                sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
        //        }
        //    }
        //}

        //private void CheckWalkableDamageAreaCollision(AnimatedSprite sprite, ref Vector2 motion, ref Point spriteCell, Point? upLeft, Point? up,
        //    Point? upRight, Point? left, Point? right, Point? downLeft, Point? down, Point? downRight)
        //{
        //    if (up != null && tileMap.CollisionLayer.GetCellIndex(up.Value) == 31)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(up.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            sprite.areTakingDamage = true;
        //            sprite.Damage = 0.1f;
        //            return;
        //        }
        //    }
        //    if (down != null && tileMap.CollisionLayer.GetCellIndex(down.Value) == 31)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(down.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            sprite.areTakingDamage = true;
        //            sprite.Damage = 0.1f;
        //            return;
        //        }
        //    }
        //    if (left != null && tileMap.CollisionLayer.GetCellIndex(left.Value) == 31)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(left.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            sprite.areTakingDamage = true;
        //            sprite.Damage = 0.1f;
        //            return;
        //        }

        //    }
        //    if (right != null && tileMap.CollisionLayer.GetCellIndex(right.Value) == 31)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(right.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            sprite.areTakingDamage = true;
        //            sprite.Damage = 0.1f;
        //            return;
        //        }

        //    }

        //    if (upLeft != null && tileMap.CollisionLayer.GetCellIndex(upLeft.Value) == 31)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(upLeft.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            sprite.areTakingDamage = true;
        //            sprite.Damage = 0.1f;
        //            return;
        //        }
        //    }

        //    if (upRight != null && tileMap.CollisionLayer.GetCellIndex(upRight.Value) == 31)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(upRight.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            sprite.areTakingDamage = true;
        //            sprite.Damage = 0.1f;
        //            return;
        //        }
        //    }

        //    if (downLeft != null && tileMap.CollisionLayer.GetCellIndex(downLeft.Value) == 31)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(downLeft.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            sprite.areTakingDamage = true;
        //            sprite.Damage = 0.1f;
        //            return;
        //        }
        //    }

        //    if (downRight != null && tileMap.CollisionLayer.GetCellIndex(downRight.Value) == 31)
        //    {
        //        Rectangle cellRect = Engine.CreateRectForCell(downRight.Value);
        //        Rectangle spriteRect = sprite.Bounds;

        //        if (cellRect.Intersects(spriteRect))
        //        {
        //            sprite.areTakingDamage = true;
        //            sprite.Damage = 0.1f;
        //            return;
        //        }
        //    }

        //    Point cell = Engine.ConvertPostionToCell(sprite.Origin);

        //    int colIndex = tileMap.CollisionLayer.GetCellIndex(cell);

        //    if (colIndex == 31)
        //    {
        //        sprite.areTakingDamage = true;
        //        sprite.Damage = 0.1f;
        //        return;
        //    }


        //    sprite.areTakingDamage = false;
        //}


        //#endregion //Collision Around Sprite region end

        //#region Code for Collision on SpriteCell
        //private Vector2 CheckCollisionForMotion(Vector2 motion, AnimatedSprite sprite)
        //{
        //    Point cell = Engine.ConvertPostionToCell(sprite.Origin);

        //    int colIndex = tileMap.CollisionLayer.GetCellIndex(cell);

        //    if (colIndex == 2)
        //        return motion * .2f;

        //    return motion;
        //}

        //private Vector2 CheckCollisionAutomaticMotion(Vector2 motion, AnimatedSprite sprite)
        //{
        //    Point cell = Engine.ConvertPostionToCell(sprite.Origin);

        //    int colIndex = tileMap.CollisionLayer.GetCellIndex(cell);

        //    if (colIndex == 21)
        //    {
        //        motion.X = 0;
        //        motion.Y = 1;
        //        motion *= 2;
        //    }
        //    if (colIndex == 22)
        //    {
        //        motion.X = 0;
        //        motion.Y = -1;
        //        motion *= 2;
        //    }
        //    if (colIndex == 23)
        //    {
        //        motion.X = 1;
        //        motion.Y = 0;
        //        motion *= 2;
        //    }
        //    if (colIndex == 24)
        //    {
        //        motion.X = -1;
        //        motion.Y = 0;
        //        motion *= 2;
        //    }

        //    return motion;
        //}
        //#endregion //EndforMotionCollisionRegion


        //#endregion //EndforCollisionWithTerrainRegion

        //public void SetPlayerPosition(int x, int y)
        //{
        //    //player.SetSpritePositionInGameWorld(new Vector2(x, y));
        //}
    }
}
