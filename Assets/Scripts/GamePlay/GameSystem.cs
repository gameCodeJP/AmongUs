using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameSystem : NetworkBehaviour
{
    public static GameSystem Instance;

    public List<InGameMoverCharacter> players = new List<InGameMoverCharacter>();

    public void AddPlayer(InGameMoverCharacter player)
    {
        if (players.Contains(player))
            return;

        players.Add(player);

        if (isServer == false)
            return;

        AmongUsRoomManager manager = NetworkManager.singleton as AmongUsRoomManager;
        if (manager.roomSlots.Count != players.Count)
            return;

        for(int i = 0; i < manager.imposterCount; ++i)
        {
            InGameMoverCharacter curPlayer = players[Random.Range(0, players.Count)];
            if(curPlayer.playerType != EPlayerType.Imposter)
            {
                player.playerType = EPlayerType.Imposter;
            }
            else
            {
                --i;
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
