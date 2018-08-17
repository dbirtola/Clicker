using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerHUD : MonoBehaviour {

    public EndlessTower endlessTower;

    public Text timerText;

    public GameObject resultScreen;
    public Text artifactPointText;
    public Text roundsClearedText;


    public void Update()
    {
        float timeRemaining = endlessTower.GetTimeRemaining();
        timerText.text = ((int)timeRemaining).ToString();
    }

    public void Start()
    {
        endlessTower.towerFinishedEvent.AddListener(ShowResultScreen);
    }

    public void ShowResultScreen()
    {
        resultScreen.gameObject.SetActive(true);
        artifactPointText.text = endlessTower.artifactPointsEarned.ToString();
        roundsClearedText.text = (endlessTower.currentLevel - endlessTower.towerStartLevel).ToString();
    }
}
