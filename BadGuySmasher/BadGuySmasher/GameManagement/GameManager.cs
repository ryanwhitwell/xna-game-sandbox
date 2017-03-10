using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BadGuySmasher.GameManagement.Interfaces;
using BadGuySmasher.GameManagement.Menus.Interfaces;
using BadGuySmasher.GameManagement.Menus;
using System.ComponentModel;
using System.Diagnostics;

namespace BadGuySmasher.GameManagement
{
  public class GameManager : IGameManager
  {
    private IMenuManager  _menuManager;
    private ILevelManager _levelManager;
    private GameState     _gameState;
      
    public GameManager(ContentManager contentManager, GraphicsDeviceManager graphicsManager, GraphicsDevice graphicsDevice)
    {
      _gameState    = GameState.Ready;
      _menuManager  = new MenuManager(contentManager, graphicsManager, this);
      _levelManager = new LevelManager(contentManager, graphicsDevice, this);
    }
    
    public IMenuManager MenuManager
    {
      get
      {
        return _menuManager;
      }
    }

    public ILevelManager LevelManager
    {
      get
      {
        return _levelManager;
      }
    }

    public GameState GameState
    {
      get
      {
        return _gameState;
      }

      set
      {
        switch (value)
        {
          case GameState.Ready:
            LevelManager.LevelBegin();
            break;
        }

        _gameState = value;
      }
    }
  }
}
