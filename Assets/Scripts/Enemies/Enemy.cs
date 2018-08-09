using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;



public class Enemy : Unit{

    [SyncVar]
    public int damage;
   
    public int level = 1;
    protected int experience = 0;

    protected override void Awake()
    {
        base.Awake();
        //unitName = "Enemy";
    }

    protected virtual void Update()
    {

    }

	// Use this for initialization
	protected override void Start () {
      //  GetComponent<SpriteRenderer>().material.color = new Color(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3));

        if (isServer)
        {
            StartCoroutine(BehaviourTick());
        }
    }

    public virtual void SetLevel(int level)
    {
        this.level = level;
    }


    //Called by the server
    public override void OnDeath(GameObject killer)
    {
        base.OnDeath(killer);
 
    }

    public float GetExperienceValue()
    {
        experience = (int)Mathf.Ceil(health.maxHealth + (health.maxHealth / 5) * Mathf.Pow(1.02f, level));
        return experience;
    }
    

    public virtual void RpcAttack()
    {
        foreach(var player in FindObjectsOfType<PlayerPawn>())
        {
            player.GetComponent<Unit>().ReceiveAttack(gameObject, damage);


        }
    }

    protected virtual IEnumerator BehaviourTick()
    {

        while (true)
        {

            yield return new WaitForSeconds(GetEffectiveSpeed());

            RpcAttack();
        }
    }


    //Called for all clients

    [ClientRpc]
    public override void RpcProcessDeath()
    {

        FindObjectOfType<Player>().killedEnemyEvent.Invoke(this);
        FindObjectOfType<PlayerStats>().AddExperience(experience);
        FindObjectOfType<PlayerInventory>().addItem(FindObjectOfType<ItemFactory>().GetItemDrop(level));

        Destroy(gameObject);
    }
   
}
