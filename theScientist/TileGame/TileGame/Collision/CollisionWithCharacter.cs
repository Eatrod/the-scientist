using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine;
using TileEngine.Sprite;

namespace TileGame.Collision
{
    public class CollisionWithCharacter
    {

        public void UpdateCollisionForCharacters(
             GameTime gameTime,
             List<BaseSprite> SpriteObjectInGameWorld,
             PlayerCharacter player,
             List<BaseSprite> SpriteObject,
             List<AnimatedProjectile> playerprojectiles,
             List<BaseSprite> renderList,
             List<BaseSprite> AnimatedSpriteObject)
        {
            foreach (BaseSprite s in SpriteObjectInGameWorld)
            {
                s.Update(gameTime);



                if (BaseSprite.AreColliding(player, s))
                {
                    Vector2 d = Vector2.Normalize(s.Origin - player.Origin);

                    if (s is CharacterSprite)
                    {
                        player.Position =
                            s.Position - (d * (player.CollisionRadius + s.CollisionRadius));
                    }

                    if (s is TreeStandingBridge)
                    {
                        player.Position = player.Position + (-d) * player.Speed;
                    }
                                   
                }
            }


            foreach (BaseSprite sprite in SpriteObject)
            {
                for (int Projectile = 0; Projectile < playerprojectiles.Count(); Projectile++)
                {
                    if (BaseSprite.AreColliding(playerprojectiles[Projectile], sprite) && SpriteObjectInGameWorld.Contains(sprite))
                    {

                        playerprojectiles.RemoveAt(Projectile);
                        //SpriteObjectInGameWorld.Remove(sprite);
                        //renderList.Remove(sprite);
                    }
                }
            }

            foreach (AnimatedSprite sprite in AnimatedSpriteObject)
            {
                for (int Projectile = 0; Projectile < playerprojectiles.Count(); Projectile++)
                {
                    //if (BaseSprite.AreColliding(playerprojectiles[Projectile], sprite) && SpriteObjectInGameWorld.Contains(sprite))
                    //{
                    if (playerprojectiles[Projectile].Bounds.Intersects(new Rectangle((int)sprite.Position.X + 20,
                        (int)sprite.Position.Y + 10, 10, 55)) && sprite.Life > 0)
                    {
                        sprite.Life -= playerprojectiles[Projectile].damageofprojectile;
                        //sprite.ArrowDirection = playerprojectiles[Projectile].Origin - sprite.Origin;
                        sprite.HitByArrowMethod(playerprojectiles[Projectile]);
                        sprite.AttackersDirection = player.Position - sprite.Position;
                        playerprojectiles.RemoveAt(Projectile);
                        //sprite.AggroStartingPosition = sprite.Position;
                        //sprite.HitByArrow = true;                       
                        //sprite.Aggro = true;
                        //if (sprite.Life <= 0)
                        //{
                        //    //SpriteObjectInGameWorld.Remove(sprite);
                        //    //renderList.Remove(sprite);
                        //    //sprite.Life = sprite.FullHp;                         
                        //}
                    }
                    //}
                }
            }
        }

    } 
        
}
