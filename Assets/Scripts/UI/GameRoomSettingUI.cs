using Mirror;
using Mirror.BouncyCastle.Crypto.Generators;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoomSettingUI : SettingUI
{
    public void Open()
    {
        gameObject.SetActive(true);
        SetMyPlayerMovable(false);
    }

    public override void Close()
    {
        base.Close();

        SetMyPlayerMovable(true);
    }

    public void ExitGameRoom()
    {
        NetworkManager manager = AmongUsRoomManager.singleton;
        if(manager.mode == Mirror.NetworkManagerMode.Host)
        {
            manager.StopHost();
        }
        else if(manager.mode == Mirror.NetworkManagerMode.ClientOnly)
        {
            manager.StopClient();
        }
    }

    private void OnEnable()
    {
        SetMyPlayerMovable(false);
    }

    private void SetMyPlayerMovable(bool isMoveable)
    {
        AmongUsRoomPlayer myRoomPlayer = AmongUsRoomPlayer.MyRoomPlayer;
        if (myRoomPlayer == null)
            return;

        if (myRoomPlayer.lobbyPlayerCharacter == null)
            return;

        myRoomPlayer.lobbyPlayerCharacter.IsMovable = isMoveable;
    }
}
