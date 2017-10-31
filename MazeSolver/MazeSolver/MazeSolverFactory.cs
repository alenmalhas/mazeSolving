using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MazeSolver.MazeSolver;

namespace MazeSolver
{
    internal class MazeSolverFactory
    {
        internal static CharLocationTag[][] Create(IMaze m)
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
    }
}
