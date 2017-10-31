using System;
using System.IO;

namespace MazeSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var sampleMaze = new Maze();
            sampleMaze.ReadFile(Path.Combine(System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), @"TestSampleFiles\ExampleMaze1.txt"));
            var explorer = new Explorer(sampleMaze);
            var correctPath = explorer.ExploreMaze();

            //DisplayMaze(sampleMaze);
            // remove S and F from correctPath as they are already displayed on the maze
            correctPath.RemoveAt(correctPath.Count - 1);
            correctPath.RemoveAt(0);
            correctPath.ForEach(c => {
                sampleMaze.SetByCoordinate(c.RowNo, c.ColNo, '.');
            });

            DisplayMaze(sampleMaze);
            Console.ReadKey();
        }

        static void DisplayMaze(Maze maze)
        {
            for (int i = 0; i < maze.RowCount; i++)
            {
                for (int j = 0; j < maze.ColCount; j++)
                {
                    Console.Write(maze.QueryByCoordinate(i, j));
                }
                Console.WriteLine();
            }
        }
    }
}
