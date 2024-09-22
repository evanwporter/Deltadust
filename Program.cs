using System;
using Deltadust.Core;

namespace Deltadust
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            // Use the singleton instance of Game1
            Game1.Instance.Run();
        }
    }
}
