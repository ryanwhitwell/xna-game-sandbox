using BadGuySmasher.GameManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher.Sprites.Interfaces
{
  public interface ISprite
  {
    Rectangle      Bounds         { get; }
    ContentManager ContentManager { get; }
    bool           DrawBounds     { get; set; }
    string         Id             { get; }
    Vector2        Position       { get; set; }
    float          Rotation       { get; set; }
    SpriteFont     SpriteFont     { get; }
    float          Squishiness    { get; }
    Texture2D      Texture        { get; }
    Vector2        Velocity       { get; set; }
    Level       WorldMap       { get; }

    void SetXPosition(float value);
    void SetXVelocity(float value);
    void SetYPosition(float value);
    void SetYVelocity(float value);
    void Delete();
    void Draw(GameTime gameTime);
    void Update(GameTime gameTime);
  }
}
