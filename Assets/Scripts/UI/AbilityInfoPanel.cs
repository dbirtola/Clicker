using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityInfoPanel : MonoBehaviour {

    public Ability ability;
    public Text abilityName;
    public Text abilityDescription;
    public Image abilityIcon;

    /*
    public void OnDrag(PointerEventData eventData)
    {
        transform.localPosition = new Vector2(transform.localPosition.x + eventData.delta.x, transform.localPosition.y + eventData.delta.y); 
        Debug.Log("Dragging ability info panel");
    }
    */

    public void UpdateWithAbility(Ability ability)
    {
        this.ability = ability;
        abilityName.text = ability.abilityName;
        abilityDescription.text = ability.GetDescription();
        abilityIcon.sprite = ability.icon;
    }
}
