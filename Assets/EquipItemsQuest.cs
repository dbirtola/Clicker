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
                Debug.Log("Not enough");
                return;
            }
        }

        FinishQuest();
        Debug.Log("Enough!");
        playerController.GetComponent<PlayerInventory>().itemEquippedEvent.RemoveListener(CheckItems);

    }

}
