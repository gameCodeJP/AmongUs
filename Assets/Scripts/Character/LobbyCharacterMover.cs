using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCharacterMover : CharacterMover
{
    [SyncVar(hook = nameof(SetOwnerNetId_Hook))]
    public uint OwnerNetId;

    public void SetOwnerNetId_Hook(uint _, uint newOwnerId)
    {
        var players = FindObjectsOfType<AmongUsRoomPlayer>();
        foreach(var player in players)
        {
            if(newOwnerId == player.netId)
            {
                player.lobbyPlayerCharacter = this;
                break;
            }
        }
    }

    public void ComplateSpawn()
    {
        if (isOwned == false)
            return;

        isMoveable = true;
    }
}
