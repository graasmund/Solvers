using System;
using System.Diagnostics;

namespace ConsoleApplication3
{
    class Program
    { 
        static void Main(string[] args)
        {
            int n = 25;

            var solver = new BruteForceSolver(n);
            Console.WriteLine($"Solving for {n} queens, started {DateTime.Now}");

            var sw = Stopwatch.StartNew();

            solver.Solve();

            sw.Stop();

            Console.Clear();
            Console.WriteLine(solver.ToString());
            Console.WriteLine($"Problemsize {n} solved in {sw.Elapsed}");
            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
