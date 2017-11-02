using NUnit.Framework;
using System;
using System.IO;


namespace MazeSolver.UnitTests
{
    [TestFixture]
    public class MazeSolverTests
    {
        [Test]
        public void MazeSolver_Solves_The_Maze()
        {
            var filePath = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), @"TestSampleFiles\ExampleMaze.txt");
            var sampleMaze = new Maze(filePath);


            var sut = new MazeSolver(sampleMaze);
            var startLocation = sampleMaze.QueryByCellType('S');
            var solved = sut.Solve(startLocation.RowNo, startLocation.ColNo);

            sut.CorrectPath.ForEach(c => Console.WriteLine($"row: {c.RowNo} col: {c.ColNo}"));

            Assert.IsTrue(solved);
        }

        //ExampleMaze1.txt
    }
}
