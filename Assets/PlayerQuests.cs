using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerQuests : MonoBehaviour {

    public QuestFinishedEvent questFinishedEvent { get; private set; }

    public GameObject questObject;
    public Quest[] quests;

    void Awake()
    {
        questFinishedEvent = new QuestFinishedEvent();
        quests = questObject.GetComponents<Quest>();
    }

    void Start()
    {
        foreach(Quest q in quests)
        {
            q.questFinishedEvent.AddListener(questFinishedEvent.Invoke);
        }
    }

    
}
