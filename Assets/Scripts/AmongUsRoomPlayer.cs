using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AmongUsRoomPlayer : NetworkRoomPlayer
{
    private static AmongUsRoomPlayer myRoomPlayer;
    public static AmongUsRoomPlayer MyRoomPlayer
    {
        get
        {
            if(myRoomPlayer == null)
            {
                var players = FindObjectsOfType<AmongUsRoomPlayer>();
                foreach (var player in players)
                {
                    if(player.isOwned)
                    {
                        myRoomPlayer = player;
                    }
                }
            }

            return myRoomPlayer;
        }
    }

    [SyncVar]
    public EPlayerColor playerColor;

    public override void Start()
    {
        base.Start();

        if (isServer)
        {
            SpawnLobbyPlayerCharacter();
        }
    }

    private void SpawnLobbyPlayerCharacter()
    {
        var roomSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;
        EPlayerColor color = EPlayerColor.Red;
        for(int i = 0; i < (int)EPlayerColor.Lime + 1; ++i)
        {
            bool isFindSameColor = false;   
            foreach(var roomPlayer in roomSlots)
            {
                var amongUsRoomPlayer = roomPlayer as AmongUsRoomPlayer;
                if(amongUsRoomPlayer.playerColor == (EPlayerColor)i &&
                    roomPlayer.netId != netId)
                {
                    isFindSameColor = true;
                    break;
                }
            }

            if(isFindSameColor == false)
            {
                color = (EPlayerColor)i;
                break;
            }
        }

        playerColor = color;

        Vector3 spawnPos = FindObjectOfType<SpawnPositions>().GetSpawnPosition();

        // Lobby Player Character »ý¼º
        var playerCharacter = Instantiate(AmongUsRoomManager.singleton.spawnPrefabs[0], spawnPos, Quaternion.identity).GetComponent<LobbyCharacterMover>();
        NetworkServer.Spawn(playerCharacter.gameObject, connectionToClient);

        playerCharacter.playerColor = color;
    }
}
