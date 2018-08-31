using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BuffEvent : UnityEvent<Buff>
{

}



public class Buffs : MonoBehaviour {



    public BuffEvent buffAddedEvent { get; private set; }
    public BuffEvent buffRemovedEvent { get; private set; }
    public List<Buff> activeBuffs;
    
    public void Awake()
    {
        buffAddedEvent = new BuffEvent();
        buffRemovedEvent = new BuffEvent();

        activeBuffs = new List<Buff>();
    }	


    public T AddBuff<T>() where T : Buff
    {
        T newBuff = gameObject.AddComponent<T>();
        activeBuffs.Add(newBuff);

        buffAddedEvent.Invoke(newBuff);

        return newBuff;
    }

    public void RemoveBuff(Buff buff)
    {
        activeBuffs.Remove(buff);
        buffRemovedEvent.Invoke(buff);

    }

}
