using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentHud : MonoBehaviour {

    public QuestFinishedPopup questBanner;
	
    public void Start()
    {
        FindObjectOfType<PlayerQuests>().questFinishedEvent.AddListener(ShowQuestBanner);
    }

    public void ShowQuestBanner(Quest quest)
    {
        questBanner.Show(quest.questName);
    }
}
