using SprintLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SprintConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SprintCalculator sprintCalc = new SprintCalculator(11);

            List<Issue> issues = new List<Issue>()
            {
                new Issue() { Name = "Issue 1", Cost = 1, WinFactor = 2 },
                new Issue() { Name = "Issue 2", Cost = 3, WinFactor = 3 },
                new Issue() { Name = "Issue 3", Cost = 3, WinFactor = 3 },
                new Issue() { Name = "Issue 4", Cost = 4, WinFactor = 7 },
                new Issue() { Name = "Issue 5", Cost = 5, WinFactor = 10 },
            };

            Dictionary<int, List<Issue>> iterations = sprintCalc.ArrangeIssues(issues);
            foreach(KeyValuePair<int, List<Issue>> iteration in iterations)
            {
                Console.WriteLine($"Sprint {iteration.Key + 1} has following tasks =>");
                foreach (var issue in iteration.Value)
                {
                    Console.WriteLine($"  {issue.ToString()}");
                }
                Console.WriteLine($"Total Cost = {iteration.Value.Sum(o => o.Cost)}, Total Win = {iteration.Value.Sum(o => o.WinFactor)}");
                Console.WriteLine();
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
