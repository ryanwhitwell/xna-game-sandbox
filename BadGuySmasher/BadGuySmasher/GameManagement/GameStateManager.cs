using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BadGuySmasher.GameManagement.Interfaces;
using BadGuySmasher.GameManagement.Menus.Interfaces;

namespace BadGuySmasher.GameManagement
{
  public class GameStateManager : IGameStateManager
  {
    private const string MainMenuTitleFont = "TitleFont";

    private MainMenu          _mainMenu;
    private IWorldMapManager  _worldMapManager;

    public GameStateManager(ContentManager contentManager, GraphicsDeviceManager graphicsManager, GraphicsDevice graphicsDevice)
    {
      _mainMenu         = new MainMenu(contentManager, graphicsManager, MainMenuTitleFont);
      _worldMapManager  = new WorldMapManager(contentManager, graphicsDevice);
    }

    public IMainMenu MainMenu
    {
      get
      {
        return _mainMenu;
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
