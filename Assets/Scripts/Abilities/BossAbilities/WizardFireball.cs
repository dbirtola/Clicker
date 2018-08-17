using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WizardFireball : Ability {

    GameObject fireballPrefab;

    public float fireballCastDelay = 1f;

    public PositionSquare targetSquare;

    public override void Use(GameObject target)
    {
        base.Use(target);

        //StartCoroutine(Fireball(target.GetComponent<PositionSquare>()));

    }

    /*
    IEnumerator Fireball(PositionSquare targetSquare)
    {
        var fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
       // NetworkServer.Spawn(fireball);
        yield return new WaitForSeconds(fireballCastDelay);
        //send fireball
        yield return new WaitForSeconds(0.2f); //Slight delay to mimic fireball flight time without having to do actual collision detection
        
        foreach(PlayerPawn p in targetSquare.pawnsOnSquare)
        {
            p.ReceiveAttack(owningUnit.gameObject, 200);
        }
       
        foreach(PositionSquare ps in targetSquare.GetAdjacentSquares())
        {
            foreach(PlayerPawn p in ps.pawnsOnSquare)
            {
                p.ReceiveAttack(owningUnit.gameObject, 100);
            }
        }
    }
    */

}
