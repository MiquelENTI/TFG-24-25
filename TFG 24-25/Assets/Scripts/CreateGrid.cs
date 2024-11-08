using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    [SerializeField] GameObject board;

    [SerializeField] GameObject cell;

    [SerializeField] GameObject[] playerObjectReferences;


    void Start()
    {
        InitGrid(6,6);
    }

    void Update()
    {
        
    }

    void InitGrid(float columns, float rows)
    {
        Vector2 boardSize = new Vector2(board.transform.localScale.x, board.transform.localScale.z);
        Vector2 cellSize = new(boardSize.x / columns, boardSize.y / rows);
        Vector2 cellOffset = new(cellSize.x / 2.0f, cellSize.y / 2.0f);

        cell.transform.localScale = new Vector3(cellSize.x * 0.9f, cell.transform.localScale.y, cellSize.y * 0.9f);

        Vector2 boardCorner = new Vector2(
            board.transform.position.x + (boardSize.y / 2.0f - cellSize.y/2.0f), 
            board.transform.position.z - (boardSize.x / 2.0f - cellSize.x/2.0f)
            );

        CellNodeManager.Instance.SetGridSize(new Vector2(columns, rows));
        List<CellNode> nodeGrid = new();

        for (int i = 0; i < rows; i++)
        {
            List<CellNode> row = new List<CellNode>();
            for (int j = 0; j < columns; j++)
            {
                GameObject obj = Instantiate(cell, new Vector3(boardCorner.y + j * cellSize.x, 0.5f, boardCorner.x - i * cellSize.y), Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Cell: " + i.ToString() + "-" + j.ToString();
                obj.GetComponent<Tile>().tileId = (int)(i * rows + j);
                CellNode node = new CellNode(obj.transform.position);
            }
        }

        CellNodeManager.Instance.CreateDefaultConnections();
        CellNodeManager.Instance.CreateSpawnableTiles(2);
        CellNodeManager.Instance.PrintNodeGridStatus();
        
        // Map References

        for (int i = 0; i < playerObjectReferences.Length; i++)
        {
            playerObjectReferences[i].transform.localScale = new Vector3(boardSize.x, 1, 1);
        }
        playerObjectReferences[0].transform.position = new Vector3(0, 0, -(boardSize.y / 2.0f + playerObjectReferences[0].transform.localScale.z / 2.0f));
        playerObjectReferences[1].transform.position = new Vector3(0, 0, boardSize.y / 2.0f + playerObjectReferences[1].transform.localScale.z/2.0f);

    }
}
