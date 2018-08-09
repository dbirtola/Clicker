using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;





public class PlayerPawn : Unit {

    public Player player;
    public PlayerStats playerStats;
    PlayerStatStruct statStruct;

    //For boss fights
    public PositionSquare positionSquare;

    List<DamageOverTimeEffect> poisonDamageEffects;
    public int maxPoisonStacks = 5;

	// Use this for initialization
	protected override void Awake () {
        base.Awake();

        poisonDamageEffects = new List<DamageOverTimeEffect>();

        DontDestroyOnLoad(this);

    }
	

    protected override void Start()
    {
        if (!isLocalPlayer)
        {
            Color c = GetComponent<SpriteRenderer>().material.color;
            c.a = 0.5f;
            GetComponent<SpriteRenderer>().material.color = c;
        }
    }

    public override void OnStartLocalPlayer()
    {
        Debug.Log("Player Pawn set to: " + gameObject);

        player = FindObjectOfType<Player>();
        player.SetPlayerPawn(this);
        playerStats = player.GetComponent<PlayerStats>();
        playerStats.levelUpEvent.AddListener((int lvl) => { CmdInitializeStats(playerStats.GetTotalStatStruct()); });
    }



    //childindex == -1 refers to hitting the base object
    [Command]
    public void CmdAttack(GameObject target, int childIndex, int damage)
    {
       // Debug.Log("Hitting child: " + childIndex);

        Unit unit = target.GetComponent<Unit>();

       // int damage = statStruct.damage;
        if (unit != null)
        {
            unit.ReceiveAttack(gameObject, damage, childIndex);

            if(statStruct.poisonDamage > 0)
            {
                CmdApplyPoisonDamage(unit.gameObject);
            }
        }
        /*
        EnemyComponent eComp = target.GetComponent<EnemyComponent>();
        if(eComp != null)
        {
            eComp.OnHit(damage);
        }
        */
    }

    

    [Command]
    void CmdApplyPoisonDamage(GameObject target)
    {
        var debuff = target.GetComponent<Debuffs>();

        DamageOverTimeEffect currentEffect = null;

        List<DamageOverTimeEffect> toBeRemoved = new List<DamageOverTimeEffect>();
        //Check if player already has a poison damage effect on the enemy
        //Also check if any effects are expired and need to be removed
        for(int i = 0; i < poisonDamageEffects.Count; i++)
        {
            if(poisonDamageEffects[i].target == target && poisonDamageEffects[i].IsActive())
            {
                currentEffect = poisonDamageEffects[i];
            }
            if(poisonDamageEffects[i].target == null || !poisonDamageEffects[i].IsActive())
            {
                toBeRemoved.Add(poisonDamageEffects[i]);
            }
        }

        for(int i = 0; i < toBeRemoved.Count; i++)
        {
            poisonDamageEffects.Remove(toBeRemoved[i]);
        }

        if (currentEffect == null)
        {
            currentEffect = new DamageOverTimeEffect(target, (int)Mathf.Ceil(statStruct.damage * statStruct.poisonDamage), 1, 5);
            currentEffect.instigator = gameObject;
            poisonDamageEffects.Add(currentEffect);
        }
       
        if (currentEffect.IsActive())
        {
            currentEffect.stacks += 1;
            if(currentEffect.stacks > maxPoisonStacks)
            {
                currentEffect.stacks = maxPoisonStacks;
            }
            currentEffect.RefreshDuration();
        }else
        {
            debuff.ServerAddDamageOverTimeEffect(currentEffect);
        }
        


    }


    [Command]
    public void CmdInitializeStats(PlayerStatStruct statStruct)
    {
        this.statStruct = statStruct;
        GetComponent<Health>().health = statStruct.maxHealth;
        GetComponent<Health>().maxHealth = statStruct.maxHealth;

    }

    [Command]
    public void CmdMoveSquares(PositionSquareLocation loc)
    {

        positionSquare = FindObjectOfType<BossFightManager>().getPositionSquare(loc);
        transform.position = positionSquare.transform.position;
        RpcUpdatePosition(loc);
    }

    [ClientRpc]
    public void RpcUpdatePosition(PositionSquareLocation loc)
    {
        positionSquare = FindObjectOfType<BossFightManager>().getPositionSquare(loc);
        transform.position = positionSquare.transform.position;
    }

    [Command]
    public void CmdMoveLeft()
    {
        if(positionSquare.location == PositionSquareLocation.center)
        {
            CmdMoveSquares(PositionSquareLocation.left);
        }else if(positionSquare.location == PositionSquareLocation.right)
        {
            CmdMoveSquares(PositionSquareLocation.center);
        }
    }

    [Command]
    public void CmdMoveRight()
    {
        if (positionSquare.location == PositionSquareLocation.center)
        {
            CmdMoveSquares(PositionSquareLocation.right);
        }
        else if (positionSquare.location == PositionSquareLocation.left)
        {
            CmdMoveSquares(PositionSquareLocation.center);
        }
    }


    [ClientRpc]
    public void RpcInitializeStats()
    {
       
        if (!isLocalPlayer)
        {
            return;
        }
        this.statStruct = player.GetComponent<PlayerStats>().GetTotalStatStruct();
       
        GetComponent<Health>().health = statStruct.maxHealth;
        GetComponent<Health>().maxHealth = statStruct.maxHealth;
        CmdInitializeStats(statStruct);
    }




    public void MoveToSquare(PositionSquare square)
    {
        transform.position = square.transform.position;
        positionSquare = square;
    }
}
