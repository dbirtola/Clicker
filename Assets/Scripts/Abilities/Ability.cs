using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Ability : MonoBehaviour {



    public Player owningPlayer;
    public PlayerPawn owningPawn;
    protected PlayerStats playerStats;
    protected Mana pawnMana;

    public string abilityName;
    public string description;
   

    public float cooldown;
    public int manaCost;

    //Whether the ability should be queud to fire on next click, or used immediately.
    public bool requiresQueue = true;

    public float timeOffCooldown;

    public Sprite icon;


    //Keep ahold of the multiplier struct in case the player cancels an ability
    protected DamageMultiplierStruct damageMultiplierStruct;


    public virtual void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
      
    }

    public virtual void Start()
    {
       // owningPawn = playerStats.GetComponent<Player>().GetPlayerPawn();
   
    }

    public bool isOnCooldown()
    {
        if(timeOffCooldown < Time.time)
        {
            return false;
        }

        return true;
    }

    public void TriggerCooldown()
    {
        timeOffCooldown = Time.time + cooldown;
    }

    public void DeductMana(int cost)
    {
        //deduct mana
        
        owningPawn.GetComponent<Mana>().UseMana(cost);
    }
    
    //Checks cooldowns and mana before using the ability
    public bool AttemptUse()
    {
        if (isOnCooldown())
        {
            return false;
        }

        //Do mana check
        if(owningPawn.GetComponent<Mana>().currentMana < manaCost)
        {
            return false;
        }

        if (requiresQueue)
        {
            QueueAbility();
        }else
        {
            Use(owningPawn.gameObject);
        }
        return true;
    }

    //Prepares the ability to fire on next click
    public virtual void QueueAbility()
    {
        owningPlayer.tappedEnemyEvent.AddListener(Use);
    }


    public virtual void CancelQueue()
    {
        owningPlayer.tappedEnemyEvent.RemoveListener(Use);

        if(damageMultiplierStruct != null)
        {
            playerStats.RemoveDamageMultiplier(damageMultiplierStruct);
            damageMultiplierStruct = null;
        }
    }


    //Bypasses mana and cooldown checks and just uses the ability
    public virtual void Use(GameObject target)
    {
        Debug.Log("Used ability: " + abilityName);
        TriggerCooldown();
        DeductMana(manaCost);
        

        if (requiresQueue)
        {
            owningPlayer.tappedEnemyEvent.RemoveListener(Use);
        }
    }



    public virtual string GetDescription()
    {
        return description;
    }

}
