using System;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class PuzzleState {

    public int GridSize;
    public int Depth = 0;
    public PuzzleState Prev = null;

    int[,] Data;

    public PuzzleState(int[,] data, PuzzleState prev=null) {
        this.Data = data;
        this.GridSize = data.GetLength(0);
        this.Prev = prev;
    }

    public PuzzleState(string textData) {
        // Convert to 2D array format.
        var tokens = textData.Split(',');
        GridSize = (int)Math.Sqrt(tokens.Length);
        Data = new int[GridSize, GridSize];
        for (var i = 0; i < GridSize; i++) {
            for (var j = 0; j < GridSize; j++) {
                Data[i, j] = Int32.Parse(tokens[i * GridSize + j]);
            }
        }
    }

    public List<PuzzleState> GetSuccessors() {
        // Find empty tile (0) coordinates.
        int eRow = 0;
        int eCol = 0;
        bool foundZero = false;
        for (var i = 0; i < Data.GetLength(0); i++) {
            for (var j = 0; j < Data.GetLength(1); j++) {
                if (Data[i, j] == 0) {
                    eRow = i;
                    eCol = j;
                    foundZero = true;
                }
            }
            if (foundZero)
                break;
        }

        List<PuzzleState> successors = new List<PuzzleState>();

        if (eRow > 0) {
            var newData = new int[Data.GetLength(0), Data.GetLength(1)];
            Array.Copy(Data, newData, Data.Length);
            // Swap 0 tile with above element.
            newData[eRow, eCol] = newData[eRow - 1, eCol];
            newData[eRow - 1, eCol] = 0;
            successors.Add(new PuzzleState(newData, this));
        }

        if (eRow < Data.GetLength(0) - 1) {
            var newData = new int[Data.GetLength(0), Data.GetLength(1)];
            Array.Copy(Data, newData, Data.Length);
            // Swap 0 tile with below element.
            newData[eRow, eCol] = newData[eRow + 1, eCol];
            newData[eRow + 1, eCol] = 0;
            successors.Add(new PuzzleState(newData, this));
        }

        if (eCol > 0) {
            var newData = new int[Data.GetLength(0), Data.GetLength(1)];
            Array.Copy(Data, newData, Data.Length);
            // Swap 0 tile with left element.
            newData[eRow, eCol] = newData[eRow, eCol - 1];
            newData[eRow, eCol - 1] = 0;
            successors.Add(new PuzzleState(newData, this));
        }

        if (eCol < Data.GetLength(1) - 1) {
            var newData = new int[Data.GetLength(0), Data.GetLength(1)];
            Array.Copy(Data, newData, Data.Length);
            // Swap 0 tile with right element.
            newData[eRow, eCol] = newData[eRow, eCol + 1];
            newData[eRow, eCol + 1] = 0;
            successors.Add(new PuzzleState(newData, this));
        }

        return successors;
    }

    public int ComputeMisplacedTileDistance(PuzzleState other) {

        if (other == null)
            throw new NullReferenceException();

        int numMisplaced = 0;

        for (int i = 0; i < Data.GetLength(0); i++) {
            for (int j = 0; j < Data.GetLength(1); j++) {
                if (this.Data[i, j] != 0 && this.Data[i, j] != other.Data[i, j])
                    numMisplaced++;
            }
        }
        return numMisplaced;
    }

    public int ComputeManhattanDistance(PuzzleState other) {
        if (other == null)
            throw new NullReferenceException();

        int manhattanDist = 0;

        int[,] myLookup = new int[GridSize * GridSize, 2];
        int[,] otherLookup = new int[GridSize * GridSize, 2];

        for (int i = 0; i < Data.GetLength(0); i++) {
            for (int j = 0; j < Data.GetLength(1); j++) {
                myLookup[this.Data[i, j], 0] = i;
                myLookup[this.Data[i, j], 1] = j;

                otherLookup[other.Data[i, j], 0] = i;
                otherLookup[other.Data[i, j], 1] = j;
            }
        }

        // Use lookups to compare row and column differences. (ignore empty tile)
        for(int i=1; i<myLookup.GetLength(0); i++) {

            int newDist = Math.Abs(myLookup[i, 0] - otherLookup[i, 0]) +
                          Math.Abs(myLookup[i, 1] - otherLookup[i, 1]);
            manhattanDist += newDist;
                
        }

        return manhattanDist;
    }


    public override bool Equals(object obj) {

        if (obj == null)
            return false;

        var other = (PuzzleState)(obj);
        for (var i = 0; i < Data.GetLength(0); i++) {
            for (var j = 0; j < Data.GetLength(1); j++) {
                if (this.Data[i, j] != other.Data[i, j])
                    return false;
            }
        }
        return true;
    }

    public override int GetHashCode() {
        return this.Data.GetHashCode();
    }
}