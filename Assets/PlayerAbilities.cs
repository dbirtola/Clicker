using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class AbilityGainedEvent : UnityEvent<Ability>
{

}



public class PlayerAbilities : MonoBehaviour {

    const int MAX_EQUIPPED_ABILITIES = 4;

    public AbilityGainedEvent abilityGainedEvent;

    public List<Ability> equippedAbilities;
    public List<Ability> availableAbilities;

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


    //Return type not useful, just here for testing should change tho
    public Ability AddAbility(Ability ability)
    {
        if (availableAbilities.Contains(ability))
        {
            return null;
        }

        availableAbilities.Add(ability);

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
        ability.owningPawn = GetComponent<Player>().GetPlayerPawn();
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
                equippedAbilities[i].owningPawn = null;
                equippedAbilities[i] = null;
                return true;
            }
        }

        return false;
 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            var ab1 = AddAbility(FindObjectOfType<HeavySlash>());
            abilityGainedEvent.Invoke(ab1);

            ab1 = AddAbility(FindObjectOfType<Berserk>());
            abilityGainedEvent.Invoke(ab1);

            ab1 = AddAbility(FindObjectOfType<Cleave>());
            abilityGainedEvent.Invoke(ab1);

            ab1 = AddAbility(FindObjectOfType<Gouge>());
            abilityGainedEvent.Invoke(ab1);

            ab1 = AddAbility(FindObjectOfType<HealingLight>());
            abilityGainedEvent.Invoke(ab1);

            ab1 = AddAbility(FindObjectOfType<FrostBolt>());
            abilityGainedEvent.Invoke(ab1);

            ab1 = AddAbility(FindObjectOfType<DoubleSwing>());
            abilityGainedEvent.Invoke(ab1);
        }
    }
}
