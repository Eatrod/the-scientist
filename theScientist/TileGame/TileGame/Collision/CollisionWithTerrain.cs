using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine.Sprite;
using TileGame.GameScreens;
using TileEngine;


namespace TileGame.Collision
{
    public class CollisionWithTerrain
    {
        public void CheckForCollisionAroundSprite(AnimatedSprite sprite, Vector2 motion, PlayerScreen screen)
        {
            Point spriteCell = Engine.ConvertPostionToCell(sprite.Origin);

            Point? upLeft = null, up = null, upRight = null,
                left = null, right = null,
                downLeft = null, down = null, downRight = null;

            if (spriteCell.Y > 0)
                up = new Point(spriteCell.X, spriteCell.Y - 1);

            if (spriteCell.Y < screen.tileMap.CollisionLayer.Height - 1) //Hämta från nuvarande collisionslager
                down = new Point(spriteCell.X, spriteCell.Y + 1);

            if (spriteCell.X > 0)
                left = new Point(spriteCell.X - 1, spriteCell.Y);

            if (spriteCell.X < screen.tileMap.CollisionLayer.Width - 1)
                right = new Point(spriteCell.X + 1, spriteCell.Y);

            if (spriteCell.X > 0 && spriteCell.Y > 0)
                upLeft = new Point(spriteCell.X - 1, spriteCell.Y - 1);

            if (spriteCell.X < screen.tileMap.CollisionLayer.Width - 1 && spriteCell.Y > 0)
                upRight = new Point(spriteCell.X + 1, spriteCell.Y - 1);

            if (spriteCell.X > 0 && spriteCell.Y < screen.tileMap.CollisionLayer.Height - 1)
                downLeft = new Point(spriteCell.X - 1, spriteCell.Y + 1);

            if (spriteCell.X < screen.tileMap.CollisionLayer.Width - 1 &&
                spriteCell.Y < screen.tileMap.CollisionLayer.Height - 1)
                downRight = new Point(spriteCell.X + 1, spriteCell.Y + 1);

            CheckNoneWalkebleArea(sprite, ref motion, ref spriteCell, upLeft, up, upRight, left, right, downLeft, down, downRight, screen);
            CheckWalkebleAreaFromOneDirection(sprite, ref motion, ref spriteCell, upLeft, up, upRight, left, right, downLeft, down, downRight,screen);
            CheckWalkableDamageAreaCollision(sprite, ref motion, ref spriteCell, upLeft, up, upRight, left, right, downLeft, down, downRight,screen);
        }

