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

namespace BadGuySmasher.GameManagement
{
  public class GameStateManager : IGameStateManager
  {
    private IMenuManager      _menuManager;
    private IWorldMapManager  _worldMapManager;
    private GameState         _gameState;

    public GameStateManager(ContentManager contentManager, GraphicsDeviceManager graphicsManager, GraphicsDevice graphicsDevice)
    {
      _gameState        = GameState.Menu;
      _menuManager      = new MenuManager(contentManager, graphicsManager, this);
      _worldMapManager  = new WorldMapManager(contentManager, graphicsDevice);
    }

    public GameState GameState 
    { 
      get
      {
        return _gameState;
      }

      set
      {
        _gameState = value;
      }
    }
    
    public IMenuManager MenuManager
    {
      get
      {
        return _menuManager;
      }
    }

    public IWorldMapManager WorldMapManager
    {
      get
      {
        return _worldMapManager;
      }
    }
  }
}
