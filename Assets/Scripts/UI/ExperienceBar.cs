using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour {

    PlayerStats playerStats;
    public Text levelText;


	// Use this for initialization
	void Start () {
        playerStats = FindObjectOfType<PlayerStats>();

        if(levelText != null)
        {
            playerStats.levelUpEvent.AddListener(updateLevel);
        }
	}

    public void updateLevel(int level)
    {
        levelText.text = "Lv. " + level;
    }
    // Update is called once per frame
    void Update()
    {

        GetComponent<Slider>().value = (float)playerStats.GetExperience() / playerStats.GetExpTillLevel();
    }
    

}
