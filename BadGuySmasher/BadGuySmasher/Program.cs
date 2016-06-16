using System;

namespace BadGuySmasher
{
  #if WINDOWS || XBOX
  static class Program
  {
    static void Main(string[] args)
    {
      using (BadGuySmasherGame game = new BadGuySmasherGame())
      {
        game.Run();
      }
    }
  }
  #endif
}