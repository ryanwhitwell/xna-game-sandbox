using System;
using System.Linq;
using System.Collections.Generic;
using BadGuySmasher.Sprites.BadGuys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher.Sprites.BadGuys
{
  public class BadGuyGenerator : Sprite
  {
    private const int SpriteVectorRange = 200;
    
    private int       _maxSprites;
    private bool      _inSpawnCycle = true;
    private int       _spawnCycle = 0;
    private int       _totalSpawned = 0;
    private TimeSpan  _lastSpriteGeneratedAt = new TimeSpan();
    private Random    _random = new Random(DateTime.Now.Millisecond);
    private string    _spriteTextureAssetName;
    private TimeSpan  _timeBetweenGenerations;

    private List<Sprite> _generatedSprites = new List<Sprite>();

    public BadGuyGenerator(ContentManager contentManager, GraphicsDevice graphicsDevice, WorldMap worldMap, Vector2 spritePosition, int maxBadGuys, int secondsBetweenGeneratons, string generatorTextureAssetName, string spriteTextureAssetName) 
      : base(contentManager, graphicsDevice, worldMap, spritePosition, generatorTextureAssetName, new SpriteProperties(0, new Vector2(), new Vector2()))
    {
      if (string.IsNullOrWhiteSpace(spriteTextureAssetName))
      {
        throw new ArgumentNullException("badGuyTextureAssetName");
      }
      
      _maxSprites             = maxBadGuys;
      _spriteTextureAssetName = spriteTextureAssetName;
      _timeBetweenGenerations = new TimeSpan(0, 0, secondsBetweenGeneratons);
    }

    private Vector2 GetSpriteVector()
    {
      int randomX, randomY;

      do
      {
        randomX = _random.Next(SpriteVectorRange * -1, SpriteVectorRange);
        randomY = _random.Next(SpriteVectorRange * -1, SpriteVectorRange);
         
      } while (randomX == 0 && randomY == 0);

      return new Vector2(randomX, randomY);
    }

    private bool InRange(float one, float two, float tolerance)
    {
      return Math.Abs(one - two) <= tolerance;
    }

    protected override void Move(GameTime gameTime)
    {
      CleanUpDeadBadGuys();

      if (!_inSpawnCycle || _totalSpawned >= _maxSprites)
      {
        if (_inSpawnCycle)
        {
          if (_spawnCycle > 1)
          {
            return;
          }

          _spawnCycle++;
          _inSpawnCycle = false;
        }
        else if (_spawnCycle <= 1 && _generatedSprites.Count <= _maxSprites / 2)
        {
          _inSpawnCycle = true;
          _maxSprites += _maxSprites / 2;
        }
        
        return;
      }

      if (gameTime.TotalGameTime - _lastSpriteGeneratedAt > _timeBetweenGenerations)
      {
        Vector2 spriteVector = GetSpriteVector();
        Vector2 spritePosition = new Vector2(base.Position.X, base.Position.Y);
        BadGuy generatedSprite = new BadGuy(base.ContentManager, base.GraphicsDevice, base.WorldMap, spriteVector, spritePosition, _spriteTextureAssetName, new SpriteProperties(-5, new Vector2(250, 250), new Vector2(50, 50)));

        generatedSprite.DrawBounds = this.DrawBounds;

        _generatedSprites.Add(generatedSprite);

        base.WorldMap.Sprites.Add(generatedSprite);

        _lastSpriteGeneratedAt = gameTime.TotalGameTime;
        _totalSpawned++;
      }
    }

    private void CleanUpDeadBadGuys()
    {
      for (int i = _generatedSprites.Count() - 1; i >= 0; i--)
      {
        BadGuy badGuy = _generatedSprites[i] as BadGuy;
        if (!WorldMap.Sprites.Any(s => s.Equals(badGuy)))
        {
          _generatedSprites.RemoveAt(i);
        }
      }
    }
  }
}