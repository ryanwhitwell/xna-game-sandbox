using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadGuySmasher.GameManagement
{
  public class User
  {
    // TODO: Store data in a save game file: https://msdn.microsoft.com/en-us/library/bb203924.aspx

    ICollection<CompletedLevel> CompletedLevels { get; set; }
  }
}
