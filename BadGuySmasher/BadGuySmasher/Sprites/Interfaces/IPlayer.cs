using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadGuySmasher.Sprites.Interfaces
{
  public interface IPlayer
  {
    void GetHit(IBadGuy badGuy);

    void Die();
  }
}
