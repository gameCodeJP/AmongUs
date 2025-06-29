using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoomItem : MonoBehaviour
{
    [SerializeField] private GameObject inactiveObject;

    // Start is called before the first frame update
    void Start()
    {
        if (AmongUsRoomPlayer.MyRoomPlayer.isServer == false)
        {
            inactiveObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
