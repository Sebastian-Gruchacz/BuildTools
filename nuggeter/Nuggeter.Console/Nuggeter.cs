namespace Nuggeter.Console
{
    using System;

    using Console = System.Console;

    public static class NuggeterTest
    {
        [STAThread]
        public static void Main()
        {
            //string path = @"C:\GIT\Private\ObscureWare\Console\ObscureWare.Console.sln";
            string path = @"J:\GitHub\Obscureware\Console\ObscureWare.Console.sln";

            var solution = NuggetedSolution.FromFile(path);
            if (!solution.Exists)
            {
                Console.WriteLine($"Could not find solution file at path: '{path}'");
                return;
            }

            if (!solution.IsValid)
            {
                Console.WriteLine($"Solution at '{path}' is either invalid or path is wrong.");
                return;
            }

            Console.WriteLine($"Opened solution '{solution.Name}'");
            if (solution.NuggetConfig.Exists)
            {
                Console.WriteLine($"Global solution's 'nugget.config' found at '{solution.NuggetConfig.Path}'.");
            }
            else
            {
                Console.WriteLine("Global solution's 'nugget.config' was not found.");
            }

            Console.WriteLine("Nuggetable projects:");





            Console.WriteLine("Press ENTER");
            Console.ReadLine();
        }

    }
}
