using System;

namespace BadGuySmasher
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
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

