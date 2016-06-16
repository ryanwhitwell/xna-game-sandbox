using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher
{
  public class SpriteGenerator : Sprite
  {
    private const int SpriteVectorRange = 200;
    
    private int       _maxSprites;
    private TimeSpan  _lastSpriteGeneratedAt = new TimeSpan();
    private Random    _random = new Random(DateTime.Now.Millisecond);
    private string    _spriteTextureAssetName;
    private TimeSpan  _timeBetweenGenerations;

    private List<Sprite> _generatedSprites = new List<Sprite>();

    public SpriteGenerator(ContentManager contentManager, GraphicsDevice graphicsDevice, WorldMap worldMap, Vector2 spritePosition, int maxBadGuys, int secondsBetweenGeneratons, string generatorTextureAssetName, string spriteTextureAssetName) 
      : base(contentManager, graphicsDevice, worldMap, spritePosition, generatorTextureAssetName, new SpriteProperties(5, new Vector2(), new Vector2()))
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

    public override void Update(GameTime gameTime)
    {
      if (_generatedSprites.Count >= _maxSprites)
      {
        return;
      }

      if (gameTime.TotalGameTime - _lastSpriteGeneratedAt > _timeBetweenGenerations)
      {
        Vector2 spriteVector    = GetSpriteVector();
        Vector2 spritePosition  = new Vector2(base.Position.X, base.Position.Y);
        Sprite  generatedSprite = new Sprite(base.ContentManager, base.GraphicsDevice, base.WorldMap, spriteVector, spritePosition, _spriteTextureAssetName, new SpriteProperties(0, new Vector2(250, 250), new Vector2(50, 50)));

        generatedSprite.DrawBounds = this.DrawBounds;

        _generatedSprites.Add(generatedSprite);

        base.WorldMap.Sprites.Add(generatedSprite);

        _lastSpriteGeneratedAt = gameTime.TotalGameTime;
      }

      base.Update(gameTime);
    }
  }
}