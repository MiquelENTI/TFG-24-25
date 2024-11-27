using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CellNodeManager : Singleton<CellNodeManager>
{
    private List<CellNode> nodeGrid;
    private Vector2 gridSize;
    private int currentId;

    private void Awake()
    {
        nodeGrid = new List<CellNode>();
    }

    public Vector2 GetGridSize() 
    { return gridSize; }

    public void SetGridSize(Vector2 size)
    { gridSize = size; }

    public CellNode GetNodeById(int id)
    {
        return nodeGrid[id];
    }

    public void AddNode(CellNode node)
    {
        nodeGrid.Add(node);
        node.SetId(currentId);
        currentId++;
    }

    public void PrintNodeGridStatus()
    {
        if (nodeGrid == null)
        {
            Debug.Log("NodeGrid does not exist or error");
            return;
        }

        foreach (CellNode node in nodeGrid)
        {
            node.PrintStatus();
        }
    }

    public void CreateDefaultConnections()
    {
        // -1,-1   0,-1   1,-1
        // -1, 0   0, 0   0, 0
        // -1, 1   0, 1   1, 1

        List<Vector2Int> directions = new List<Vector2Int>
        {
            new Vector2Int(-1, 0),   // Up
            new Vector2Int(1, 0),    // Down
            new Vector2Int(0, -1),   // Left
            new Vector2Int(0, 1),    // Right
            new Vector2Int(-1, -1),  // Up-Left
            new Vector2Int(-1, 1),   // Up-Right
            new Vector2Int(1, -1),   // Down-Left
            new Vector2Int(1, 1)     // Down-Right
        };

        Dictionary<Vector2Int, CellConnection> conversion = new Dictionary<Vector2Int, CellConnection> 
        {
            { new Vector2Int(-1, 0),  CellConnection.UP},
            { new Vector2Int(1, 0),  CellConnection.DOWN},
            { new Vector2Int(0, -1),  CellConnection.LEFT},
            { new Vector2Int(0, 1),  CellConnection.RIGHT},
            { new Vector2Int(-1, -1),  CellConnection.UPLEFT},
            { new Vector2Int(-1, 1),  CellConnection.UPRIGHT},
            { new Vector2Int(1, -1),  CellConnection.DOWNLEFT},
            { new Vector2Int(1, 1),  CellConnection.DOWNRIGHT}
        };

        for (int j = 0; j < gridSize.y; j++) 
        {
            for (int i = 0; i < gridSize.y; i++)
            {
                foreach (Vector2Int direction in directions)
                {
                    int newX = i + direction.x;
                    int newY = j + direction.y;

                    if (newX >= 0 && newX < gridSize.x && newY >= 0 && newY < gridSize.y)
                    {
                        int currentNodeId = (int)(j * gridSize.y) + i;
                        int nodeToAddId = (int)(newY * gridSize.y) + newX;

                        nodeGrid[currentNodeId].AddConnection(conversion[direction], nodeGrid[nodeToAddId]);
                    }
                }
            }
        }
    }

    public void CreateSpawnableTiles(int numSpawnableRows)
    {
        for (int i = 0;i < gridSize.x * numSpawnableRows ;i++)
        {
            nodeGrid[i].SetSpawnable(CellSpawnable.RED);
            nodeGrid[nodeGrid.Count - i-1].SetSpawnable(CellSpawnable.BLUE);
        }
    }
}
