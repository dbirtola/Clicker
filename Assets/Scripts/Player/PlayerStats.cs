using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class LevelUpEvent : UnityEvent<int>
{


}

public class DamageMultiplierStruct
{
    public float multiplierAmount;
    public int numberOfTapsRemaining;
    public float expireTime;
}

//Stats that are recreatable from items/artifacts/etc. should not be saved.
[System.Serializable]
public class PlayerStatData
{
    public int level;
    public int experience;
}

//Using a struct to store players stats to make it easier to pass the information to the server
//Also was originally intended to suit an idea that was removed from the game, and its not likely that refactoring would be worth the time
[System.Serializable]
public struct PlayerStatStruct
{
    public int level;
    public int currentExperience;

    public int damage;
    public int maxHealth;
    public int armor;
    public float criticalChance;
    public float criticalDamageBonus;
    public float manaPerTap;
    public float lifeSteal;
    //Poison damage works as a multiple of tap damage dealt every second for 5 seconds, stackable up to 5 times.
    public float poisonDamage;
    //Mechanic used by multiple features so just including it here to avoid one tap resulting in 3-4 attacks
    public float doubleAttackChance;

    public float experienceMultiplier;
    public float specialMonsterMultiplier;

    public float materialMultiplier;
    public float oreRateMultiplier;

    public int towerStartBonus;
}

public class PlayerStats : MonoBehaviour {

    public PlayerStatStruct baseStats;
    //BonusStats are stats given by armor/artifacts/effects outside of combat. These will not be immediately present on the players pawn
    //until combat starts and the pawn is told to initialize its stats.
    public PlayerStatStruct bonusStats;

    public LevelUpEvent gainedExperienceEvent;
    public LevelUpEvent levelUpEvent;


    public List<DamageMultiplierStruct> damageMultipliers;
    float damagePercentIncrease = 1;
   // float skillDamageIncrease = 1;

    // Use this for initialization
    void Awake ()
    {
        levelUpEvent = new LevelUpEvent();
        gainedExperienceEvent = new LevelUpEvent();
        damageMultipliers = new List<DamageMultiplierStruct>();

        //Initializing all base stats
        baseStats.level = 1;
        baseStats.currentExperience = 0;

        baseStats.damage = 3;
        baseStats.maxHealth = 1500;
        baseStats.armor = 0;
        baseStats.criticalChance = 0;
        baseStats.criticalDamageBonus = 1;
        baseStats.manaPerTap = 1;
        baseStats.lifeSteal = 0;
        baseStats.poisonDamage = 0;
        baseStats.doubleAttackChance = 0;
        baseStats.specialMonsterMultiplier  = 1f;
        baseStats.experienceMultiplier = 1f;
        

        baseStats.materialMultiplier = 1;
        baseStats.oreRateMultiplier = 1;

        baseStats.towerStartBonus = 0;

        //Initialize all bonus stats
        bonusStats.level = 0;
        bonusStats.currentExperience = 0;
        bonusStats.damage = 0;
        bonusStats.maxHealth = 0;
        bonusStats.armor = 0;
        bonusStats.criticalChance = 0;
        bonusStats.criticalDamageBonus = 0;
        bonusStats.manaPerTap = 0;
        bonusStats.lifeSteal = 0;
        bonusStats.poisonDamage = 0;
        bonusStats.doubleAttackChance = 0;

        bonusStats.experienceMultiplier = 0f;
        bonusStats.specialMonsterMultiplier = 0;

        bonusStats.materialMultiplier = 0;
        bonusStats.oreRateMultiplier = 0f;

        bonusStats.towerStartBonus = 0;

    }



    //1 = 100%
    public void AddPersistentDamagePercentIncrease(float percent)
    {
        damagePercentIncrease += percent;
    }

    public void AddBonusHealth(int health)
    {
        bonusStats.maxHealth += health;
        //GetComponent<Player>().GetPlayerPawn().GetComponent<Health>().maxHealth = GetTotalStatStruct().maxHealth;

    }

    public void AddBonusDamage(int damage)
    {
        bonusStats.damage += damage;
    }

    public void AddDoubleSwingChance(float chance)
    {
        bonusStats.doubleAttackChance += chance;
    }

    public void AddBonusCriticalChance(float chance)
    {
        bonusStats.criticalChance += chance;
    }

    public void AddBonusCriticalDamage(float damage)
    {
        bonusStats.criticalDamageBonus += damage;
    }

    public void AddBonusArmor(int armor)
    {
        bonusStats.armor += armor;
    }

    public void AddManaPerTap(float mana)
    {
        bonusStats.manaPerTap += mana;
    }

    public void AddLifeSteal(float lifeSteal)
    {
        bonusStats.lifeSteal += lifeSteal;
    }

    public void AddPoisonDamage(float pDamage)
    {
        bonusStats.poisonDamage += pDamage;
    }
	
    public void AddSpecialMonsterMultiplier(float chance)
    {
        bonusStats.specialMonsterMultiplier += chance;
    }

    public void AddExperienceMultiplier(float multi)
    {
        bonusStats.experienceMultiplier += multi;
    }

    //float from 0-1
    public void AddMaterialMultiplier(float multiplier)
    {
        bonusStats.materialMultiplier += multiplier;
    }

    public void AddOreRateMultiplier(float multiplier)
    {
        bonusStats.oreRateMultiplier += multiplier;
    }

    public void AddTowerStartBonus(int bonus)
    {
        bonusStats.towerStartBonus += bonus;
    }

    public PlayerStatStruct GetBaseStatStruct()
    {
        return baseStats;
    }

    public PlayerStatStruct GetBonusStatStruct()
    {
        return bonusStats;
    }

