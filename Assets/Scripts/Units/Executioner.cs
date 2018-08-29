using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Executioner : Enemy {
    
    public float executeDelay = 2f;

    public Coroutine executeRoutine;

    override protected void Awake()
    {
        base.Awake();
        //Execute move is interupted if the enemy takes damage
        GetComponent<Health>().tookDamageEvent.AddListener((GameObject inst, int dmg) => {
            if(executeRoutine != null)
            {
                StopCoroutine(executeRoutine);
                DisplayText("Interupted");
            }
        });
    }

    void Execute()
    {
        Debug.Log("Execute");
        executeRoutine = StartCoroutine(ExecuteRoutine());
    }

    IEnumerator ExecuteRoutine()
    {
        
        DisplayText("Execute!");
        yield return new WaitForSeconds(executeDelay);

        DamageAll(100000);
        DisplayText("Finished!");
       
    }

    protected override IEnumerator BehaviourTick()
    {
        while (true)
        {

            yield return new WaitForSeconds(GetEffectiveSpeed());

            if(Random.Range(0, 5) == 0)
            {
                Execute();
            }else
            {
                RpcAttack();
            }
        }

    }

}
