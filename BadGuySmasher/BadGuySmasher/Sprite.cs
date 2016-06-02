using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher
{
  public class Sprite : SpriteBatch, ISprite
  {
    private Texture2D       _texture;
    private GraphicsDevice  _graphicsDevice;
    private Vector2         _spritePosition;
    private Vector2         _spriteVelocity;
    private Rectangle       _spriteBounds;
    private WorldMap        _worldMap;
    private string          _id;

    public Sprite(ContentManager contentManager, GraphicsDevice graphicsDevice, WorldMap worldMap, Vector2 spriteSpeed, Vector2 spritePosition) : base(graphicsDevice)
    {
      if (contentManager == null)
      {
        throw new ArgumentNullException("contentManager");
      }

      if (graphicsDevice == null)
      {
        throw new ArgumentNullException("graphicsDevice");
      }

      if (spriteSpeed == null)
      {
        throw new ArgumentNullException("spriteSpeed");
      }

      if (spritePosition == null)
      {
        throw new ArgumentNullException("spritePosition");
      }

      if (worldMap == null)
      {
        throw new ArgumentNullException("worldMap");
      }

      _texture        = contentManager.Load<Texture2D>("badguy");
      _graphicsDevice = graphicsDevice;
      _spriteVelocity = spriteSpeed;
      _spritePosition = spritePosition;
      _spriteBounds   = new Rectangle ((int)(spritePosition.X - _texture.Width / 2), (int)(spritePosition.Y - _texture.Height / 2), _texture.Width, _texture.Height );
      _worldMap       = worldMap;
      _id             = Guid.NewGuid().ToString("N");
    }

    public Texture2D Texture { get { return _texture; } private set { } }
    public Vector2 SpritePosition { get { return _spritePosition; } }

    public string Id { get { return _id; } private set { } }

    public Rectangle GetSpriteBounds()
    {
      return _spriteBounds;
    }

    public void Draw(GameTime gameTime)
    {
      this.Begin();
      this.Draw(_texture, _spritePosition, Color.White);
      this.End();

      this.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
      RasterizerState state = new RasterizerState();
      state.FillMode = FillMode.WireFrame;
      this.GraphicsDevice.RasterizerState = state;

      this.Draw(_texture, _spritePosition, Color.White);
      this.End();
    }

    public void UpdateSpriteVectors(GameTime gameTime)
    {
      //Move the sprite by speed, scaled by elapsed time.
      Vector2 originalPosition = _spritePosition;

      _spritePosition += _spriteVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
      UpdateSpriteBounds(_spritePosition);

      CollisionResults collisionResults = _worldMap.GetCollisionResults(this);

      if (collisionResults.X != CollisionResults.Result.None)
      {
        _spriteVelocity.X *= -1;
        _spritePosition.X = originalPosition.X;
      }

      if (collisionResults.Y != CollisionResults.Result.None)
      {
        _spriteVelocity.Y *= -1;
        _spritePosition.Y = originalPosition.Y;
      }

      UpdateSpriteBounds(_spritePosition);
    }

    private void UpdateSpriteBounds(Vector2 spritePosition)
    {
      _spriteBounds.X = (int)spritePosition.X;
      _spriteBounds.Y = (int)spritePosition.Y;
    }
  }
}