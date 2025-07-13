using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPointLight : MonoBehaviour
{
    private Animator animator;
    private WaitForSeconds wait = new WaitForSeconds(0.15f);
    private List<WeaponPointLight> lights = new List<WeaponPointLight>();

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        for(int i = 0; i < transform.childCount; ++i)
        {
            var child = transform.GetChild(i).GetComponent<WeaponPointLight>();
            if(child)
            {
                lights.Add(child);
            }
        }
    }
        
    public void TurnOnLight()
    {
        animator.SetTrigger("on");
        StartCoroutine(TurnOnLightAtChild()); 
    }

    private IEnumerator TurnOnLightAtChild()
    {
        yield return wait;

        foreach (var child in lights)
        {   
            child.TurnOnLight();
        }
    }
}
