using System;
using System.Collections.Generic;
using BadGuySmasher.Sprites;
using BadGuySmasher.Sprites.Interfaces;
using Microsoft.Xna.Framework;
namespace BadGuySmasher.GameManagement
{
  public interface IWorldMap
  {
    ICollection<ISprite> Sprites { get; }
    
    void DrawSprites(GameTime gameTime);
    void SetNumberOfPlayers(int numberOfPlayers);
    void UpdateSpriteVectors(GameTime gameTime);
    void LoadMap();
  }
}
