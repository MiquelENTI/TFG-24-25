using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CreateGrid : MonoBehaviour
{
    [SerializeField] GameObject board;

    [SerializeField] GameObject cell;

    [SerializeField] GameObject redBase;
    [SerializeField] GameObject blueBase;

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
        CellNodeManager.Instance.SetGridSize(new Vector2(columns, rows));
        List<CellNode> nodeGrid = new();

        Vector3 centerGrid = new Vector3((columns-1.0f) / 1.18f /2.0f / transform.localScale.x, 0f, -(rows-1.0f) / 1.18f / 2.0f / transform.localScale.z);
        Vector3 gridSize = new Vector3(columns/ transform.localScale.x, 1f, rows / transform.localScale.z+0.43f);
        transform.GetChild(1).GetComponent<BoxCollider>().center = centerGrid;
        transform.GetChild(1).GetComponent<BoxCollider>().size = gridSize;

        cell.transform.localScale = new Vector3(1.2f, 0.5f, 1.2f);

        for (int i = 0; i < rows; i++)
        {
            List<CellNode> row = new List<CellNode>();
            for (int j = 0; j < columns; j++) 
            {
                Vector3 position = new Vector3(j /1.18f, 0.5f, -i / 1.18f);
                GameObject obj = Instantiate(cell, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Cell: " + j.ToString() + "-" + i.ToString();
                obj.GetComponent<Tile>().tileId = (int)(i * rows + j);
                CellNode node = new CellNode(obj.transform.position, obj);

                CreateModelBoard(i, j, rows,columns, position);
            }
        }


        // TEMP
        cell.transform.localScale = new Vector3(rows*1.5f, 2f, columns/3f);

        GameObject obj2 = Instantiate(cell, redBase.transform.position, Quaternion.identity, board.transform.GetChild(0));
        obj2.GetComponent<Tile>().tileId = (int)(rows*columns);
        GameObject obj3 = Instantiate(cell, blueBase.transform.position, Quaternion.identity, board.transform.GetChild(0));
        obj3.GetComponent<Tile>().tileId = (int)(rows * columns)+1;
        CellNode redBaseNode = new CellNode(obj2.transform.position, obj2);
        CellNode blueBaseNode = new CellNode(obj3.transform.position, obj3);

        Character dummy = new Character(TemporalCardDataBase.Instance.GetTemporalStats(-1), TeamType.RED, null, null);
        Character dummy2 = new Character(TemporalCardDataBase.Instance.GetTemporalStats(-1), TeamType.BLUE, null, null);

        redBaseNode.SetCharacter(dummy);
        blueBaseNode.SetCharacter(dummy2);

        // END TEMP

        CellNodeManager.Instance.CreateSpawnableTiles(2);
        CellNodeManager.Instance.CreateDefaultConnections(redBaseNode, blueBaseNode);
        //CellNodeManager.Instance.PrintNodeGridStatus();
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
