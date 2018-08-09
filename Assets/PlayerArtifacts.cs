using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArtifactChangedEvent : UnityEvent<Artifact>
{

}

public class PlayerArtifacts : MonoBehaviour {

    public ArtifactChangedEvent artifactsChangedEvent { get; private set; }

    public const int ARTIFACT_MAX_LEVEL = 5;
    public int artifactPoints = 0;

    public GameObject artifactHolder;

    public void Awake()
    {
        artifactsChangedEvent = new ArtifactChangedEvent();
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
}
