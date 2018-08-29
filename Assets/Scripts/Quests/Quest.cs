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
    Repeatable,
    Progression
}

[System.Serializable]
public class QuestState
{
    public string questType;
    public bool completed;
    public bool rewardCollected;
    public string questState;
}

public class Quest : MonoBehaviour {

    protected Player playerController;

    public QuestStartedEvent questStartedEvent { get; private set; }
    public QuestFinishedEvent questFinishedEvent { get; private set; }
    
    public string questName = "Kill Monster";
    public string description;

    public int experienceReward = 0;

    protected bool completed = false;
    protected bool rewardCollected = false;
    
    public virtual void Awake()
    {
        questStartedEvent = new QuestStartedEvent();
        questFinishedEvent = new QuestFinishedEvent();

        playerController = FindObjectOfType<Player>();


        //For resting leave this in start
        //StartQuest();
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

    //Returns a string that the quest can interpret to return itself to the proper state
    public virtual QuestState SaveQuestState()
    {
        QuestState qs = new QuestState();
        qs.questType = GetType().ToString();
        qs.completed = completed;
        qs.rewardCollected = rewardCollected;

        return qs;
    }

    //Returns the quest to its previous state based on the string created from SaveQuestState
    public virtual void LoadQuestState(QuestState questState)
    {
        //No need to set the type, the type is just a flag to find the correct quest
        completed = questState.completed;
        rewardCollected = questState.rewardCollected;

    }

    public virtual string GetQuestName()
    {
        return questName;
    }

}
