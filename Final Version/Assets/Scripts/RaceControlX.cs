using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RaceControlX : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI lapText;
    public TextMeshProUGUI turboText;
    public PlayerController player;

    public float timer = 0;

    public bool raceActive = true;

    void Start()
    {
        GameObject playerScript = GameObject.Find("Player");
        player = playerScript.GetComponent<PlayerController>();
    }

    void Update()
    {
        if(raceActive)
        {
            timerText.SetText("Time: " + timer.ToString("F3"));
            lapText.SetText("Lap: " + player.playerLap + "/2");
            timer += Time.deltaTime;
        }
        else
        {
            timerText.SetText("Finished! \nFinal time: " 
                            + timer.ToString("F3")
                            + "\n Press R to try again");
        }

        if(player.powerupTimer > 0)
        {
            turboText.SetText("TURBO!!!");
        }
        else
        {
            turboText.SetText("");
        }
    }
}
