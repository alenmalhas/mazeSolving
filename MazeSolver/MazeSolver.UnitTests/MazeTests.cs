using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace MazeSolver.UnitTests
{
    [TestFixture]
    public class MazeTests
    {
        Maze sut;
        private string AcceptableCharsInMaze = "SF X\r\n";
        [SetUp]
        public void TestSetup()
        {
            sut = new Maze();
            sut.ReadFile(Path.Combine(System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), @"TestSampleFiles\ExampleMaze.txt"));
        }

        [Test]
        public void Maze_String_Should_Not_Be_Empty_String()
        {
            Assert.IsNotEmpty(sut.MazeString);
        }

        [Test]
        public void Maze_Should_Have_RowCount_ColCount()
        {
            Assert.IsTrue(sut.RowCount > 0);
            Assert.IsTrue(sut.ColCount > 0);
        }


        [Test]
        public void MazeShouldOnlyHave_WallSpaceStartFinish()
        {
            var q = sut.MazeString.All(c => AcceptableCharsInMaze.IndexOf(c) != -1);
            Assert.IsTrue(q);
        }

        [Test]
        public void MazeShouldOnlyHave_OneStart()
        {
            var q = sut.MazeString.Count(c => "S".IndexOf(c) != -1);
            Assert.IsTrue(q == 1);
        }

        [Test]
        public void MazeShouldOnlyHave_OneFinish()
        {
            var q = sut.MazeString.Count(c => "F".IndexOf(c) != -1);
            Assert.IsTrue(q == 1);
        }

        [Test]
        public void MazeShould_Expose_NumberOf_Walls()
        {
            Assert.IsTrue(sut.NumOfWalls > 0);
        }

        [Test]
        public void MazeShould_Expose_NumberOf_EmptySpaces()
        {
            Assert.IsTrue(sut.NumOfEmptySpaces > 0);
        }

        [Test]
        public void Maze_Should_Expose_Query_For_A_Coordinate()
        {
            var charValue = sut.QueryByCoordinate(1, 2);
            Assert.IsTrue(AcceptableCharsInMaze.IndexOf(charValue) != -1);
        }

        [Test]
        public void Maze_Should_Expose_Query_By_CellType_Throws_Exception_For_Non_Start_Finish_Cell()
        {
            Assert.Throws<NotSupportedException>(() => sut.QueryByCellType(' '));
        }

        [Test]
        public void Maze_Should_Expose_Query_By_CellType_Returns_Location_For_Start_or_Finish_Cell()
        {
            var loc = sut.QueryByCellType('F');
            Assert.IsTrue(loc.RowNo >= 0 && loc.ColNo >= 0);
        }

        [Test]
        public void Maze_Should_Return_Consistent_Coordinates()
        {
            char inputChar = 'F';
            var loc = sut.QueryByCellType(inputChar);
            var retChar = sut.QueryByCoordinate(loc.RowNo, loc.ColNo);

            Assert.IsTrue(retChar == inputChar);
        }


    }
}
