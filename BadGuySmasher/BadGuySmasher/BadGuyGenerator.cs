using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher
{
  class BadGuyGenerator : SpriteBatch
  {
    private Texture2D       _texture;
    private ContentManager  _contentManager;
    private GraphicsDevice  _graphicsDevice;
    private Rectangle       _titleSafeArea;
    private Vector2         _spritePosition;
    private Vector2         _spriteSpeed;
    private int             _maxBadGuys;
    private TimeSpan        _lastGuyGeneratedAt = new TimeSpan();
    private Random          _random = new Random((int)DateTime.Now.Ticks);
    
    private readonly TimeSpan  _timeBetweenGenerations = new TimeSpan(0, 0, 3);

    private List<BasicBadGuy> _basicBadGuys = new List<BasicBadGuy>();

    public BadGuyGenerator(ContentManager contentManager, GraphicsDevice graphicsDevice, Vector2 spritePosition, int maxBadGuys) : base(graphicsDevice)
    {
      if (contentManager == null)
      {
        throw new ArgumentNullException("contentManager");
      }

      if (graphicsDevice == null)
      {
        throw new ArgumentNullException("graphicsDevice");
      }

      if (spritePosition == null)
      {
        throw new ArgumentNullException("spritePosition");
      }

      _texture        = contentManager.Load<Texture2D>("BadGuyGenerator");
      _contentManager = contentManager;
      _graphicsDevice = graphicsDevice;
      _titleSafeArea  = GetTitleSafeArea(.8f);
      _spriteSpeed    = new Vector2(0, 0);
      _spritePosition = spritePosition;
      _maxBadGuys     = maxBadGuys;
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

    public void Update(GameTime gameTime)
    {
      foreach (BasicBadGuy badGuy in _basicBadGuys)
      {
        badGuy.UpdateSpriteVectors(gameTime);
      }

      if (_basicBadGuys.Count >= _maxBadGuys)
      {
        return;
      }

      if (gameTime.TotalGameTime - _lastGuyGeneratedAt > _timeBetweenGenerations)
      {
        Vector2 vectorSpeed = new Vector2(_random.Next(-200, 200), _random.Next(-200, 200));

        Vector2 vectorPosition = new Vector2(_spritePosition.X + _random.Next(-100, 100), _spritePosition.Y + _random.Next(-100, 100));
        BasicBadGuy basicBadGuy = new BasicBadGuy(_contentManager, _graphicsDevice, vectorSpeed, vectorPosition);

        _basicBadGuys.Add(basicBadGuy);

        _lastGuyGeneratedAt = gameTime.TotalGameTime;
      }
    }

    public void Draw(GameTime gameTime)
    {
      this.Begin();
      this.Draw(_texture, _spritePosition, Color.White);

      foreach (BasicBadGuy badGuy in _basicBadGuys)
      {
        badGuy.Draw(gameTime);
      }

      this.End();
    }
  }
}
