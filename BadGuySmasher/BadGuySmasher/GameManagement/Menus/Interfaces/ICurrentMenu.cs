using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadGuySmasher.GameManagement.Menus.Interfaces
{
  public interface ICurrentMenu : IMenu 
  {
    void SetMenuState(MenuState menuState);
  }
}
