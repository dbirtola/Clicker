using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianAxe : Enemy{


    Rigidbody2D rb;

    float trackSpeedX = 0.001f;
    float trackSpeedY = 0.0005f;
    GameObject owner;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        //Ignore call to base.start() so we dont tick damage as if we are attacking
    }



    public void SetDamage(GameObject owner, int damage)
    {
        this.owner = owner;
        this.damage = damage;
    }

    public void TrackTarget(GameObject target, Vector2 initialVelocity)
    {
        rb.velocity = initialVelocity;
        StartCoroutine(TrackTargetRoutine(target));
    }

    IEnumerator TrackTargetRoutine(GameObject target)
    {
        while (true)
        {
            Vector2 direction = target.transform.position - transform.position;
            float x = direction.normalized.x * trackSpeedX * Time.time;
            float y = direction.normalized.y * trackSpeedY * Time.time;
            rb.velocity += new Vector2(x, y);
            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<PlayerPawn>())
        {
            coll.gameObject.GetComponent<Health>().CmdTakeDamage(owner, damage);
            Destroy(gameObject);
        }
    }
    
}
