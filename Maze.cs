using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    using MazeData = List<List<Cell>>;
    class Cell
    {
        public int x, y;
        public char val;

        public Cell(int X, int Y) : this(X, Y, 'X') { }
        public Cell(int X, int Y, char Val)
        {
            x = X;
            y = Y;
            val = Val;
        }

        public override bool Equals(object obj)
        {
            // TODO
        }
    }
    internal class Maze
    {
        private MazeData m_maze;

        public Maze(int iSize)
        {
            m_maze = new MazeData(iSize);
            MazeGenerator.InitMaze(ref m_maze, iSize);
            MazeGenerator.CreatePath(ref m_maze);
        }

        public MazeData GetMaze { get { return m_maze; } }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (List<Cell> row in m_maze)
            {
                foreach (Cell cell in row)
                {
                    sb.Append(cell.val);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }

    internal static class MazeGenerator
    {
        private static Random rand = new Random();
        public static void InitMaze(ref MazeData maze, int iSize)
        {
            maze = new MazeData(iSize);
            for (int x = 0; x < iSize; x++)
            {
                List<Cell> initializerList = new List<Cell>();
                for (int y = 0; y < iSize; y++)
                {
                    initializerList.Add(new Cell(x, y));
                }
                maze.Add(initializerList);
            }

            maze[0][0].val = 'S';
            maze[iSize - 1][rand.Next(1, iSize)].val = 'E';
        }

        public static void CreatePath(ref MazeData maze)
        {
            // TODO: Create path from start to end.
        }

        private static List<Cell> GetUnvisitedNeighbors(ref MazeData maze, Cell currentCell)
        {
            List<Cell> unvisitedNeighbors = new List<Cell>();

            if (currentCell.x > 0) { unvisitedNeighbors.Add(); }
            if (currentCell.y > 0) { unvisitedNeighbors.Add(); }
            if (currentCell.x < maze.Count - 1) { unvisitedNeighbors.Add(); }
            if (currentCell.y < maze.Count - 1) { unvisitedNeighbors.Add(); }

            return unvisitedNeighbors;
        }

    }
}
