using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class TileNodeManager : Singleton<TileNodeManager>
{
    private List<TileNode> nodeGrid;
    private Vector2 gridSize;
    private int currentId;

    public UnityEvent<int> hidePossibleMovements;
    public UnityEvent<int> showPossibleMovements;
    public UnityEvent<TeamType> hidePossibleSpawnTiles;
    public UnityEvent<TeamType> showPossibleSpawnTiles;

    private List<TileNode> redSpawnTiles;
    private List<TileNode> blueSpawnTiles;

    private void Awake()
    {
        nodeGrid = new List<TileNode>();
        redSpawnTiles = new List<TileNode>();
        blueSpawnTiles = new List<TileNode>();

        // Event that Triggers ToggleVisibilityPossibleMovements Function

        showPossibleMovements = new UnityEvent<int>();
        showPossibleMovements.AddListener((int tileId) =>
        {
            ToggleVisibilityPossibleMovements(tileId, true);
        });
        hidePossibleMovements = new UnityEvent<int>();
        hidePossibleMovements.AddListener((int tileId) =>
        {
            ToggleVisibilityPossibleMovements(tileId, false);
        });

        // Event that Triggers ToggleVisibilityAvailableSpawnTiles
        showPossibleSpawnTiles = new UnityEvent<TeamType>();
        showPossibleSpawnTiles.AddListener((TeamType teamType) =>
        {
            ToggleVisibilityAvailableSpawnTiles(teamType, true);
        });

        // Event that Triggers ToggleVisibilityAvailableSpawnTiles
        hidePossibleSpawnTiles = new UnityEvent<TeamType>();
        hidePossibleSpawnTiles.AddListener((TeamType teamType) =>
        {
            ToggleVisibilityAvailableSpawnTiles(teamType, false);
        });
    }
    public void SetGridSize(Vector2 size)
    { gridSize = size; }

    public TileNode GetNodeById(int id)
    {
        //Debug.Log("HOVERING: " + id);
        try
        {
            return nodeGrid[id];
        }
        catch
        {
            return null;
        }
    }

    public void AddNode(TileNode node)
    {
        nodeGrid.Add(node);
        node.SetId(currentId);
        currentId++;
    }

    // Debugging Function to check all the TileNodes' status
    public void PrintNodeGridStatus()
    {
        if (nodeGrid == null)
        {
            Debug.Log("NodeGrid does not exist or error");
            return;
        }

        foreach (TileNode node in nodeGrid)
        {
            node.PrintStatus();
        }
    }

    // Function that Creates All the TileNodes Connections
    public void CreateDefaultConnections()
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

        // Dictionary to Convert a Vector2 to TileConnection
        Dictionary<Vector2Int, TileConnection> conversion = new Dictionary<Vector2Int, TileConnection> 
        {
            { new Vector2Int(0, -1),  TileConnection.UP},
            { new Vector2Int(0, 1),  TileConnection.DOWN},
            { new Vector2Int(-1, 0),  TileConnection.LEFT},
            { new Vector2Int(1, 0),  TileConnection.RIGHT},
            { new Vector2Int(-1, -1),  TileConnection.UPLEFT},
            { new Vector2Int(1, -1),  TileConnection.UPRIGHT},
            { new Vector2Int(-1, 1),  TileConnection.DOWNLEFT},
            { new Vector2Int(1, 1),  TileConnection.DOWNRIGHT}
        };

        // Loop that initializes all the connections between TileNodes
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

    // Function that Sets Special Properties to the Existing Tiles
    public void CreateSpecialTiles(int numSpawnableRows)
    {
        // Assigns the TileSpawnable Properties to Be Able to Spawn Tokens
        // Parameter Dictates how Many Rows each Player has to Spawn Tokens

        for (int i = 0; i < gridSize.x * numSpawnableRows; i++)
        {
            nodeGrid[i].SetSpawnable(TileSpawnable.RED);
            redSpawnTiles.Add(nodeGrid[i]);
            nodeGrid[nodeGrid.Count - i-1].SetSpawnable(TileSpawnable.BLUE);
            blueSpawnTiles.Add(nodeGrid[nodeGrid.Count - i - 1]);
        }

        SetScoreNodes4x2();
        SetEnemyScoreNodes();
    }

    // Function that Higlihts all the Token's possible movements
    public void ToggleVisibilityPossibleMovements(int tileId, bool state) // Character's OnTile
    {
        // By Getting a Central TileNode Using the Parameter, Check each surrounding TileNode to See if there's an Ally Character, Enemy Character or it's Empty

        VFXManager.Instance.RemoveTeamHighlights();

        Character character = nodeGrid[tileId].GetCharacter();
        TileNode centralTile = nodeGrid[tileId];

        bool hasCharacterJumped = false;

        if (character == null) 
        { return; }

        if (PlayerStats.Instance.GetCurrentMana() < character.GetCharacterStats().manaCost) 
        { return; }
        
        foreach (TileConnection direction in character.GetDirections())
        {
            TileNode nextTile = centralTile;

            // If the character cannot move, goto check if character can attack
            if (!character.GetCanMove())
            { goto Attack; }

            // Movement
            for (int i = 0; i<character.GetCharacterStats().movementRange; i++)
            {
                if (!nextTile.CheckConnectionNode(direction))
                { continue; }

                // Tile To Check
                nextTile = nextTile.GetTileByDirection(direction);

                if (character.GetJumpMove() && !hasCharacterJumped)
                { hasCharacterJumped = true; continue; }

                Character isCharacter = nextTile.GetCharacter();

                // If Character Exists
                if (isCharacter != null)
                { break; }

                // The cell is empty and is highlighted with movement color
                if (state)
                {
                    VFXManager.Instance.SetHighlightParticles(TileHighlightState.Movement, nextTile.GetPosition()+new Vector3(0,0.02f,0));
                }
                //nextTile.ChangeMovementIndicatorVisibility(state);
            }
            
            hasCharacterJumped = false;
            nextTile = centralTile;

            Attack:
            // If character cannot attack, return function
            if (!character.GetCanAttack())
            {
                return;
            }

            for (int i = 0; i < character.GetCharacterStats().attackRange; i++)
            {
                if (!nextTile.CheckConnectionNode(direction))
                { continue; }

                // Tile To Check
                nextTile = nextTile.GetTileByDirection(direction);

                if (character.GetJumpMove() && !hasCharacterJumped)
                { hasCharacterJumped = true; continue; }

                Character isCharacter = nextTile.GetCharacter();

                // If Character Exists
                if (isCharacter != null)
                {
                    // And are on different teams, cell is highlighted with attacking color
                    if (isCharacter.GetTeamType() != character.GetTeamType())
                    {
                        VFXManager.Instance.SetHighlightParticles(TileHighlightState.Attack, nextTile.GetPosition() + new Vector3(0, 0.02f, 0));
                        break;
                    }
                    break;
                }
            }
            hasCharacterJumped = false;
        }
    }

    // Function that Highlights all the Available SpawnTiles
    public void ToggleVisibilityAvailableSpawnTiles(TeamType teamType, bool state)
    {
        if (!state)
        {
            VFXManager.Instance.RemoveTeamHighlights();
        }
        // Parameter Determines Which Tiles to Highlight
        switch (teamType)
        {
            case TeamType.RED:
                // Highlight Red Spawn Tiles
                foreach (TileNode spawnableTile in redSpawnTiles)
                {
                    VFXManager.Instance.SetHighlightParticles(TileHighlightState.Movement, spawnableTile.GetPosition() + new Vector3(0, 0.02f, 0));
                }
                break;
            case TeamType.BLUE:
                // Highlight Red Spawn Tiles
                foreach (TileNode spawnableTile in blueSpawnTiles)
                {
                    VFXManager.Instance.SetHighlightParticles(TileHighlightState.Movement, spawnableTile.GetPosition() + new Vector3(0, 0.02f, 0));
                }
                break;
            default:
                break;
        }
    }

    void SetScoreNodes4x2()
    {
        nodeGrid[13].SetScoreNode(TileScoreType.POINTS);
        nodeGrid[14].SetScoreNode(TileScoreType.POINTS);
        nodeGrid[15].SetScoreNode(TileScoreType.POINTS);
        nodeGrid[16].SetScoreNode(TileScoreType.POINTS);
        nodeGrid[19].SetScoreNode(TileScoreType.POINTS);
        nodeGrid[20].SetScoreNode(TileScoreType.POINTS);
        nodeGrid[21].SetScoreNode(TileScoreType.POINTS);
        nodeGrid[22].SetScoreNode(TileScoreType.POINTS);
    }
    void SetEnemyScoreNodes()
    {
        nodeGrid[0].SetScoreNode(TileScoreType.POINTS);
        nodeGrid[1].SetScoreNode(TileScoreType.POINTS);
        nodeGrid[2].SetScoreNode(TileScoreType.POINTS);
        nodeGrid[3].SetScoreNode(TileScoreType.POINTS);
        nodeGrid[4].SetScoreNode(TileScoreType.POINTS);
        nodeGrid[5].SetScoreNode(TileScoreType.POINTS);

        nodeGrid[0].SetScoreAmount(TileScoreAmount.ENEMYROWS);
        nodeGrid[1].SetScoreAmount(TileScoreAmount.ENEMYROWS);
        nodeGrid[2].SetScoreAmount(TileScoreAmount.ENEMYROWS);
        nodeGrid[3].SetScoreAmount(TileScoreAmount.ENEMYROWS);
        nodeGrid[4].SetScoreAmount(TileScoreAmount.ENEMYROWS);
        nodeGrid[5].SetScoreAmount(TileScoreAmount.ENEMYROWS);

        nodeGrid[30].SetScoreNode(TileScoreType.POINTS);
        nodeGrid[31].SetScoreNode(TileScoreType.POINTS);
        nodeGrid[32].SetScoreNode(TileScoreType.POINTS);
        nodeGrid[33].SetScoreNode(TileScoreType.POINTS);
        nodeGrid[34].SetScoreNode(TileScoreType.POINTS);
        nodeGrid[35].SetScoreNode(TileScoreType.POINTS);

        nodeGrid[30].SetScoreAmount(TileScoreAmount.ENEMYROWS);
        nodeGrid[31].SetScoreAmount(TileScoreAmount.ENEMYROWS);
        nodeGrid[32].SetScoreAmount(TileScoreAmount.ENEMYROWS);
        nodeGrid[33].SetScoreAmount(TileScoreAmount.ENEMYROWS);
        nodeGrid[34].SetScoreAmount(TileScoreAmount.ENEMYROWS);
        nodeGrid[35].SetScoreAmount(TileScoreAmount.ENEMYROWS);
    }
}
