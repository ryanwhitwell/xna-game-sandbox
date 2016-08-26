using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadGuySmasher.Sprites;
using BadGuySmasher.Sprites.Interfaces;

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

      HandleSpritePhysics(sprite, collisionResults);

      HandleSpritePhysics(sprite2, collisionResults);
    }

    private void HandleSpritePhysics(Sprite sprite, CollisionResults collisionResults)
    {
      if (sprite is ISquishy)
      {
        ISquishy squishySprite = sprite as ISquishy;
        squishySprite.Squish(collisionResults.Sprite.Squishiness);
      }
    }
  }
}
