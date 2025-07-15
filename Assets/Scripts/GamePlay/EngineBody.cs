using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EngineBody : MonoBehaviour
{
    [SerializeField] public List<GameObject> steams = new List<GameObject>();
    [SerializeField] public List<SpriteRenderer> sparks = new List<SpriteRenderer>();
    [SerializeField] public List<Sprite> sparkSprites = new List<Sprite>();

    private int nowIdx = 0;

    private void Start()
    {
        foreach(GameObject gameObject in steams)
        {
            StartCoroutine(RandomSteam(gameObject));
        }

        StartCoroutine(SparkEngine());
    }

    private IEnumerator RandomSteam(GameObject steam)
    {
        while(true)
        {
            float timer = Random.Range(0.5f, 1.5f);
            while(timer >= 0f)
            {
                yield return null;
                timer -= Time.deltaTime;
            }

            steam.SetActive(true);

            timer = 0f;
            while (timer < 0.5f)
            {
                yield return null;
                timer += Time.deltaTime;
            }

            steam.SetActive(false);
        }
    }

    private IEnumerator SparkEngine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.06f);
        while (true)
        {
            float timer = Random.Range(0.2f, 1.5f);
            while (timer >= 0f)
            {
                yield return null;
                timer -= Time.deltaTime;
            }

            int length = Random.Range(4, 7);
            for(int i = 0; i < length; ++i)
            {
                yield return wait;
                sparks[nowIdx].sprite = sparkSprites[Random.Range(0, sparkSprites.Count)];
            }

            yield return wait;

            sparks[nowIdx++].sprite = null;
            if(nowIdx >= sparks.Count)
            {
                nowIdx = 0;
            }
        }
    }
}
