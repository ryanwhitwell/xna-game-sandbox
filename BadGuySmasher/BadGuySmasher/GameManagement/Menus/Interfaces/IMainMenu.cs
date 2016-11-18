using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static BadGuySmasher.MainMenu;

namespace BadGuySmasher.GameManagement.Menus.Interfaces
{
  public interface IMainMenu
  {
    int NumberOfPlayers { get; }

    void  Draw();
    State UpdateInput();
  }
}
