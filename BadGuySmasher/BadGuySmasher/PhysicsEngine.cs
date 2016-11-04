using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadGuySmasher.Sprites;
using BadGuySmasher.Sprites.Interfaces;
using BadGuySmasher.Sprites.Players;

namespace BadGuySmasher
{
  public class PhysicsEngine
  {
    public void DoPhysics(Sprite sprite, CollisionResults collisionResults)
    {
      if (collisionResults.Empty)
      {
        return;
      }

      Sprite sprite2 = collisionResults.Sprite;
      
      HandleSpritePhysics(sprite, sprite2);

      HandleSpritePhysics(sprite2, sprite);
    }

    private void HandleSpritePhysics(Sprite sprite1, Sprite sprite2)
    {
      if (sprite1 is ISquishy)
      {
        ISquishy squishySprite = sprite1 as ISquishy;
        squishySprite.Squish(sprite2.Squishiness);
      }
      
      if (sprite1 is IProjectile)
      {
        IProjectile projectileSprite = sprite1 as IProjectile;

        if (!(sprite2 is Player))
        {
          projectileSprite.Dissolve();
        }
      }
      
      if (sprite1 is IBadGuy)
      {
        IBadGuy badGuySprite = sprite1 as IBadGuy;

        PlayerProjectile playerProjectile = sprite2 as PlayerProjectile;

        if (playerProjectile != null)
        {
          badGuySprite.GetHit(playerProjectile);
        }
      }
    }
  }
}
