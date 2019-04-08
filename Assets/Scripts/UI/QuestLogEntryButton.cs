using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLogEntryButton : MonoBehaviour {

    Quest targetQuest;
    

    public Text questNameText;
    public Slider questProgress;
    public Text completedText;

    public void UpdateWithQuest(Quest q)
    {
        targetQuest = q;
        questNameText.text = q.GetQuestName();
        questProgress.value = q.GetProgress();
        if(q.GetProgress() >= 1)
        {
            completedText.gameObject.SetActive(true);
        }else
        {
            completedText.gameObject.SetActive(false);
        }
    }
}
