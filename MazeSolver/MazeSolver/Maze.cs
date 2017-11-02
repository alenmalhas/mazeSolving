using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MazeSolver.Maze;

namespace MazeSolver
{
    public interface IMaze
    {
        string MazeString { get; }

        int RowCount { get; }
        int ColCount { get; }


        int NumOfWalls { get; }
        int NumOfEmptySpaces { get; }
        CharLocationTag[][] MazeNavigator { get; }

        char QueryByCoordinate(int rowNo, int colNo);
        Location QueryByCellType(char cellType);
        bool Movable(Location loc);
        void SetByCoordinate(int rowNo, int colNo, char charVal);
    }

    public class Maze : IMaze
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

        public string MovableCellTypes = "S F";
        public string MazeString { get; private set; }
        public char[][] MazeJaggedArray { get; set; }

        public int NumOfWalls
        {
            get { return MazeString.Count(c => c == 'X'); }
        }
        public int NumOfEmptySpaces
        {
            get { return MazeString.Count(c => c == ' '); }
        }

        public int RowCount { get { return MazeJaggedArray.Length; } }
        public int ColCount { get { return MazeJaggedArray[0].Length; } }

        //CharLocationTag[][] IMaze.MazeNavigator => throw new NotImplementedException();
        public CharLocationTag[][] MazeNavigator { get; private set; }

        public Maze(string mazeFilePath)
        {
            MazeString = File.ReadAllText(mazeFilePath);
            var stringList = MazeString.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            MazeJaggedArray = new char[stringList.Length][];
            int i = 0;
            foreach (var mazeLine in stringList)
            {
                MazeJaggedArray[i++] = mazeLine.ToCharArray();
            }

            this.MazeNavigator = CreateMazeNavigator();
        }

        public bool Movable(Location loc)
        {
            if (MazeNavigator[loc.RowNo][loc.ColNo].tag != RouteTag.OBSTACLE
                || MazeNavigator[loc.RowNo][loc.ColNo].tag != RouteTag.DEAD_END)
                return true;

            return false;
        }

        
        private CharLocationTag[][] CreateMazeNavigator()
        {
            var mazeNavigator = new CharLocationTag[this.RowCount][];
            for (int i = 0; i < this.RowCount; i++)
            {
                mazeNavigator[i] = new CharLocationTag[this.ColCount];
                for (int j = 0; j < mazeNavigator[i].Length; j++)
                {
                    RouteTag curTag = RouteTag.NOT_TRIED;
                    var symbol = this.QueryByCoordinate(i, j);
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

        public void SetByCoordinate(int rowNo, int colNo, char charVal)
        {
            MazeJaggedArray[rowNo][colNo] = charVal;
        }

        public char QueryByCoordinate(int rowNo, int colNo)
        {
            return MazeJaggedArray[rowNo][colNo];
        }

        public Location QueryByCellType(char cellType)
        {
            var countOfCell = MazeString.Count(c => c == cellType);
            if (countOfCell > 1)
                throw new NotSupportedException("Only start and finish cells location can be returned");

            Location retVal = new Location();
            for (int i = 0; i < MazeJaggedArray.Length; i++)
            {
                for (int j = 0; j < MazeJaggedArray[i].Length; j++)
                {
                    if (MazeJaggedArray[i][j] == cellType)
                    {
                        retVal.RowNo = i;
                        retVal.ColNo = j;
                        return retVal;
                    }
                }
            }
            return retVal;
        }

    }

}
