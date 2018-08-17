using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ElderMagician : Boss {

    public GameObject fireballPrefab;
    public float fireballCastDelay = 1f;
    float fireballSpeed = 4f;

    DamageOverTimeEffect burnDot;

    public Sprite fireSprite;

    [ClientRpc]
    public void RpcFireball()
    {
        pauseBehaviorTick = true;


        StartCoroutine(Fireball());
    }

    IEnumerator Fireball()
    {
        var fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        // NetworkServer.Spawn(fireball);

        yield return new WaitForSeconds(fireballCastDelay);
        //send fireball

        var targetSquare = GetRandomPlayerPawn().positionSquare;

        targetSquare.SetColor(Color.red);
        foreach (PositionSquare ps in targetSquare.GetAdjacentSquares())
        {
            ps.SetColor(Color.yellow);
        }
        fireball.GetComponent<Rigidbody2D>().velocity = (targetSquare.transform.position - transform.position).normalized * fireballSpeed; ;
        yield return new WaitForSeconds(0.8f); //Slight delay to mimic fireball flight time without having to do actual collision detection
        Destroy(fireball);
        foreach (PlayerPawn p in targetSquare.pawnsOnSquare)
        {
            p.ReceiveAttack(gameObject, 200);
        }

        foreach (PositionSquare ps in targetSquare.GetAdjacentSquares())
        {
            foreach (PlayerPawn p in ps.pawnsOnSquare)
            {
                p.ReceiveAttack(gameObject, 100);
            }
        }

        targetSquare.SetColor(Color.white);
        foreach (PositionSquare ps in targetSquare.GetAdjacentSquares())
        {
            ps.SetColor(Color.white);
        }
        pauseBehaviorTick = false;
    }

    public void ServerBurn()
    {
        burnDot = new DamageOverTimeEffect(playerPawn.gameObject, 10, 0.5f, 10);
        burnDot.instigator = gameObject;
        burnDot.graphicSprite = fireSprite;
        playerPawn.GetComponent<Debuffs>().ServerAddDamageOverTimeEffect(burnDot);
        playerPawn.GetComponent<PlayerPawn>().movedSquaresEvent.AddListener(RefreshDamage);
        StartCoroutine(BurnRoutine());
    }

    IEnumerator BurnRoutine()
    {
        for(int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(0.5f);
            burnDot.damage = (int)(burnDot.damage * 1.5f);
        }
        burnDot = null;
    }

    public void RefreshDamage()
    {
        if(burnDot == null)
        {
            playerPawn.GetComponent<PlayerPawn>().movedSquaresEvent.RemoveListener(RefreshDamage);
            return;
        }
        burnDot.damage = 10;
    }


[ClientRpc]
    public void RpcBurn()
    {

    }

    
    [ClientRpc]
    public void RpcMissiles()
    {

    }

    [ClientRpc]
    public void RpcSpawnOrb()
    {
        
    }
    
    protected override IEnumerator BehaviourTick()
    {
        
        while (true)
        {

            yield return new WaitForSeconds(currentSpeed);

            //Should probably find a better way to implement this to restart time between attacks after the boss finishes an ability
            if (pauseBehaviorTick)
                continue;

            int decision = Random.Range(0, 2);
            switch (decision)
            {
                case 0:
                    RpcFireball();
                    break;
                case 1:
                    if(burnDot == null)
                    {
                        ServerBurn();
                    }
                    break;
                case 2:
                    RpcMissiles();
                    break;
                case 3:
                    RpcAttack();
                    break;
            }
        }
        
        
    }
}
