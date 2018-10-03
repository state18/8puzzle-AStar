// Author: Samuel Tate
// Date: 9/14/2018

using System;
using System.Collections.Generic;
using System.Linq;


class Program {

    static void Main(string[] args) {

        PuzzleState initState = null;
        PuzzleState goalState = null;

        try {
            initState = new PuzzleState(args[0]);
            goalState = new PuzzleState(args[1]);
        } catch {
            Console.WriteLine("Invalid input!\n" +
                "Correct Example- Input in the form: 8,3,5,4,1,6,2,7,0 1,2,3,8,0,4,7,6,5 yields the following...\n" +
                "Initial State\n" +
                "8 3 5\n" +
                "4 1 6\n" +
                "2 7 0\n\n" +
                "Goal State\n" +
                "1 2 3\n" +
                "8 0 4\n" +
                "7 6 5\n");
            Console.WriteLine("Press enter to close...");
            Console.Read();
            Environment.Exit(1);
        }

        Heuristic[] heuristics = new Heuristic[] { Heuristic.ManhattanDistance };

        foreach (Heuristic h in heuristics) {

            PuzzleSolver solver = new PuzzleSolver(initState, goalState);
            Console.WriteLine(string.Format("\n\nSolving with {0} heuristic...", h.ToString()));
            var watch = System.Diagnostics.Stopwatch.StartNew();
            PuzzleResults results = solver.ApplyAStar(h);
            watch.Stop();

            results.Print();
            Console.WriteLine(string.Format("Time elapsed: {0} ms", watch.ElapsedMilliseconds));
        }

        Console.WriteLine("Done! Press Enter key to close...");
        Console.Read();
    }

}
