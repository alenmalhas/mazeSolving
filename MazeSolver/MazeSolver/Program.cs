using Castle.MicroKernel.Registration;
using System;
using System.IO;

namespace MazeSolver
{
    class Program
    {
        static Castle.Windsor.WindsorContainer _container;
        static Castle.Windsor.WindsorContainer Container
        {
            get
            {
                if (null == _container)
                {
                    _container = new Castle.Windsor.WindsorContainer();
                    _container.Register(Component.For<IMaze>().ImplementedBy<Maze>().LifestyleTransient());
                    _container.Register(Component.For<IExplorer>().ImplementedBy<Explorer>().LifestyleTransient());
                    _container.Register(Component.For<IMazeSolver>().ImplementedBy<MazeSolver>().LifestyleTransient());
                }
                return _container;
            }
        }

        static void Main(string[] args)
        {
            //var sampleMaze = new Maze();
            var sampleMaze = Container.Resolve<IMaze>();
            sampleMaze.ReadFile(Path.Combine(System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), @"TestSampleFiles\ExampleMaze.txt"));
            var mazeSolver = Container.Resolve<IMazeSolver>(new { maze = sampleMaze });
            var explorer = Container.Resolve<IExplorer>(new { m = sampleMaze, mazeSolver = mazeSolver });
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

        static void DisplayMaze(IMaze maze)
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
