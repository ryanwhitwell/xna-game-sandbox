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
      foreach (ISprite sprite in _sprites)
      {
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
      }
      else if(spriteBounds.X > MaxX)
      {
        collisionResults.X = CollisionResults.Result.TooBig;
      }
      
      if(spriteBounds.Y < MinY)
      {
        collisionResults.Y = CollisionResults.Result.TooSmall;
      }
      else if(spriteBounds.Y > MaxY)
      {
        collisionResults.Y = CollisionResults.Result.TooBig;
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

          if (spriteLeft >= collisionObjectLeft && spriteLeft <= collisionObjectRight)
          {
            collisionResults.X = CollisionResults.Result.TooSmall;
          }
          else if (spriteRight >= collisionObjectLeft && spriteRight <= collisionObjectRight)
          {
            collisionResults.X = CollisionResults.Result.TooBig;
          }

          if (spriteTop >= collisionObjectBottom && spriteTop <= collisionObjectTop)
          {
            collisionResults.Y = CollisionResults.Result.TooSmall;
          }
          else if (spriteBottom >= collisionObjectTop && spriteBottom <= collisionObjectBottom)
          {
            collisionResults.Y = CollisionResults.Result.TooBig;
          }

          return collisionResults;
        }
      }

      return collisionResults;
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