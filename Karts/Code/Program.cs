using System;

namespace Karts
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Karts game = new Karts())
            {
                game.Run();
            }
        }
    }
}

