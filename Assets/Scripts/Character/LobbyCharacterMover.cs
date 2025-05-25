using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCharacterMover : CharacterMover
{
    public void ComplateSpawn()
    {
        if (isOwned == false)
            return;

        isMoveable = true;
    }
}
