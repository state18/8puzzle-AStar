using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class PuzzleResults {
    public int NodesGenerated;
    public int NodesExpanded;
    public List<PuzzleState> Path;

    public PuzzleResults(int nodesGenerated, int nodesExpanded, List<PuzzleState> path) {
        this.NodesGenerated = nodesGenerated;
        this.NodesExpanded = nodesExpanded;
        this.Path = path;
    }

    public void Print() {
        int i = 0;
        foreach (PuzzleState s in Path) {
            s.Print(string.Format("State: {0}", i));
            i++;
        }

        Console.WriteLine(string.Format("Number of moves {0} -- Nodes generated: {1}  -- Nodes expanded: {2}", Path.Count - 1, NodesGenerated, NodesExpanded));
    }
}
