using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatAbilityButton : MonoBehaviour {

    public PlayerAbilities playerAbilities;
    public Ability ability;
    public int slot;
    public Sprite noAbilityIcon;


    public Image cooldownOverlay;


    public void Awake()
    {
        playerAbilities = FindObjectOfType<PlayerAbilities>();
    }

    public void Update()
    {
        if(ability == null)
        {
            return;
        }
        if (ability.isOnCooldown()){
            SetCooldownOverlay(true);
        }else
        {
            SetCooldownOverlay(false);
        }
    }

    public void SetCooldownOverlay(bool visible)
    {
        if (visible)
        {
            cooldownOverlay.gameObject.SetActive(true);
        }else
        {
            cooldownOverlay.gameObject.SetActive(false);
        }
    }

    public void UpdateWithAbility(Ability ability)
    {

        this.ability = ability;

        if (ability == null)
        {
            GetComponent<Image>().sprite = noAbilityIcon;
        }
        else{
            GetComponent<Image>().sprite = ability.icon;
            GetComponent<Button>().onClick.RemoveAllListeners();
            GetComponent<Button>().onClick.AddListener(UseAbility);
        }
    }

    public void UseAbility()
    {
        // playerAbilities.
        ability.AttemptUse();
    }
}
