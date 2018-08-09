using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinineScreen : MonoBehaviour {

    public Text materialsText;
    public PlayerCrafting playerCrafting;

    public Text craftingLevelText;
    public Slider craftingExperienceSlider;

    public void Start()
    {
        playerCrafting.materialGainedEvent.AddListener(UpdateMaterialsText);
        playerCrafting.craftingLevelUpEvent.AddListener(UpdateCraftingLevel);
    }

    public void UpdateMaterialsText(int materialsGained)
    {
        materialsText.text = playerCrafting.materials.ToString();
        UpdateExperienceSlider();

    }

    public void UpdateCraftingLevel(int newLevel)
    {
        craftingLevelText.text = "Level: " + newLevel;
        UpdateExperienceSlider();
    }

    public void UpdateExperienceSlider()
    {
        craftingExperienceSlider.value = (float)playerCrafting.craftingExperience / playerCrafting.getExperienceTillNextLevel();
    }
}
