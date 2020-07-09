using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadGuySmasher.Sprites.Players;

namespace BadGuySmasher.Sprites.Interfaces
{
  public interface IBadGuy
  {
    int HitPoints { get; set; }
    
    void Die();

    void GetHit(PlayerProjectile playerProjectile);
  }
}
