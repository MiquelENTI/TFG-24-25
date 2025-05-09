using UnityEngine.Events;
using Photon.Pun;

public class MyEventHandler : Singleton<MyEventHandler>
{
    public UnityEvent<int, int> moveToken;
    public UnityEvent<int, int, bool> spawnToken;

    public UnityEvent<int> hidePossibleMovements;
    public UnityEvent<int> showPossibleMovements;
    public UnityEvent<TeamType> hidePossibleSpawnTiles;
    public UnityEvent<TeamType> showPossibleSpawnTiles;

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

        // Event that Triggers ToggleVisibilityPossibleMovements Function

        showPossibleMovements = new UnityEvent<int>();
        showPossibleMovements.AddListener((int tileId) =>
        {
            CellNodeManager.Instance.ToggleVisibilityPossibleMovements(tileId, true);
        });
        hidePossibleMovements = new UnityEvent<int>();
        hidePossibleMovements.AddListener((int tileId) =>
        {
            CellNodeManager.Instance.ToggleVisibilityPossibleMovements(tileId, false);
        });

        // Event that Triggers ToggleVisibilityAvailableSpawnCells
        showPossibleSpawnTiles = new UnityEvent<TeamType>();
        showPossibleSpawnTiles.AddListener((TeamType teamType) =>
        {
            CellNodeManager.Instance.ToggleVisibilityAvailableSpawnCells(teamType, true);
        });

        // Event that Triggers ToggleVisibilityAvailableSpawnCells
        hidePossibleSpawnTiles = new UnityEvent<TeamType>();
        hidePossibleSpawnTiles.AddListener((TeamType teamType) =>
        {
            CellNodeManager.Instance.ToggleVisibilityAvailableSpawnCells(teamType, false);
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

    public void InvokeShowPossibleSpawnTiles(bool isBlue)
    {
        TeamType type = isBlue ? TeamType.BLUE : TeamType.RED;
        showPossibleSpawnTiles.Invoke(type);
    }
    public void InvokeHidePossibleSpawnTiles(bool isBlue)
    {
        TeamType type = isBlue ? TeamType.BLUE : TeamType.RED;
        hidePossibleSpawnTiles.Invoke(type);
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