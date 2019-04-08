using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLog : MonoBehaviour {

    public QuestLogBox questLogBox;



	public void Refresh()
    {
        Quest[] quests = FindObjectOfType<PlayerQuests>().quests;

        questLogBox.UpdateWithQuests(quests);
    }
}
