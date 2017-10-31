using System.Collections.Generic;
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
        private IMazeSolver MazeSolver;

        #region public methods

        public Explorer(IMaze m, IMazeSolver mazeSolver)
        {
            CurrentMaze = m;
            CurrentLocation = CurrentMaze.QueryByCellType('S');
            //mazeSolver = new MazeSolver(m);
            MazeSolver = mazeSolver;
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

            var isNewLocMovable = MazeSolver.Movable(newLoc);
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

        public List<Location> ExploreMaze()
        {
            var startLocation = CurrentMaze.QueryByCellType('S');
            //var mazeSolver = new MazeSolver(CurrentMaze);
            var solved = MazeSolver.Solve(startLocation.RowNo, startLocation.ColNo, CurrentMaze);


            return solved ? MazeSolver.CorrectPath : null;
        }

        #endregion
    }
}
