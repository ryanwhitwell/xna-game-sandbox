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
    private Vector2         _position;
    private Vector2         _velocity;
    private Rectangle       _bounds;
    private WorldMap        _worldMap;
    private ContentManager  _contentManager;
    private string          _id;
    private SpriteFont      _spriteFont;

    public Sprite(ContentManager contentManager, GraphicsDevice graphicsDevice, WorldMap worldMap, Vector2 velocity, Vector2 position, string textureAssetName) : base(graphicsDevice)
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

      _texture        = contentManager.Load<Texture2D>(textureAssetName);
      _graphicsDevice = graphicsDevice;
      _velocity       = velocity;
      _position       = position;
      _bounds         = new Rectangle ((int)(position.X - _texture.Width / 2), (int)(position.Y - _texture.Height / 2), _texture.Width, _texture.Height );
      _worldMap       = worldMap;
      _contentManager = contentManager;
      _id             = Guid.NewGuid().ToString("N");
      _spriteFont     = _contentManager.Load<SpriteFont>("SpriteFont");
    }

    public Sprite(ContentManager contentManager, GraphicsDevice graphicsDevice, WorldMap worldMap, Vector2 position, string textureAssetName)
      :this(contentManager, graphicsDevice, worldMap, new Vector2(0, 0), position, textureAssetName)
    {
    }

    public Texture2D Texture { get { return _texture; } private set { } }

    public Vector2 Position { get { return _position; } }

    public void SetXPosition(float value)
    {
      _position.X = value;
    }

    public void SetYPosition(float value)
    {
      _position.Y = value;
    }

    public string Id { get { return _id; } private set { } }

    public Rectangle Bounds { get { return _bounds; } }

    public WorldMap WorldMap { get { return _worldMap; } }

    public ContentManager ContentManager { get { return _contentManager; } }

    public bool DrawBounds { get; set; }

    public void Draw(GameTime gameTime)
    {
      this.Begin();
      this.Draw(_texture, _position, Color.White);
      this.End();

      if (DrawBounds)
      {
        this.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
        RasterizerState state = new RasterizerState();
        state.FillMode = FillMode.WireFrame;
        this.GraphicsDevice.RasterizerState = state;

        this.Draw(_texture, _position, Color.White);
        this.End();

        this.Begin();
        string xText = "X:" + this._velocity.X.ToString();
        string yText = "Y:" + this._velocity.Y.ToString();
        this.DrawString(_spriteFont, xText, new Vector2(_position.X + 10, _position.Y + 10), Color.WhiteSmoke);
        this.DrawString(_spriteFont, yText, new Vector2(_position.X + 10, _position.Y + 20), Color.WhiteSmoke);
        this.End();
      }
    }

    public virtual void Update(GameTime gameTime)
    {
      if (_velocity == null)
      {
        return;
      }
      
      //Move the sprite by speed, scaled by elapsed time.
      Vector2 originalPosition = _position;

      _position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
      UpdateSpriteBounds(_position);

      CollisionResults collisionResults = _worldMap.GetCollisionResults(this);

      bool changedX = false;
      bool changedY = false;

      if (collisionResults.XMove != 0)
      {
        bool velocityPos = _velocity.X > 0;
        bool movePos = collisionResults.XMove > 0;

        if (velocityPos != movePos)
        {
          _velocity.X *= -1;
          _position.X = originalPosition.X;
          changedX = true;
        }
      } 

      if (collisionResults.YMove != 0)
      {
        bool velocityPos = _velocity.Y > 0;
        bool movePos = collisionResults.YMove > 0;

        if (velocityPos != movePos)
        {
          _velocity.Y *= -1;
          _position.Y = originalPosition.Y;
          changedY = true;
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

      UpdateSpriteBounds(_position);
    }

    private void UpdateSpriteBounds(Vector2 spritePosition)
    {
      _bounds.X = (int)spritePosition.X;
      _bounds.Y = (int)spritePosition.Y;
    }
  }
}