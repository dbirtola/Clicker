using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RectTransformSlider : MonoBehaviour {

    public ExpandDirection direction;

    Vector2 defaultOffsetMin;
   // Vector2 defaultOffsetMax;

    //bool expanded;

    public Vector2 offsetMin { set
        {
            GetComponent<RectTransform>().offsetMin = this.offsetMin;
        }
        get
        {
            return offsetMin;
        }

    }

    public Vector2 offsetMax
    {
        set
        {
            GetComponent<RectTransform>().offsetMax = this.offsetMax;
        }
        get
        {
            return offsetMax;
        }

    }

    void Awake()
    {
        defaultOffsetMin = GetComponent<RectTransform>().offsetMin;
       // defaultOffsetMax = GetComponent<RectTransform>().offsetMax;
    }


    public void Expand()
    {
        
        switch (direction)
        {
            case ExpandDirection.BottomToTop:

                break;
            case ExpandDirection.TopToBottom:
                break;
            case ExpandDirection.LeftToRight:
                gameObject.SetActive(true);
                StartCoroutine(ExpandLeftToRight(GetComponent<RectTransform>(), 1800));
                break;
            case ExpandDirection.RightToLeft:
                gameObject.SetActive(true);
                StartCoroutine(ExpandRightToLeft(GetComponent<RectTransform>(), 1800));
                break;
        }

        //if(direction == ExpandDirection.BottomToTop || direction ==)
    }

    IEnumerator ExpandRightToLeft(RectTransform rectTransform, float speed)
    {


        Debug.Log("Expanding!!");
        

        Vector2 step = new Vector2(1, 0) * speed * Time.deltaTime * -1;

        
        while (rectTransform.offsetMin.x + step.x > 0)
        {
            rectTransform.offsetMin = rectTransform.offsetMin + step;
            yield return null;

        }

        rectTransform.offsetMin = new Vector2(0, rectTransform.offsetMin.y);



        // yield return null;


        while (rectTransform.offsetMin.x < defaultOffsetMin.x)
        {
            rectTransform.offsetMin = rectTransform.offsetMin - new Vector2(1, 0) * speed * Time.deltaTime * -1;

            yield return null;
        }


        yield return null;



        gameObject.SetActive(false);

        rectTransform.offsetMin = defaultOffsetMin;
       // expanded = true;
       
    }

    IEnumerator ExpandLeftToRight(RectTransform rectTransform, float speed)
    {


        Debug.Log("Expanding!!");


        Vector2 step = new Vector2(1, 0) * speed * Time.deltaTime;

        Debug.Log("Moving lower left to: " + Display.main.renderingWidth * rectTransform.anchorMax.x);
        while (rectTransform.offsetMax.x + step.x > rectTransform.anchorMax.x * Display.main.renderingWidth)
        {
            rectTransform.offsetMin = rectTransform.offsetMin + step;
            yield return null;

        }

        rectTransform.offsetMin = new Vector2(0, rectTransform.offsetMin.y);



        // yield return null;


        while (rectTransform.offsetMin.x > defaultOffsetMin.x)
        {
            rectTransform.offsetMin = rectTransform.offsetMin + new Vector2(1, 0) * speed * Time.deltaTime * -1;

            yield return null;
        }


        yield return null;



        gameObject.SetActive(false);

        rectTransform.offsetMin = defaultOffsetMin;
       // expanded = true;

    }
}
