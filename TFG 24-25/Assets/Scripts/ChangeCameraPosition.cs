using Photon.Pun;
using UnityEngine;

public class ChangeCameraPosition : MonoBehaviour
{
    private bool isBlue;

    GameObject playerCamera;

    private bool lookingBoard = false;
    private Vector3 boardPos = new Vector3(0, -0.15f, 0.965f);
    private Vector3 boardRot = new Vector3(90f, 0f, 0f);

    private bool lookingBook = false;
    private Vector3 blueBookPos = new Vector3(0.5f, -0.05f, 0.98f);
    private Vector3 redBookPos = new Vector3(-0.5f, -0.05f, 0.98f);
    private Vector3 blueBookRot = new Vector3(10.0f, 90f, 0f);
    private Vector3 redBookRot = new Vector3(10.0f, -90f, 0f);

    private Vector3 bluePlayerPos = new Vector3(0.0f, 0f, 0.0f);
    private Vector3 bluePlayerRot = new Vector3(33.0f, 0f, 0.0f);
    private Vector3 redPlayerPos = new Vector3(0.0f, 0f, 0.0f);
    private Vector3 redPlayerRot = new Vector3(33.0f, 0f, 0.0f);


    void Start()
    {
        isBlue = PhotonNetwork.IsMasterClient;
        playerCamera = GetComponent<PlayerController>().playerCamera;
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardInputs();
    }

    private void KeyboardInputs()
    {
        //return;
        if (Input.GetKeyDown(KeyCode.W)) // Look Board
        {
            MoveCamera(lookingBoard, boardPos, boardRot);
            lookingBoard = !lookingBoard;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ResetCamera();
        }
        else if (Input.GetKeyDown(KeyCode.A) && !isBlue)
        {
            MoveCamera(lookingBook, redBookPos, redBookRot);
            lookingBook = !lookingBook;
        }
        else if (Input.GetKeyDown(KeyCode.D) && isBlue)
        {
            MoveCamera(lookingBook, blueBookPos, blueBookRot);

            lookingBook = !lookingBook;
        }
    }

    private void MoveCamera(bool isThere, Vector3 toPos, Vector3 toRot)
    {
        if (isThere)
        { MoveCamera(false, isBlue ? bluePlayerPos : redPlayerPos, isBlue ? bluePlayerRot : redPlayerRot); return; }

        playerCamera.transform.SetLocalPositionAndRotation(toPos, Quaternion.Euler(toRot));
    }

    private void ResetCamera()
    {
        if (!lookingBoard && !lookingBook) { return; }

        MoveCamera(false, isBlue ? bluePlayerPos : redPlayerPos, isBlue ? bluePlayerRot : redPlayerRot);

        lookingBook = lookingBook = false;
    }
}
