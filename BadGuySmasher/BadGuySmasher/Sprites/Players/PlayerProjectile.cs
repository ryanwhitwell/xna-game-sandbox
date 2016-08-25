using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadGuySmasher.Sprites.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher.Sprites.Players
{
  public class PlayerProjectile : Sprite, IPlayerProjectile
  {
    public const int MAX_DISTANCE         = 500;
    public const int ProjectileMoveSpeed  = 900;
 
    public bool Visible = false;
 
    Vector2 _startPosition;
    Vector2 _speed;
    Vector2 _direction;

    public PlayerProjectile(ContentManager contentManager, GraphicsDevice graphicsDevice, WorldMap worldMap, Vector2 position, Vector2 speed, Vector2 direction, string textureAssetName) : base(contentManager, graphicsDevice, worldMap, position, textureAssetName, null) 
    { 
      _startPosition  = position;
      _speed          = speed;
      _direction      = direction;
      //Visible = true;
    }
    
    public override void Update(GameTime gameTime)
    {
      //if (Vector2.Distance(_startPosition, Position) > MAX_DISTANCE)
      //{
      //  Visible = false;
      //}

      UpdatePosition(gameTime, _direction, _speed);
 
      //if (Visible == true)
      //{
      //  UpdatePosition(gameTime, _direction, _speed);
      //}
      UpdateSpriteBounds(Position);
    }

    private void UpdatePosition(GameTime gameTime, Vector2 speed, Vector2 direction)
    {
      Position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    //public void Fire(Vector2 startPosition, Vector2 speed, Vector2 direction)
    //{
    //  Position        = startPosition;            
    //  _startPosition  = startPosition;
    //  _speed          = speed;
    //  _direction      = direction;
    //  Visible         = true;
    //}
  }
}
