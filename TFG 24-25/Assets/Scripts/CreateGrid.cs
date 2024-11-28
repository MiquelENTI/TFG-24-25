using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CreateGrid : MonoBehaviour
{
    [SerializeField] GameObject board;

    [SerializeField] GameObject cell;

    [SerializeField] GameObject[] playerObjectReferences;

    [SerializeField] Vector2 GridSize;

    [SerializeField] GameObject cornerPrefab;
    [SerializeField] GameObject edgePrefab;
    [SerializeField] GameObject centralPrefab;

    void Start()
    {
        InitGrid(GridSize.x,GridSize.y);
    }

    void Update()
    {
        
    }

    void InitGrid(float columns, float rows)
    {
        Vector2 boardSize = new Vector2(board.transform.localScale.x, board.transform.localScale.z);
        Vector2 cellSize = new(boardSize.x / columns, boardSize.y / rows);

        //cell.transform.localScale = new Vector3(cellSize.x, cell.transform.localScale.y, cellSize.y);
        //cell.transform.localScale = new Vector3(cellSize.x * 0.18f*columns, cell.transform.localScale.y, cellSize.y * 0.18f * rows);

        Vector2 boardCorner = new Vector2(
            board.transform.position.x, 
            board.transform.position.z 
            );

        /*
        Vector2 boardCorner = new Vector2(
            board.transform.position.x + (boardSize.y / 2.0f - cellSize.y / 2.0f),
            board.transform.position.z - (boardSize.x / 2.0f - cellSize.x / 2.0f)
            );
        */

        CellNodeManager.Instance.SetGridSize(new Vector2(columns, rows));
        List<CellNode> nodeGrid = new();

        for (int i = 0; i < rows; i++)
        {
            List<CellNode> row = new List<CellNode>();
            for (int j = 0; j < columns; j++) 
            {
                Vector3 position = new Vector3(j * 1/1.18f, 0.5f, -i * 1 / 1.18f);
                GameObject obj = Instantiate(cell, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Cell: " + i.ToString() + "-" + j.ToString();
                obj.GetComponent<Tile>().tileId = (int)(i * rows + j);
                CellNode node = new CellNode(obj.transform.position);

                CreateModelBoard(i, j, rows,columns, position);
            }
        }

        CellNodeManager.Instance.CreateDefaultConnections();
        CellNodeManager.Instance.CreateSpawnableTiles(2);
        //CellNodeManager.Instance.PrintNodeGridStatus();
        
        // Map References

        for (int i = 0; i < playerObjectReferences.Length; i++)
        {
            playerObjectReferences[i].transform.localScale = new Vector3(boardSize.x, 1, 1);
        }
        playerObjectReferences[0].transform.position = new Vector3(0, 0, -(boardSize.y / 2.0f + playerObjectReferences[0].transform.localScale.z / 2.0f));
        playerObjectReferences[1].transform.position = new Vector3(0, 0, boardSize.y / 2.0f + playerObjectReferences[1].transform.localScale.z/2.0f);

    }

    void CreateModelBoard(int i, int j, float rows, float columns, Vector3 position)
    {
        if (i == 0) // Top
        {
            if (j == 0)
            {
                // Instantiate Corner TopLeft
                GameObject obj = Instantiate(cornerPrefab, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Corner Top Left";
                obj.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
            else if (j == columns - 1)
            {
                // Instantiate Corner TopRight
                GameObject obj = Instantiate(cornerPrefab, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Corner Top Right";
            }
            else
            {
                // Instantiate SideTop
                GameObject obj = Instantiate(edgePrefab, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Edge Top";
                obj.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
        else if(i == rows - 1)
        {
            if (j == 0)
            {
                // Instantiate Corner BotLeft
                GameObject obj = Instantiate(cornerPrefab, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Corner Bot Left";
                obj.transform.rotation = Quaternion.Euler(0, -180, 0);
            }
            else if (j == columns - 1)
            {
                // Instantiate Corner BotRight
                GameObject obj = Instantiate(cornerPrefab, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Corner Bot Right";
                obj.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                // Instantiate SideBot
                GameObject obj = Instantiate(edgePrefab, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Edge Bot";
                obj.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
        else if (j == 0)
        {
            // Instantiate SideLeft
            GameObject obj = Instantiate(edgePrefab, position, Quaternion.identity, board.transform.GetChild(0));
            obj.name = "Edge Left";
            obj.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (j == columns - 1)
        {
            // Instantiate SideRight
            GameObject obj = Instantiate(edgePrefab, position, Quaternion.identity, board.transform.GetChild(0));
            obj.name = "Edge Right";
        }
        else 
        {
            // Instantiate Inside
            GameObject obj = Instantiate(centralPrefab, position, Quaternion.identity, board.transform.GetChild(0));
            obj.name = "Central";
        }
        
    }
}
