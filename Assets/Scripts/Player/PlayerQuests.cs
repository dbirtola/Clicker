using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerQuests : MonoBehaviour {

    public QuestFinishedEvent questFinishedEvent { get; private set; }

    public UnityEvent endlessTowerUnlockedEvent;

    public GameObject questObject;
    public Quest[] quests;

    bool endlessTowerUnlocked = false;

    //Temporary list for testing presistance
    public List<QuestState> questStates;

    void Awake()
    {
        questFinishedEvent = new QuestFinishedEvent();
        endlessTowerUnlockedEvent = new UnityEvent();

        quests = questObject.GetComponents<Quest>();


    }



    void Start()
    {
        foreach(Quest q in quests)
        {
            q.questFinishedEvent.AddListener(questFinishedEvent.Invoke);
        }


        GetComponent<PersistanceManager>().persistantDataLoadedEvent.AddListener(() =>
        {
            if (GetComponent<PlayerStats>().GetBaseStatStruct().level >= 20)
            {
                UnlockEndlessTower();

            }
            else
            {
                GetComponent<PlayerStats>().levelUpEvent.AddListener((int lvl) =>
                {
                    if (lvl >= 20)
                    {
                        UnlockEndlessTower();
                    }
                });
            }
        });
    }


    void UnlockEndlessTower()
    {
        endlessTowerUnlocked = true;
        endlessTowerUnlockedEvent.Invoke();

    }


    public void LoadQuestState(List<QuestState> loadStates)
    {
        questStates = loadStates;

        //Not the best performance, but need to search for the correct quest based on type, since
        //the quests type is the least likely thing to change between updates of the game.

        foreach(Quest q in quests)
        {
            foreach(QuestState qs in questStates)
            {
                if(q.GetType() == System.Type.GetType(qs.questType))
                {
                    //Quest matches the type we were looking for so load it with the questState data
                    q.LoadQuestState(qs);
                }
            }
        }
    }

    //Returns a serializable list of quest states from which it can recreate the state of all quests when given the list back
    public List<QuestState> SaveQuestState()
    {
        questStates = new List<QuestState>();
        foreach(Quest q in quests)
        {
            questStates.Add(q.SaveQuestState());
        }

        return questStates;
    }
}
