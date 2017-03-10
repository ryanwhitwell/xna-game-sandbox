using System;
using BadGuySmasher.GameManagement;
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
    private Level          _worldMap;
    private ContentManager    _contentManager;
    private string            _id;
    private SpriteFont        _spriteFont;
    private float             _rotation;
    public  SpriteProperties  _spriteProperties;

    public Sprite(ContentManager contentManager, GraphicsDevice graphicsDevice, Level worldMap, Vector2 velocity, Vector2 position, string textureAssetName, SpriteProperties spriteProperties) : base(graphicsDevice)
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

    public Sprite(ContentManager contentManager, GraphicsDevice graphicsDevice, Level worldMap, Vector2 position, string textureAssetName, SpriteProperties spriteProperties)
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

    public Vector2 Velocity
    {
      get { return _velocity; }
      set { _velocity = value; }
    }

    public SpriteFont SpriteFont 
    { 
      get { return _spriteFont; } 
    }

    public string         Id             { get { return _id; } }
    public Rectangle      Bounds         { get { return _bounds; } }
    public Level       WorldMap       { get { return _worldMap; } }
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

    protected virtual void Move(GameTime gameTime)
    {
      // default is to do nothing
    }

    protected virtual void HandleCollesionResults(Vector2 originalPosition, CollisionResults collisionResults)
    {


    }

    public void Update(GameTime gameTime)
    {
      if (_velocity == null)
      {
        return;
      }

      Vector2 originalPosition = _position;

      Move(gameTime);

      UpdateSpriteBounds(_position);

      CollisionResults collisionResults = _worldMap.GetCollisionResults(this);

      HandleCollesionResults(originalPosition, collisionResults);

      UpdateSpriteBounds(_position);
    }

    protected bool XTooFast
    {
      get { return _velocity.X > 0 ? _velocity.X > _spriteProperties.MaxVelocity.X : _velocity.X < -_spriteProperties.MaxVelocity.X; }
    }

    protected bool XTooSlow
    {
      get { return _velocity.X > 0 ? _velocity.X < _spriteProperties.MinVelocity.X : _velocity.X > -_spriteProperties.MinVelocity.X; }
    }

    protected bool YTooFast
    {
      get { return _velocity.Y > 0 ? _velocity.Y > _spriteProperties.MaxVelocity.Y : _velocity.Y < -_spriteProperties.MaxVelocity.Y; }
    }

    protected bool YTooSlow
    {
      get { return _velocity.Y > 0 ? _velocity.Y < _spriteProperties.MinVelocity.Y : _velocity.Y > -_spriteProperties.MinVelocity.Y; }
    }

    protected void SlowDownX(float decrease)
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

    protected void SlowDownY(float decrease)
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

    protected void SpeedUpX(float increase)
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

    protected void SpeedUpY(float increase)
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

    private void UpdateSpriteBounds(Vector2 spritePosition)
    {
      _bounds.X = (int)spritePosition.X;
      _bounds.Y = (int)spritePosition.Y;
    }

    public virtual void Delete()
    {
      _worldMap.Sprites.Remove(this);
    }
  }
}