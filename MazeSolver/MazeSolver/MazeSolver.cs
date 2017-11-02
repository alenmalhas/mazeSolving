using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MazeSolver.Maze;

namespace MazeSolver
{
    public interface IMazeSolver
    {
        bool Solve(int rowNo, int colNo);
        List<Location> CorrectPath { get; }
    }

    public class MazeSolver : IMazeSolver
    {
        public List<Location> CorrectPath { get; } = new List<Location>();

        private IMaze maze;

        public MazeSolver(IMaze maze)
        {
            this.maze = maze;
        }

        
        public bool Solve(int rowNo, int colNo)
        {
            Debug.WriteLine($"{rowNo} {colNo}");
            if (rowNo < 0 || rowNo >= maze.RowCount
                || colNo < 0 || colNo >= maze.ColCount)// assuming all rows are the same, but jagged arrays support variable row lengths
                return false;

            if (maze.MazeNavigator[rowNo][colNo].symbol == 'F')
            {
                CorrectPath.Add(maze.MazeNavigator[rowNo][colNo].loc);
                return true;
            }


            if (maze.MazeNavigator[rowNo][colNo].tag == RouteTag.OBSTACLE
                || maze.MazeNavigator[rowNo][colNo].tag == RouteTag.TRIED)
                return false;

            maze.MazeNavigator[rowNo][colNo].tag = RouteTag.TRIED;

            var found =
                Solve(rowNo, colNo - 1) ||
                Solve(rowNo, colNo + 1) ||
                Solve(rowNo + 1, colNo) ||
                Solve(rowNo - 1, colNo);

            if (found)
            {
                maze.MazeNavigator[rowNo][colNo].tag = RouteTag.PART_OF_PATH;
                CorrectPath.Add(maze.MazeNavigator[rowNo][colNo].loc);
            }
            else
            {
                maze.MazeNavigator[rowNo][colNo].tag = RouteTag.DEAD_END;
            }

            return found;
        }
    }
}
