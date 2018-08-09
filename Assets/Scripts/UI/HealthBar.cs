using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public GameObject target;
    Health targetHealth;

	// Use this for initialization
	void Start () {
        //playerPawn = FindObjectOfType<Player>().GetPlayerPawn();
	}
	
    public void SetTarget(GameObject target)
    {
        if (target.GetComponent<Health>())
        {
            targetHealth = target.GetComponent<Health>();
        }
    }

	// Update is called once per frame
	void Update () {
        //playerHealth = playerPawn.GetComponent<Health>();
        if(targetHealth != null)
        {
            GetComponent<Slider>().value = (float)targetHealth.health / targetHealth.maxHealth;
        }
    }
}
