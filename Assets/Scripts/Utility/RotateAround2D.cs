using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround2D : MonoBehaviour {

    public GameObject target;
    float horizontalRadius = 0.1f;
    float verticalRadius = 0.05f;

    //float speed = 1;
   
   // bool goingRight = true;
    

    void Start()
    {
        target = transform.parent.gameObject;
    }

    void Update()
    {

        float targetX = horizontalRadius * Mathf.Cos(Time.time);
        float targetY = verticalRadius * Mathf.Sin( Time.time);

        if(targetX < transform.localPosition.x)
        {
            GetComponent<SpriteRenderer>().sortingOrder = target.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }else
        {
            GetComponent<SpriteRenderer>().sortingOrder = target.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }

        transform.localPosition = new Vector3(targetX, targetY, 0);

        /*
        if (goingRight)
        {
            targetX = target.transform.position.x + horizontalRadius;
        }else
        {
            targetX = target.transform.position.x - horizontalRadius;
        }

        if(goingRight && transform.position.x < 0)
        {
            targetY = target.transform.position.x +
        }
        */
    }
}
