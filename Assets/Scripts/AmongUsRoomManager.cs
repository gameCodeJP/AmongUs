using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class AmongUsRoomManager : NetworkRoomManager
{
    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
    {
        base.OnRoomServerConnect(conn);
    }
}
