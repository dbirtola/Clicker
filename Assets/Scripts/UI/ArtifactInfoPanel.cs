using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactInfoPanel : MonoBehaviour {


    public Text artifactName;
    public Text artifactDescription;
    public Text artifactCurrentEffect;
    public Text artifactNextEffect;
    public Text artifactCostText;

    public void UpdateWithArtifact(Artifact artifact)
    {
        artifactName.text = artifact.artifactName + " Lv. " + artifact.level;
        artifactDescription.text = artifact.description;
        artifactCurrentEffect.text = artifact.GetEffectText(artifact.level);
        artifactNextEffect.text = artifact.GetEffectText(artifact.level + 1);
        artifactCostText.text = "Cost: " + artifact.GetCostToUpgrade();

        var button = GetComponent<Button>();
        button.onClick.AddListener(() => { FindObjectOfType<ArtifactsPanel>().ArtifactSelected(artifact); });
    }


}
