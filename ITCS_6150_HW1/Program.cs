// Author: Samuel Tate
// Date: 9/14/2018

using System;
using System.Collections.Generic;
using System.Linq;


class Program {

    static void Main(string[] args) {

        // Test PriorityQueue
        PriorityQueue<int>.TestPriorityQueue();

        PuzzleState initState = null;
        PuzzleState goalState = null;

        try {
            initState = new PuzzleState(args[0]);
            goalState = new PuzzleState(args[1]);
        } catch {
            Console.WriteLine("Invalid input!\n" +
                "Correct Example- Input in the form: 4,3,2,1,5,7,6,8,0 1,2,3,4,5,6,7,8,0 yields the following...\n" +
                "Initial State\n" +
                "4 3 2\n" +
                "1 5 7\n" +
                "6 8 0\n\n" +
                "Goal State\n" +
                "1 2 3\n" +
                "4 5 6\n" +
                "7 8 0\n");
            Environment.Exit(1);
        }


        PuzzleSolver solver = new PuzzleSolver(initState, goalState);
        var watch = System.Diagnostics.Stopwatch.StartNew();
        PuzzleResults results1 = solver.ApplyAStar(Heuristic.MisplacedTiles);
        watch.Stop();
        Console.WriteLine(string.Format("Time for Heuristic 1: {0} ms", watch.ElapsedMilliseconds));

        watch = System.Diagnostics.Stopwatch.StartNew();
        PuzzleResults results2 = solver.ApplyAStar(Heuristic.ManhattanDistance);
        watch.Stop();
        Console.WriteLine(string.Format("Time for Heuristic 2: {0} ms", watch.ElapsedMilliseconds));

        // TODO:
        // Print method for PuzzleState (visualizes grid)
        // Display found path... maybe PrintResults method on PuzzleResults object that displays path, nodes expanded/generated
        // Verify that my generated/expanded numbers are correctly computed according to course standards.

        Console.WriteLine("done");
    }

}
