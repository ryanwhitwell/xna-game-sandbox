using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher
{
  public class BasicBadGuy : SpriteBatch
  {
    private Texture2D       _texture;
    private GraphicsDevice  _graphicsDevice;
    private Rectangle       _titleSafeArea;
    private Vector2         _spritePosition;
    private Vector2         _spriteSpeed;

    public BasicBadGuy(ContentManager contentManager, GraphicsDevice graphicsDevice, Vector2 spriteSpeed, Vector2 spritePosition) : base(graphicsDevice)
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

      _texture        = contentManager.Load<Texture2D>("basicbadguy");
      _graphicsDevice = graphicsDevice;
      _titleSafeArea  = GetTitleSafeArea(.8f);
      _spriteSpeed    = spriteSpeed;
      _spritePosition = spritePosition;
    }

    public Texture2D Texture { get { return _texture; } private set { } }

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

    public void Draw(GameTime gameTime)
    {
      this.Begin();
      this.Draw(_texture, _spritePosition, Color.White);
      this.End();
    }

    public void UpdateSpriteVectors(GameTime gameTime)
    {
      // Move the sprite by speed, scaled by elapsed time.
      _spritePosition += _spriteSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

      int MaxX = _titleSafeArea.Width - _texture.Width;
      int MinX = 0;
      int MaxY = _titleSafeArea.Height - _texture.Height;
      int MinY = 0;

      // Check for bounce.
      if (_spritePosition.X > MaxX)
      {
        _spriteSpeed.X *= -1;
        _spritePosition.X = MaxX;
      }

      else if (_spritePosition.X < MinX)
      {
        _spriteSpeed.X *= -1;
        _spritePosition.X = MinX;
      }

      if (_spritePosition.Y > MaxY)
      {
        _spriteSpeed.Y *= -1;
        _spritePosition.Y = MaxY;
      }
      else if (_spritePosition.Y < MinY)
      {
        _spriteSpeed.Y *= -1;
        _spritePosition.Y = MinY;
      }
    }
  }
}