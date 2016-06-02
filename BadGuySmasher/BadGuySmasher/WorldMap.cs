using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher
{
  public class WorldMap
  {
    private ICollection<ISprite> _sprites;
    private GraphicsDevice  _graphicsDevice;
    private Rectangle       _titleSafeArea;

    public WorldMap(GraphicsDevice graphicsDevice) 
    {
      _sprites = new Collection<ISprite>();
      _graphicsDevice = graphicsDevice;
      _titleSafeArea  = GetTitleSafeArea(.8f);
    }

    public ICollection<ISprite> Sprites
    {
      get { return _sprites; }
      private set { }
    }

    public void DrawSprites(GameTime gameTime)
    {
      foreach (ISprite sprite in _sprites)
      {
        sprite.Draw(gameTime);
      }
    }

    public void UpdateSpriteVectors(GameTime gameTime)
    {
      for (int i = 0; i < _sprites.Count; i++)
      {
        ISprite sprite = _sprites.ElementAt(i);
        sprite.UpdateSpriteVectors(gameTime);
      }
    }

    private CollisionResults GetBoundryCollisionResult(Rectangle spriteBounds)
    {
      CollisionResults collisionResults = new CollisionResults();
      
      int MaxX = _titleSafeArea.Width - spriteBounds.Width;
      int MinX = 0;
      int MaxY = _titleSafeArea.Height - spriteBounds.Height;
      int MinY = 0;

      if (spriteBounds.X < MinX)
      {
        collisionResults.X = CollisionResults.Result.TooSmall;
        collisionResults.XMove = MinX - spriteBounds.X;
      }
      else if(spriteBounds.X > MaxX)
      {
        collisionResults.X = CollisionResults.Result.TooBig;
        collisionResults.XMove = MaxX - spriteBounds.X;
      }
      
      if(spriteBounds.Y < MinY)
      {
        collisionResults.Y = CollisionResults.Result.TooSmall;
        collisionResults.YMove = MinY - spriteBounds.Y;
      }
      else if(spriteBounds.Y > MaxY)
      {
        collisionResults.Y = CollisionResults.Result.TooBig;
        collisionResults.YMove = MaxY - spriteBounds.Y;
      }

      return collisionResults;
    }

    public CollisionResults GetCollisionResults(ISprite sprite)
    {
      // If the object hits the boundary immediately return
      CollisionResults collisionResults = GetBoundryCollisionResult(sprite.GetSpriteBounds());

      if (!collisionResults.Empty)
      {
        return collisionResults;
      }

      return GetSpriteCollisionResult(sprite);
    }

    private CollisionResults GetSpriteCollisionResult(ISprite sprite)
    {
      CollisionResults collisionResults = new CollisionResults();
      
      IEnumerable<ISprite> otherStuff = this.Sprites.Where(x => !x.Id.Equals(sprite.Id));

      foreach (ISprite thing in otherStuff)
      {
        if (sprite.GetSpriteBounds().Intersects(thing.GetSpriteBounds()))
        {
          int spriteLeft = sprite.GetSpriteBounds().Left;
          int spriteRight = sprite.GetSpriteBounds().Right;

          int collisionObjectLeft = thing.GetSpriteBounds().Left;
          int collisionObjectRight = thing.GetSpriteBounds().Right;

          int spriteTop = sprite.GetSpriteBounds().Top;
          int spriteBottom = sprite.GetSpriteBounds().Bottom;

          int collisionObjectTop = thing.GetSpriteBounds().Top;
          int collisionObjectBottom = thing.GetSpriteBounds().Bottom;

          /*
          if (spriteLeft >= collisionObjectLeft && spriteLeft <= collisionObjectRight)
          {
            collisionResults.X = CollisionResults.Result.TooSmall;
          }
          else if (spriteRight >= collisionObjectLeft && spriteRight <= collisionObjectRight)
          {
            collisionResults.X = CollisionResults.Result.TooBig;
          }

          if (spriteTop <= collisionObjectBottom && spriteTop >= collisionObjectTop)
          {
            collisionResults.Y = CollisionResults.Result.TooSmall;
          }
          else if (spriteBottom >= collisionObjectTop && spriteBottom <= collisionObjectBottom)
          {
            collisionResults.Y = CollisionResults.Result.TooBig;
          }
          */

          int threshold = 20;
          if (InRange(spriteLeft, collisionObjectRight, threshold))
          {
            collisionResults.X = CollisionResults.Result.TooSmall;
            collisionResults.XMove = collisionObjectRight - spriteLeft;
          }
          else if (InRange(spriteRight, collisionObjectLeft, threshold))
          {
            collisionResults.X = CollisionResults.Result.TooBig;
            collisionResults.XMove = collisionObjectLeft - spriteRight;
          }

          if (InRange(spriteTop, collisionObjectBottom, threshold))
          {
            collisionResults.Y = CollisionResults.Result.TooSmall;
            collisionResults.YMove = spriteTop - collisionObjectBottom;
          }
          else if (InRange(spriteBottom, collisionObjectTop, threshold))
          {
            collisionResults.Y = CollisionResults.Result.TooBig;
            collisionResults.YMove = collisionObjectTop - spriteBottom;
          }

          return collisionResults;
        }
      }

      return collisionResults;
    }

    private bool InRange(int one, int two, int tolerance)
    {
      return Math.Abs(one - two) <= tolerance;
    }

    private Rectangle GetTitleSafeArea(float percent)
    {
      Rectangle retval = new Rectangle(
      _graphicsDevice.Viewport.X,
      _graphicsDevice.Viewport.Y,
      _graphicsDevice.Viewport.Width,
      _graphicsDevice.Viewport.Height);

      #if XBOX
        float border = (1 - percent) / 2;
        retval.X = (int)(border * retval.Width);
        retval.Y = (int)(border * retval.Height);
        retval.Width = (int)(percent * retval.Width);
        retval.Height = (int)(percent * retval.Height);
        return retval;            
      #else
        return retval;
      #endif
    }
  }
}