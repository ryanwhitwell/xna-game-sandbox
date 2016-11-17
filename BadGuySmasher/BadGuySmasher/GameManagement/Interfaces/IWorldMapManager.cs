using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BadGuySmasher.GameManagement.Interfaces
{
  public interface IWorldMapManager
  {
    void SetNumberOfPlayers(int numberOfPlayers);
    void UpdateWorldMapSpriteVectors(GameTime gameTime);
    void DrawWorldMapSprites(GameTime gameTime);
    void GameBegin();
    void GameEnd();
  }
}
