using BadGuySmasher.GameManagement;
using BadGuySmasher.Sprites.BadGuys;
using BadGuySmasher.Sprites.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher.Sprites.Players
{
  public class PlayerProjectile : Sprite, IProjectile
  {
    public const int MAX_DISTANCE         = 1500;
    public const int ProjectileMoveSpeed  = 900;

    private Vector2 _startPosition;
    private Vector2 _speed;
    private Vector2 _direction;
    private bool    _visible;
    private int     _power;

    public PlayerProjectile(ContentManager contentManager, 
                            GraphicsDevice graphicsDevice, 
                            WorldMap       worldMap, 
                            int            power,
                            Vector2        position, 
                            Vector2        speed, 
                            Vector2        direction, 
                            string         textureAssetName) 
      : base(contentManager, graphicsDevice, worldMap, position, textureAssetName, null) 
    {
      _power          = power;
      _startPosition  = position;
      _speed          = speed;
      _direction      = direction;
      _visible        = true;
    }

    public int Power { get { return _power; } set { _power = value; } }

    public bool Visible { get { return _visible; } set { _visible = value; } }

    protected override void Move(GameTime gameTime)
    {
      if (Vector2.Distance(_startPosition, Position) > MAX_DISTANCE)
      {
        Visible = false;
      }

      if (Visible == true)
      {
        UpdatePosition(gameTime, _direction, _speed);
      }
    }

    private void UpdatePosition(GameTime gameTime, Vector2 speed, Vector2 direction)
    {
      Position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void Dissolve()
    {
      Delete();
    }
  }
}
