using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher.Sprites.Interfaces
{
  public interface ISprite
  {
    string          Id                { get; }
    Texture2D       Texture           { get; }
    Vector2         Position          { get; set; }
    Rectangle       Bounds            { get; }
    bool            DrawBounds        { get; set; }
    WorldMap        WorldMap          { get; }
    ContentManager  ContentManager    { get; }
    float           Squishiness       { get; }
    float           Rotation          { get; }

    void Draw(GameTime gameTime);
    void Update(GameTime gameTime);
    void SetXPosition(float value);
    void SetYPosition(float value);
    void SetXVelocity(float value);
    void SetYVelocity(float value);
  }
}