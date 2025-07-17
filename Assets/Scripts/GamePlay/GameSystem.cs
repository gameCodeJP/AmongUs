using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameSystem : NetworkBehaviour
{
    public static GameSystem Instance { get; private set; }

    private List<IngameMoverCharacter> players = new List<IngameMoverCharacter>();

    [SerializeField]
    private Transform spawnTransform = default;

    [SerializeField]
    private float SpawnDistance = 0f;

    public void AddPlayer(IngameMoverCharacter player)
    {
        if (isServer == false)
            return;

        if (players.Contains(player))
            return;

        players.Add(player);

        var manager = NetworkManager.singleton as AmongUsRoomManager;
        if (manager == null || manager.roomSlots.Count != players.Count)
            return;

        AssignImposters(manager.imposterCount);


        for (int i = 0; i < players.Count; ++i)
        {
            float radian = (2f * Mathf.PI) / players.Count;
            radian *= i;

            players[i].RpcTeleport(spawnTransform.position + (new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f) * SpawnDistance));
        }

        RpcStartGame(players);
    }

    private void AssignImposters(int imposterCount)
    {
        int assigned = 0; 
        var available = new List<IngameMoverCharacter>(players);

        while (assigned < imposterCount && available.Count > 0)
        {
            int idx = Random.Range(0, available.Count);
            var candidate = available[idx];

            candidate.playerType = EPlayerType.Imposter;
            assigned++;

            available.RemoveAt(idx);
        }
    }

    [ClientRpc]
    private void RpcStartGame(List<IngameMoverCharacter> players)
    {
        this.players = players;

        StartCoroutine(GameReady());
    }

    private IEnumerator GameReady()
    {
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(IngameUIManager.Instance.IngameIntroUI.ShowIntroSequence());

        yield return new WaitForSeconds(3f);

        IngameUIManager.Instance.IngameIntroUI.Close();
    }

    public List<IngameMoverCharacter> GetPlayerList() => players;

    private void Awake()
    {
        Instance = this;
    }
}