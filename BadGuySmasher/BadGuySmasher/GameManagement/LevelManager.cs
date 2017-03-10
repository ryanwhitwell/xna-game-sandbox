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
  public sealed class LevelManager : ILevelManager
  {
    private ContentManager  _contentManager;
    private GraphicsDevice  _graphicsDevice;
    private IGameManager    _gameManager;

    // TODO Config file - Need something like a custom configuration section or JSON file for loading up the levels
    
    private ILevel _currentLevel;

    private User _user;

    public LevelManager(ContentManager contentManager, GraphicsDevice graphicsDevice, IGameManager gameManager)
    {
      if (contentManager == null)
      {
        throw new ArgumentNullException("contentManager");
      }

      if (graphicsDevice == null)
      {
        throw new ArgumentNullException("graphicsDevice");
      }

      if (gameManager == null)
      {
        throw new ArgumentNullException("gameManager");
      }
      
      _contentManager   = contentManager;
      _graphicsDevice   = graphicsDevice;
      _gameManager      = gameManager;
      _user             = new User();
    }

    public User User
    {
      get
      {
        return _user;
      }
    }

    public void SetNumberOfPlayers(int numberOfPlayers)
    {
      if (_currentLevel != null)
      {
        _currentLevel.SetNumberOfPlayers(numberOfPlayers);
      }
    }

    public void UpdateWorldMapSpriteVectors(GameTime gameTime)
    {
      if (_currentLevel != null)
      {
        _currentLevel.UpdateSpriteVectors(gameTime);

        UpateWorldMapGameState();
      }
    }

    public void DrawWorldMapSprites(GameTime gameTime)
    {
      if (_currentLevel != null)
      {
        _currentLevel.DrawSprites(gameTime);
      }
    }

    public void LevelBegin()
    {
      // Needs to get the first world from a file.
      _currentLevel = new Level(_contentManager, _graphicsDevice, "");
      _currentLevel.LoadMap();
      _gameManager.GameState = GameState.Play;
    }

    public void LevelEnd()
    {
      _gameManager.MenuManager.SetCurrentMenu(MenuManager.GameOverMenu);
      _gameManager.GameState = GameState.End;
    }

    public void RespawnPlayers()
    {
      _currentLevel.RespawnPlayers();
    }

    private void UpateWorldMapGameState()
    {
      CheckPlayerState();

      CheckMapClearedState();
    }

    private void CheckPlayerState()
    {
      Console.Out.WriteLine("Player1Health: {0}", _currentLevel.Player1Health);

      bool playerOneDead = _currentLevel.Player1Health != int.MinValue && _currentLevel.Player1Health <= 0;

      if (_currentLevel.NumberOfPlayers == 1 && playerOneDead)
      {
        LevelEnd();
      }
      else if (_currentLevel.NumberOfPlayers == 2 && (playerOneDead && (_currentLevel.Player2Health != int.MinValue && _currentLevel.Player2Health <= 0)))
      {
        LevelEnd();
      }
    }

    private void CheckMapClearedState()
    {
      if (_currentLevel.MapCleared)
      {
        // TODO: Next World - The game needs to show another menu with a world score summary allowing the user to press a key when they're ready to begin the next level
        LevelEnd();
      }
    }
  }
}
