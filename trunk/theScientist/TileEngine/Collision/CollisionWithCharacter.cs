using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine.Sprite;
using TileEngine;

namespace TileEngine.Collision
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
                    player.Position =
                        s.Position - (d * (player.CollisionRadius + s.CollisionRadius));
                }
            }


            foreach (BaseSprite sprite in SpriteObject)
            {
                for (int Projectile = 0; Projectile < playerprojectiles.Count(); Projectile++)
                {
                    if (BaseSprite.AreColliding(playerprojectiles[Projectile], sprite) && SpriteObjectInGameWorld.Contains(sprite))
                    {

                        playerprojectiles.RemoveAt(Projectile);
                        SpriteObjectInGameWorld.Remove(sprite);
                        renderList.Remove(sprite);
                    }
                }
            }

            foreach (AnimatedSprite sprite in AnimatedSpriteObject)
            {
                for (int Projectile = 0; Projectile < playerprojectiles.Count(); Projectile++)
                {
                    if (BaseSprite.AreColliding(playerprojectiles[Projectile], sprite) && SpriteObjectInGameWorld.Contains(sprite))
                    {
                        sprite.Life -= playerprojectiles[Projectile].damageofprojectile;
                        playerprojectiles.RemoveAt(Projectile);

                        if (sprite.Life <= 0)
                        {
                            SpriteObjectInGameWorld.Remove(sprite);
                            renderList.Remove(sprite);
                            sprite.Life = sprite.FullHp;
                        }
                    }
                }
            }
        }
    }
}
