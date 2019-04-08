using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineMaterialsQuest : Quest {

    int progress = 0;
    public int materialsRequired = 200;

    public override void StartQuest()
    {
        base.StartQuest();
        playerController.GetComponent<PlayerCrafting>().materialMinedEvent.AddListener(CheckProgress);
    }

    public void CheckProgress(int matsGained)
    {
        progress += matsGained;
        if(progress >= materialsRequired)
        {
            playerController.GetComponent<PlayerCrafting>().materialMinedEvent.RemoveListener(CheckProgress);

            FinishQuest();

        }

    }

    public override QuestState SaveQuestState()
    {
        QuestState qs = base.SaveQuestState();
        qs.questState = progress.ToString();
        return qs;
    }

    public override void LoadQuestState(QuestState questState)
    {
        base.LoadQuestState(questState);
        progress = int.Parse(questState.questState);


        if (completed)
        {
            //completed
        }else
        {
            StartQuest();
        }
    }

    public override float GetProgress()
    {
        return (float)progress / materialsRequired;
    }
}
