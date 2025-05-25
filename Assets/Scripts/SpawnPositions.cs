using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositions : MonoBehaviour
{
    [SerializeField]
    private Transform[] positions;

    private int idx;

    public Vector3 GetSpawnPosition()
    {
        Vector3 pos = positions[idx].position;

        ++idx;
        if (idx >= positions.Length)
        {
            idx = 0;
        }

        return pos;
    }
}
