using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MazeSolver.MazeSolver;
using static MazeSolver.Utils;

namespace MazeSolver
{
    public interface IExplorer
    {
        List<Location> ExploreMaze();
    }

    public class Explorer : IExplorer
    {
        public IMaze CurrentMaze { get; private set; }
        List<Location> LocationHistory = new List<Location>();
        private CharLocationTag[][] MazeNavigator;

        #region public methods

        public Explorer(IMaze m)
        {
            CurrentMaze = m;
            CurrentLocation = CurrentMaze.QueryByCellType('S');
            MazeNavigator = MazeSolverFactory.Create(m);
        }

        public bool Move(Movement direction)
        {
            Location newLoc = new Location();
            switch (direction)
            {
                case Movement.NORTH:
                    newLoc = Utils.GetLocation(CurrentLocation, -1, 0);
                    break;
                case Movement.SOUTH:
                    newLoc = Utils.GetLocation(CurrentLocation, 0, -1);
                    break;
                case Movement.WEST:
                    newLoc = Utils.GetLocation(CurrentLocation, 0, -1);
                    break;
                case Movement.EAST:
                    newLoc = Utils.GetLocation(CurrentLocation, 0, 1);
                    break;
                default:
                    break;
            }

            var isNewLocMovable = Movable(newLoc);
            if (isNewLocMovable)
                CurrentLocation = newLoc;

            return isNewLocMovable;
        }

        #endregion

        #region private methods

        private Location _currentLocation = new Location();
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                LocationHistory.Add(value);
                _currentLocation = value;
            }
        }

        bool Movable(Location loc)
        {
            if (MazeNavigator[loc.RowNo][loc.ColNo].tag != RouteTag.OBSTACLE
                || MazeNavigator[loc.RowNo][loc.ColNo].tag != RouteTag.DEAD_END)
                return true;

            return false;
        }

        public List<Location> ExploreMaze()
        {
            var startLocation = CurrentMaze.QueryByCellType('S');
            var mazeSolver = new MazeSolver(CurrentMaze);
            var solved = mazeSolver.RecursiveSolve(startLocation.RowNo, startLocation.ColNo);


            return solved ? mazeSolver.correctPath : null;
        }

        #endregion



    }
}
