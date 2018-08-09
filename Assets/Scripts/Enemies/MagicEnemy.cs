using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyModifier
{
    Strong,
    Fast, //Has known issues with barbarian speed
    Healthy,
    Wealthy,
    Small,
    Explosive
}

public class MagicEnemy : MonoBehaviour {

    public Enemy target;

    const int numModifiers = 2;

    public List<EnemyModifier> modifiers;

    public void Awake()
    {
        modifiers = new List<EnemyModifier>();
    }

    public void Start()
    {
        target = GetComponent<Enemy>();
        if (target == null)
        {
            Debug.Log("Magic enemy has no target");
            Destroy(this);
        }

        for(int i = 0; i < numModifiers; i++)
        {
            EnemyModifier newMod;
            do
            {
                newMod = (EnemyModifier)Random.Range(0, 6);

            } while (HasModifier(newMod));
             modifiers.Add(newMod);
        }

        ApplyEffects();
    }

    public bool HasModifier(EnemyModifier mod)
    {
        foreach(EnemyModifier em in modifiers)
        {
            if(em == mod)
            {
                return true;
            }
        }

        return false;
    }

    void ApplyEffects()
    {
        foreach (EnemyModifier em in modifiers)
        {

            switch (em)
            {
                case EnemyModifier.Fast:
                    target.baseSpeed /= 1.5f;
                    target.baseSpeed /= 1.5f;
                    break;
                case EnemyModifier.Healthy:
                    Health h = target.GetComponent<Health>();
                    h.health = (int)(h.health * 1.5f);
                    h.maxHealth = (int)(h.maxHealth * 1.5f);
                    break;
                case EnemyModifier.Small:
                    target.transform.localScale /= 2;
                    break;
                case EnemyModifier.Strong:
                    target.damage *= 2;
                    break;
                case EnemyModifier.Wealthy:
                    //Something with increased drops
                    break;
            }
        }


    }

    
}
