using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AbilitiesPanel : MonoBehaviour {


    public PlayerAbilities playerAbilities;
    public GameObject availableAbilitiesPanel;

    public AbilityInfoPanel abilityInfoPanelPrefab;

    public AbilityInfoBox abilityInfoBox;

    void Awake()
    {
        playerAbilities = FindObjectOfType<PlayerAbilities>();
    }

    void Start()
    {
        playerAbilities.abilityGainedEvent.AddListener(UpdateWithAbilities);
        UpdateWithAbilities(null);
    }

    public void UpdateWithAbilities(Ability newAbility)
    {
        foreach(AbilityInfoPanel go in availableAbilitiesPanel.GetComponentsInChildren<AbilityInfoPanel>())
        {
            Destroy(go.gameObject);
        }

        foreach(Ability ability in playerAbilities.availableAbilities)
        {
            AbilityInfoPanel newPanel = Instantiate(abilityInfoPanelPrefab);
            newPanel.UpdateWithAbility(ability);
            newPanel.transform.SetParent(availableAbilitiesPanel.transform, false);
            newPanel.GetComponent<Button>().onClick.AddListener(()=> { ShowAbilityInfoBox(ability); });
        }


    }

    void ShowAbilityInfoBox(Ability ability)
    {
        abilityInfoBox.gameObject.SetActive(true);
        abilityInfoBox.UpdateWithAbility(ability);
    }



    
}
