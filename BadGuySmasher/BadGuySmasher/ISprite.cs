using Microsoft.Xna.Framework;

namespace BadGuySmasher
{
  public interface ISprite
  {
    string Id { get; }
    
    void Draw(GameTime gameTime);
    void UpdateSpriteVectors(GameTime gameTime);
    Rectangle GetSpriteBounds();
  }
}
