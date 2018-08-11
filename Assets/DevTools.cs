using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevTools : MonoBehaviour {


    public InputField levelField;
    public InputField craftingLevelField;
    public InputField materialField;
    public InputField apField;
    public InputField iLvlField;
    public InputField statField;

    public Dropdown itemQualityDropdown;
    public Dropdown itemSlotDropdown;
    public Dropdown statDropdown;

    public Toggle autoSaveToggle;
    public Toggle resetSaveToggle;

    public Player player;

    public CharacterStatsPanel characterStatsPanel;

    public void Start()
    {
        player = FindObjectOfType<Player>();
        characterStatsPanel = FindObjectOfType<CharacterStatsPanel>();

        var pm = player.GetComponent<PersistanceManager>();

        pm.devDataLoadedEvent.AddListener(
            () =>
            {
                autoSaveToggle.isOn = pm.saveOnQuit;
                resetSaveToggle.isOn = pm.resetPersistantData;
            }
        );

        autoSaveToggle.onValueChanged.AddListener(SetAutoSave);
        resetSaveToggle.onValueChanged.AddListener(SetResetSave);

        
    }

    public void SetAutoSave(bool val)
    {
        var p = player.GetComponent<PersistanceManager>();
        p.saveOnQuit = val;
    }

    public void SetResetSave(bool val)
    {
        var p = player.GetComponent<PersistanceManager>();
        p.resetPersistantData = val;
    }

    public void SetLevel()
    {
        if(levelField.text != null)
        {
            player.GetComponent<PlayerStats>().SetLevel(int.Parse(levelField.text));
            characterStatsPanel.Refresh(null);
        }
    }

    public void SetCraftingLevel()
    {
        if(craftingLevelField.text != null)
        {
            player.GetComponent<PlayerCrafting>().craftingLevel = int.Parse(craftingLevelField.text);
            player.GetComponent<PlayerCrafting>().craftingLevelUpEvent.Invoke(player.GetComponent<PlayerCrafting>().craftingLevel);
            
        }
    }

    public void SetMaterials()
    {
        var crafting = player.GetComponent<PlayerCrafting>();
        if(materialField.text != null)
        {
            int amt = int.Parse(materialField.text);
            crafting.materials = amt;
            crafting.materialGainedEvent.Invoke(amt);

        }
    }

    public void SetArtifactPoints()
    {
        var art = player.GetComponent<PlayerArtifacts>();
        if(apField.text != null)
        {
            int amt = int.Parse(apField.text);
            art.artifactPoints = amt;
            art.artifactsChangedEvent.Invoke(null);
        }
    }

    public void SpawnItem()
    {
        ItemFactory iFactory = FindObjectOfType<ItemFactory>();
        var item = iFactory.SpawnItemOfType(itemSlotDropdown.captionText.text);
        Debug.Log("Caption text was: " + itemSlotDropdown.captionText.text);
        item.SetQuality(itemQualityDropdown.value);

        if(iLvlField.text != null)
        {
            item.itemLevel = int.Parse(iLvlField.text);
        }

        item.RollProperties(item.itemLevel);

        player.GetComponent<PlayerInventory>().addItem(item);

    }

    public void SetStat()
    {

        PlayerStats stats = player.GetComponent<PlayerStats>();

        if(statField.text != null)
        {
            int val = int.Parse(statField.text);

            switch (statDropdown.captionText.text)
            {
                case "Damage":
                    stats.baseStats.damage = val;
                    break;
                case "Health":
                    stats.baseStats.maxHealth = val;
                    break;
                case "Armor":
                    stats.baseStats.armor = val;
                    break;
                case "Critical Chance":
                    stats.baseStats.criticalChance = val;
                    break;
                case "Critical Damage Bonus":
                    stats.baseStats.criticalDamageBonus = val;
                    break;
                case "Mana Per Tap":
                    stats.baseStats.manaPerTap = val;
                    break;
                case "Poison Damage":
                    stats.baseStats.poisonDamage = val;
                    break;
            }

            characterStatsPanel.Refresh(null);
        }
    }

}
