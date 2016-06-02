using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher
{
  public interface ISprite
  {
    string Id { get; }
    Texture2D Texture { get; }
    Vector2 SpritePosition { get; }
    
    void Draw(GameTime gameTime);
    void UpdateSpriteVectors(GameTime gameTime);
    Rectangle GetSpriteBounds();
  }
}
