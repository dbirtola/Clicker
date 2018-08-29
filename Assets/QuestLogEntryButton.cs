using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLogEntryButton : MonoBehaviour {

    Quest targetQuest;

    public Button button;

    public Text questNameText;

    public void UpdateWithQuest(Quest q)
    {
        targetQuest = q;
        questNameText.text = q.GetQuestName();
        
    }
}
