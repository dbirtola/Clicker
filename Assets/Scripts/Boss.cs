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
	
    public void TargetSquare(PositionSquare square)
    {
        square.SetColor(Color.red);
    }

    public void UntargetSquare(PositionSquare square)
    {
        square.SetColor(Color.white);
    }


}
