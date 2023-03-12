using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Labyrinth
{
    class Path
    {
        public static readonly int M = 10;
        public static readonly int N = 10;

        public static bool isSafe(int[,] grid, bool[,] visited, int x, int y)
        {
            if (grid[x, y] == 1 || visited[x, y] == true)
            {
                return false;
            }
            return true;
        }

        public static bool isValid(int x, int y)
        {
            if (x < M && y < N && x >= 0 && y >= 0)
            {
                return true;
            }
            return false;
        }

        public static int solve(
            int[,] grid,
            bool[,] visited,
            int i,
            int j,
            int dest_x,
            int dest_y,
            int curr_dist,
            int min_dist,
            List<int> shortestPath,
            List<int> currentPath)
        {
            if (i == dest_x && j == dest_y)
            {
                // if destination is found, update min_dist
                if (curr_dist < min_dist)
                {
                    // If a shorter distance is found
                    min_dist = curr_dist;
                    shortestPath.Clear();
                    shortestPath.AddRange(currentPath);
                    shortestPath.Add(dest_x);
                    shortestPath.Add(dest_y);
                }
                return Math.Min(curr_dist, min_dist);
            }
            // set (i, j) cell as visited
            visited[i, j] = true;

            currentPath.Add(i);
            currentPath.Add(j);

            // go to bottom cell
            if (isValid(i + 1, j) && isSafe(grid, visited, i + 1, j))
            {
                min_dist = solve(grid, visited, i + 1, j, dest_x, dest_y, curr_dist + 1, min_dist, shortestPath, currentPath);
            }
            // go to right cell
            if (isValid(i, j + 1) && isSafe(grid, visited, i, j + 1))
            {
                min_dist = solve(grid, visited, i, j + 1, dest_x, dest_y, curr_dist + 1, min_dist, shortestPath, currentPath);
            }
            // go to top cell
            if (isValid(i - 1, j) && isSafe(grid, visited, i - 1, j))
            {
                min_dist = solve(grid, visited, i - 1, j, dest_x, dest_y, curr_dist + 1, min_dist, shortestPath, currentPath);
            }
            // go to left cell
            if (isValid(i, j - 1) && isSafe(grid, visited, i, j - 1))
            {
                min_dist = solve(grid, visited, i, j - 1, dest_x, dest_y, curr_dist + 1, min_dist, shortestPath, currentPath);
            }
            
            visited[i, j] = false;

            currentPath.RemoveAt(currentPath.Count - 1);
            currentPath.RemoveAt(currentPath.Count - 1);
            
            return min_dist;
        }
    }
}