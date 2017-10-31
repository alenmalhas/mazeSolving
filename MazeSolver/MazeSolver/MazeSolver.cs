using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    public interface IMazeSolver
    {
        bool Solve(int rowNo, int colNo, IMaze maze);
        bool Movable(Location loc);
        List<Location> CorrectPath { get; }
    }

    public class MazeSolver
    {
        public enum RouteTag
        {
            OBSTACLE, DEAD_END, NOT_TRIED, TRIED, PART_OF_PATH,
        }

        public struct CharLocationTag
        {
            public char symbol;
            public Location loc;
            public RouteTag tag;
        }

        public List<Location> correctPath = new List<Location>();

        private IMaze CurrentMaze;
        private CharLocationTag[][] MazeNavigator;

        public MazeSolver(IMaze maze)
        {
            this.CurrentMaze = maze;
            this.MazeNavigator = MazeSolverFactory.Create(maze);
        }


        public bool RecursiveSolve(int rowNo, int colNo)
        {
            Debug.WriteLine($"{rowNo} {colNo}");
            if (rowNo < 0 || rowNo >= CurrentMaze.RowCount
                || colNo < 0 || colNo >= CurrentMaze.ColCount)// assuming all rows are the same, but jagged arrays support variable row lengths
                return false;

            if (MazeNavigator[rowNo][colNo].symbol == 'F')
            {
                correctPath.Add(MazeNavigator[rowNo][colNo].loc);
                return true;
            }


            if (MazeNavigator[rowNo][colNo].tag == RouteTag.OBSTACLE
                || MazeNavigator[rowNo][colNo].tag == RouteTag.TRIED)
                return false;

            MazeNavigator[rowNo][colNo].tag = RouteTag.TRIED;

            var found =
                RecursiveSolve(rowNo, colNo - 1) ||
                RecursiveSolve(rowNo, colNo + 1) ||
                RecursiveSolve(rowNo + 1, colNo) ||
                RecursiveSolve(rowNo - 1, colNo);

            if (found)
            {
                MazeNavigator[rowNo][colNo].tag = RouteTag.PART_OF_PATH;
                correctPath.Add(MazeNavigator[rowNo][colNo].loc);
            }
            else
            {
                MazeNavigator[rowNo][colNo].tag = RouteTag.DEAD_END;
            }

            return found;
        }
    }
}
