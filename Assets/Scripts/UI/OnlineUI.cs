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

        // �� �г����� ��� �ִϸ��̼��� ������� �г��� �ִϸ��̼� ����
        animator.SetTrigger("on");
        return true;
    }

    public void OnClickCreateRoomButton()
    {
        if (EmptyCheckName())
            return;

        // �游��� UI�� Ȱ��ȭ
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
