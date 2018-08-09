using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ManaBar : MonoBehaviour {


    public GameObject target;
    Mana targetMana;
    public Text manaText;

    // Use this for initialization
    void Start()
    {
        //playerPawn = FindObjectOfType<Player>().GetPlayerPawn();
    }

    public void SetTarget(GameObject target)
    {
        if (target.GetComponent<Health>())
        {
            targetMana = target.GetComponent<Mana>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //playerHealth = playerPawn.GetComponent<Health>();
        if (targetMana != null)
        {
            GetComponent<Slider>().value = (float)targetMana.currentMana / targetMana.maxMana;
            manaText.text = ((int)targetMana.currentMana).ToString() + "/" + ((int)targetMana.maxMana).ToString();
        }
    }
}
