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
        Debug.Log("Start fired\n");
        playerAbilities.abilityGainedEvent.AddListener(UpdateWithAbilities);
        UpdateWithAbilities(null);
    }

    public void UpdateWithAbilities(Ability newAbility)
    {
        Debug.Log("Update Fired\n");
        foreach (AbilityInfoPanel go in availableAbilitiesPanel.GetComponentsInChildren<AbilityInfoPanel>())
        {
            Destroy(go.gameObject);
        }
        Debug.Log("Abilities panel " + playerAbilities.availableAbilities.Count);

        foreach(Ability ability in playerAbilities.availableAbilities)
        {
            if(ability == null)
            {
                Debug.LogError("Null ability");
                continue;
            }
            Debug.Log("Working on ability : " + ability.abilityName);
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
