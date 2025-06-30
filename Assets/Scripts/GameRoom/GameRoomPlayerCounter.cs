using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class GameRoomPlayerCounter : NetworkBehaviour
{
    [SyncVar]
    private int minPlayer;
    [SyncVar]
    private int maxPlayer;

    [SerializeField]
    private Text playerCountText;


    private int playerCount = 0;
    public int PlayerCount
    {
        get
        {
            return playerCount;
        }
        set
        {
            playerCount = value;
            UpdatePlayerCount();
        }
    }

    public void UpdatePlayerCount()
    {
        bool isStartable = playerCount >= minPlayer;

        playerCountText.color = isStartable ? Color.white : Color.red;
        playerCountText.text = string.Format("{0}/{1}", PlayerCount, maxPlayer);

        LobbyUIManager.Instance.SetInteractableStartButton(isStartable);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isServer == false)
            return;

        var manager = NetworkManager.singleton as AmongUsRoomManager;
        minPlayer = manager.minPlayerCount;
        maxPlayer = manager.maxConnections;
    }
}
