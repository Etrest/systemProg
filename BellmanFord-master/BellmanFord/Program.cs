using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellmanFord
{
    class Program
    {
        private static List<string> vertices = new List<string>() { "x1", "x2", "x3", "x4", "x5", "x6", "x7", "x8", "x9" };

        static Dictionary<string, double> memo = new Dictionary<string, double>()
            {
                { "x1", 0 },
                {"x2", double.MaxValue },
                {"x3", double.MaxValue },
                {"x4", double.MaxValue },
                {"x5", double.MaxValue },
                {"x6", double.MaxValue },
                {"x7", double.MaxValue },
                {"x8", double.MaxValue },
                {"x9", double.MaxValue },
            };

        static List<Path> graph = new List<Path>()
            {
                // Values given in original JavaScript version in book are wrong!
                new Path("x1", "x2", (double)11/15),
                new Path("x1", "x3", (double)1/2),
                new Path("x1", "x5", (double)14 / 8),
                //x2
                new Path("x2", "x3", (double)8 / 9),
                new Path("x2", "x4", (double)16 / 15),
                //x3
                new Path("x3", "x5", (double)3 / 9),
                new Path("x3", "x4", (double)10 / 12),
                new Path("x3", "x6", (double)15 / 8),
                //x4
                new Path("x4", "x8", (double)12 / 10),
                //x5
                new Path("x5", "x7", (double)8 / 9),
                //x6
                new Path("x6", "x7", (double)5 / 6),
                new Path("x6", "x9", (double)4 / 7),
                //x7
                new Path("x7", "x9", (double)15 / 10),
                //x8
                new Path("x8", "x9", (double)14 / 11),
                
            };

        static void Main(string[] args)
        {
            // Implementation of Bellman Ford algorithm based on Rob Conery's "Imposters Handbook"
            // This is my conversion of JavaScript original in book

            foreach (string vertex in vertices)
            {
                if (!Iterate())
                    break;
            }

            foreach (KeyValuePair<string, double> memoItem in memo)
            {
                Console.WriteLine("{0} = {1}", memoItem.Key, memoItem.Value.ToString());
            }

            Console.ReadLine();

        }

        private static bool Iterate()
        {
            // Do we need another iteration? Decided below
            bool doItAgain = false;

            // Loop all vertices
            foreach (string fromVertex in vertices)
            {
                Path[] edges = graph.Where(x => x.From == fromVertex).ToArray();

                foreach (Path edge in edges)
                {
                    // If from is maxvalue, it's wrapping around, so handle that!
                    double potentialCost = memo[edge.From];
                    if (potentialCost != double.MaxValue)
                        potentialCost += edge.Cost;

                    if (potentialCost < memo[edge.To])
                    {
                        memo[edge.To] = potentialCost;
                        doItAgain = true;
                    }
                }
            }

            return doItAgain;
        }
    }
}
