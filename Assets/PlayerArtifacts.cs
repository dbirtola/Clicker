using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class ArtifactChangedEvent : UnityEvent<Artifact>
{

}

[System.Serializable]
public class ArtifactData
{
    public int artifactPoints;
    public List<ArtifactState> artifactStates;
}

public class PlayerArtifacts : MonoBehaviour {

    public ArtifactChangedEvent artifactsChangedEvent { get; private set; }

    public const int ARTIFACT_MAX_LEVEL = 5;
    public int artifactPoints = 0;

    public GameObject artifactHolder;
    public Artifact[] artifacts;

    public void Awake()
    {
        artifactsChangedEvent = new ArtifactChangedEvent();
        artifacts = artifactHolder.GetComponents<Artifact>();
    }


    public bool PurchaseArtifact(Artifact artifact)
    {
        if (artifact.level == ARTIFACT_MAX_LEVEL)
        {
            return false;
        }

        Debug.Log("Purchasing " + artifact.artifactName);
        artifact.IncreaseLevel();
        artifactsChangedEvent.Invoke(artifact);

        return true;
    }

    public ArtifactData SaveArtifacts()
    {
        ArtifactData artifactData = new ArtifactData();

        artifactData.artifactPoints = artifactPoints;

        //Populate the save data with each artifact that exists
        List<ArtifactState> artifactStates = new List<ArtifactState>();
        foreach (Artifact a in artifacts)
        {
            artifactStates.Add(a.SaveArtifact());
        }

        artifactData.artifactStates = artifactStates;

        return artifactData;
    }

    public void LoadArtifacts(ArtifactData artifactData)
    {
        artifactPoints = artifactData.artifactPoints;

        foreach(ArtifactState aState in artifactData.artifactStates)
        {
            foreach (Artifact a in artifacts)
            {
                if(aState.artifactType == a.GetType().ToString())
                {
                    a.LoadArtifact(aState);
                }
            }
        }
    }
}
