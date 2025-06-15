using System;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Character
{
    private TileNode tileToMove;
    private int splashDamage;

    private Dictionary<TileConnection, Tuple<TileConnection, TileConnection>> translateDirection = new()
    {
        { TileConnection.UP, Tuple.Create(TileConnection.UPLEFT, TileConnection.UPRIGHT) },
        { TileConnection.LEFT, Tuple.Create(TileConnection.UPLEFT, TileConnection.DOWNLEFT) },
        { TileConnection.RIGHT, Tuple.Create(TileConnection.DOWNRIGHT, TileConnection.UPRIGHT) },
        { TileConnection.DOWN, Tuple.Create(TileConnection.DOWNLEFT, TileConnection.DOWNRIGHT) },
    };

    public Dragon(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        splashDamage = stats.dmg;
    }

    public override bool OnMovement(TileNode tileToMove)
    {
        this.tileToMove = tileToMove;
        Debug.Log("TileToMove: " + tileToMove.GetId().ToString());

        if (!base.OnMovement(tileToMove))
        { return false; }


        return true;
    }

    public override void Attack(Character enemy)
    {
        base.Attack(enemy);

        TileNode onTileNode = TileNodeManager.Instance.GetNodeById(onTile);

        TileConnection attackDirection = onTileNode.GetDirectionToTileByRange(tileToMove.GetId(), stats.attackRange).Item2;

        Debug.Log("AttackDirection: " + attackDirection.ToString());

        Tuple<TileConnection, TileConnection> diagonals = translateDirection[attackDirection];

        if (onTileNode.CheckConnectionNode(diagonals.Item1))
        { 
            if (onTileNode.GetTileByDirection(diagonals.Item1).GetCharacter() != null)
            {
                onTileNode.GetTileByDirection(diagonals.Item1).GetCharacter().ReceiveDamage(this, splashDamage); 
            }
        }

        if (onTileNode.CheckConnectionNode(diagonals.Item2))
        {
            if (onTileNode.GetTileByDirection(diagonals.Item2).GetCharacter() != null)
            {
                onTileNode.GetTileByDirection(diagonals.Item2).GetCharacter().ReceiveDamage(this, splashDamage); 
            }
        }
    }
     
     //Implementació del so 
        
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003034001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003034002, token.transform.position);
    }

}
