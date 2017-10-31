using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver.UnitTests
{
    [TestFixture]
    public class UtilsTests
    {
        [Test]
        public void Maze_String_Should_Not_Be_Empty_String()
        {
            //var sut = new Utils();
            int[][][] my3DArray = Utils.CreateJaggedArray<int[][][]>(1, 2, 5);
            int[][] my2DArray1 = Utils.CreateJaggedArray<int[][]>(1, 3);

            Assert.IsTrue(my3DArray[0][0][0] == 0);
            Assert.IsTrue(my2DArray1[0][0] == 0);
        }

        [Test]
        public void init_Bool_Jagged_Array()
        {
            bool[][] my2DArray1 = Utils.CreateJaggedArray<bool[][]>(1, 3);

            Assert.IsFalse(my2DArray1[0][0]);
            Assert.IsFalse(my2DArray1[0][1]);
            Assert.IsFalse(my2DArray1[0][2]);
        }

    }
}
