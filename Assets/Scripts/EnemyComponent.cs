using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ComponentHitEvent : UnityEvent<int>
{

}

public class EnemyComponent : MonoBehaviour {

    // Use this for initialization

    public ComponentHitEvent componentHitEvent;
    public int childIndex = 0;
    public GameObject baseObject = null;

    void Awake()
    {
        componentHitEvent = new ComponentHitEvent();
        childIndex = transform.GetSiblingIndex();
        if(baseObject == null)
        {
            baseObject = transform.parent.gameObject;
        }
    }

    public void OnHit(int damage)
    {
        componentHitEvent.Invoke(damage);
    }
}
