using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Stores the state of an individual artifact
[System.Serializable]
public class ArtifactState
{
    public string artifactType;
    public int level;
}

public class Artifact : MonoBehaviour {

    protected Player player;

    public string artifactName;
    public string description;
    bool isOwned;

    public int level = 0;
    public float[] effectStrength;

    int[] costToUpgrade;

    public virtual void Awake()
    {
        player = FindObjectOfType<Player>();
        costToUpgrade = new int[] { 100, 300, 750, 1500, 3000};
    }
     
    protected virtual void Activate()
    {
        isOwned = true;
    }
    

    protected virtual void Deactivate()
    {
        isOwned = false;
    }
    
    public int GetCostToUpgrade()
    {
        return costToUpgrade[level];
    }

    public virtual string GetEffectText(int level)
    {
        if(level == 0)
        {
            return "0";
        }else
        {
            return effectStrength[level - 1].ToString();
        }
    }

    public void IncreaseLevel()
    {
        //Deactivating then reactivating is easier than calculating the differences independantly for each artifact
        //However, we do not want to deactivate if it is our first time purchasing the item (going from lvl 0-1)
        if(level > 0)
        {
            Deactivate();
        }
        level++;
        Activate();
    }

    public bool GetOwned()
    {
        return isOwned;
    }


    public ArtifactState SaveArtifact()
    {
        ArtifactState ad = new ArtifactState();
        ad.artifactType = GetType().ToString();
        ad.level = level;

        return ad;
    }

    public void LoadArtifact(ArtifactState aState)
    {
        //Need checks on type here? Checks should be performed beforehand but look here if it causes issues

        level = aState.level;

        if(level > 0)
        {
            isOwned = true;
            Activate();
        }
    }
}
