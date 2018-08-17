using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BarbarianChieftan : Boss{

    public BarbarianAxe barbarianAxePrefab;

    [ClientRpc]
    public void RpcChargeAttack(PositionSquareLocation target)
    {
        Debug.LogError("Charge Attack! " + target);
        
        PositionSquare targetSquare = bossFightManager.getPositionSquare(target);
        targetSquare.SetColor(Color.red);

        StartCoroutine(chargeAttack(targetSquare));

    }

    IEnumerator chargeAttack(PositionSquare square)
    {
        pauseBehaviorTick = true;

        yield return new WaitForSeconds(1);
        
        if(playerPawn.positionSquare == square)
        {
            Debug.LogError("Player hit by charge attack!");
            playerPawn.ReceiveAttack(gameObject, 150);
        }
        square.SetColor(Color.white);

        pauseBehaviorTick = false;

    }

    [ClientRpc]
    public void RpcChoppingBlock()
    {
        StartCoroutine(choppingBlock());
    }

    IEnumerator choppingBlock()
    {
        Debug.LogError("Chopping Block! ");

        pauseBehaviorTick = true;
        for(int i = 0; i < 7; i++)
        {

            PositionSquare target = bossFightManager.getRandomPositionSquare();
            TargetSquare(target);
            yield return new WaitForSeconds(0.75f);
            if (playerPawn.positionSquare == target)
            {
                Debug.LogError("Player hit by Chopping Block!");
                playerPawn.ReceiveAttack(gameObject, 80);
            }

            UntargetSquare(target);

        }
        pauseBehaviorTick = false;
    }


    [ClientRpc]
    public void RpcAxeBarrage()
    {
        StartCoroutine(axeBarrage());
        Debug.Log("Axe barrage!");
    }

    IEnumerator axeBarrage()
    {
        pauseBehaviorTick = true;
        for(int i = 0; i < 8; i++)
        {
            ThrowAxe();
            yield return new WaitForSeconds(0.5f);
        }
        pauseBehaviorTick = false;
    }

    public void ThrowAxe()
    {
        Debug.Log("Throwing an axe!");
        var axe = Instantiate(barbarianAxePrefab, transform.position, Quaternion.identity);
        axe.SetDamage(gameObject, 35);
        Vector2 initialVelocity = new Vector2(Random.Range(0.3f, 0.4f), -1);

        //Offset and possibly throw to the right instead of the left
        if(Random.Range(0, 2) == 0)
        {
            //Throw left
            initialVelocity *= -1;
            axe.transform.position = axe.transform.position + new Vector3(-1, 0, 0);
            axe.GetComponent<Rigidbody2D>().angularVelocity = 360;
        }else
        {
            axe.transform.position = axe.transform.position + new Vector3(1, 0, 0);
            axe.GetComponent<Rigidbody2D>().angularVelocity = -360;


        }
        axe.TrackTarget(playerPawn.gameObject, initialVelocity);
    }

    protected override IEnumerator BehaviourTick()
    {

        while (true)
        {

            yield return new WaitForSeconds(currentSpeed);

            //Should probably find a better way to implement this to restart time between attacks after the boss finishes an ability
            if (pauseBehaviorTick)
                continue;

            int decision = Random.Range(0, 3);
            switch (decision)
            {
                case 0:
                    RpcChoppingBlock();
                    break;
                case 1:
                    RpcChargeAttack((PositionSquareLocation)Random.Range(0, 3));
                    break;
                case 2:
                    RpcAttack();
                    break;
                case 3:
                    RpcAxeBarrage();
                    break;
            }
            //RpcAttack();
        }
    }

}
