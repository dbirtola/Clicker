using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ExpandDirection
{
    RightToLeft,
    LeftToRight
}

public class HelpPanel : MonoBehaviour {

    RectTransform rectTransform;
    public float downExtent;
    public float horizontalExpandSpeed;
    public float verticalExpandSpeed;

    public ExpandDirection expandDirection;

    public Vector3 defaultOffsetMin;

    bool expanded = false;

    public GameObject[] controlsActive;

    public void Expand()
    {
       
        if (expanded)
        {
            StartCoroutine(Retract());
        }else
        {
            StartCoroutine(ExpandRoutine());
        }
        
    }


    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        defaultOffsetMin = rectTransform.offsetMin;
    }


    IEnumerator ExpandRoutine()
    {
        foreach(GameObject go in controlsActive)
        {
            go.SetActive(true);
        }

        if (expandDirection == ExpandDirection.RightToLeft)
        {

            Debug.Log("Expanding!!");
            bool doneLeft = false;
            bool doneDown = false;

            float deltaX = rectTransform.offsetMin.x;
            float deltaY = rectTransform.offsetMin.y - downExtent;

            while(doneLeft == false)// || doneDown == false)
            {
                
                Vector2 step = new Vector2(1, 0) * horizontalExpandSpeed * Time.deltaTime * -1;
                if (rectTransform.offsetMin.x + step.x > 0)
                {
                    rectTransform.offsetMin = rectTransform.offsetMin + step;
                    
                }else
                {
                    doneLeft = true;
                }


                yield return null;

            }

            rectTransform.offsetMin = new Vector2(0, rectTransform.offsetMin.y);

            while (doneDown == false)
            {
                //By checking how far we would travel each frame, we snap straight TO the coordinates instead of passing them then snapping BACK.
                //This has the slight drawback of being a few pixels off since it relies on Time.deltaTime.
                Vector2 step = new Vector2(0, 1) * verticalExpandSpeed * Time.deltaTime * -1;

                if (rectTransform.offsetMin.y  + step.y> downExtent)
                {
                    rectTransform.offsetMin = rectTransform.offsetMin + step;

                }
                else
                {
                    doneDown = true;
                    break;
                }


                yield return null;
            }


            rectTransform.offsetMin = new Vector2(0, downExtent);
            expanded = true;
        }
    }

    IEnumerator Retract()
    {


        if (expandDirection == ExpandDirection.RightToLeft)
        {

            Debug.Log("Expanding!!");
            bool doneLeft = false;
            bool doneDown = false;

            float deltaX = rectTransform.offsetMin.x;
            float deltaY = rectTransform.offsetMin.y - downExtent;

            while (doneDown == false)
            {

                if (rectTransform.offsetMin.y < defaultOffsetMin.y)
                {
                    rectTransform.offsetMin = rectTransform.offsetMin - new Vector2(0, 1) * verticalExpandSpeed * Time.deltaTime * -1;

                }
                else
                {
                    doneDown = true;
                    break;
                }


                yield return null;
            }

            while (doneLeft == false)// || doneDown == false)
            {

                if (rectTransform.offsetMin.x < defaultOffsetMin.x)
                {
                    rectTransform.offsetMin = rectTransform.offsetMin - new Vector2(1, 0) * horizontalExpandSpeed * Time.deltaTime * -1;

                }
                else
                {
                    doneLeft = true;
                }


                yield return null;

            }

            foreach (GameObject go in controlsActive)
            {
                go.SetActive(false);
            }

            rectTransform.offsetMin = defaultOffsetMin;
            expanded = false;

            
        }

    }

}
