﻿using System;
using System.Linq;
using System.Collections.Generic;
using BadGuySmasher.Sprites.BadGuys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BadGuySmasher.Sprites.Interfaces;
using BadGuySmasher.Sprites.Players;
using BadGuySmasher.GameManagement;

namespace BadGuySmasher.Sprites.BadGuys
{
  public class BadGuyGenerator : Sprite, IBadGuy
  {
    private const int SpriteVectorRange = 200;
    private const int GeneratedBadGuyHP = 20;
    
    private int       _maxSprites;
    private bool      _inSpawnCycle = true;
    private int       _spawnCycle = 0;
    private int       _totalSpawned = 0;
    private TimeSpan  _lastSpriteGeneratedAt = new TimeSpan();
    private Random    _random = new Random(DateTime.Now.Millisecond);
    private string    _spriteTextureAssetName;
    private TimeSpan  _timeBetweenGenerations;
    private int       _hitPoints;

    private List<Sprite> _generatedSprites = new List<Sprite>();

    public BadGuyGenerator(ContentManager contentManager, GraphicsDevice graphicsDevice, Level worldMap, Vector2 spritePosition, int maxBadGuys, int secondsBetweenGeneratons, int hitPoints, string generatorTextureAssetName, string spriteTextureAssetName) 
      : base(contentManager, graphicsDevice, worldMap, spritePosition, generatorTextureAssetName, new SpriteProperties(0, new Vector2(), new Vector2()))
    {
      if (string.IsNullOrWhiteSpace(spriteTextureAssetName))
      {
        throw new ArgumentNullException("badGuyTextureAssetName");
      }
      
      _maxSprites             = maxBadGuys;
      _hitPoints              = hitPoints;
      _spriteTextureAssetName = spriteTextureAssetName;
      _timeBetweenGenerations = new TimeSpan(0, 0, secondsBetweenGeneratons);
    }

    public int HitPoints
    { 
      get { return _hitPoints; } 
      set { _hitPoints = value; } 
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

    public override void Draw(GameTime gameTime)
    {
      this.Begin();

      string xText = "HP:" + _hitPoints.ToString();
        
      this.DrawString(SpriteFont, xText, new Vector2(Bounds.Right + 5, Bounds.Top + 30), Color.WhiteSmoke);

      this.End();
      
      base.Draw(gameTime);
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
        BadGuy generatedSprite = new BadGuy(base.ContentManager, base.GraphicsDevice, base.WorldMap, spriteVector, spritePosition, GeneratedBadGuyHP, _spriteTextureAssetName, new SpriteProperties(-5, new Vector2(250, 250), new Vector2(50, 50)));

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

    public void Die()
    {
      Delete();
    }

    public void GetHit(PlayerProjectile playerProjectile)
    {
      _hitPoints -= playerProjectile.Power;

      if (_hitPoints <= 0)
      {
        this.Die();
      }
    }
  }
}