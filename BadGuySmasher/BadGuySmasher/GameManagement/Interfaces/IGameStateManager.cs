using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadGuySmasher.GameManagement.Menus.Interfaces;

namespace BadGuySmasher.GameManagement.Interfaces
{
  public interface IGameStateManager
  {
    IWorldMapManager  WorldMapManager { get; }
    GameState         GameState       { get; set; }
    IMenuManager      MenuManager     { get; }
  }
}
