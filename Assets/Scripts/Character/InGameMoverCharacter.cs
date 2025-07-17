using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerType
{
    Crew,
    Imposter
}

public class IngameMoverCharacter : CharacterMover
{
    [SyncVar]
    public EPlayerType playerType;

    [ClientRpc]
    public void RpcTeleport(Vector3 position)
    {
        transform.position = position;
    }

    public override void Start()
    {
        base.Start();

        GameSystem.Instance.AddPlayer(this);

        if (isOwned == false)
            return;

        isMovable = true;

        AmongUsRoomPlayer myRoomPlayer = AmongUsRoomPlayer.MyRoomPlayer;
        CmdSetPlayerChracter(myRoomPlayer.nickname, myRoomPlayer.playerColor);
    }

    [Command]
    private void CmdSetPlayerChracter(string nickname, EPlayerColor color)
    {
        this.nickname = nickname;
        playerColor = color;
    }
}
