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
    public const string StartMenu     = "Start";
    public const string PauseMenu     = "Pause";
    public const string GameOverMenu  = "GameOver";
    
    private const string MainMenuTitleFont = "TitleFont";

    private Dictionary<string, IMenu> _availableMenus;
    private IGameManager              _gameManager;
    private ICurrentMenu              _currentMenu;
    
    public MenuManager(ContentManager contentManager, GraphicsDeviceManager graphicsDeviceManager, IGameManager gameManager)
    {
      if (contentManager == null)
      {
        throw new ArgumentNullException("contentManager");
      }

      if (graphicsDeviceManager == null)
      {
        throw new ArgumentNullException("graphicsDeviceManager");
      }

      if (gameManager == null)
      {
        throw new ArgumentNullException("gameStateManager");
      }

      _gameManager = gameManager;

      InitializeAvailalbeMenus(contentManager, graphicsDeviceManager, this);
    }

    private void InitializeAvailalbeMenus(ContentManager contentManager, GraphicsDeviceManager graphicsDeviceManager, IMenuManager menuManager)
    {
      StartMenu     startMenu     = new StartMenu(contentManager,     graphicsDeviceManager, MainMenuTitleFont, menuManager, StartMenu); 
      PauseMenu     pauseMenu     = new PauseMenu(contentManager,     graphicsDeviceManager, MainMenuTitleFont, menuManager, PauseMenu); 
      GameOverMenu  gameOverMenu  = new GameOverMenu(contentManager,  graphicsDeviceManager, MainMenuTitleFont, menuManager, GameOverMenu); 

      _availableMenus = new Dictionary<string, IMenu>();

      _availableMenus.Add(StartMenu,    startMenu);
      _availableMenus.Add(PauseMenu,    pauseMenu);
      _availableMenus.Add(GameOverMenu, gameOverMenu);

      // Set the current Menu
      SetCurrentMenu(StartMenu);
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

      // TODO RPW - Currently stuck here. Cannot get a full game cycle. end game to re-start game.
      _currentMenu.SetGameState(GameState.Menu);
    }

    public IGameManager GameManager
    {
      get
      {
        return _gameManager;
      }
    }
  }
}
