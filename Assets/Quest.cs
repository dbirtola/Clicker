using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestStartedEvent : UnityEvent<Quest>
{

}

public class QuestFinishedEvent : UnityEvent<Quest>
{

}

public enum QuestCategory
{
    Beginner,
    Milestone,
    Progression
}

public class Quest : MonoBehaviour {

    protected Player playerController;

    public QuestStartedEvent questStartedEvent { get; private set; }
    public QuestFinishedEvent questFinishedEvent { get; private set; }


    public string questName = "Kill Monster";
    public string description;

    public int experienceReward = 0;

    bool completed = false;
    
    public virtual void Start()
    {
        questStartedEvent = new QuestStartedEvent();
        questFinishedEvent = new QuestFinishedEvent();

        playerController = FindObjectOfType<Player>();


        //For resting leave this in start
        StartQuest();
    }

    public virtual void StartQuest()
    {
        questStartedEvent.Invoke(this);
    }

    public virtual void FinishQuest()
    {
        completed = true;
        questFinishedEvent.Invoke(this);
        Debug.Log("Finished " + questName +")");

        this.enabled = false;
    }

    public virtual string GetRewardText()
    {
        return "No Reward";
    }

}
