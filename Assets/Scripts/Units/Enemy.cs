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

    public string description = "No description";
    public string mechanic = "None";
    public string counterplay = "None";

    public float dropChanceMultiplier { get; private set; }
    public float dropQualityMultiplier { get; private set; }

    GameObject floatingTextPrefab;

    //This is used to determine how much health a mob should have. Weak mobs should have a multiplier of 0.5-1, while tankier mobs should have 1+
    public float healthMultiplier = 1f;

    //Used to determine how hard the mob hits. Take into consideration the speed of the enemy as well, Weak mobs should have a multiplier less than speed, stronger should have more than speed
    public float damageMultiplier = 1f;

    protected override void Awake()
    {
        base.Awake();
        floatingTextPrefab = Resources.Load("FloatingText") as GameObject;

        dropChanceMultiplier = 1f;
        dropQualityMultiplier = 1f;
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

    /*
    public virtual void SetLevel(int level)
    {
        this.level = level;
    }*/

    public virtual void SetLevel(int level)
    {

        //base.SetLevel(level);
        this.level = level;
        health.maxHealth = (int)(Mathf.Ceil(50 + 10 * level* Mathf.Pow(1.1f, level))*healthMultiplier);
        health.health = health.maxHealth;
        damage = (int)(Mathf.Ceil(4 + 1.5f *level) * damageMultiplier);

        experience = (int)Mathf.Ceil(health.maxHealth / 10 + (health.maxHealth / 5) * Mathf.Pow(1.02f, level));

    }


    //Called by the server
    public override void OnDeath(GameObject killer)
    {
        base.OnDeath(killer);
 
    }

    public void AddExperienceMultiplier(float multiplier)
    {
        experience = (int)(experience * multiplier);
    }

    public float GetExperienceValue()
    {
        experience = (int)Mathf.Ceil(health.maxHealth + (health.maxHealth / 5) * Mathf.Pow(1.02f, level));
        return experience;
    }
    
    //For sure buggy because it will run on the boss on each client, effectively doing it twice. Shouldve used RpcAttack. fix this
    public virtual void DamageAll(int dmg)
    {
        foreach (var player in FindObjectsOfType<PlayerPawn>())
        {
            player.GetComponent<Unit>().ReceiveAttack(new DamageInfo(gameObject, gameObject, player.gameObject, dmg));


        }
    }

    //Meant to be the basic attack, will cause default animation to be used
    public virtual void RpcAttack()
    {
        foreach(var player in FindObjectsOfType<PlayerPawn>())
        {
            player.GetComponent<Unit>().ReceiveAttack(new DamageInfo(gameObject, gameObject, player.gameObject, damage));


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
        FindObjectOfType<PlayerInventory>().PickUpItem(FindObjectOfType<ItemFactory>().GetItemDrop(this));

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

    public void AddDropQualityMultipler(float multi)
    {
        dropQualityMultiplier += multi;
    }

    public void AddDropChanceMultiplier(float multi)
    {
        dropChanceMultiplier += multi;
    }
}
