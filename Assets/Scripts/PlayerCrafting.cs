﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class MaterialGainedEvent : UnityEvent<int>{

}

public class CraftingLevelUpEvent : UnityEvent<int>
{

}

public class ItemSelectedForCraftingEvent : UnityEvent<Item>
{

}

public class ItemRefinedEvent : UnityEvent<Item>
{

}


public class PlayerCrafting : MonoBehaviour {

    const int MAX_CRAFTING_LEVEL = 100;
    const int MAX_REFINE_LEVEL = 20;

    public PlayerStats playerStats;

    public MaterialGainedEvent materialGainedEvent;
    public MaterialGainedEvent materialMinedEvent;
    //Could create new event type for this, dont think its necessary
    public MaterialGainedEvent oreMinedEvent;
    public CraftingLevelUpEvent craftingLevelUpEvent;
    public ItemSelectedForCraftingEvent itemSelectedForCraftingEvent;
    public ItemRefinedEvent itemRefinedEvent;


    public int craftingLevel = 1;
    public int craftingExperience = 0;

    public int materials = 0;

    

    public void Awake()
    {
        materialGainedEvent = new MaterialGainedEvent();
        materialMinedEvent = new MaterialGainedEvent();
        oreMinedEvent = new MaterialGainedEvent();
        craftingLevelUpEvent = new CraftingLevelUpEvent();
        itemSelectedForCraftingEvent = new ItemSelectedForCraftingEvent();
        itemRefinedEvent = new ItemRefinedEvent();

        playerStats = FindObjectOfType<PlayerStats>();
    }

    int GetMiningMaterialAmount()
    {
        //Do not add the players material bonus here, or at low amounts the rounding error will be noticeable
        return (int)Mathf.Ceil(2 * craftingLevel + (1 / 2 * Mathf.Pow(1.09f, craftingLevel)));
    }
    //Materials gained = 2 * CraftingLevel + (1/2 * 1.09^CraftingLevel)
    public void MineRock()
    {
        int materialsMined = (int)(GetMiningMaterialAmount() * playerStats.GetMaterialMultiplier());
        AddMaterials(materialsMined);
        AddCraftingExperience(materialsMined);
        materialMinedEvent.Invoke(materialsMined);
    }

    public void AddMaterials(int amount)
    {
        materials += amount;
        materialGainedEvent.Invoke(amount);
    }

    //Materials gained = 2 * CraftingLevel + (1/2 * 1.09^CraftingLevel)
    public void MineOre(int oreID)
    {
        int materialsMined = GetMiningMaterialAmount();
        Debug.Log("Mining materials: " + materialsMined);
        switch (oreID)
        {
            case 0: //Iron
                materialsMined *= 3;
                break;
            case 1: //Silver
                materialsMined *= 5;
                break;
            case 2: //Gold
                materialsMined *= 10;
                break;
            case 3: //Mithril
                materialsMined *= 20;
                break;
            case 4: //Diamond
                materialsMined *= 50;
                break;
        }

        //Include players crafting bonus. Done late in calculation to avoid smaller rounding errors
        materialsMined = (int)(materialsMined * playerStats.GetMaterialMultiplier());
        AddMaterials(materialsMined);
        AddCraftingExperience(materialsMined);
        materialMinedEvent.Invoke(materialsMined);
        oreMinedEvent.Invoke(oreID);
    }


    void AddCraftingExperience(int exp)
    {
        craftingExperience += exp;

        while(craftingExperience >= getExperienceTillNextLevel())
        {
            LevelUp();
        }
       
    }

    void LevelUp()
    {
        if(craftingLevel >= MAX_CRAFTING_LEVEL)
        {
            return;
        }

        craftingExperience -= getExperienceTillNextLevel();
        craftingLevel += 1;
        craftingLevelUpEvent.Invoke(craftingLevel);

    }

    //=250 * CraftingLevel * 1.12^CraftingLevel
    public int getExperienceTillNextLevel()
    {
        return (int)Mathf.Ceil(250 * craftingLevel * Mathf.Pow(1.12f, craftingLevel));
    }


    public int GetRefineCost(Item item)
    {
        return (int)Mathf.Ceil(100 * (item.refineLevel + 1) + Mathf.Pow(4, (item.refineLevel + 1)));
    }

    public int GetPropertyUpgradeCost(Item item)
    {

        return 100 * item.itemLevel;
    }

    public int GetAddRandomPropertyCost(Item item)
    {
        return 100 * item.itemLevel;
    }

    public int GetRemovePropertyCost(Item item)
    {
        return 100 * item.itemLevel;
    }

    public bool RefineItem(Item item)
    {
        if(materials < GetRefineCost(item) || item.refineLevel >= MAX_REFINE_LEVEL)
        {
            return false;
        }else
        {
            materials -= GetRefineCost(item);

            if (item.GetEquipped())
            {
                item.Unequip();
                item.refineLevel++;
                item.Equip();
            }else
            {
                item.refineLevel++;
            }

            itemRefinedEvent.Invoke(item);

            return true;
        }
    }

    public bool UpgradeProperties(Item item)
    {
        if(materials < GetPropertyUpgradeCost(item))
        {
            return false;
        }

        materials -= GetPropertyUpgradeCost(item);

        bool wasEquipped = item.GetEquipped();

        if (wasEquipped)
        {
            item.Unequip();
        }

        foreach(ItemProperty ip in item.GetItemProperties())
        {
            if (!ip.IsMax()){
                ip.UpgradeRoll();
            }
 
        }

        if (wasEquipped)
        {
            item.Equip();
        }


        return true;
    }

    public bool AddRandomProperty(Item item)
    {
        if(materials < GetAddRandomPropertyCost(item))
        {
            return false;
        }

        materials -= GetAddRandomPropertyCost(item);

        item.AddRandomProperty();

        return true;
    }

    public bool RemoveProperty(Item item, ItemProperty property)
    {
        if(materials < GetRemovePropertyCost(item))
        {
            return false;
        }

        materials -= GetRemovePropertyCost(item);

        item.RemoveProperty(property);

        return true;
    }

}