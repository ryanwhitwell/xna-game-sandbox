using BadGuySmasher.Sprites.BadGuys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher.Sprites.Players
{
  public class PlayerProjectile : Sprite
  {
    public const int MAX_DISTANCE         = 1500;
    public const int ProjectileMoveSpeed  = 900;

    private Vector2 _startPosition;
    private Vector2 _speed;
    private Vector2 _direction;
    private bool    _visible;

    public PlayerProjectile(ContentManager contentManager, 
                            GraphicsDevice graphicsDevice, 
                            WorldMap       worldMap, 
                            Vector2        position, 
                            Vector2        speed, 
                            Vector2        direction, 
                            string         textureAssetName) 
      : base(contentManager, graphicsDevice, worldMap, position, textureAssetName, null) 
    { 
      _startPosition  = position;
      _speed          = speed;
      _direction      = direction;
      _visible        = true;
    }

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

    protected override void HandleCollesionResults(Vector2 originalPosition, CollisionResults collisionResults)
    {
      BadGuy badGuy = collisionResults.Sprite as BadGuy;

      if (badGuy != null)
      {
        badGuy.Delete();
        return;
      }

      if (collisionResults.Sprite != null && !(collisionResults.Sprite is Player))
      {
        Delete();
      }
    }
  }
}
