using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AbilityButtonSelected : UnityEvent<EquippedAbilityButton>
{

}

public class EquippedAbilitiesPanel : MonoBehaviour {

    public AbilityButtonSelected abilityButtonSelected;

    public PlayerAbilities playerAbilities;
    ////public Button abilityButton1;
    //public Button abilityButton2;
    //public Button abilityButton3;
    //public Button abilityButton4;

    public EquippedAbilityButton[] abilityButtons;

    public Sprite noAbilitySprite;

    public GameObject fadeBox;

    public Ability abilityPlayerIsEquipping = null;

    void Awake()
    {
        abilityButtonSelected = new AbilityButtonSelected();
        abilityButtons = GetComponentsInChildren<EquippedAbilityButton>();
        foreach (EquippedAbilityButton ab in abilityButtons)
        {
            ab.GetComponent<Button>().onClick.AddListener(() => { abilityButtonSelected.Invoke(ab); });
        }
        
        
    }

    void Start()
    {
        UpdateEquippedAbilities();
    }

    public void UpdateEquippedAbilities()
    {
        playerAbilities = FindObjectOfType<PlayerAbilities>();
        List<Ability> equippedAbilities = playerAbilities.equippedAbilities;
        for (int i = 0; i < abilityButtons.Length; i++)
        {
           // Debug.Log("Stuck on: " + equippedAbilities.Count);
            if (equippedAbilities.Count > i)
            {
                abilityButtons[i].UpdateWithAbility(equippedAbilities[i]);
            }else
            {
                abilityButtons[i].UpdateWithAbility(null);
            }
        }
    }

 

    public void GetPlayerSlotDecision()
    {
        fadeBox.SetActive(true);
        abilityButtonSelected.AddListener(EquipAbility);
        return;

    }

    void EquipAbility(EquippedAbilityButton eaButton)
    {
        abilityPlayerIsEquipping = FindObjectOfType<AbilityInfoBox>().ability;

       // playerAbilities.UnequipAbility(eaButton.ability);
       // playerAbilities.UnequipAbility(abilityPlayerIsEquipping);
        playerAbilities.EquipAbility(abilityPlayerIsEquipping, eaButton.slot);
        abilityButtonSelected.RemoveListener(EquipAbility);
        UpdateEquippedAbilities();
        fadeBox.SetActive(false);
        FindObjectOfType<AbilityInfoBox>().gameObject.SetActive(false);
    }
}