        public void CheckNoneWalkebleArea(AnimatedSprite sprite, ref Vector2 motion, ref Point spriteCell, Point? upLeft, Point? up,
            Point? upRight, Point? left, Point? right, Point? downLeft, Point? down, Point? downRight, PlayerScreen screen)
        {
            if (up != null && screen.tileMap.CollisionLayer.GetCellIndex(up.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(up.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.Y = up.Value.Y * Engine.TileHeight + sprite.Bounds.Height;
                }
            }
            if (down != null && screen.tileMap.CollisionLayer.GetCellIndex(down.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(down.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.Y = down.Value.Y * Engine.TileHeight - sprite.Bounds.Height;
                }
            }
            if (left != null && screen.tileMap.CollisionLayer.GetCellIndex(left.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(left.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.X = left.Value.X * Engine.TileWidth + sprite.Bounds.Width;
                }

            }
            if (right != null && screen.tileMap.CollisionLayer.GetCellIndex(right.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(right.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.X = right.Value.X * Engine.TileWidth - sprite.Bounds.Width;
                }

            }

            if (upLeft != null && screen.tileMap.CollisionLayer.GetCellIndex(upLeft.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(upLeft.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    if (motion.X != 0)
                        sprite.Position.X = spriteCell.X * Engine.TileWidth;
                    if (motion.Y != 0)
                        sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
                }
            }

            if (upRight != null && screen.tileMap.CollisionLayer.GetCellIndex(upRight.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(upRight.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    if (motion.X != 0)
                        sprite.Position.X = spriteCell.X * Engine.TileWidth;
                    if (motion.Y != 0)
                        sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
                }
            }

            if (downLeft != null && screen.tileMap.CollisionLayer.GetCellIndex(downLeft.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(downLeft.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    if (motion.X != 0)
                        sprite.Position.X = spriteCell.X * Engine.TileWidth;
                    if (motion.Y != 0)
                        sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
                }
            }

            if (downRight != null && screen.tileMap.CollisionLayer.GetCellIndex(downRight.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(downRight.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    if (motion.X != 0)
                        sprite.Position.X = spriteCell.X * Engine.TileWidth;
                    if (motion.Y != 0)
                        sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
                }
            }
        }

        public void CheckWalkebleAreaFromOneDirection(AnimatedSprite sprite, ref Vector2 motion, ref Point spriteCell, Point? upLeft, Point? up,
            Point? upRight, Point? left, Point? right, Point? downLeft, Point? down, Point? downRight, PlayerScreen screen)
        {
            if (up != null && (screen.tileMap.CollisionLayer.GetCellIndex(up.Value) == 14 || screen.tileMap.CollisionLayer.GetCellIndex(up.Value) == 16 || screen.tileMap.CollisionLayer.GetCellIndex(up.Value) == 18))
            {
                Rectangle cellRect = Engine.CreateRectForCell(up.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    if (!(motion.Y >= 0))
                        sprite.Position.Y = up.Value.Y * Engine.TileHeight + sprite.Bounds.Height;
                }
            }
            if (down != null && (screen.tileMap.CollisionLayer.GetCellIndex(down.Value) == 13 || screen.tileMap.CollisionLayer.GetCellIndex(down.Value) == 17 || screen.tileMap.CollisionLayer.GetCellIndex(down.Value) == 15))
            {
                Rectangle cellRect = Engine.CreateRectForCell(down.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    if (!(motion.Y <= 0))
                        sprite.Position.Y = down.Value.Y * Engine.TileHeight - sprite.Bounds.Height;
                }
            }
            if (left != null && (screen.tileMap.CollisionLayer.GetCellIndex(left.Value) == 11 || screen.tileMap.CollisionLayer.GetCellIndex(left.Value) == 15 || screen.tileMap.CollisionLayer.GetCellIndex(left.Value) == 16))
            {
                Rectangle cellRect = Engine.CreateRectForCell(left.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    if (!(motion.X >= 0))
                        sprite.Position.X = left.Value.X * Engine.TileWidth + sprite.Bounds.Width;
                }

            }
            if (right != null && (screen.tileMap.CollisionLayer.GetCellIndex(right.Value) == 12 || screen.tileMap.CollisionLayer.GetCellIndex(right.Value) == 17 || screen.tileMap.CollisionLayer.GetCellIndex(right.Value) == 18))
            {
                Rectangle cellRect = Engine.CreateRectForCell(right.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    if (!(motion.X <= 0))
                        sprite.Position.X = right.Value.X * Engine.TileWidth - sprite.Bounds.Width;
                }

            }

            if (upLeft != null && screen.tileMap.CollisionLayer.GetCellIndex(upLeft.Value) == 16)
            {
                Rectangle cellRect = Engine.CreateRectForCell(upLeft.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    if (!(motion.X >= 0))
                        sprite.Position.X = spriteCell.X * Engine.TileWidth;
                    if (!(motion.Y <= 0))
                        sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
                }
            }

            if (upRight != null && screen.tileMap.CollisionLayer.GetCellIndex(upRight.Value) == 18)
            {
                Rectangle cellRect = Engine.CreateRectForCell(upRight.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    if (!(motion.X <= 0))
                        sprite.Position.X = spriteCell.X * Engine.TileWidth;
                    if (!(motion.Y >= 0))
                        sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
                }
            }

            if (downLeft != null && screen.tileMap.CollisionLayer.GetCellIndex(downLeft.Value) == 15)
            {
                Rectangle cellRect = Engine.CreateRectForCell(downLeft.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    if (!(motion.X >= 0))
                        sprite.Position.X = spriteCell.X * Engine.TileWidth;
                    if (!(motion.Y <= 0))
                        sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
                }
            }

            if (downRight != null && screen.tileMap.CollisionLayer.GetCellIndex(downRight.Value) == 17)
            {
                Rectangle cellRect = Engine.CreateRectForCell(downRight.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    if (!(motion.X <= 0))
                        sprite.Position.X = spriteCell.X * Engine.TileWidth;
                    if (!(motion.Y <= 0))
                        sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
                }
            }
        }

        public void CheckWalkableDamageAreaCollision(AnimatedSprite sprite, ref Vector2 motion, ref Point spriteCell, Point? upLeft, Point? up,
            Point? upRight, Point? left, Point? right, Point? downLeft, Point? down, Point? downRight, PlayerScreen screen)
        {
            if (up != null && screen.tileMap.CollisionLayer.GetCellIndex(up.Value) == 31)
            {
                Rectangle cellRect = Engine.CreateRectForCell(up.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    sprite.areTakingDamage = true;
                    sprite.Damage = 0.1f;
                    return;
                }
            }
            if (down != null && screen.tileMap.CollisionLayer.GetCellIndex(down.Value) == 31)
            {
                Rectangle cellRect = Engine.CreateRectForCell(down.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    sprite.areTakingDamage = true;
                    sprite.Damage = 0.1f;
                    return;
                }
            }
            if (left != null && screen.tileMap.CollisionLayer.GetCellIndex(left.Value) == 31)
            {
                Rectangle cellRect = Engine.CreateRectForCell(left.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    sprite.areTakingDamage = true;
                    sprite.Damage = 0.1f;
                    return;
                }

            }
            if (right != null && screen.tileMap.CollisionLayer.GetCellIndex(right.Value) == 31)
            {
                Rectangle cellRect = Engine.CreateRectForCell(right.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    sprite.areTakingDamage = true;
                    sprite.Damage = 0.1f;
                    return;
                }

            }

            if (upLeft != null && screen.tileMap.CollisionLayer.GetCellIndex(upLeft.Value) == 31)
            {
                Rectangle cellRect = Engine.CreateRectForCell(upLeft.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    sprite.areTakingDamage = true;
                    sprite.Damage = 0.1f;
                    return;
                }
            }

            if (upRight != null && screen.tileMap.CollisionLayer.GetCellIndex(upRight.Value) == 31)
            {
                Rectangle cellRect = Engine.CreateRectForCell(upRight.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    sprite.areTakingDamage = true;
                    sprite.Damage = 0.1f;
                    return;
                }
            }

            if (downLeft != null && screen.tileMap.CollisionLayer.GetCellIndex(downLeft.Value) == 31)
            {
                Rectangle cellRect = Engine.CreateRectForCell(downLeft.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    sprite.areTakingDamage = true;
                    sprite.Damage = 0.1f;
                    return;
                }
            }

            if (downRight != null && screen.tileMap.CollisionLayer.GetCellIndex(downRight.Value) == 31)
            {
                Rectangle cellRect = Engine.CreateRectForCell(downRight.Value);
                Rectangle spriteRect = sprite.Bounds;

                if (cellRect.Intersects(spriteRect))
                {
                    sprite.areTakingDamage = true;
                    sprite.Damage = 0.1f;
                    return;
                }
            }

            Point cell = Engine.ConvertPostionToCell(sprite.Origin);

            int colIndex = screen.tileMap.CollisionLayer.GetCellIndex(cell);

            if (colIndex == 31)
            {
                sprite.areTakingDamage = true;
                sprite.Damage = 0.1f;
                return;
            }


            sprite.areTakingDamage = false;
        }





        public Vector2 CheckCollisionForMotion(Vector2 motion, AnimatedSprite sprite , PlayerScreen screen )
        {
            Point cell = Engine.ConvertPostionToCell(sprite.Origin);
            //screen = (PlayerScreen)StateManager.CurrentState;
            int colIndex = screen.tileMap.CollisionLayer.GetCellIndex(cell);

            if (colIndex == 2)
                return motion * .2f;

            return motion;
        }

        public Vector2 CheckCollisionAutomaticMotion(Vector2 motion, AnimatedSprite sprite, PlayerScreen screen)
        {
            Point cell = Engine.ConvertPostionToCell(sprite.Origin);

            int colIndex = screen.tileMap.CollisionLayer.GetCellIndex(cell);

            if (colIndex == 21)
            {
                motion.X = 0;
                motion.Y = 1;
                motion *= 2;
            }
            if (colIndex == 22)
            {
                motion.X = 0;
                motion.Y = -1;
                motion *= 2;
            }
            if (colIndex == 23)
            {
                motion.X = 1;
                motion.Y = 0;
                motion *= 2;
            }
            if (colIndex == 24)
            {
                motion.X = -1;
                motion.Y = 0;
                motion *= 2;
            }

            return motion;
        }
    }
}
