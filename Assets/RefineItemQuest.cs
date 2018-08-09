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
}
