using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardMissiles : MonoBehaviour {

    //Create a single projectile class that all abilities can use
    
    PositionSquare targetSquare;
    GameObject instigator;
    int damage;

    public void FireAt(GameObject instigator, PositionSquare square, float speed, int damage)
    {
        // NetworkServer.Spawn(fireball);
        targetSquare = square;
        this.instigator = instigator;
        this.damage = damage;

        targetSquare.SetColor(Color.red);

        GetComponent<Rigidbody2D>().velocity = (targetSquare.transform.position - transform.position).normalized * speed;




    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<PositionSquare>())
        {
            foreach (PlayerPawn p in targetSquare.pawnsOnSquare)
            {
                p.ReceiveAttack(new DamageInfo(instigator, gameObject, p.gameObject, damage));
            }


            targetSquare.SetColor(Color.white);


            Destroy(gameObject);
        }

    }
}
