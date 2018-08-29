using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineOreQuest : Quest {

    public OreTypes targetOre;
    int progress = 0;
    public int required = 5;

    public override void StartQuest()
    {
        base.StartQuest();
        playerController.GetComponent<PlayerCrafting>().oreMinedEvent.AddListener(CheckProgress);
    }

    void CheckProgress(int oreMined)
    {
        if(oreMined == (int)targetOre)
        {
            progress++;
            if(progress >= required)
            {
                playerController.GetComponent<PlayerCrafting>().oreMinedEvent.RemoveListener(CheckProgress);
                FinishQuest();
            }
        }
    }

    public override string GetQuestName()
    {
        string val = "Mine " + required + " ";

        switch (targetOre)
        {
            case 0:
                val += "Iron Ore";
                break;
            case (OreTypes)1:
                val += "Silver Ore";
                break;
            case (OreTypes)2:
                val += "Gold Ore";
                break;
            case (OreTypes)3:
                val += "Mithril Ore";
                break;
            case (OreTypes)4:
                val += "Diamonds";
                break;
        }

        return val;
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
}
