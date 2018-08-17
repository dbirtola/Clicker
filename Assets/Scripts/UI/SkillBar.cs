using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBar : MonoBehaviour {

    public PlayerAbilities playerAbilities;
    public CombatAbilityButton[] combatAbilityButtons;


    void Awake()
    {
        combatAbilityButtons = GetComponentsInChildren<CombatAbilityButton>();
    }

	// Use this for initialization
	void Start () {
        playerAbilities = FindObjectOfType<PlayerAbilities>();
        UpdateWithAbilities(playerAbilities);
    
	}
	

    public void UpdateWithAbilities(PlayerAbilities playerAbilities) 
    {
        List<Ability> equippedAbilities = playerAbilities.equippedAbilities;
        for (int i = 0; i < combatAbilityButtons.Length; i++)
        {
            // Debug.Log("Stuck on: " + equippedAbilities.Count);
            if (equippedAbilities.Count > i)
            {
                combatAbilityButtons[i].UpdateWithAbility(equippedAbilities[i]);
            }
            else
            {
                combatAbilityButtons[i].UpdateWithAbility(null);
            }
        }
    }
}
