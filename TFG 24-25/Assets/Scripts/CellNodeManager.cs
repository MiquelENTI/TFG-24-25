using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.Examples;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class CellNodeManager : Singleton<CellNodeManager>
{
    private List<CellNode> nodeGrid;
    private Vector2 gridSize;
    private int currentId;

    public UnityEvent<int> togglePossibleMovements;
    public UnityEvent<TeamType> togglePossibleSpawnTiles;

    private List<CellNode> redSpawnTiles;
    private List<CellNode> blueSpawnTiles;

    // New Movement
    public Dictionary<Vector2, CellNode> nodeGrid2D;

    private void Awake()
    {
        nodeGrid = new List<CellNode>();
        redSpawnTiles = new List<CellNode>();
        blueSpawnTiles = new List<CellNode>();

        // Event that Triggers ToggleVisibilityPossibleMovements Function
        togglePossibleMovements = new UnityEvent<int>();
        togglePossibleMovements.AddListener((int tileId) =>
        {
            ToggleVisibilityPossibleMovements(tileId);
        });

        // Event that Triggers ToggleVisibilityAvailableSpawnCells
        togglePossibleSpawnTiles = new UnityEvent<TeamType>();
        togglePossibleSpawnTiles.AddListener((TeamType teamType) =>
        {
            ToggleVisibilityAvailableSpawnCells(teamType);
        });

        // New Movement
        nodeGrid2D = new Dictionary<Vector2, CellNode>();
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

    // Debugging Function to check all the CellNodes' status
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

    // Function that Creates All the CellNodes Connections
    public void CreateDefaultConnections(CellNode redBase, CellNode blueBase)
    {
        // Vector2 Directions Reference
        // -1,-1   0,-1   1,-1
        // -1, 0   0, 0   0, 0
        // -1, 1   0, 1   1, 1

        // List to Easily Convert i and j Loop Variables to a Vector2
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

        // Dictionary to Convert a Vector2 to CellConnection
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

        // Loop that initializes all the connections between CellNodes
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

                // New Movement
                nodeGrid2D.Add(new Vector2(i,j), nodeGrid[j * (int)gridSize.y + i]);
            }
        }
    }

    // Function that Sets Special Properties to the Existing Cells
    public void CreateSpecialTiles(int numSpawnableRows)
    {
        // Assigns the CellSpawnable Properties to Be Able to Spawn Tokens
        // Parameter Dictates how Many Rows each Player has to Spawn Tokens

        for (int i = 0; i < gridSize.x * numSpawnableRows; i++)
        {
            nodeGrid[i].SetSpawnable(CellSpawnable.RED);
            redSpawnTiles.Add(nodeGrid[i]);
            nodeGrid[nodeGrid.Count - i-1].SetSpawnable(CellSpawnable.BLUE);
            blueSpawnTiles.Add(nodeGrid[nodeGrid.Count - i - 1]);
        }

        //SetScoreNodes2x2();
        SetEnemyScoreNodes();
        SetScoreNodes4x2();
        SetQuickScoreNodes();
    }

    // Function that Higlihts all the Token's possible movements
    void ToggleVisibilityPossibleMovements(int tileId) // Character's OnTile
    {
        // By Getting a Central CellNode Using the Parameter, Check each surrounding CellNode to See if there's an Ally Character, Enemy Character or it's Empty

        Character character = nodeGrid[tileId].GetCharacter();
        CellNode centralCell = nodeGrid[tileId];

        if(character == null) { return; }

        foreach (CellConnection direction in character.GetDirections())
        {
            if (!centralCell.CheckConnectionNode(direction))
            {
                continue;
            }

            // Cell To Check
            CellNode nextCell = centralCell.GetCellByDirection(direction);

            Character isCharacter = nextCell.GetCharacter();

            // If Character Exists
            if (isCharacter != null)
            {
                // And are in the same Team Skip, cell is not highlighted
                if (isCharacter.GetTeamType() == character.GetTeamType())
                {
                    continue;
                }
                // Or are on different teams, cell is highlighted with attacking color
                else
                {
                    // Set Cell Red and visible
                    continue;
                }
            }
            // The cell is empty and is highlighted with movement color
            nextCell.ChangeMovementIndicatorVisibility();
        }
    }

    // Function that Highlights all the Available SpawnTiles
    void ToggleVisibilityAvailableSpawnCells(TeamType teamType)
    {
        // Parameter Determines Which Tiles to Highlight
        switch (teamType)
        {
            case TeamType.RED:
                // Highlight Red Spawn Tiles
                foreach (CellNode spawnableTile in redSpawnTiles)
                {
                    spawnableTile.ChangeMovementIndicatorVisibility();
                }
                break;
            case TeamType.BLUE:
                // Highlight Red Spawn Tiles
                foreach (CellNode spawnableTile in blueSpawnTiles)
                {
                    spawnableTile.ChangeMovementIndicatorVisibility();
                }
                break;
            default:
                break;
        }
    }

    void SetScoreNodes2x2()
    {
        nodeGrid[14].SetScoreNode(CellScoreType.NORMAL);
        nodeGrid[15].SetScoreNode(CellScoreType.NORMAL);
        nodeGrid[20].SetScoreNode(CellScoreType.NORMAL);
        nodeGrid[21].SetScoreNode(CellScoreType.NORMAL);
    }

    void SetScoreNodes4x2()
    {
        nodeGrid[13].SetScoreNode(CellScoreType.QUICK);
        nodeGrid[14].SetScoreNode(CellScoreType.QUICK);
        nodeGrid[15].SetScoreNode(CellScoreType.QUICK);
        nodeGrid[16].SetScoreNode(CellScoreType.QUICK);
        nodeGrid[19].SetScoreNode(CellScoreType.QUICK);
        nodeGrid[20].SetScoreNode(CellScoreType.QUICK);
        nodeGrid[21].SetScoreNode(CellScoreType.QUICK);
        nodeGrid[22].SetScoreNode(CellScoreType.QUICK);
    }

    void SetQuickScoreNodes()
    {
        nodeGrid[0].SetScoreNode(CellScoreType.QUICK);
        nodeGrid[1].SetScoreNode(CellScoreType.QUICK);
        nodeGrid[2].SetScoreNode(CellScoreType.QUICK);
        nodeGrid[3].SetScoreNode(CellScoreType.QUICK);
        nodeGrid[4].SetScoreNode(CellScoreType.QUICK);
        nodeGrid[5].SetScoreNode(CellScoreType.QUICK);

        nodeGrid[30].SetScoreNode(CellScoreType.QUICK);
        nodeGrid[31].SetScoreNode(CellScoreType.QUICK);
        nodeGrid[32].SetScoreNode(CellScoreType.QUICK);
        nodeGrid[33].SetScoreNode(CellScoreType.QUICK);
        nodeGrid[34].SetScoreNode(CellScoreType.QUICK);
        nodeGrid[35].SetScoreNode(CellScoreType.QUICK);
    }

    void SetEnemyScoreNodes()
    {
        nodeGrid[0].SetScoreAmount(CellScoreAmount.ENEMYROWS);
        nodeGrid[1].SetScoreAmount(CellScoreAmount.ENEMYROWS);
        nodeGrid[2].SetScoreAmount(CellScoreAmount.ENEMYROWS);
        nodeGrid[3].SetScoreAmount(CellScoreAmount.ENEMYROWS);
        nodeGrid[4].SetScoreAmount(CellScoreAmount.ENEMYROWS);
        nodeGrid[5].SetScoreAmount(CellScoreAmount.ENEMYROWS);

        nodeGrid[30].SetScoreAmount(CellScoreAmount.ENEMYROWS);
        nodeGrid[31].SetScoreAmount(CellScoreAmount.ENEMYROWS);
        nodeGrid[32].SetScoreAmount(CellScoreAmount.ENEMYROWS);
        nodeGrid[33].SetScoreAmount(CellScoreAmount.ENEMYROWS);
        nodeGrid[34].SetScoreAmount(CellScoreAmount.ENEMYROWS);
        nodeGrid[35].SetScoreAmount(CellScoreAmount.ENEMYROWS);
    }
}
