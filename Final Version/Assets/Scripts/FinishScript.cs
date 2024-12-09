using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class FinishScript : MonoBehaviour
{
    public AudioSource finishAudio;

    public Button retryButton;
    public Button quitButton;

    public TextMeshProUGUI bestTimeText;
    public TextMeshProUGUI rankText;

    void Start()
    {
        retryButton.onClick.AddListener(retryClicked);
        quitButton.onClick.AddListener(quitClicked);

        bestTimeText.SetText("Congrats! Your best time is " + Global.bestTime.ToString("F3"));

        if(Global.bestTime <= 75)
        {
            rankText.SetText("GOLD");
        }
        else if(Global.bestTime <= 86)
        {
            rankText.SetText("SILVER");
        }
        else if(Global.bestTime <= 100)
        {
            rankText.SetText("BRONZE");
        }
        else
        {
            rankText.SetText("NO RANK");
        }
    }

    void retryClicked()
    {
        SceneManager.LoadScene("Level 1");
    }

    void quitClicked()
    {
        Application.Quit();
    }
}
