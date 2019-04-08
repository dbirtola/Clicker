using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour {

    public Sprite sprite;

    //Temp until art comes in. This is the text displayed on the icon
    public string buffText;


    public virtual void Awake()
    {

    }

    public virtual void ActivateBuff(Unit target, int duration)
    {

    }
    

    protected IEnumerator ExpireAfterTime(Unit target, int duration)
    {
        yield return new WaitForSeconds(duration);
        DeactivateBuff(target);
    }

    public virtual void DeactivateBuff(Unit target)
    {

    }
}
