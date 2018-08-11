using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Events;




[System.Serializable]
public class DevSettingsData
{
    public bool resetPersistentData;
    public bool saveOnQuit;
}

//All data that wants to be saved needs to be stored in a Serializable class
//For organization, each major player component gets its own serializable class
//Which is responsible for saving and restoring the state. The PersistanceManager
//takes all those major classes and read/writes them to a binary file from which they can
//all be restored and loaded at the appropriate time.

[System.Serializable]
public class PlayerData
{
    public List<QuestState> questStates;
    public CraftingData craftingData;
    public ArtifactData artifactData;
    public AbilityData abilityData;
    public PlayerStatData playerStatData;
    public PlayerInventoryData playerInventoryData;
    
}


//This class has lowest priority, so it can load on start while being sure that all other objects
//Have had the time to run their start methods and set themselves up properly before loading

public class PersistanceManager : MonoBehaviour {

    public UnityEvent persistantDataLoadedEvent;
    public UnityEvent devDataLoadedEvent;

    PlayerQuests playerQuests;
    PlayerCrafting playerCrafting;
    PlayerArtifacts playerArtifacts;
    PlayerAbilities playerAbilities;
    PlayerStats playerStats;
    PlayerInventory playerInventory;

    public bool loadOnStart = true;
    public bool saveOnQuit = true;
    public bool resetPersistantData = false;

    //Temporary field so I can test and view in inspector;
    public PlayerData playerData;

    void Awake()
    {
        persistantDataLoadedEvent = new UnityEvent();
        devDataLoadedEvent = new UnityEvent();

        playerQuests = GetComponent<PlayerQuests>();
        playerCrafting = GetComponent<PlayerCrafting>();
        playerArtifacts = GetComponent<PlayerArtifacts>();
        playerAbilities = GetComponent<PlayerAbilities>();
        playerStats = GetComponent<PlayerStats>();
        playerInventory = GetComponent<PlayerInventory>();
    }


    //Should always be the last object to run Start().
    void Start()
    {
        LoadDev();

        if(loadOnStart == true)
        {
            if (File.Exists(Application.persistentDataPath + "/playerData.dat") && !resetPersistantData)
            {
                LoadGame();
            }
            else
            {
                //Hacky way to not have to initialize all the values for everything.
                SaveGame();
                LoadGame();
            }
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SaveGame();
        }
    }

    public DevSettingsData SavePlayerSettings() 
    {
        DevSettingsData psd = new DevSettingsData();

        psd.resetPersistentData = resetPersistantData;
        psd.saveOnQuit = saveOnQuit;

        return psd;
    }


    public void LoadPlayerSettings(DevSettingsData psd)
    {
        resetPersistantData = psd.resetPersistentData;
        saveOnQuit = psd.saveOnQuit;
    }

    public void SaveDev()
    {
        DevSettingsData devSettingsData = new DevSettingsData();
        devSettingsData.resetPersistentData = resetPersistantData;
        devSettingsData.saveOnQuit = saveOnQuit;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/devData.dat", FileMode.Create);
        Debug.Log("Saved dev data to: " + Application.persistentDataPath + "/devData.dat");
        bf.Serialize(file, devSettingsData);
        file.Close();
    }

    public void LoadDev()
    {
        if (File.Exists(Application.persistentDataPath + "/devData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/devData.dat", FileMode.Open);
            var devData = (DevSettingsData)bf.Deserialize(file);
            file.Close();

            resetPersistantData = devData.resetPersistentData;
            saveOnQuit = devData.saveOnQuit;
        }

        devDataLoadedEvent.Invoke();
    }

    public void SaveGame()
    {
        playerData = new PlayerData();

        playerData.questStates = playerQuests.SaveQuestState();
        playerData.craftingData = playerCrafting.SaveCraftingState();
        playerData.artifactData = playerArtifacts.SaveArtifacts();
        playerData.abilityData = playerAbilities.SaveAbilityData();
        playerData.playerStatData = playerStats.SaveStats();
        playerData.playerInventoryData = playerInventory.SaveInventoryData();


        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Create);
        Debug.Log("Saved to: " + Application.persistentDataPath + "/playerData.dat");
        bf.Serialize(file, playerData);
        file.Close();
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);
            playerData = (PlayerData)bf.Deserialize(file);
            file.Close();

            playerQuests.LoadQuestState(playerData.questStates);
            playerCrafting.LoadCraftingState(playerData.craftingData);
            playerArtifacts.LoadArtifacts(playerData.artifactData);
            playerAbilities.LoadAbilityData(playerData.abilityData);
            playerStats.LoadStats(playerData.playerStatData);
            playerInventory.LoadInventoryData(playerData.playerInventoryData);

        }

        persistantDataLoadedEvent.Invoke();
    }

    void OnApplicationQuit()
    {
        SaveDev();
        if (saveOnQuit)
        {
            SaveGame();
        }
    }
}
