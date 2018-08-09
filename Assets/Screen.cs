using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeftViewEvent : UnityEvent
{

}

public class Screen : MonoBehaviour {

    protected LeftViewEvent leftViewEvent;

    public virtual void Awake()
    {
        leftViewEvent = new LeftViewEvent();
    }
}
