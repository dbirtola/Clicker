using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefineItemQuest : Quest {



    public override void StartQuest()
    {
        base.StartQuest();
        playerController.GetComponent<PlayerCrafting>().itemRefinedEvent.AddListener(CheckProgress);
    }

    void CheckProgress(Item item)
    {
        playerController.GetComponent<PlayerCrafting>().itemRefinedEvent.RemoveListener(CheckProgress);
        FinishQuest();
    }

    public override QuestState SaveQuestState()
    {
        return base.SaveQuestState();
        //Dont care about anything other than completed or not
    }

    public override void LoadQuestState(QuestState questState)
    {
        base.LoadQuestState(questState);

        if (completed)
        {
            Debug.Log(questName + " loaded as completed");
        }
        else
        {
            StartQuest();
        }
    }

    public override float GetProgress()
    {
        return this.enabled ? 0 : 1;
    }
}
