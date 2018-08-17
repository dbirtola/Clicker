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

    GameObject floatingTextPrefab;

    protected override void Awake()
    {
        base.Awake();
        floatingTextPrefab = Resources.Load("FloatingText") as GameObject;
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
    
    public virtual void DamageAll(int dmg)
    {
        foreach (var player in FindObjectsOfType<PlayerPawn>())
        {
            player.GetComponent<Unit>().ReceiveAttack(gameObject, dmg);


        }
    }

    //Meant to be the basic attack, will cause default animation to be used
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
        CleanUp();

        FindObjectOfType<Player>().killedEnemyEvent.Invoke(this);
        FindObjectOfType<PlayerStats>().AddExperience(experience);
        FindObjectOfType<PlayerInventory>().PickUpItem(FindObjectOfType<ItemFactory>().GetItemDrop(level));

        Destroy(gameObject);
    }
   
    //Allows for some clean up whenever an enemy is removed, whether by death or by being destroyed/run away
    //Cant always use Destroy because destroy waits till next frame. CleanUp will be called before the death animation
    public virtual void CleanUp()
    {

    }

    protected void DisplayText(string text)
    {

        var txt = Instantiate(floatingTextPrefab, FindObjectOfType<CombatHud>().transform).GetComponent<FloatingText>();
        var position = Camera.main.WorldToScreenPoint(transform.position);
        txt.Float(position, text);
    }

    //Could optimize
    public List<PlayerPawn> GetPlayerPawns()
    {
        return new List<PlayerPawn>(FindObjectsOfType<PlayerPawn>());
    }
}
