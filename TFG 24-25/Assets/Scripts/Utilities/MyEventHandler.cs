using UnityEngine.Events;
using Photon.Pun;

public class MyEventHandler : Singleton<MyEventHandler>
{
    public UnityEvent<int, int> moveToken;
    public UnityEvent<int, int, bool> spawnToken;

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        moveToken = new UnityEvent<int, int>();
        spawnToken = new UnityEvent<int, int, bool>();

        moveToken.AddListener((int cellId, int characterId) =>
        {
            photonView.RPC("RPC_MoveToken", RpcTarget.AllBuffered, cellId, characterId);
            photonView.RPC("RPC_RemoveCharacters", RpcTarget.AllBuffered);
        });

        spawnToken.AddListener((int cellId, int characterId, bool bypassSpawn) =>
        {
            photonView.RPC("RPC_SpawnToken", RpcTarget.AllBuffered, cellId, characterId, bypassSpawn);
            photonView.RPC("RPC_RemoveCharacters", RpcTarget.AllBuffered);
        });
        
    }

    public void InvokeMoveToken(int cellId, int characterId)
    {
        CellNode cellToMove = CellNodeManager.Instance.GetNodeById(cellId);
        Character character = CharactersManager.Instance.GetCharacterInBoardById(characterId);

        character.OnMovement(cellToMove);
    }
    public void InvokeSpawnToken(int cellId, int characterId, bool bypassSpawn)
    {
        CellNode cellToMove = CellNodeManager.Instance.GetNodeById(cellId);
        Character character = CharactersManager.Instance.GetCharacterInBoardById(characterId);

        if (bypassSpawn)
        {
            character.BypassSpawn(cellToMove);
        }
        else
        {
            SaveData.Instance.BufferCheck(character.OnSpawn(cellToMove));
        } 
    }
    public void RemoveCharacters()
    { photonView.RPC("RPC_RemoveCharacters", RpcTarget.AllBuffered); }

    [PunRPC]
    public void RPC_MoveToken(int cellId, int characterId)
    { InvokeMoveToken(cellId, characterId); }

    [PunRPC]
    public void RPC_SpawnToken(int cellId, int characterId, bool bypassSpawn)
    { InvokeSpawnToken(cellId, characterId, bypassSpawn); }

    [PunRPC]
    public void RPC_RemoveCharacters()
    { CharactersManager.Instance.RemoveCharacters(); }
}