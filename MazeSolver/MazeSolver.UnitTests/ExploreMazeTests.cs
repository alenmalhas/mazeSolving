using Moq;
using NUnit.Framework;
using System;
using System.IO;

namespace MazeSolver.UnitTests
{
    [TestFixture]
    public class ExploreMazeTests
    {
        Explorer sut;
        Mock<IMaze> mockMaze = new Mock<IMaze>();
        Mock<IMazeSolver> mockMazeSolver = new Mock<IMazeSolver>();

        [SetUp]
        public void TestSetup()
        {
            sut = new Explorer(mockMaze.Object, mockMazeSolver.Object);
        }

        [Test]
        public void Explorer_CurrentMaze_Should_Not_Be_Null()
        {
            Assert.IsNotNull(sut.CurrentMaze);
        }


        [Test]
        public void Explorer_Exposes_Current_Location()
        {
            var loc = sut.CurrentLocation;
            mockMaze.Setup(a => a.QueryByCoordinate(It.IsAny<int>(), It.IsAny<int>())).Returns('S');
            Assert.IsNotNull(sut.CurrentMaze.QueryByCoordinate(loc.RowNo, loc.ColNo));
        }

        [Test]
        public void Explorer_Initial_Location_Should_Be_Start_Of_Maze()
        {
            var sampleMaze = new Maze();
            sampleMaze.ReadFile(Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), @"TestSampleFiles\ExampleMaze.txt"));
            sut = new Explorer(sampleMaze, mockMazeSolver.Object);

            var loc = sut.CurrentLocation;
            Assert.IsTrue(sut.CurrentMaze.QueryByCoordinate(loc.RowNo, loc.ColNo) == 'S');
        }

        [Test]
        public void Explorer_Moves_CurrentLocation_Changes()
        {
            mockMazeSolver.Setup(a => a.Movable(It.IsAny<Location>())).Returns(true);
            var sampleMaze = new Maze();
            sampleMaze.ReadFile(Path.Combine(System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), @"TestSampleFiles\ExampleMaze.txt"));
            sut = new Explorer(sampleMaze, mockMazeSolver.Object);

            var curLoc = sut.CurrentLocation;
            // act
            sut.Move(Utils.Movement.EAST);
            var newLoc = sut.CurrentLocation;

            Assert.IsTrue(curLoc.ColNo + 1 == newLoc.ColNo);
            Assert.IsTrue(curLoc.RowNo == newLoc.RowNo);
        }

    }
}
