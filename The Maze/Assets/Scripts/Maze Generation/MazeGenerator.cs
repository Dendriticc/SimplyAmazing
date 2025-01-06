using UnityEngine;
using Unity.AI.Navigation;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    
    private int length, width;

    public GameObject start, exit, wall, floor, roof, goodItem, badItem, mystery;
    private Cell[,] grid;
    private List<Vector2Int> wallList;
    private Vector2Int endCell;

    void Start()
    {
        length = GameManager.Length;
        width = GameManager.Width;

        InitializeGrid();
        GenerateMaze();
        DrawMaze();
        
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    void InitializeGrid()
    {
        grid = new Cell[length, width];

        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < width; y++)
            {
                grid[x, y] = new Cell();
            }
        }
    }

    void GenerateMaze()
    {

        wallList = new List<Vector2Int>();

        // Choose a random starting cell
        int startX = Random.Range(1, length - 1);
        int startY = Random.Range(1, width - 1);

        // Ensure starting on an odd coordinate
        startX = startX % 2 == 0 ? startX - 1 : startX;
        startY = startY % 2 == 0 ? startY - 1 : startY;

        endCell = new Vector2Int(startX, startY);

        // Mark the starting cell as a passage
        grid[startX, startY].IsVisited = true;
        grid[startX, startY].IsWall = false;

        Instantiate(start, new Vector3(startX * 2, start.transform.localScale.y / 2, startY * 2), Quaternion.identity, transform);

        // Add the neighboring walls to the wall list
        AddWallsToList(startX, startY);

        // Main loop of the algorithm
        while (wallList.Count > 0)
        {
            // Select a random wall from the list
            int randomIndex = Random.Range(0, wallList.Count);
            Vector2Int wall = wallList[randomIndex];
            wallList.RemoveAt(randomIndex);
            endCell = new Vector2Int(wall.x, wall.y);
            // Check if the wall divides a visited and unvisited cell
            ProcessWall(wall);
        }
        ProcessEndCell();
        Instantiate(exit, new Vector3(endCell.x * 2, exit.transform.localScale.y / 2, endCell.y * 2), Quaternion.identity, transform);
    }
    void AddWallsToList(int x, int y)
    {
        if (x - 2 > 0)
            wallList.Add(new Vector2Int(x - 1, y));

        if (x + 2 < length - 1)
            wallList.Add(new Vector2Int(x + 1, y));

        if (y - 2 > 0)
            wallList.Add(new Vector2Int(x, y - 1));

        if (y + 2 < width - 1)
            wallList.Add(new Vector2Int(x, y + 1));
    }

    void ProcessWall(Vector2Int wall)
    {
        int x = wall.x;
        int y = wall.y;

        // Determine the cells on either side of the wall
        List<Vector2Int> neighbors = GetNeighbors(x, y);

        if (neighbors.Count == 2)
        {
            Cell cell1 = grid[neighbors[0].x, neighbors[0].y];
            Cell cell2 = grid[neighbors[1].x, neighbors[1].y];

            // If one of the cells is unvisited
            if (cell1.IsVisited != cell2.IsVisited)
            {
                // Make the wall a passage
                grid[x, y].IsWall = false;

                // Mark the unvisited cell as visited
                if (!cell1.IsVisited)
                {
                    cell1.IsVisited = true;
                    cell1.IsWall = false;
                    AddWallsToList(neighbors[0].x, neighbors[0].y);
                }
                else
                {
                    cell2.IsVisited = true;
                    cell2.IsWall = false;
                    AddWallsToList(neighbors[1].x, neighbors[1].y);
                }
            }
        }
    }

    void ProcessEndCell()
    {
        if (grid[endCell.x, endCell.y].IsWall)
        {
            List<Vector2Int> neighbors = GetNeighbors(endCell.x, endCell.y);
            foreach (Vector2Int cell in neighbors)
            {
                if (!grid[cell.x, cell.y].IsWall)
                {
                    endCell = cell;
                    return;
                }
            }
        }
    }

    List<Vector2Int> GetNeighbors(int x, int y)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        if (x % 2 == 1)
        {
            // Vertical wall
            if (y - 1 >= 0) neighbors.Add(new Vector2Int(x, y - 1));
            if (y + 1 < width) neighbors.Add(new Vector2Int(x, y + 1));
        }
        else if (y % 2 == 1)
        {
            // Horizontal wall
            if (x - 1 >= 0) neighbors.Add(new Vector2Int(x - 1, y));
            if (x + 1 < length) neighbors.Add(new Vector2Int(x + 1, y));
        }

        return neighbors;
    }

    void DrawMaze()
    {
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < width; y++)
            {
                Vector3 position = new Vector3(x * 2, 0, y * 2);

                if (grid[x, y].IsWall)
                {
                    Instantiate(wall, new Vector3(position.x, wall.transform.localScale.y/2, position.z), Quaternion.identity, transform);
                }
                else
                {
                    Instantiate(floor, position, Quaternion.identity, transform);
                    Instantiate(roof, new Vector3(position.x, wall.transform.localScale.y, position.z), Quaternion.identity, transform);
                    if (Random.Range(0, 1f) < GameManager.ProbabilityOfMystery)
                    {
                        Instantiate(mystery, new Vector3(position.x, mystery.transform.localScale.y/2, position.z), Quaternion.identity, transform);
                        grid[x,y].IsRevealed = false;
                    }
                    if (Random.Range(0, 1f) < GameManager.ProbabilityOfGoodItem) 
                    {
                        Instantiate(goodItem, new Vector3(position.x, 1f, position.z), Quaternion.identity, transform);
                        grid[x, y].hiddenReward += 10f;
                    }
                    if (Random.Range(0, 1f) < GameManager.ProbabilityOfBadItem && grid[x,y].hiddenReward == 0) 
                    {
                        Instantiate(badItem, new Vector3(position.x, 0.1f, position.z), Quaternion.identity, transform);
                        grid[x, y].hiddenReward -= 5f;
                    }
                }
            }
        }
    }

    public Cell[,] GetCells() {return grid;}
}

