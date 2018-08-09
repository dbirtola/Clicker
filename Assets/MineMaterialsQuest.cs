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
}
