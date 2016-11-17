using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadGuySmasher.Sprites;

namespace BadGuySmasher
{
  public class CollisionResults
  {
    public int XMove { get; set; }
    public int YMove { get; set; }

    public Sprite Sprite { get; set; }

    public bool Empty { get { return XMove == 0 && YMove == 0; } }
  }
}
