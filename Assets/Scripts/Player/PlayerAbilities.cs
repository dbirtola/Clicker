using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class AbilityGainedEvent : UnityEvent<Ability>
{

}

[System.Serializable]
public class AbilityData
{
    public string[] equippedAbilityTypes;
}


public class PlayerAbilities : MonoBehaviour {

    const int MAX_EQUIPPED_ABILITIES = 4;

    public AbilityGainedEvent abilityGainedEvent;

    public List<Ability> equippedAbilities;
    public List<Ability> availableAbilities;

    public GameObject abilityHolder;

	void Awake()
    {
        abilityGainedEvent = new AbilityGainedEvent();
        equippedAbilities = new List<Ability>();
        for(int i = 0; i < MAX_EQUIPPED_ABILITIES; i++)
        {
            equippedAbilities.Add(null);
        }
        availableAbilities = new List<Ability>();


    }

  
    void Start()
    {

        foreach(Ability ab in abilityHolder.GetComponentsInChildren<Ability>(true))
        {
            AddAbility(ab);
        }
        /*
        var ab1 = AddAbility(FindObjectOfType<HeavySlash>());
        ab1 = AddAbility(FindObjectOfType<Berserk>());
        ab1 = AddAbility(FindObjectOfType<Cleave>());
        ab1 = AddAbility(FindObjectOfType<Gouge>());
        ab1 = AddAbility(FindObjectOfType<HealingLight>());
        ab1 = AddAbility(FindObjectOfType<FrostBolt>());
        ab1 = AddAbility(FindObjectOfType<DoubleSwing>());
        ab1 = AddAbility(FindObjectOfType<DivineLight>());
        ab1 = AddAbility(FindObjectOfType<Innervate>());
        */
    }

    //Return type not useful, just here for testing should change tho
    public Ability AddAbility(Ability ability)
    {
        if (availableAbilities.Contains(ability) || ability == null)
        {
            Debug.Log("Not adding");
            return null;
        }

        availableAbilities.Add(ability);

        abilityGainedEvent.Invoke(ability);

        return ability;
    }

    public bool EquipAbility(Ability ability, int slot)
    {
        //if(equippedAbilities.Count >= MAX_EQUIPPED_ABILITIES)
        //{
        //    return false;
       // }

        if (!availableAbilities.Contains(ability))
        {
            return false;
        }

        if(slot >= equippedAbilities.Count || slot < 0)
        {
            Debug.Log("Slot: " + slot + " exceeding equipped abilities size");
            return false;
        }

        UnequipAbility(ability);
        UnequipAbility(equippedAbilities[slot]);

        equippedAbilities[slot] = ability;
        ability.owningUnit = GetComponent<Player>().GetPlayerPawn();
        ability.owningPlayer = GetComponent<Player>();
        //ability.owningPawn.gameObject.AddComponent(ability.GetComponent<Ability>());



        return true;

    }

    public bool UnequipAbility(Ability ability)
    {
        if(ability == null)
        {
            return false;
        }

        for(int i = 0; i < equippedAbilities.Count; i++)
        {
            if(equippedAbilities[i] == ability)
            {
                equippedAbilities[i].owningUnit = null;
                equippedAbilities[i] = null;
                return true;
            }
        }

        return false;
 
    }

    void Update()
    {

    }


    public AbilityData SaveAbilityData()
    {
        AbilityData AbilityData = new AbilityData();

        string[] equippedAbilityTypes = new string[MAX_EQUIPPED_ABILITIES];
        for(int i = 0; i < equippedAbilities.Count; i++)
        {
            if(equippedAbilities[i] != null)
            {
                equippedAbilityTypes[i] = equippedAbilities[i].GetType().ToString();
            }else
            {
                equippedAbilityTypes[i] = "null";
            }
        }

        AbilityData.equippedAbilityTypes = equippedAbilityTypes;

        return AbilityData;
    }

    public void LoadAbilityData(AbilityData abilityData)
    {
        for (int i = 0; i < equippedAbilities.Count; i++)
        {
            if (abilityData.equippedAbilityTypes[i] != "null")
            {
                //Find the matching ability to equip by comparing types

                foreach(Ability ab in availableAbilities)
                {
                    if(ab.GetType().ToString() == abilityData.equippedAbilityTypes[i])
                    {
                        EquipAbility(ab, i);
                    }
                }
            }
            else
            {
                equippedAbilities[i] = null;
            }
        }
    }
}
