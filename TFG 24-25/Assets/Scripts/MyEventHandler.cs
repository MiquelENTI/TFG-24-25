using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun; // Asegúrate de incluir Photon.Pun

public class MyEventHandler : Singleton<MyEventHandler>
{
    public UnityEvent<int, int> moveToken;
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        moveToken = new UnityEvent<int, int>();

        moveToken.AddListener((int cellId, int characterId) =>
        {
            // Invocar el movimiento localmente (esto es para el cliente que ejecuta el evento)
            InvokeMoveToken(cellId, characterId);

            // Enviar el movimiento al otro jugador a través de un RPC
            photonView.RPC("RPC_MoveToken", RpcTarget.Others, cellId, characterId);
        });
    }

    // Este método se llamará tanto localmente como remotamente
    private void InvokeMoveToken(int cellId, int characterId)
    {
        Debug.Log("INVOKE");

        // Implementación de movimiento de personajes
        CellNode cellToMove = CellNodeManager.Instance.GetNodeById(cellId);
        Character character = CharactersManager.Instance.GetCharacterInBoardById(characterId);

        if (character.IsToSpawn())
        {
            character.OnSpawn(cellToMove);
            return;
        }

        character.OnMovement(cellToMove);
    }

    // Este es el método RPC que será llamado remotamente
    [PunRPC]
    public void RPC_MoveToken(int cellId, int characterId)
    {
        // Ejecutar el movimiento en el cliente remoto
        InvokeMoveToken(cellId, characterId);
    }
}

