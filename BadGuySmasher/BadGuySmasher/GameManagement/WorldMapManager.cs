using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadGuySmasher.GameManagement.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher.GameManagement
{
  public sealed class WorldMapManager : IWorldMapManager
  {
    private ContentManager _contentManager;
    private GraphicsDevice _graphicsDevice;

    // TODO Config file - Need something like a custom configuration section or JSON file for loading up the worlds
    
    private IWorldMap _currentWorldMap;

    public WorldMapManager(ContentManager contentManager, GraphicsDevice graphicsDevice)
    {
      if (contentManager == null)
      {
        throw new ArgumentNullException("contentManager");
      }

      if (graphicsDevice == null)
      {
        throw new ArgumentNullException("graphicsDevice");
      }
      
      _contentManager = contentManager;
      _graphicsDevice = graphicsDevice;
    }

    public void SetNumberOfPlayers(int numberOfPlayers)
    {
      if (_currentWorldMap != null)
      {
        _currentWorldMap.SetNumberOfPlayers(numberOfPlayers);
      }
    }

    public void UpdateWorldMapSpriteVectors(GameTime gameTime)
    {
      if (_currentWorldMap != null)
      {
        _currentWorldMap.UpdateSpriteVectors(gameTime);
      }
    }

    public void DrawWorldMapSprites(GameTime gameTime)
    {
      if (_currentWorldMap != null)
      {
        _currentWorldMap.DrawSprites(gameTime);
      }
    }

    public void GameBegin()
    {
      // Needs to get the first world from a file.
      _currentWorldMap = new WorldMap(_contentManager, _graphicsDevice, "");
      _currentWorldMap.LoadMap();
    }

    public void GameEnd()
    {
      // unload the current world map
      // show the score summary
      // wait for user input
    }
  }
}
