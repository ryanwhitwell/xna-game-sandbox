using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadGuySmasher
{
  public class CollisionResults
  {
    public Result X { get; set; }
    public Result Y { get; set; }

    public enum Result
    {
      None,
      TooBig,
      TooSmall
    }

    public bool Empty { get { return X == Result.None && Y == Result.None; } }
  }
}
