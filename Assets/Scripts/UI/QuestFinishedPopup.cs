﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestFinishedPopup : MonoBehaviour {


    public float slideSpeed = 25f;
    public float holdDuration = 3;

    public Text questNameText;

    public Queue<string> questQueue;
    public void Awake()
    {
        questQueue = new Queue<string>();
    }

    public void Show(string questName)
    {
        questNameText.text = questName;
        gameObject.SetActive(true);

        if(questQueue.Count == 0)
        {
            StartCoroutine(scrollRight());
        }else
        {
            questQueue.Enqueue(questName);
        }
    }
    

    IEnumerator scrollRight()
    {
        RectTransform rt = GetComponent<RectTransform>();

        Vector2 step = new Vector2(1, 0) * slideSpeed;
        while (rt.offsetMax.x + step.x < 0)
        {
            rt.offsetMax = rt.offsetMax + step * Time.time;
            yield return null;
        }
        rt.offsetMax = new Vector2(0, rt.offsetMax.y);

        yield return new WaitForSeconds(holdDuration);
        
        
        while (rt.offsetMax.x + step.x > -375)
        {
            rt.offsetMax = rt.offsetMax - step * Time.time;
            yield return null;
        }
        rt.offsetMax = new Vector2(-375, rt.offsetMax.y);

        if(questQueue.Count == 0)
        {
            gameObject.SetActive(false);
        }else
        {
            questNameText.text = questQueue.Dequeue();
            StartCoroutine(scrollRight());
        }
    }

}
