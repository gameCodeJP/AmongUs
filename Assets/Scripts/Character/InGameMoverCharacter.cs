using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerType
{
    Crew,
    Imposter
}

public class InGameMoverCharacter : CharacterMover
{
    [SyncVar]
    public EPlayerType playerType;

    public override void Start()
    {
        base.Start();

        if (isClient == false)
            return;

        isMovable = true;

        AmongUsRoomPlayer myRoomPlayer = AmongUsRoomPlayer.MyRoomPlayer;
        CmdSetPlayerChracter(myRoomPlayer.nickname, myRoomPlayer.playerColor);

        GameSystem.Instance.AddPlayer(this);
    }

    [Command]
    private void CmdSetPlayerChracter(string nickname, EPlayerColor color)
    {
        this.nickname = nickname;
        playerColor = color;
    }
}
