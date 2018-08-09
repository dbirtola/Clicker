using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityInfoBox : MonoBehaviour {


    public Ability ability;

    public Image abilityImage;
    public Text abilityName;
    public Text cooldownText;
    public Text manaCostText;
    public Text descriptionText;


    public Button equipButton;

	// Use this for initialization
	void Start () {
		
	}
	



    public void UpdateWithAbility(Ability ability)
    {
        this.ability = ability;

        abilityImage.sprite = ability.icon;
        abilityName.text = ability.abilityName;
        cooldownText.text = "Cooldown: " +  ability.cooldown.ToString();
        manaCostText.text = "Mana Cost: " + ability.manaCost.ToString();
        descriptionText.text = "\t" + ability.GetDescription();

       // equipButton.onClick.RemoveAllListeners();
        //equipButton.onClick.AddListener(() => { gameObject.SetActive(false); });
        //equipButton.onClick.AddListener()
    }

    void SelectedAbility()
    {
       // PresentAbilitySlotSelect();
    }
}
