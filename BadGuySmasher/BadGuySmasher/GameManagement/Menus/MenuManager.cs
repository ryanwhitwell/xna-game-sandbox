using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BadGuySmasher.GameManagement.Interfaces;
using BadGuySmasher.GameManagement.Menus.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BadGuySmasher.GameManagement.Menus
{
  public class MenuManager : IMenuManager
  {
    private const string MainMenuTitleFont = "TitleFont";

    private Dictionary<string, IMenu> _availableMenus;
    private IGameStateManager         _gameStateManager;
    private ICurrentMenu              _currentMenu;
    
    public MenuManager(ContentManager contentManager, GraphicsDeviceManager graphicsDeviceManager, IGameStateManager gameStateManager)
    {
      if (contentManager == null)
      {
        throw new ArgumentNullException("contentManager");
      }

      if (graphicsDeviceManager == null)
      {
        throw new ArgumentNullException("graphicsDeviceManager");
      }

      if (gameStateManager == null)
      {
        throw new ArgumentNullException("gameStateManager");
      }

      _gameStateManager = gameStateManager;

      InitializeAvailalbeMenus(contentManager, graphicsDeviceManager, gameStateManager);
    }

    private void InitializeAvailalbeMenus(ContentManager contentManager, GraphicsDeviceManager graphicsDeviceManager, IGameStateManager gameStateManager)
    {
      StartMenu startMenu = new StartMenu(contentManager, graphicsDeviceManager, MainMenuTitleFont, gameStateManager); 

      PauseMenu pauseMenu = new PauseMenu(contentManager, graphicsDeviceManager, MainMenuTitleFont, gameStateManager); 

      _availableMenus = new Dictionary<string, IMenu>();

      _availableMenus.Add("StartMenu", startMenu);
      _availableMenus.Add("PauseMenu", pauseMenu);

      // Set the current Menu
      SetCurrentMenu("StartMenu");
    }

    public ICurrentMenu CurrentMenu
    {
      get
      {
        return _currentMenu;
      }
    }

    public void SetCurrentMenu(string menuName)
    {
      IMenu menu = _availableMenus[menuName];

      if (menu == null)
      {
        Debug.WriteLine("Could not find menu with name {0}.", menuName);
        return;
      }

      ICurrentMenu currentMenu = menu as ICurrentMenu;

      if (currentMenu == null)
      {
        Debug.WriteLine("Could not cast menu with name as {0} as ICurrentMenu.", menuName);
        return;
      }

      _currentMenu = currentMenu;
    }
  }
}
