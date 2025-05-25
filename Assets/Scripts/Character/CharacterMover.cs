using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterMover : NetworkBehaviour
{
    private Animator animator;
    private new Rigidbody2D rigidbody2D;

    protected bool isMovable;
    public bool IsMovable 
    {
        get { return isMovable; }
        set
        {
            if (value == false)
            {
                animator.SetBool("isMove", false);
            }

            isMovable = value;
        }
    }

    [SyncVar]
    public float speed = 2f;

    private SpriteRenderer spriteRenderer;

    [SyncVar(hook = nameof(SetPlayerColor_Hook))]
    public EPlayerColor playerColor;

    public void SetPlayerColor_Hook(EPlayerColor oldColor, EPlayerColor newColor)
    {
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(newColor));
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetColor("_PlayerCOlor", PlayerColor.GetColor(playerColor));

        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        if (isOwned == false)
            return;

        Camera cam = Camera.main;
        cam.transform.SetParent(transform);
        cam.transform.localPosition = new Vector3(0f, 0f, -10f);
        cam.orthographicSize = 2.5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Mode();
    }

    public void Mode()
    {
        // 권한이 있는 캐릭터만 움직일 수 있다.
        if (isOwned == false || isMovable == false)
            return;

        bool isMove = false;
        Vector3 dir = Vector3.zero;

        if (PlayerSettings.controlType == EControlType.KeyboardMouse)
        {
            dir = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f),1f);
        }
        else
        {
            if(Input.GetMouseButton(0))
            {
                dir = (Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f)).normalized;
            }
        }

        isMove = MoveCharacter(dir);
        animator.SetBool("isMove", isMove);
    }

    private bool MoveCharacter(in Vector3 dir)
    {
        // 움직임이 없는 경우
        if (dir.magnitude == 0f)
            return false;

        transform.localScale = new Vector3(dir.x < 0f ? - 0.5f : 0.5f, 0.5f, 1f);
        transform.position += dir * speed * Time.fixedDeltaTime;

        rigidbody2D.MovePosition(dir * speed * Time.fixedDeltaTime);

        return true;
    }
}
