using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    [SerializeField]
    private Transform Back;

    [SerializeField]
    private Transform Front;

    public int GetSortingOrder(GameObject obj)
    {
        float objDist = Mathf.Abs(Back.position.y - obj.transform.position.y);
        float totalDist = Mathf.Abs(Back.position.y - Front.position.y);

        // 보간 값을 활용하여 Gameobject에 sortingOrder값을 구함
        return (int)(Mathf.Lerp(System.Int16.MinValue, System.Int16.MaxValue, objDist / totalDist));
    }
}
