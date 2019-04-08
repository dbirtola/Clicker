using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy{

    protected bool pauseBehaviorTick = false;

    public PlayerPawn playerPawn;
    protected BossFightManager bossFightManager;

    

	// Use this for initialization
	protected override void Start () {
        base.Start();
        playerPawn = FindObjectOfType<Player>().GetPlayerPawn();
        bossFightManager = FindObjectOfType<BossFightManager>();
  

    }

    public override void RpcAttack()
    {
        base.RpcAttack();
        DisplayText("Attack!");
    }
    public void TargetSquare(PositionSquare square)
    {
        square.SetColor(Color.red);
    }

    public void UntargetSquare(PositionSquare square)
    {
        square.SetColor(Color.white);
    }


    //Possible place for optimization
   // protected List<PlayerPawn> GetPlayerPawns()
   // {
   //     return new List<PlayerPawn>(FindObjectsOfType<PlayerPawn>());
   // }

    protected PlayerPawn GetRandomPlayerPawn()
    {
        return GetPlayerPawns()[Random.Range(0, GetPlayerPawns().Count)];
    }

}
