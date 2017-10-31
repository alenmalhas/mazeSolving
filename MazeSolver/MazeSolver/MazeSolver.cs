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

    public class MazeSolver : IMazeSolver
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

        public List<Location> CorrectPath { get; } = new List<Location>();

        //private IMaze CurrentMaze;
        private CharLocationTag[][] MazeNavigator;

        public MazeSolver(Maze maze)
        {
            //this.CurrentMaze = maze;
            this.MazeNavigator = CreateMazeNavigator(maze);
        }

        private CharLocationTag[][] CreateMazeNavigator(Maze m)
        {
            var mazeNavigator = new CharLocationTag[m.RowCount][];
            for (int i = 0; i < m.RowCount; i++)
            {
                mazeNavigator[i] = new CharLocationTag[m.ColCount];
                for (int j = 0; j < mazeNavigator[i].Length; j++)
                {
                    RouteTag curTag = RouteTag.NOT_TRIED;
                    var symbol = m.QueryByCoordinate(i, j);
                    if (symbol == 'X')
                        curTag = RouteTag.OBSTACLE;
                    else if (symbol == 'S')
                        curTag = RouteTag.PART_OF_PATH;
                    else if (symbol == 'F')
                        curTag = RouteTag.PART_OF_PATH;

                    mazeNavigator[i][j] = new CharLocationTag()
                    {
                        symbol = symbol,
                        loc = new Location { RowNo = i, ColNo = j },
                        tag = curTag,
                    };
                }
            }

            return mazeNavigator;
        }

        public bool Movable(Location loc)
        {
            if (MazeNavigator[loc.RowNo][loc.ColNo].tag != RouteTag.OBSTACLE
                || MazeNavigator[loc.RowNo][loc.ColNo].tag != RouteTag.DEAD_END)
                return true;

            return false;
        }

        public bool Solve(int rowNo, int colNo, IMaze maze)
        {
            Debug.WriteLine($"{rowNo} {colNo}");
            if (rowNo < 0 || rowNo >= maze.RowCount
                || colNo < 0 || colNo >= maze.ColCount)// assuming all rows are the same, but jagged arrays support variable row lengths
                return false;

            if (MazeNavigator[rowNo][colNo].symbol == 'F')
            {
                CorrectPath.Add(MazeNavigator[rowNo][colNo].loc);
                return true;
            }


            if (MazeNavigator[rowNo][colNo].tag == RouteTag.OBSTACLE
                || MazeNavigator[rowNo][colNo].tag == RouteTag.TRIED)
                return false;

            MazeNavigator[rowNo][colNo].tag = RouteTag.TRIED;

            var found =
                Solve(rowNo, colNo - 1, maze) ||
                Solve(rowNo, colNo + 1, maze) ||
                Solve(rowNo + 1, colNo, maze) ||
                Solve(rowNo - 1, colNo, maze);

            if (found)
            {
                MazeNavigator[rowNo][colNo].tag = RouteTag.PART_OF_PATH;
                CorrectPath.Add(MazeNavigator[rowNo][colNo].loc);
            }
            else
            {
                MazeNavigator[rowNo][colNo].tag = RouteTag.DEAD_END;
            }

            return found;
        }
    }
}
