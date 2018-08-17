using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedAbilityButton : MonoBehaviour {



    public Ability ability;
    public Sprite noAbilitySprite;
    public int slot;

    public void UpdateWithAbility(Ability ability)
    {

        this.ability = ability;

        if (ability == null)
        {
            GetComponent<Image>().sprite = noAbilitySprite;
        }else
        {
            GetComponent<Image>().sprite = ability.icon;
        }
    }
}
