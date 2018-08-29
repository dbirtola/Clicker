using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ArtifactsPanel : MonoBehaviour {

    public ArtifactInfoPanel artifactInfoPanelPrefab;

    public GameObject artifactView;

    public GameObject artifacts;

    public Text artifactPointsText;

    public Artifact selectedArtifact;

    public GameObject confirmationBox;
    public Text confirmationArtifactName;
    public Text confirmationArtifactCost;
    public Button confirmationAccept;

    public void Awake()
    {
        artifacts = FindObjectOfType<PlayerArtifacts>().artifactHolder;
    }

    public void Start()
    {   
        Refresh();
        FindObjectOfType<PlayerArtifacts>().artifactsChangedEvent.AddListener((Artifact a)=> { Refresh(); });

        FindObjectOfType<PersistanceManager>().persistantDataLoadedEvent.AddListener(Refresh);
    }

    public void Refresh()
    {
        foreach (ArtifactInfoPanel go in artifactView.GetComponentsInChildren<ArtifactInfoPanel>())
        {
            Destroy(go.gameObject);
        }

        foreach (Artifact artifact in artifacts.GetComponents<Artifact>())
        {
            ArtifactInfoPanel newPanel = Instantiate(artifactInfoPanelPrefab);
            newPanel.UpdateWithArtifact(artifact);
            newPanel.transform.SetParent(artifactView.transform, false);
            //newPanel.GetComponent<Button>().onClick.AddListener(() => { ShowAbilityInfoBox(ability); });
        }

        artifactPointsText.text = "Artifact Points: " + FindObjectOfType<PlayerArtifacts>().artifactPoints.ToString();


    }

    public void ArtifactSelected(Artifact artifact)
    {
        selectedArtifact = artifact;
        confirmationArtifactName.text = artifact.artifactName;
        confirmationArtifactCost.text = "for " + artifact.GetCostToUpgrade().ToString() + " points";
        if(artifact.GetCostToUpgrade() > FindObjectOfType<PlayerArtifacts>().artifactPoints)
        {
            confirmationAccept.interactable = false;
        }else
        {
            confirmationAccept.interactable = true;
        }
        confirmationBox.SetActive(true);
        
    }

    public void ConfirmPurchase()
    {
        FindObjectOfType<PlayerArtifacts>().PurchaseArtifact(selectedArtifact);
    }

    public void CancelPurchase()
    {

    }
}
