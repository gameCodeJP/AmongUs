using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeUI : MonoBehaviour
{
    [SerializeField]
    private Button colorButton;
    [SerializeField]
    private GameObject colorPanel;
    [SerializeField]
    private Button gameRuleButton;
    [SerializeField]
    private GameObject gameRulePanel;

    [SerializeField]
    private Image characterPreview;

    [SerializeField]
    private List<ColorSelectButton> colorSelectButtons;

    private void Start()
    {
        var inst = Instantiate(characterPreview.material);
        characterPreview.material = inst;
    }

    public void ActiveColorPanel()
    {
        colorButton.image.color = new Color(0f, 0f, 0f, 0.75f);
        gameRuleButton.image.color = new Color(0f, 0f, 0f, 0.25f);

        colorPanel.SetActive(true);
        gameRulePanel.SetActive(false);
    }

    public void ActiveGameRulePanel()
    {
        colorButton.image.color = new Color(0f, 0f, 0f, 0.25f);
        gameRuleButton.image.color = new Color(0f, 0f, 0f, 0.75f);

        colorPanel.SetActive(false);
        gameRulePanel.SetActive(true);
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

    public void UpdateSelectColorButton(EPlayerColor color, bool isInteractable)
    {
        colorSelectButtons[(int)color].SetInteractable(isInteractable);
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
        AmongUsRoomPlayer.MyRoomPlayer.lobbyPlayerCharacter.IsMovable = false;
        gameObject.SetActive(true);

        ActiveColorPanel();
    }

    public void Close()
    {
        AmongUsRoomPlayer.MyRoomPlayer.lobbyPlayerCharacter.IsMovable = true;
        gameObject.SetActive(false);
    }
}
