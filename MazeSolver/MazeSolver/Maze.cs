using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    // Represents a 2-D array of Cells
    using MazeData = List<List<Cell>>;

    // Class so that it is passed by ref and mutable.
    // X,Y position and a Value as a char
    internal class Cell
    {
        // x and y position of cell in maze
        public int x, y;

        // value of cell
        public string val;

        // Not yet used
        // enum CellType { Wall, Space, Start, End};

        // Note: No default constructor, X and Y values are always required
        public Cell(int X, int Y) : this(X, Y, "X") { }
        public Cell(int X, int Y, string Val)
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
        
        // TODO: Implement
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
    class Maze
    {
        private MazeData m_maze;

        // Construct a new Maze, size is required
        public Maze(int iSize)
        {
            m_maze = new MazeData(iSize);

            // If maze generation fails ( could not find end because algorithm isn't smart ),
            // then retry until it succeeds.
            do
            {
                MazeGenerator.InitMaze(out m_maze, iSize);
            }while (!MazeGenerator.CreatePath(ref m_maze));

            // Write the maze as a string
            Console.WriteLine(ToString());
        }

        private string longestVal() 
        {
            string longestVal = "";
            foreach (List<Cell> row in m_maze)
            {
                foreach (Cell cell in row)
                {
                    if (cell.val.Length > longestVal.Length) { longestVal = cell.val; }
                }
            }
            return longestVal;
        }

        // Return the maze list.
        public MazeData GetMaze { get { return m_maze; } }

        // Convert the maze to a string
        public override string ToString()
        {
            int cellSize = longestVal().Length;
            StringBuilder sb = new StringBuilder();
            foreach (List<Cell> row in m_maze)
            {
                foreach (Cell cell in row)
                {
                    // Add each cell in a row to a line
                    sb.Append(cell.val);
                    for (int spaceLeft = longestVal().Length - cell.val.Length; spaceLeft > 0; spaceLeft--)
                    {
                        sb.Append(' ');
                    }
                    sb.Append(' ');
                }
                // Divide rows with new lines
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }

    // Handles the functions related to constructing a new maze.
    static class MazeGenerator
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
            int pathOrder = 1;
            // Dimensions of maze
            int iSize = maze.Count;

            // Note: Objects are references, not copies

            // Set the currentCell to the start of the maze (0, 0)
            Cell currentCell = maze[0][0];
            currentCell.val = "S";

            // Set a random end cell somewhere on the right side
            Cell end = maze[iSize-1][rand.Next(1, iSize)];
            end.val = "E";

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

                // Pick a random neighbor and 'move' to it
                currentCell = unvisitedNeighbors[rand.Next(0, unvisitedNeighbors.Count)];

                // Set cell to a path
                currentCell.val = (pathOrder++).ToString();
            }
            return true;
        }

        // Return list of unvisited neighbors from around currentCell
        private static List<Cell> GetUnvisitedNeighbors(ref MazeData maze, Cell currentCell)
        {
            List<Cell> unvisitedNeighbors = new List<Cell>();

            // Add neighbors to the list if they have not been visited (value of "X" or "E").
            if (currentCell.x > 0 && maze[currentCell.x - 1][currentCell.y].val is "X" or "E")              { unvisitedNeighbors.Add(maze[currentCell.x - 1][currentCell.y]); }
            if (currentCell.y > 0 && maze[currentCell.x][currentCell.y - 1].val is "X" or "E")              { unvisitedNeighbors.Add(maze[currentCell.x][currentCell.y - 1]); }
            if (currentCell.x < maze.Count - 1 && maze[currentCell.x + 1][currentCell.y].val is "X" or "E") { unvisitedNeighbors.Add(maze[currentCell.x + 1][currentCell.y]); }
            if (currentCell.y < maze.Count - 1 && maze[currentCell.x][currentCell.y + 1].val is "X" or "E") { unvisitedNeighbors.Add(maze[currentCell.x][currentCell.y + 1]); }

            return unvisitedNeighbors;
        }

    }
}
