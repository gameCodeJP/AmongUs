using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeUI : MonoBehaviour
{
    [SerializeField]
    private Image characterPreview;

    [SerializeField]
    private List<ColorSelectButton> colorSelectButtons;

    private void Start()
    {
        var inst = Instantiate(characterPreview.material);
        characterPreview.material = inst;
    }

    private void OnEnable()
    {
        UpdateColorButton();

        var roomSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;
        foreach(var player in roomSlots)
        {
            var roomPlayer = player as AmongUsRoomPlayer;
            if(roomPlayer.isLocalPlayer)
            {
                UpdatePreviewColor(roomPlayer.playerColor);
                break;
            }
        }
    }

    public void UpdateColorButton()
    {
        // 일단 모든 컬러들을 활성화
        for (int i = 0; i < colorSelectButtons.Count; ++i)
        {
            colorSelectButtons[i].SetInteractable(true);
        }

        // 대기방에 있는 모든 플레이어들을 조회하여 사용하고 있는 컬러버튼은 비활성화 한다.
        var roomSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;
        foreach (var player in roomSlots)
        {
            var roomPlayer = player as AmongUsRoomPlayer;
            colorSelectButtons[(int)roomPlayer.playerColor].SetInteractable(false);
        }
    }

    public void UpdatePreviewColor(EPlayerColor color)
    {
        characterPreview.material.SetColor("_PlayerColor", PlayerColor.GetColor(color));
    }

    public void OnClickButton(int idx)
    {
        // 선택 가능한 컬러가 아니라면 return
        if (colorSelectButtons[idx].isInteractable == false)
            return;

        AmongUsRoomPlayer.MyRoomPlayer.CmdSetPlayerColor((EPlayerColor)idx);
        UpdatePreviewColor((EPlayerColor)idx);
    }

    public void Open()
    {
        AmongUsRoomPlayer.MyRoomPlayer.lobbyPlayerCharacter.isMoveable = false;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        AmongUsRoomPlayer.MyRoomPlayer.lobbyPlayerCharacter.isMoveable = true;
        gameObject.SetActive(false);
    }
}