    public PlayerStatStruct GetTotalStatStruct()
    {
        PlayerStatStruct temp = new PlayerStatStruct();
        temp.level = baseStats.level + bonusStats.level;
        temp.currentExperience = baseStats.currentExperience + bonusStats.currentExperience;
        temp.damage = baseStats.damage + bonusStats.damage;
        temp.maxHealth = baseStats.maxHealth + bonusStats.maxHealth;
        temp.armor = baseStats.armor + bonusStats.armor;
        temp.criticalChance = baseStats.criticalChance + bonusStats.criticalChance;
        temp.criticalDamageBonus = baseStats.criticalDamageBonus + bonusStats.criticalDamageBonus;
        temp.manaPerTap = baseStats.manaPerTap + bonusStats.manaPerTap;
        temp.lifeSteal = baseStats.lifeSteal + bonusStats.lifeSteal;
        temp.poisonDamage = baseStats.poisonDamage + bonusStats.poisonDamage;
        return temp;
    }

    //This will include the players multipliers, not meant to add a fixed amount of exp but to add exp gained from quests or kills
    public void AddExperience(int experience)
    {
        baseStats.currentExperience += (int)(Mathf.Ceil(experience * GetExperienceMultiplier()));
        gainedExperienceEvent.Invoke(experience);

        while (baseStats.currentExperience > GetExpTillLevel())
        {
            if (baseStats.currentExperience > GetExpTillLevel())
            {
                baseStats.currentExperience -= GetExpTillLevel();
                SetLevel(baseStats.level + 1);
                levelUpEvent.Invoke(baseStats.level);
            }
        }

    }

    public void SetLevel(int level)
    {
        baseStats.level = level;
        CalculateBaseStats();
        levelUpEvent.Invoke(baseStats.level);
    }

    public void CalculateBaseStats()
    {
        baseStats.damage = 1 + baseStats.level * 2;
        baseStats.maxHealth = 100 + 20 * baseStats.level * 2;
    }

    //= 100 + (CharacterLevel - 1) * 30 * 1.07^CharacterLevel
    public int GetExpTillLevel()
    {
        return (int)Mathf.Ceil(100 + (baseStats.level -1) *30 * Mathf.Pow(1.07f, baseStats.level));
    }
    public int GetExperience()
    {
        return baseStats.currentExperience;
    }
    

    public void AddDamageMultiplierForDuration(float multiplier, float duration)
    {
        DamageMultiplierStruct dms = new DamageMultiplierStruct();

        dms.multiplierAmount = multiplier;
        dms.expireTime = Time.time + duration;
        dms.numberOfTapsRemaining = 0;

        damageMultipliers.Add(dms);
    }



    public DamageMultiplierStruct AddDamageMultiplierForTaps(float multiplier, int numberOfTaps)
    {
        DamageMultiplierStruct dms = new DamageMultiplierStruct();

        dms.multiplierAmount = multiplier;
        dms.expireTime = 0;
        dms.numberOfTapsRemaining = numberOfTaps;

        damageMultipliers.Add(dms);

        return dms;
    }

    public void RemoveDamageMultiplier(DamageMultiplierStruct dms)
    {
        damageMultipliers.Remove(dms);
    }

    //Tap damage is calculated as (baseDamage + bonusDamage) * tapDamagePercentIncrease * skillDamageIncrease
    public int GetNextTapDamage()
    {
        //Creating a seperate list to remove them after the damage is calculated to avoid problems during the iteration
        List<DamageMultiplierStruct> expiredDms = new List<DamageMultiplierStruct>();

        //Multiply our total flat damage by any damage bonuses we are receiving from armor and persistent buffs
        int damage = (int)(GetTotalStatStruct().damage * damagePercentIncrease);

        //Multiply any temporary buffs, typically added on by skills
        foreach(DamageMultiplierStruct dms in damageMultipliers)
        {

            if(dms.expireTime > Time.time || dms.numberOfTapsRemaining > 0)
            {
                Debug.Log("Adding damage of: " + dms.multiplierAmount);
                damage = (int)(damage *  dms.multiplierAmount);
                dms.numberOfTapsRemaining--;
            }else
            {
                expiredDms.Add(dms);
            }

        }

        foreach(DamageMultiplierStruct dms in expiredDms)
        {
            damageMultipliers.Remove(dms);
        }


        return damage;
    }


    public float GetExperienceMultiplier()
    {
        return baseStats.experienceMultiplier + bonusStats.experienceMultiplier;
    }

    public float GetCriticalChance()
    {
        return baseStats.criticalChance + bonusStats.criticalChance;
    }

    public float GetCriticalDamage()
    {
        return baseStats.criticalDamageBonus + bonusStats.criticalDamageBonus;
    }

    public float GetDoubleAttackChance()
    {
        return baseStats.doubleAttackChance + bonusStats.doubleAttackChance;
    }

    public float GetSpecialMonsterMultiplier()
    {
        return baseStats.specialMonsterMultiplier+ bonusStats.specialMonsterMultiplier;
    }

    public float GetMaterialMultiplier()
    {
        return baseStats.materialMultiplier + bonusStats.materialMultiplier;
    }

    public float GetOreRateMultiplier()
    {
        return baseStats.oreRateMultiplier + bonusStats.oreRateMultiplier;
    }

    public int GetTowerStartBonus()
    {
        return baseStats.towerStartBonus + bonusStats.towerStartBonus;
    }


    public PlayerStatData SaveStats()
    {
        PlayerStatData psd = new PlayerStatData();
        psd.experience = baseStats.currentExperience;
        psd.level = baseStats.level;
        return psd;
    }

    public void LoadStats(PlayerStatData playerStatData)
    {
        baseStats.currentExperience = playerStatData.experience;
        baseStats.level = playerStatData.level;
        CalculateBaseStats();


    }
}
