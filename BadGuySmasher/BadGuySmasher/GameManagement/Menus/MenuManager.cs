using System;
using System.Collections.Generic;
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
    
    private ICurrentMenu _currentMenu;
    
    public MenuManager(ContentManager contentManager, GraphicsDeviceManager graphicsManager, IGameStateManager gameStateManager)
    {
      _currentMenu = new StartMenu(contentManager, graphicsManager, MainMenuTitleFont, gameStateManager);
    }

    public ICurrentMenu CurrentMenu
    {
      get
      {
        return _currentMenu;
      }
    }
  }
}
