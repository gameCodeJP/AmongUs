using Mirror;
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
        var roomSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;

        for (int i = 0; i < colorSelectButtons.Count; ++i)
        {
            colorSelectButtons[i].SetInteractable(true);
        }

        foreach(var player in roomSlots)
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
        if (colorSelectButtons[idx].isInteractable == false)
            return;


    }
}
