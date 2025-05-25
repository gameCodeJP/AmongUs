using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class OnlineUI : MonoBehaviour
{
    [SerializeField]
    private InputField nicknameInputField;
    [SerializeField]
    private GameObject createRoomUI;

    private Animator animator;

    void Start()
    {
        animator = nicknameInputField.GetComponent<Animator>();
    }

    private bool EmptyCheckName()
    {
        if (nicknameInputField.text != "")
            return false;

        // 빈 닉네임일 경우 애니메이션을 재생시켜 닉네임 애니메이션 연출
        animator.SetTrigger("on");
        return true;
    }

    public void OnClickCreateRoomButton()
    {
        if (EmptyCheckName())
            return;

        // 방만들기 UI로 활성화
        PlayerSettings.nickname = nicknameInputField.text;
        createRoomUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnClickEnterGameRoomButton()
    {
        if (EmptyCheckName())
            return;

        NetworkManager manager = AmongUsRoomManager.singleton;
        manager.StartClient();
    }
}
