using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomUI : MonoBehaviour
{
    [SerializeField]
    private List<Image> crewImages;

    [SerializeField]
    private List<Button> imposterCountButtons;

    [SerializeField]
    private List<Button> maxPlayerCountButtons;

    private CreateGameRoomData roomData;

    // Start is called before the first frame update
    void Start()
    {
        roomData = new CreateGameRoomData() { imposterCount = 1, maxPlayerCount = 10 };

        // Material이 공유 되어 색상을 변경 시, 모든 이미지의 색상이 같이 변경되는 문제점이 있다.
        // 그렇기에 모든 이미지에 Meterial을 새로 만들어 할당
        foreach (Image image in crewImages)
        {
            image.material = Instantiate(image.material);
        }

        UpdateCrewImages();
    }
    
    public void UpdateMaxPlayerCount(int count)
    {
        if (roomData.maxPlayerCount == count)
            return;

        roomData.maxPlayerCount = count;

        HighlightSelectedButton(maxPlayerCountButtons, count - 4);
        UpdateCrewImages();
    }

    public void UpdateImposterCount(int count)
    {
        if (roomData.imposterCount == count)
            return;

        roomData.imposterCount = count;

        HighlightSelectedButton(imposterCountButtons, count - 1);

        // 임포스터수에 따른 최소 Player수를 구함
        int minPlayerCount = 0;
        if (count == 1) minPlayerCount = 4;
        else if (count == 2) minPlayerCount = 7;
        else if (count == 3) minPlayerCount = 9;

        // Player수가 limitPlayerCount보다 적을 경우 현재 최대인원을 limitPlayerCount로 변경
        if (roomData.maxPlayerCount < minPlayerCount)
        {
            UpdateMaxPlayerCount(minPlayerCount);
        }
        else // Player수에 변화가 없으니 크루원의 이미지만 다시 그림
        {
            UpdateCrewImages();
        }

        // limitPlayerCount보다 낮은 숫자버튼은 비활성화
        for (int i = 0; i < maxPlayerCountButtons.Count; ++i)
        {
            bool isInteractable = i >= minPlayerCount - 4;
            Text buttonText = maxPlayerCountButtons[i].GetComponentInChildren<Text>();

            maxPlayerCountButtons[i].interactable = isInteractable;
            buttonText.color = isInteractable ? Color.white : Color.gray;
        }
    }

    private void HighlightSelectedButton(List<Button> buttons, int selectedIndex)
    {
        for (int i = 0; i < buttons.Count; ++i)
        {
            float alpha = i == selectedIndex ? 1f : 0f;
            buttons[i].image.color = new Color(1f, 1f, 1f, alpha);
        }
    }

    private void UpdateCrewImages()
    {
        for (int i = 0; i < crewImages.Count; ++i)
        {
            crewImages[i].material.SetColor("_PlayerColor", Color.white);
            crewImages[i].gameObject.SetActive(i < roomData.maxPlayerCount);
        }

        List<int> idxList = Enumerable.Range(0, roomData.maxPlayerCount)
                     .OrderBy(x => UnityEngine.Random.value)
                     .Take(roomData.imposterCount)
                     .ToList();

        // 임포스터 색상 설정
        foreach (int idx in idxList)
        {
            if (idx >= roomData.maxPlayerCount)
                continue;

            crewImages[idx].material.SetColor("_PlayerColor", Color.red);
        }
    }

    public void CreateRoom()
    {
        NetworkManager manager = AmongUsRoomManager.singleton;

        manager.StartHost();
    }
}

public struct CreateGameRoomData
{
    public int imposterCount;
    public int maxPlayerCount;
}