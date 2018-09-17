// Author: Samuel Tate
// Date: 9/14/2018

using System;
using System.Collections.Generic;
using System.Linq;

namespace ITCS_6150_HW1 {
    class Program {

        public static Random rnd;

        static void Main(string[] args) {

            rnd = new Random();

            var initData = new int[,] {
                {0, 1, 3},
                {4, 2, 5},
                {7, 8, 6}
            };

            var initState = new PuzzleState(initData);
            var solver = new PuzzleSolver(initState);
            int[] solverOutput = solver.ApplyBFS();
            Console.WriteLine(String.Format("It took {0} generated unduplicate states to find the goal. \n" +
                "The range of unduplicate states that COULD have been generated depending on successor generation order: {1} to {2}", solverOutput[0], solverOutput[1], solverOutput[2]));
            Console.ReadKey();
        }
    }

    class PuzzleSolver {

        PuzzleState InitState;
        PuzzleState GoalState;

        public PuzzleSolver(PuzzleState initState) {
            this.InitState = initState;
            this.GoalState = ComputeGoalState();
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

            while(q.Count > 0) {
                var current = q.Dequeue();

                if(currentDepth < current.Depth) {
                    currentDepth = current.Depth;
                    depthMap[currentDepth] = depthMap[currentDepth-1];
                }

                depthMap[currentDepth]++;

                if(seenGoal && currentDepth > goalDepth) {
                    break;                   
                }

                var successors = current.GetSuccessors();
                foreach(var s in successors) {
                    s.Depth = currentDepth + 1;
                    if(!visited.Contains(s)) {
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

            return new int[] { numGeneratedAtFirstGoalSighting, depthMap[goalDepth - 1] + 1, depthMap[goalDepth]};

        }

        private PuzzleState ComputeGoalState() {
            var goalData = new int[InitState.GridSize, InitState.GridSize];
            for(var i=0; i<InitState.GridSize; i++) {
                for(var j=0; j<InitState.GridSize; j++) {
                    goalData[i, j] = i * InitState.GridSize + j + 1;
                }
            }
            goalData[InitState.GridSize - 1, InitState.GridSize - 1] = 0;
            return new PuzzleState(goalData);
        }

    }

    class PuzzleState {

        public int GridSize;
        public int Depth = 0;

        int[,] Data;
        
        public PuzzleState(int[,] data) {
            this.Data = data;
            this.GridSize = data.GetLength(0);
        }

        public List<PuzzleState> GetSuccessors() {
            // Find empty tile (0) coordinates.
            int eRow = 0;
            int eCol = 0;
            bool foundZero = false;
            for(var i=0; i<Data.GetLength(0); i++) {
                for(var j=0; j<Data.GetLength(1); j++) {
                    if (Data[i,j] == 0) {
                        eRow = i;
                        eCol = j;
                        foundZero = true;
                    }
                }
                if (foundZero)
                    break;
            }

            List<PuzzleState> successors = new List<PuzzleState>();

            if(eRow > 0) {
                var newData = new int[Data.GetLength(0), Data.GetLength(1)];
                Array.Copy(Data, newData, Data.Length);
                // Swap 0 tile with above element.
                newData[eRow, eCol] = newData[eRow - 1, eCol];
                newData[eRow - 1, eCol] = 0;
                successors.Add(new PuzzleState(newData));
            }

            if(eRow < Data.GetLength(0) - 1) {
                var newData = new int[Data.GetLength(0), Data.GetLength(1)];
                Array.Copy(Data, newData, Data.Length);
                // Swap 0 tile with below element.
                newData[eRow, eCol] = newData[eRow + 1, eCol];
                newData[eRow + 1, eCol] = 0;
                successors.Add(new PuzzleState(newData));
            }

            if (eCol > 0) {
                var newData = new int[Data.GetLength(0), Data.GetLength(1)];
                Array.Copy(Data, newData, Data.Length);
                // Swap 0 tile with left element.
                newData[eRow, eCol] = newData[eRow, eCol - 1];
                newData[eRow, eCol - 1] = 0;
                successors.Add(new PuzzleState(newData));
            }

            if (eCol < Data.GetLength(1) - 1) {
                var newData = new int[Data.GetLength(0), Data.GetLength(1)];
                Array.Copy(Data, newData, Data.Length);
                // Swap 0 tile with right element.
                newData[eRow, eCol] = newData[eRow, eCol + 1];
                newData[eRow, eCol + 1] = 0;
                successors.Add(new PuzzleState(newData));
            }

            return successors.OrderBy(item => Program.rnd.Next()).ToList();
        }

        public override bool Equals(object obj) {

            if (obj == null)
                return false;

            var other = (PuzzleState)(obj);
            for(var i=0; i<Data.GetLength(0); i++) {
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
}
