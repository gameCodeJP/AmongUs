using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCrew : MonoBehaviour
{
    private EPlayerColor playerColor;
    public EPlayerColor SetColor
    {
        set
        {
            this.playerColor = value;
            spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(playerColor));
        }
    }
    public EPlayerColor GetColor { get { return playerColor; } }

    private SpriteRenderer spriteRenderer;
    private Vector3 direction;
    private float floatingSpeed;
    private float rotateSpeed;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetFloatingCrew(Sprite sprite, Vector3 direction, 
        float floatingSpeed, float rotateSpeed, float size)
    {
        this.direction = direction;
        this.floatingSpeed = floatingSpeed;
        this.rotateSpeed = rotateSpeed;

        spriteRenderer.sprite = sprite;

        transform.localScale = new Vector3(size, size, size);
        spriteRenderer.sortingOrder = (int)Mathf.Lerp(1, 32767, size);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * floatingSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, 0f, rotateSpeed));
    }
}
