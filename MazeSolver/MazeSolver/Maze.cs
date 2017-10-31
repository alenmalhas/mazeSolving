using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    public interface IMaze
    {
        string MazeString { get; }

        int RowCount { get; }
        int ColCount { get; }


        int NumOfWalls { get; }
        int NumOfEmptySpaces { get; }

        void ReadFile(string mazeFilePath);

        char QueryByCoordinate(int rowNo, int colNo);
        Location QueryByCellType(char cellType);
    }

    public class Maze : IMaze
    {
        public string MovableCellTypes = "S F";

        public string MazeString { get; private set; }
        private char[][] MazeJaggedArray { get; set; }

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

        public void ReadFile(string mazeFilePath)
        //public void Maze(string mazeFilePath)
        {
            MazeString = File.ReadAllText(mazeFilePath);
            var stringList = MazeString.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            MazeJaggedArray = new char[stringList.Length][];
            int i = 0;
            foreach (var mazeLine in stringList)
            {
                MazeJaggedArray[i++] = mazeLine.ToCharArray();
            }
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
