using System;
using BadGuySmasher.Sprites.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher.Sprites
{
  public class Sprite : SpriteBatch, ISprite
  {
    private Texture2D         _texture;
    private GraphicsDevice    _graphicsDevice;
    private Vector2           _position;
    private Vector2           _velocity;
    private Rectangle         _bounds;
    private WorldMap          _worldMap;
    private ContentManager    _contentManager;
    private string            _id;
    private SpriteFont        _spriteFont;
    private float             _rotation;
    public  SpriteProperties  _spriteProperties;

    public Sprite(ContentManager contentManager, GraphicsDevice graphicsDevice, WorldMap worldMap, Vector2 velocity, Vector2 position, string textureAssetName, SpriteProperties spriteProperties) : base(graphicsDevice)
    {
      if (contentManager == null)
      {
        throw new ArgumentNullException("contentManager");
      }

      if (graphicsDevice == null)
      {
        throw new ArgumentNullException("graphicsDevice");
      }

      if (velocity == null)
      {
        throw new ArgumentNullException("velocity");
      }

      if (position == null)
      {
        throw new ArgumentNullException("position");
      }

      if (worldMap == null)
      {
        throw new ArgumentNullException("worldMap");
      }

      if (string.IsNullOrWhiteSpace(textureAssetName))
      {
        throw new ArgumentNullException("textureAssetName");
      }

      _texture          = contentManager.Load<Texture2D>(textureAssetName);
      _graphicsDevice   = graphicsDevice;
      _velocity         = velocity;
      _position         = position;
      _bounds           = new Rectangle ((_texture.Bounds.X), (_texture.Bounds.Y), _texture.Width, _texture.Height );
      _worldMap         = worldMap;
      _contentManager   = contentManager;
      _id               = Guid.NewGuid().ToString("N");
      _spriteFont       = _contentManager.Load<SpriteFont>("SpriteFont");
      _spriteProperties = spriteProperties;
 
      if (_spriteProperties == null)
      {
        _spriteProperties = new SpriteProperties();
      }
    }

    public Sprite(ContentManager contentManager, GraphicsDevice graphicsDevice, WorldMap worldMap, Vector2 position, string textureAssetName, SpriteProperties spriteProperties)
      :this(contentManager, graphicsDevice, worldMap, new Vector2(0, 0), position, textureAssetName, spriteProperties)
    {
    }

    public Texture2D Texture { get { return _texture; } private set { } }

    public Vector2 Position 
    { 
      get { return _position; }
      set { _position = value; }
    }

    public void SetXPosition(float value)
    {
      _position.X = value;
    }

    public void SetYPosition(float value)
    {
      _position.Y = value;
    }

    public void SetXVelocity(float value)
    {
      _velocity.X = value;
    }

    public void SetYVelocity(float value)
    {
      _velocity.Y = value;
    }

    public float Rotation
    {
      get { return _rotation; }
      set { _rotation = value; }
    }


    public string         Id             { get { return _id; } }
    public Rectangle      Bounds         { get { return _bounds; } }
    public WorldMap       WorldMap       { get { return _worldMap; } }
    public ContentManager ContentManager { get { return _contentManager; } }
    public bool           DrawBounds     { get; set; }
    public float          Squishiness    { get { return _spriteProperties.Squishiness; } }

    public virtual void Draw(GameTime gameTime)
    {
      Vector2 textureCenter = new Vector2(_texture.Bounds.Width / 2, _texture.Bounds.Height / 2);

      this.Begin();

      Vector2 drawPosition = _position;

      if (_rotation != 0.0f)
      {
        drawPosition.X += _texture.Bounds.Width / 2;
        drawPosition.Y += _texture.Bounds.Height / 2;

        this.Draw(_texture, drawPosition, null, Color.White, _rotation, textureCenter, 1.0f, SpriteEffects.None, 0.0f);
      }
      else
      {
        this.Draw(_texture, _position, null, Color.White);
      }

      this.End();

      if (DrawBounds)
      {
        this.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
        RasterizerState state               = new RasterizerState();
        state.FillMode                      = FillMode.WireFrame;
        this.GraphicsDevice.RasterizerState = state;

        if (_rotation != 0.0f)
        {
          this.Draw(_texture, drawPosition, null, Color.White, _rotation, textureCenter, 1.0f, SpriteEffects.None, 1.0f);
        }
        else
        {
          this.Draw(_texture, _position, _bounds, Color.White);
        }

        this.End();

        this.Begin();
        string xText = "X:" + this._velocity.X.ToString();
        string yText = "Y:" + this._velocity.Y.ToString();
        string sText = "S:" + this.Squishiness.ToString();
        
        this.DrawString(_spriteFont, xText, new Vector2(Bounds.Right + 5, Bounds.Top), Color.WhiteSmoke);
        this.DrawString(_spriteFont, yText, new Vector2(Bounds.Right + 5, Bounds.Top + 10), Color.WhiteSmoke);
        this.DrawString(_spriteFont, sText, new Vector2(Bounds.Right + 5, Bounds.Top + 20), Color.WhiteSmoke);

        this.End();
      }
    }

    public virtual void Update(GameTime gameTime)
    {
      if (_velocity == null)
      {
        return;
      }
      
      Vector2 originalPosition = _position;

      // Move the sprite by speed, scaled by elapsed time.
      _position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
      
      UpdateSpriteBounds(_position);

      CollisionResults collisionResults = _worldMap.GetCollisionResults(this);

      bool changedX = false;
      bool changedY = false;

      if (collisionResults.XMove != 0)
      {
        bool velocityPos = _velocity.X > 0;
        bool movePos     = collisionResults.XMove > 0;

        if (velocityPos != movePos)
        {
          _velocity.X *= -1;
          _position.X = originalPosition.X;
          changedX    = true;
        }
      } 

      if (collisionResults.YMove != 0)
      {
        bool velocityPos = _velocity.Y > 0;
        bool movePos     = collisionResults.YMove > 0;

        if (velocityPos != movePos)
        {
          _velocity.Y *= -1;
          _position.Y = originalPosition.Y;
          changedY    = true;
        }
      }

      // If we are trying to change both the X and Y direction, see where we collided the most
      // and only change that direction.
      if (changedX && changedY)
      {
        if (Math.Abs(collisionResults.XMove) > Math.Abs(collisionResults.YMove))
        {
          _velocity.X *= -1;
        }
        else if (Math.Abs(collisionResults.YMove) > Math.Abs(collisionResults.XMove))
        {
          _velocity.Y *= -1;
        }
      }

      // apply squishiness
      if (collisionResults.Sprite != null && (changedX || changedY))
      {
        float totalSquishy = Squishiness + collisionResults.Sprite.Squishiness;

        bool  speedUp         = totalSquishy < 0;
        float absTotalSquishy = Math.Abs(totalSquishy);

        if (speedUp)
        {
          SpeedUpX(absTotalSquishy);
          SpeedUpY(absTotalSquishy);
        }
        else
        {
          SlowDownX(absTotalSquishy);
          SlowDownY(absTotalSquishy);
        }

        if (totalSquishy > 0)
        {
          if (XTooSlow || YTooSlow)
          {
            SpeedUpX(absTotalSquishy);
            SpeedUpY(absTotalSquishy);
          }
        }
        else if (totalSquishy < 0)
        {
          if (XTooFast || YTooFast)
          {
            SlowDownX(absTotalSquishy);
            SlowDownY(absTotalSquishy);
          }
        }
      }

      UpdateSpriteBounds(_position);
    }

    private bool XTooFast
    {
      get { return _velocity.X > 0 ? _velocity.X > _spriteProperties.MaxVelocity.X : _velocity.X < -_spriteProperties.MaxVelocity.X; }
    }

    private bool XTooSlow
    {
      get { return _velocity.X > 0 ? _velocity.X < _spriteProperties.MinVelocity.X : _velocity.X > -_spriteProperties.MinVelocity.X; }
    }

    private bool YTooFast
    {
      get { return _velocity.Y > 0 ? _velocity.Y > _spriteProperties.MaxVelocity.Y : _velocity.Y < -_spriteProperties.MaxVelocity.Y; }
    }

    private bool YTooSlow
    {
      get { return _velocity.Y > 0 ? _velocity.Y < _spriteProperties.MinVelocity.Y : _velocity.Y > -_spriteProperties.MinVelocity.Y; }
    }

    private void SlowDownX(float decrease)
    {
      if (_velocity.X >= 0)
      {
        _velocity.X -= decrease;
      }
      else
      {
        _velocity.X += decrease;
      }
    }

    private void SlowDownY(float decrease)
    {
      if (_velocity.Y >= 0)
      {
        _velocity.Y -= decrease;
      }
      else
      {
        _velocity.Y += decrease;
      }
    }

    private void SpeedUpX(float increase)
    {
      if (_velocity.X >= 0)
      {
        _velocity.X += increase;
      }
      else
      {
        _velocity.X -= increase;
      }
    }

    private void SpeedUpY(float increase)
    {
      if (_velocity.Y >= 0)
      {
        _velocity.Y += increase;
      }
      else
      {
        _velocity.Y -= increase;
      }
    }

    protected void UpdateSpriteBounds(Vector2 spritePosition)
    {
      _bounds.X = (int)spritePosition.X;
      _bounds.Y = (int)spritePosition.Y;
    }
  }
}