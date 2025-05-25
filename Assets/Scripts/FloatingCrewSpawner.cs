using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCrewSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private List<Sprite> sprites;

    private List<FloatingCrew> crews = new ();
    private float distance = 11f;

    private void Awake()
    {
        for(int i = 0; i < 12; ++i)
        {
            FloatingCrew crew = Instantiate(prefab, Vector3.zero, Quaternion.identity).GetComponent<FloatingCrew>();
            if (crew == null)
                continue;

            crew.SetColor = (EPlayerColor)i;
            crews.Add(crew);

            // ���� ���۽� ����ڰ� ���� ȭ�鿡 ĳ���͸� ��ġ ��Ű�� ���� 0 ~ �ִ���������� ���� ���� ���ڷ� �ѱ�
            SpawnFloatingCrew((EPlayerColor)i, Random.Range(0f, distance));
        }
    }

    private void SpawnFloatingCrew(EPlayerColor playerColor, float dist)
    {
        float angle = Random.Range(0f, 360f);
        Vector3 spawnPos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f) * dist;
        Vector3 direction = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), 0);
        float floatingSpeed = Random.Range(3f, 4f);
        float rotateSpeed = Random.Range(-2f, 2f);
        float size = Random.Range(0.5f, 1f);

        FloatingCrew crew = crews[(int)playerColor];
        if (crew == null)
            return;
        
        crew.SetFloatingCrew(sprites[Random.Range(0, sprites.Count)], direction, floatingSpeed, rotateSpeed, size);
        crew.transform.transform.position = spawnPos;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        FloatingCrew crew = collision.GetComponent<FloatingCrew>();
        if (crew == null)
            return;

        // ���� ��ġ���� ���� ���� �����Ѵ�.
        SpawnFloatingCrew(crew.GetColor, distance);
    }
}
