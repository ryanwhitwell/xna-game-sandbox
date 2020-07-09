using Microsoft.Xna.Framework;

namespace BadGuySmasher.GameManagement.Interfaces
{
  public interface ILevelManager
  {
    User User { get; }

    void SetNumberOfPlayers(int numberOfPlayers);
    void UpdateWorldMapSpriteVectors(GameTime gameTime);
    void DrawWorldMapSprites(GameTime gameTime);
    void RespawnPlayers();
    void LevelBegin();
    void LevelEnd();
  }
}
