using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour {

    public float currentMana { get; private set; }

    public float maxMana = 100;

    public void Awake()
    {
        currentMana = 0;
        //Refill();
    }

	public bool UseMana(float mana)
    {
        if(currentMana < mana)
        {
            return false;
        }else
        {
            currentMana -= mana;
            return true;
        }
    }

    public void GainMana(float mana)
    {
       
        currentMana += mana;
        if(currentMana > maxMana)
        {
            currentMana = maxMana;
        }
    }

    public void Refill()
    {
        currentMana = maxMana;
    }
}
