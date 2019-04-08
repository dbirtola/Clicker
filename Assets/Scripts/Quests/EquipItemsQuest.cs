using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItemsQuest : Quest {
    
    public ItemQuality requiredQuality = ItemQuality.Normal;


    public override void StartQuest()
    {
        base.StartQuest();

        playerController.GetComponent<PlayerInventory>().itemEquippedEvent.AddListener(CheckItems);
            
            
    }

    void CheckItems(Item recentlyEquipped)
    {

        var items = playerController.GetComponent<PlayerInventory>().GetAllEquipped();
        foreach(Item i in items)
        {
            if(i == null || i.GetQuality() < requiredQuality)
            {
                return;
            }
        }

        FinishQuest();
        Debug.Log("Enough!");
        playerController.GetComponent<PlayerInventory>().itemEquippedEvent.RemoveListener(CheckItems);

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
        }else
        {
            StartQuest();
        }
    }

    public override float GetProgress()
    {
        var items = playerController.GetComponent<PlayerInventory>().GetAllEquipped();

        int numPassing = 0;

        foreach (Item i in items)
        {
            if (i == null || i.GetQuality() < requiredQuality)
            {
                continue;
            }
            numPassing++;
        }

        return ((float)numPassing / 6);
    }
}
