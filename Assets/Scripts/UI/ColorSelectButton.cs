using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelectButton : MonoBehaviour
{
    [SerializeField]
    private GameObject x_Image;

    public bool isInteractable = true;

    public void SetInteractable(bool isInteractable)
    {
        this.isInteractable = isInteractable;
        x_Image.SetActive(!isInteractable);
    }
}
