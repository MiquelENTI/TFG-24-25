using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class CellNodeManager : Singleton<CellNodeManager>
{
    private List<CellNode> nodeGrid;
    private CellNode redBaseNode;
    private CellNode blueBaseNode;
    private Vector2 gridSize;
    private int currentId;

    public UnityEvent<int> togglePossibleMovements;

    private void Awake()
    {
        nodeGrid = new List<CellNode>();

        togglePossibleMovements = new UnityEvent<int>();

        togglePossibleMovements.AddListener((int tileId) =>
        {
            ToggleVisibilityPossibleMovements(tileId);
        });
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

    public void CreateDefaultConnections(CellNode redBase, CellNode blueBase)
    {
        // -1,-1   0,-1   1,-1
        // -1, 0   0, 0   0, 0
        // -1, 1   0, 1   1, 1

        List<Vector2Int> directions = new List<Vector2Int>
        {
            new Vector2Int(0, -1),   // Up
            new Vector2Int(0, 1),    // Down
            new Vector2Int(-1, 0),   // Left
            new Vector2Int(1, 0),    // Right
            new Vector2Int(-1, -1),  // Up-Left
            new Vector2Int(1, -1),   // Up-Right
            new Vector2Int(-1, 1),   // Down-Left
            new Vector2Int(1, 1)     // Down-Right
        };

        Dictionary<Vector2Int, CellConnection> conversion = new Dictionary<Vector2Int, CellConnection> 
        {
            { new Vector2Int(0, -1),  CellConnection.UP},
            { new Vector2Int(0, 1),  CellConnection.DOWN},
            { new Vector2Int(-1, 0),  CellConnection.LEFT},
            { new Vector2Int(1, 0),  CellConnection.RIGHT},
            { new Vector2Int(-1, -1),  CellConnection.UPLEFT},
            { new Vector2Int(1, -1),  CellConnection.UPRIGHT},
            { new Vector2Int(-1, 1),  CellConnection.DOWNLEFT},
            { new Vector2Int(1, 1),  CellConnection.DOWNRIGHT}
        };

        redBaseNode = redBase;
        blueBaseNode = blueBase;

        redBaseNode.IsBaseNode();
        blueBaseNode.IsBaseNode();

        redBaseNode.baseConnections = new();
        blueBaseNode.baseConnections = new();

        List<CellNode> redBaseNodes = new();
        List<CellNode> blueBaseNodes = new();

        for (int j = 0; j < gridSize.y; j++) 
        {
            
            for (int i = 0; i < gridSize.x; i++)
            {
                foreach (Vector2Int direction in directions)
                {
                    int newX = i + direction.x;
                    int newY = j + direction.y;

                    if (newX >= 0 && newX < gridSize.x && newY >= 0 && newY < gridSize.y)
                    {
                        int currentNodeId = (int)(j * gridSize.x) + i;
                        int nodeToAddId = (int)(newY * gridSize.x) + newX;

                        nodeGrid[currentNodeId].AddConnection(conversion[direction], nodeGrid[nodeToAddId]);
                    }
                }

                if (j == 0)
                {
                    nodeGrid[(int)((j * gridSize.x) + i)].AddConnection(CellConnection.UP, redBaseNode);
                    redBaseNodes.Add(nodeGrid[(int)(j * gridSize.x) + i]);
                }
                else if (j == gridSize.y-1)
                {
                    nodeGrid[(int)(j * gridSize.x) + i].AddConnection(CellConnection.DOWN, blueBaseNode);
                    blueBaseNodes.Add(nodeGrid[(int)(j * gridSize.x) + i]);
                }
            }
        }

        redBaseNode.AddConnectionToBase(CellConnection.DOWN, redBaseNodes);
        blueBaseNode.AddConnectionToBase(CellConnection.UP, blueBaseNodes);

        redBaseNode.PrintStatus();
        blueBaseNode.PrintStatus();
    }

    public void CreateSpawnableTiles(int numSpawnableRows)
    {
        for (int i = 0;i < gridSize.x * numSpawnableRows ;i++)
        {
            nodeGrid[i].SetSpawnable(CellSpawnable.RED);
            nodeGrid[nodeGrid.Count - i-3].SetSpawnable(CellSpawnable.BLUE);
        }
    }

    void ToggleVisibilityPossibleMovements(int tileId) // Character's OnTile
    {
        Character character = nodeGrid[tileId].GetCharacter();
        CellNode centralCell = nodeGrid[tileId];

        if(character == null) { return; }

        foreach (CellConnection direction in character.GetDirections())
        {
            if (!centralCell.CheckConnectionNode(direction))
            {
                continue;
            }

            CellNode nextCell = centralCell.GetCellByDirection(direction);

            Character isCharacter = nextCell.GetCharacter();
            if (isCharacter != null)
            {
                if (isCharacter.GetTeamType() == character.GetTeamType())
                {
                    continue;
                }
                else
                {
                    // Set Cell Red and visible
                    continue;
                }
            }

            nextCell.ChangeMovementIndicatorVisibility();
        }
    }
}
