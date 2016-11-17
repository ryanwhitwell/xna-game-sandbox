using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BadGuySmasher.Sprites;
using BadGuySmasher.Sprites.BadGuys;
using BadGuySmasher.Sprites.Interfaces;
using BadGuySmasher.Sprites.Objects;
using BadGuySmasher.Sprites.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher.GameManagement
{
  public sealed class WorldMap : IWorldMap
  {
    private ICollection<ISprite>  _sprites;
    private ContentManager        _content;
    private GraphicsDevice        _graphicsDevice;
    private Rectangle             _titleSafeArea;
    private PhysicsEngine         _physicsEngine;
    private PlayerGenerator       _playerGenerator;
    private string                _spriteAssetFilePath;

    public WorldMap(ContentManager content, GraphicsDevice graphicsDevice, string spriteAssetFilePath) 
    {
      _sprites              = new Collection<ISprite>();
      _content              = content;
      _graphicsDevice       = graphicsDevice;
      _titleSafeArea        = GetTitleSafeArea(.8f);
      _physicsEngine        = new PhysicsEngine();
      _spriteAssetFilePath  = spriteAssetFilePath;
    }

    public ICollection<ISprite> Sprites
    {
      get { return _sprites; }
    }

    public void DrawSprites(GameTime gameTime)
    {
      foreach (Sprite sprite in _sprites)
      {
        sprite.Draw(gameTime);
      }
    }

    public void UpdateSpriteVectors(GameTime gameTime)
    {
      for (int i = 0; i < _sprites.Count; i++)
      {
        ISprite sprite = _sprites.ElementAt(i);
        sprite.Update(gameTime);
      }
    }

    public void LoadMap()
    {
      // TODO Config file - Needs to be re-factored to read in the assets for itself from a file. Need something like a custom configuration section for this or JSON file
      Vector2 badGuyGeneratorPosition   = new Vector2(300.0f, 300.0f);
      BadGuyGenerator _badGuyGenerator  = new BadGuyGenerator(_content, _graphicsDevice, this, badGuyGeneratorPosition, 20, 1, 60, "BadGuyGenerator", "badguy");
      _badGuyGenerator.DrawBounds = true;

      Vector2 wallPosition = new Vector2(700.0f, 300.0f);
      Wall wall = new Wall(_content, _graphicsDevice, this, wallPosition, "wall", new SpriteProperties(50, new Vector2(), new Vector2()));
      wall.DrawBounds = true;

      Vector2 playerGeneratorPosition = new Vector2(200.0f, 600.0f);
      _playerGenerator = new PlayerGenerator(_content, _graphicsDevice, this, playerGeneratorPosition, "PlayerGenerator", "player");
      _playerGenerator.DrawBounds = true;

      Sprites.Add(_badGuyGenerator);
      Sprites.Add(wall);
      Sprites.Add(_playerGenerator);
    }

    public void SetNumberOfPlayers(int numberOfPlayers)
    {
      _playerGenerator.SetNumberOfPlayers(numberOfPlayers);
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
        collisionResults.XMove = MinX - spriteBounds.X;
      }
      else if(spriteBounds.X > MaxX)
      {
        collisionResults.XMove = MaxX - spriteBounds.X;
      }
      
      if(spriteBounds.Y < MinY)
      {
        collisionResults.YMove = MinY - spriteBounds.Y;
      }
      else if(spriteBounds.Y > MaxY)
      {
        collisionResults.YMove = MaxY - spriteBounds.Y;
      }

      return collisionResults;
    }

    public CollisionResults GetCollisionResults(Sprite sprite)
    {
      // If the object hits the boundary immediately return
      CollisionResults collisionResults = GetBoundryCollisionResult(sprite.Bounds);

      if (!collisionResults.Empty)
      {
        return collisionResults;
      }

      collisionResults = GetSpriteCollisionResult(sprite);

      // Process physics
      _physicsEngine.DoPhysics(sprite, collisionResults);

      return collisionResults;
    }

    private CollisionResults GetSpriteCollisionResult(Sprite sprite)
    {
      CollisionResults collisionResults = new CollisionResults();
      
      IEnumerable<ISprite> otherStuff = this.Sprites.Where(x => !x.Id.Equals(sprite.Id));

      foreach (Sprite thing in otherStuff)
      {
        if (sprite.Bounds.Intersects(thing.Bounds))
        {
          int spriteLeft  = sprite.Bounds.Left;
          int spriteRight = sprite.Bounds.Right;

          int collisionObjectLeft  = thing.Bounds.Left;
          int collisionObjectRight = thing.Bounds.Right;

          int spriteTop    = sprite.Bounds.Top;
          int spriteBottom = sprite.Bounds.Bottom;

          int collisionObjectTop    = thing.Bounds.Top;
          int collisionObjectBottom = thing.Bounds.Bottom;

          if (spriteLeft >= collisionObjectLeft && spriteLeft <= collisionObjectRight)
          {
            collisionResults.XMove  = collisionObjectRight - spriteLeft;
          }
          else if (spriteRight >= collisionObjectLeft && spriteRight <= collisionObjectRight)
          {
            collisionResults.XMove = collisionObjectLeft - spriteRight;
          }
          
          if (spriteTop <= collisionObjectBottom && spriteTop >= collisionObjectTop)
          {
             collisionResults.YMove = collisionObjectBottom - spriteTop;
          }
          else if (spriteBottom >= collisionObjectTop && spriteBottom <= collisionObjectBottom)
          {
            collisionResults.YMove = collisionObjectTop - spriteBottom;
          }

          collisionResults.Sprite = thing;

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