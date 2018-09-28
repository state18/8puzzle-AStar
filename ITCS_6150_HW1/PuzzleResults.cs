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
}
