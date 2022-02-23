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

        public override bool Equals(object? obj)
        {
            if (obj == null) { return false; }

            Cell c = obj as Cell;
            return x == c.x && y == c.y;
        }
    }
    internal class Maze
    {
        private MazeData m_maze;

        public Maze(int iSize)
        {
            m_maze = new MazeData(iSize);
            do
            {
                MazeGenerator.InitMaze(out m_maze, iSize);
            }while (!MazeGenerator.CreatePath(ref m_maze));
            Console.WriteLine(ToString());
        }

        // Return the maze list.
        public MazeData GetMaze { get { return m_maze; } }

        // Convert the maze to a string
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

    // Handles the functions related to constructing a new maze.
    internal static class MazeGenerator
    {
        private static readonly Random rand = new();

        // Creates an empty iSize x iSize array of Cells
        public static void InitMaze(out MazeData maze, int iSize)
        {
            // Create a new List of List<Cell> and set capacity to specified size.
            maze = new MazeData(iSize);

            // Create a list to initialize each row of maze.
            for (int x = 0; x < iSize; x++)
            {
                List<Cell> initializerList = new();

                // Add each element in row to initializerList
                for (int y = 0; y < iSize; y++) { initializerList.Add(new Cell(x, y)); }

                // Add each row of Cell to maze
                maze.Add(initializerList);
            }
        }

        // TODO: Smarter path generation algorithm
        public static bool CreatePath(ref MazeData maze)
        {
            // Dimensions of maze
            int iSize = maze.Count;

            // Note: Objects are references, not copies

            // Set the currentCell to the start of the maze (0, 0)
            Cell currentCell = maze[0][0];
            currentCell.val = 'S';

            // Set a random end cell somewhere on the right side
            Cell end = maze[iSize-1][rand.Next(1, iSize)];
            end.val = 'E';

            // Loop until end has been reached
            while (currentCell != end) 
            {
                // Get all unvisited neighbors.
                List<Cell> unvisitedNeighbors = GetUnvisitedNeighbors(ref maze, currentCell);

                // No unvisited neighbors? Regenerate and try again.
                if (unvisitedNeighbors.Count == 0) 
                { return false; }

                // If any of the unvisitedNeighbors are the end then path has been made
                if (unvisitedNeighbors.Contains(end)) { return true; }
                if (unvisitedNeighbors.Count == 0) 
                { Console.WriteLine("wtf"); }
                    // Pick a random neighbor and 'move' to it
                    currentCell = unvisitedNeighbors[rand.Next(0, unvisitedNeighbors.Count)];

                // Set cell to a path
                currentCell.val = ' ';
            }
            return true;
        }

        // Return list of unvisited neighbors from around currentCell
        private static List<Cell> GetUnvisitedNeighbors(ref MazeData maze, Cell currentCell)
        {
            List<Cell> unvisitedNeighbors = new List<Cell>();

            // Add neighbors to the list if they have not been visited (do not have a val of ' ').
            if (currentCell.x > 0 && maze[currentCell.x - 1][currentCell.y].val is not ' ' and not 'S') { unvisitedNeighbors.Add(maze[currentCell.x - 1][currentCell.y]); }
            if (currentCell.y > 0 && maze[currentCell.x][currentCell.y - 1].val is not ' ' and not 'S') { unvisitedNeighbors.Add(maze[currentCell.x][currentCell.y - 1]); }
            if (currentCell.x < maze.Count - 1 && maze[currentCell.x + 1][currentCell.y].val is not ' ' and not 'S') { unvisitedNeighbors.Add(maze[currentCell.x + 1][currentCell.y]); }
            if (currentCell.y < maze.Count - 1 && maze[currentCell.x][currentCell.y + 1].val is not ' ' and not 'S') { unvisitedNeighbors.Add(maze[currentCell.x][currentCell.y + 1]); }

            return unvisitedNeighbors;
        }

    }
}