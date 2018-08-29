using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLogBox : MonoBehaviour {


    public QuestLogEntryButton questLogEntryPrefab;

    public void UpdateWithQuests(Quest[] quests)
    {
        foreach (QuestLogEntryButton qle in GetComponentsInChildren<QuestLogEntryButton>())
        {
            Destroy(qle.gameObject);
        }

        foreach(Quest q in quests)
        {
            var entry = Instantiate(questLogEntryPrefab, transform);
            entry.UpdateWithQuest(q);
        }
    }
}
