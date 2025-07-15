using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameIntroUI : MonoBehaviour
{
    [SerializeField]
    private GameObject shhhhhObj;

    [SerializeField]
    private GameObject crewmateObj;

    [SerializeField]
    private Text playerType;

    [SerializeField]
    private Image gradientImg;

    [SerializeField]
    private IntroCharacter myCharacter;

    [SerializeField]
    private List<IntroCharacter> otherCharacters = new List<IntroCharacter>();

    [SerializeField]
    private Color crewColor;

    [SerializeField]
    private Color imposterColor;

    public IEnumerator ShowIntroSequence()
    {
        shhhhhObj.SetActive(true);

        yield return new WaitForSeconds(3f);

        shhhhhObj.SetActive(false);

        ShowPlayerType();
        crewmateObj.SetActive(true);
    }

    public void ShowPlayerType()
    {
        List<IngameMoverCharacter> players = GameSystem.Instance.GetPlayerList();
        IngameMoverCharacter myPlayer = players.Find(player => player.isOwned);

        if (myPlayer == null)
            return;

        myCharacter.SetIntroCharacter(myPlayer.nickname, myPlayer.playerColor);

        if (myPlayer.playerType == EPlayerType.Imposter)
        {
            SetPlayerTypeUI("임포스터", imposterColor);
            SetOtherCharacters(players, EPlayerType.Imposter);
        }
        else
        {
            SetPlayerTypeUI("크루원", crewColor);
            SetOtherCharacters(players, EPlayerType.Crew);
        }
    }

    private void SetPlayerTypeUI(string typeText, Color color)
    {
        playerType.text = typeText;
        playerType.color = color;
        gradientImg.color = color;
    }

    private void SetOtherCharacters(List<IngameMoverCharacter> players, EPlayerType type)
    {
        int i = 0;

        foreach (var player in players)
        {
            if (player.isOwned == false &&
                player.playerType == type &&
                i < otherCharacters.Count)
            {
                otherCharacters[i].SetIntroCharacter(player.nickname, player.playerColor);
                otherCharacters[i].gameObject.SetActive(true);
                ++i;
            }
        }
    }
}