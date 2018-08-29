using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectile : MonoBehaviour {

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
        foreach (PositionSquare ps in targetSquare.GetAdjacentSquares())
        {
            ps.SetColor(Color.yellow);
        }

        GetComponent<Rigidbody2D>().velocity = (targetSquare.transform.position - transform.position).normalized * speed;




    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<PositionSquare>())
        {
            foreach (PlayerPawn p in targetSquare.pawnsOnSquare)
            {
                p.ReceiveAttack(new DamageInfo(gameObject, gameObject, p.gameObject, damage));
            }

            foreach (PositionSquare ps in targetSquare.GetAdjacentSquares())
            {
                foreach (PlayerPawn p in ps.pawnsOnSquare)
                {
                    p.ReceiveAttack(new DamageInfo(instigator, gameObject, p.gameObject, damage/2));
                }
            }

            targetSquare.SetColor(Color.white);
            foreach (PositionSquare ps in targetSquare.GetAdjacentSquares())
            {
                ps.SetColor(Color.white);
            }

            Destroy(gameObject);
        }

    }

}
