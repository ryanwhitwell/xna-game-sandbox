using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadGuySmasher.GameManagement.Interfaces;

namespace BadGuySmasher.GameManagement.Menus.Interfaces
{
  public interface IMenuManager
  {
    ICurrentMenu  CurrentMenu { get; }
    IGameManager  GameManager { get; }

    void SetCurrentMenu(string menuName);
  }
}
