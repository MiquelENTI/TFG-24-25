using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    [SerializeField] private GameObject board;
    [SerializeField] private Vector2 gridSize;

    [SerializeField] private GameObject cell;

    [SerializeField] private GameObject cornerPointPrefab;
    [SerializeField] private GameObject edgeDarkPrefab;
    [SerializeField] private GameObject edgeWhitePrefab;
    [SerializeField] private GameObject edgePointPrefab;
    [SerializeField] private GameObject centralPrefab;
    [SerializeField] private GameObject centralPointPrefab;
    
    private void Start()
    {
        InitGrid(gridSize.x,gridSize.y);
    }

    private void InitGrid(float columns, float rows)
    {
        TileNodeManager.Instance.SetGridSize(new Vector2(columns, rows));

        cell.transform.localScale = new Vector3(1.2f, 0.5f, 1.2f);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++) 
            {
                Vector3 position = new Vector3(j /(1.18f *6f), 0.5f, -i / (1.18f *6f));
                GameObject obj = Instantiate(cell, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Tile: " + j.ToString() + "-" + i.ToString();
                obj.GetComponent<Tile>().SetTileId((int)(i * rows + j));
                TileNode node = new TileNode(position, j,i);

                CreateModelBoard(i, j, position);
            }
        }

        TileNodeManager.Instance.CreateDefaultConnections();
        TileNodeManager.Instance.CreateSpecialTiles(2);
        //TileNodeManager.Instance.PrintNodeGridStatus();
    }

    private void CreateModelBoard(int i, int j, Vector3 position)
    {
        if (i == 0) // Top
        {
            if (j == 0)
            {
                // Instantiate Corner TopLeft
                GameObject obj = Instantiate(cornerPointPrefab, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Corner Top Left";
                obj.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
            else if (j == gridSize.x - 1)
            {
                // Instantiate Corner TopRight
                GameObject obj = Instantiate(cornerPointPrefab, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Corner Top Right";
            }
            else
            {
                // Instantiate SideTop
                GameObject obj = Instantiate(edgePointPrefab, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Edge Top";
                obj.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
        else if(i == gridSize.y - 1)
        {
            if (j == 0)
            {
                // Instantiate Corner BotLeft
                GameObject obj = Instantiate(cornerPointPrefab, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Corner Bot Left";
                obj.transform.rotation = Quaternion.Euler(0, -180, 0);
            }
            else if (j == gridSize.x - 1)
            {
                // Instantiate Corner BotRight
                GameObject obj = Instantiate(cornerPointPrefab, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Corner Bot Right";
                obj.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                // Instantiate SideBot
                GameObject obj = Instantiate(edgePointPrefab, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Edge Bot";
                obj.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
        else if (j == 0)
        {
            // Instantiate SideLeft
            if (i == 1 || i == 4) // Dark
            {
                GameObject obj = Instantiate(edgeDarkPrefab, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Edge Left Dark";
                obj.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            if (i == 2 || i == 3) // White
            {
                GameObject obj = Instantiate(edgeWhitePrefab, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Edge Left White";
                obj.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            
        }
        else if (j == gridSize.x - 1)
        {
            // Instantiate SideRight
            if (i == 1 || i == 4) // Dark
            {
                GameObject obj = Instantiate(edgeDarkPrefab, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Edge Right Dark";
            }
            if (i == 2 || i == 3) // White
            {
                GameObject obj = Instantiate(edgeWhitePrefab, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Edge Right White";
            }

        }
        else
        {
            if (i == 1 || i == 4)
            {
                // Instantiate Inside
                GameObject obj = Instantiate(centralPrefab, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Central";
            }
            else if (i == 2 || i == 3)
            {
                // Instantiate Inside
                GameObject obj = Instantiate(centralPointPrefab, position, Quaternion.identity, board.transform.GetChild(0));
                obj.name = "Central Points";
            }
        }
    }
}
