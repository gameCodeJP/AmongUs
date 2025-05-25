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

        // Material�� ���� �Ǿ� ������ ���� ��, ��� �̹����� ������ ���� ����Ǵ� �������� �ִ�.
        // �׷��⿡ ��� �̹����� Meterial�� ���� ����� �Ҵ�
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

        // �������ͼ��� ���� �ּ� Player���� ����
        int minPlayerCount = 0;
        if (count == 1) minPlayerCount = 4;
        else if (count == 2) minPlayerCount = 7;
        else if (count == 3) minPlayerCount = 9;

        // Player���� limitPlayerCount���� ���� ��� ���� �ִ��ο��� limitPlayerCount�� ����
        if (roomData.maxPlayerCount < minPlayerCount)
        {
            UpdateMaxPlayerCount(minPlayerCount);
        }
        else // Player���� ��ȭ�� ������ ũ����� �̹����� �ٽ� �׸�
        {
            UpdateCrewImages();
        }

        // limitPlayerCount���� ���� ���ڹ�ư�� ��Ȱ��ȭ
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

        // �������� ���� ����
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