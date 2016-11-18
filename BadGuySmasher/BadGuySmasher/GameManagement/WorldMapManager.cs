using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadGuySmasher.GameManagement.Interfaces;
using BadGuySmasher.GameManagement.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher.GameManagement
{
  public sealed class WorldMapManager : IWorldMapManager
  {
    private ContentManager    _contentManager;
    private GraphicsDevice    _graphicsDevice;
    private IGameStateManager _gameStateManager;

    // TODO Config file - Need something like a custom configuration section or JSON file for loading up the worlds
    
    private IWorldMap _currentWorldMap;

    public WorldMapManager(ContentManager contentManager, GraphicsDevice graphicsDevice, IGameStateManager gameStateManager)
    {
      if (contentManager == null)
      {
        throw new ArgumentNullException("contentManager");
      }

      if (graphicsDevice == null)
      {
        throw new ArgumentNullException("graphicsDevice");
      }

      if (gameStateManager == null)
      {
        throw new ArgumentNullException("gameStateManager");
      }
      
      _contentManager   = contentManager;
      _graphicsDevice   = graphicsDevice;
      _gameStateManager = gameStateManager;
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

        UpateWorldMapGameState();
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
      _currentWorldMap = null;
      _gameStateManager.MenuManager.SetCurrentMenu(MenuManager.GameOverMenu);
    }

    private void UpateWorldMapGameState()
    {
      CheckPlayerState();

      CheckMapClearedState();
    }

    private void CheckPlayerState()
    {
      if (_currentWorldMap.NumberOfPlayers == 1 &&
          _currentWorldMap.Player1Health != int.MinValue && 
          _currentWorldMap.Player1Health <= 0)
      {
        GameEnd();
      }
      else if (_currentWorldMap.NumberOfPlayers == 2 && 
               _currentWorldMap.Player1Health != int.MinValue && 
               _currentWorldMap.Player1Health <= 0 &&
               _currentWorldMap.Player2Health != int.MinValue && 
               _currentWorldMap.Player2Health <= 0)
      {
        GameEnd();
      }
    }

    private void CheckMapClearedState()
    {
      if (_currentWorldMap.MapCleared)
      {
        // TODO: Next World - The game needs to show another menu with a world score summary allowing the user to press a key when they're ready to begin the next level
        GameEnd();
      }
    }
  }
}
