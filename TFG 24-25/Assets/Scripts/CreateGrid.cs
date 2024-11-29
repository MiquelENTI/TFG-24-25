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
        CellNodeManager.Instance.SetGridSize(new Vector2(columns, rows));
        List<CellNode> nodeGrid = new();

        //Vector3 centerGrid = new Vector3((columns/2.0f-0.5f)* 1 / 1.18f/10.0f, 0.5f ,-(rows / 2.0f - 0.5f) * 1 / 1.18f / 10.0f);


        Vector3 centerGrid = new Vector3((columns-1.0f) / 1.18f /2.0f / transform.localScale.x, 0f, -(rows-1.0f) / 1.18f / 2.0f / transform.localScale.z);
        Vector3 gridSize = new Vector3(columns/ transform.localScale.x, 2f, rows / transform.localScale.z);
        transform.GetChild(1).GetComponent<BoxCollider>().center = centerGrid;
        transform.GetChild(1).GetComponent<BoxCollider>().size = gridSize;



        for (int i = 0; i < rows; i++)
        {
            List<CellNode> row = new List<CellNode>();
            for (int j = 0; j < columns; j++) 
            {
                //Vector3 position = new Vector3(j * 1/1.18f, 0.5f, -i * 1 / 1.18f);
                Vector3 position = new Vector3(j /1.18f, 0.5f, -i / 1.18f);
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
