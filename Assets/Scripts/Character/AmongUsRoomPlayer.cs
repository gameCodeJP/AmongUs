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
            // 만약 null일 경우 인게임내에 AmongUsRoomPlayer중에 나의 캐릭터를 가져와 할당
            if (myRoomPlayer == null)
            {
                var players = FindObjectsOfType<AmongUsRoomPlayer>();
                foreach (var player in players)
                {
                    if(player.isOwned)
                    {
                        myRoomPlayer = player;
                        break;
                    }
                }
            }

            return myRoomPlayer;
        }
    }

    [HideInInspector]
    public CharacterMover lobbyPlayerCharacter;

    [SyncVar(hook = nameof(SetPlayerColor_Hook))]
    public EPlayerColor playerColor;

    public void SetPlayerColor_Hook(EPlayerColor oldColor, EPlayerColor newColor)
    {
        LobbyUIManager.Instance?.CustomizeUI.UpdateSelectColorButton(newColor, false);
        LobbyUIManager.Instance?.CustomizeUI.UpdateSelectColorButton(oldColor, true);
    }

    [SyncVar]
    public string nickname;

    public override void Start()
    {
        base.Start();

        if (isServer)
        {
            SpawnLobbyPlayerCharacter();
            LobbyUIManager.Instance.ActiveStartButton();
        }

        if(isLocalPlayer)
        {
            CmdSetNicname(PlayerSettings.nickname);
        }

        LobbyUIManager.Instance.GameRoomPlayerCounter.PlayerCount += 1;
    }

    private void OnDestroy()
    {
        LobbyUIManager.Instance.CustomizeUI.UpdateSelectColorButton(playerColor, true);
        LobbyUIManager.Instance.GameRoomPlayerCounter.PlayerCount -= 1;
    }

    [Command] // 해당 속성을 사용하는 함수는 앞에 cmd를 붙어야 한다.
    public void CmdSetNicname(string nickname)
    {
        this.nickname = nickname;
        lobbyPlayerCharacter.nickname = nickname;
    }

    [Command] // 해당 속성을 사용하는 함수는 앞에 cmd를 붙어야 한다.
    public void CmdSetPlayerColor(EPlayerColor color)
    {
        playerColor = color;
        lobbyPlayerCharacter.playerColor = color;
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

        // Lobby Player Character 생성
        var playerCharacter = Instantiate(AmongUsRoomManager.singleton.spawnPrefabs[0], spawnPos, Quaternion.identity).GetComponent<LobbyCharacterMover>();
        NetworkServer.Spawn(playerCharacter.gameObject, connectionToClient);
        playerCharacter.OwnerNetId = netId;

        playerCharacter.playerColor = color;
    }
}
