using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadGuySmasher.Sprites.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher.Sprites.BadGuys
{
  public class BadGuy : Sprite, ISquishy, IBadGuy
  {
    public BadGuy(ContentManager contentManager, GraphicsDevice graphicsDevice, WorldMap worldMap, Vector2 velocity, Vector2 position, string textureAssetName, SpriteProperties spriteProperties)
      : base(contentManager, graphicsDevice, worldMap, velocity, position, textureAssetName, spriteProperties)
    {
    }

    protected override void Move(GameTime gameTime)
    {
      // Move the sprite by speed, scaled by elapsed time.
      Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    protected override void HandleCollesionResults(Vector2 originalPosition, CollisionResults collisionResults)
    {
      bool changedX = false;
      bool changedY = false;

      if (collisionResults.XMove != 0)
      {
        bool velocityPos = Velocity.X > 0;
        bool movePos     = collisionResults.XMove > 0;

        if (velocityPos != movePos)
        {
          SetXVelocity(Velocity.X * -1);
          SetXPosition(originalPosition.X);
          changedX    = true;
        }
      } 

      if (collisionResults.YMove != 0)
      {
        bool velocityPos = Velocity.Y > 0;
        bool movePos     = collisionResults.YMove > 0;

        if (velocityPos != movePos)
        {
          SetYVelocity(Velocity.Y * -1);
          SetYPosition(originalPosition.Y);
          changedY    = true; 
        }
      }

      // If we are trying to change both the X and Y direction, see where we collided the most
      // and only change that direction.
      if (changedX && changedY)
      {
        if (Math.Abs(collisionResults.XMove) > Math.Abs(collisionResults.YMove))
        {
          SetXVelocity(Velocity.X * -1);
        }
        else if (Math.Abs(collisionResults.YMove) > Math.Abs(collisionResults.XMove))
        {
          SetYVelocity(Velocity.Y * -1);
        }
      }
    }

    public void Squish(float squishiness)
    {
      float totalSquishy = Squishiness + squishiness;

      bool  speedUp         = totalSquishy < 0;
      float absTotalSquishy = Math.Abs(totalSquishy);

      if (speedUp)
      {
        SpeedUpX(absTotalSquishy);
        SpeedUpY(absTotalSquishy);
      }
      else
      {
        SlowDownX(absTotalSquishy);
        SlowDownY(absTotalSquishy);
      }

      if (totalSquishy > 0)
      {
        if (XTooSlow || YTooSlow)
        {
          SpeedUpX(absTotalSquishy);
          SpeedUpY(absTotalSquishy);
        }
      }
      else if (totalSquishy < 0)
      {
        if (XTooFast || YTooFast)
        {
          SlowDownX(absTotalSquishy);
          SlowDownY(absTotalSquishy);
        }
      }
    }

    public void Die()
    {
      Delete();
    }
  }
}
