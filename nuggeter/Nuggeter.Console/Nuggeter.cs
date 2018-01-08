using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuggeter.Console
{
    using Console = System.Console;

    public static class NuggeterTest
    {
        [STAThread]
        public static void Main()
        {
            var solution = NuggetedSolution.FromFile(@"C:\GIT\Private\ObscureWare\Console\ObscureWare.Console.sln");
            if (!solution.IsValid || !solution.Exists)
            {
                return;
            }

            Console.WriteLine($"Opened solution '{solution.Name}'");
            Console.WriteLine("Nuggetable projects:");

        }

    }
}
