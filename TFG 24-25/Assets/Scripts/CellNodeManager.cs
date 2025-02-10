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
    private Vector2 gridSize;
    private int currentId;

    public UnityEvent<int> togglePossibleMovements;
    public UnityEvent<TeamType> togglePossibleSpawnTiles;

    private List<CellNode> redSpawnTiles;
    private List<CellNode> blueSpawnTiles;

    private void Awake()
    {
        nodeGrid = new List<CellNode>();
        redSpawnTiles = new List<CellNode>();
        blueSpawnTiles = new List<CellNode>();

        togglePossibleMovements = new UnityEvent<int>();

        togglePossibleMovements.AddListener((int tileId) =>
        {
            ToggleVisibilityPossibleMovements(tileId);
        });

        togglePossibleSpawnTiles = new UnityEvent<TeamType>();
        togglePossibleSpawnTiles.AddListener((TeamType teamType) =>
        {
            switch (teamType)
            {
                case TeamType.RED:
                    foreach (CellNode spawnableTile in redSpawnTiles)
                    {
                        spawnableTile.ChangeMovementIndicatorVisibility();
                    }
                    break;
                case TeamType.BLUE:
                    foreach (CellNode spawnableTile in blueSpawnTiles)
                    {
                        spawnableTile.ChangeMovementIndicatorVisibility();
                    }
                    break;
                default:
                    break;
            }
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

            }
        }
    }

    public void CreateSpecialTiles(int numSpawnableRows)
    {
        for (int i = 0;i < gridSize.x * numSpawnableRows; i++)
        {
            nodeGrid[i].SetSpawnable(CellSpawnable.RED);
            redSpawnTiles.Add(nodeGrid[i]);
            nodeGrid[nodeGrid.Count - i-1].SetSpawnable(CellSpawnable.BLUE);
            blueSpawnTiles.Add(nodeGrid[nodeGrid.Count - i - 1]);
        }

        SetScoreNodes2x2();
        //SetScoreNodes4x2();
        //SetQuickScoreNodes();
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

    void SetScoreNodes2x2()
    {
        nodeGrid[14].SetScoreNode(CellScoreType.NORMAL);
        nodeGrid[15].SetScoreNode(CellScoreType.NORMAL);
        nodeGrid[20].SetScoreNode(CellScoreType.NORMAL);
        nodeGrid[21].SetScoreNode(CellScoreType.NORMAL);
    }

    void SetScoreNodes4x2()
    {
        nodeGrid[13].SetScoreNode(CellScoreType.NORMAL);
        nodeGrid[14].SetScoreNode(CellScoreType.NORMAL);
        nodeGrid[15].SetScoreNode(CellScoreType.NORMAL);
        nodeGrid[16].SetScoreNode(CellScoreType.NORMAL);
        nodeGrid[19].SetScoreNode(CellScoreType.NORMAL);
        nodeGrid[20].SetScoreNode(CellScoreType.NORMAL);
        nodeGrid[21].SetScoreNode(CellScoreType.NORMAL);
        nodeGrid[22].SetScoreNode(CellScoreType.NORMAL);
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
}
