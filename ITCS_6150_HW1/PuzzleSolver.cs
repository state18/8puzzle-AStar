using System;
using System.Collections.Generic;

class PuzzleSolver {

    PuzzleState InitState;
    PuzzleState GoalState;

    public PuzzleSolver(PuzzleState initState, PuzzleState goalState) {
        this.InitState = initState;
        this.GoalState = goalState;
    }

    public PuzzleResults ApplyAStar(Heuristic heuristic) {

        PriorityQueue<PuzzleState> fringe = new PriorityQueue<PuzzleState>();
        // Closed set
        HashSet<PuzzleState> visited = new HashSet<PuzzleState>();

        // We start with only the initial state generated.
        int numGenerated = 1;
        int numExpanded = 0;

        PuzzleState pathEnd = null;

        fringe.Enqueue(InitState, 0);

        while(!fringe.IsEmpty()) {
            PuzzleState current = fringe.Dequeue();
            visited.Add(current);
            numExpanded++;

            // Goal check.
            if (current.Equals(GoalState)) {
                pathEnd = current;
                break;
            }

            List<PuzzleState> successors = current.GetSuccessors();

            foreach (PuzzleState s in successors) {               
                if (!visited.Contains(s)) {

                    // Compute heuristic value and add to total cost to get priority.
                    int hValue = Int32.MaxValue;
                    if (heuristic == Heuristic.MisplacedTiles) {
                        hValue = s.ComputeMisplacedTileDistance(GoalState);
                    } else if (heuristic == Heuristic.ManhattanDistance) {
                        hValue = s.ComputeManhattanDistance(GoalState);
                    } else if (heuristic == Heuristic.None) {
                        hValue = 0;
                    } else {
                        throw new ArgumentException("Expected MisplacedTiles or ManhattanDistance heuristic.");
                    }

                    int priority = s.Depth + hValue;
                    bool alreadyExisted = fringe.Enqueue(s, priority);
                    if (!alreadyExisted)
                        numGenerated++;
                }
            }

        }


        // Build path into list.
        List<PuzzleState> path = new List<PuzzleState>();
        PuzzleState currPathNode = pathEnd;
        path.Add(currPathNode);

        while(currPathNode.Prev != null) {
            path.Add(currPathNode.Prev);
            currPathNode = currPathNode.Prev;
        }

        path.Reverse();

        PuzzleResults results = new PuzzleResults(numGenerated, numExpanded, path);
        return results;
    }

    public int[] ApplyBFS() {
        var q = new Queue<PuzzleState>();
        var visited = new List<PuzzleState>();
        var depthMap = new Dictionary<int, int>();

        var seenGoal = false;
        var goalDepth = -1;
        var numGeneratedAtFirstGoalSighting = -1;

        visited.Add(InitState);
        depthMap[0] = 0;
        q.Enqueue(InitState);

        var currentDepth = 0;

        while (q.Count > 0) {
            var current = q.Dequeue();

            if (currentDepth < current.Depth) {
                currentDepth = current.Depth;
                depthMap[currentDepth] = depthMap[currentDepth - 1];
            }

            depthMap[currentDepth]++;

            if (seenGoal && currentDepth > goalDepth) {
                break;
            }

            var successors = current.GetSuccessors();
            foreach (var s in successors) {
                s.Depth = currentDepth + 1;
                if (!visited.Contains(s)) {
                    visited.Add(s);
                    if (s.Equals(GoalState)) {
                        seenGoal = true;
                        goalDepth = s.Depth;
                        numGeneratedAtFirstGoalSighting = visited.Count;
                    }
                    q.Enqueue(s);
                }
            }
        }

        return new int[] { numGeneratedAtFirstGoalSighting, depthMap[goalDepth - 1] + 1, depthMap[goalDepth] };

    }

}

public enum Heuristic { MisplacedTiles, ManhattanDistance, None }
