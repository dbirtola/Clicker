using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropNotificationPopup : MonoBehaviour {

    public float slideSpeed = 25f;
    public float holdDuration = 3;

    public Text itemNameText;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void Show(Equipment item)
    {
        itemNameText.text = "Found : " + item.itemName;
        StartCoroutine(scrollLeft(GetComponent<RectTransform>()));
    }

    IEnumerator scrollLeft(RectTransform rt)
    {
        //RectTransform rt = GetComponent<RectTransform>();

        Vector2 step = new Vector2(1, 0) * slideSpeed;
     
        while (rt.offsetMin.x - step.x > 150)
        {
            rt.offsetMin = rt.offsetMin - step * Time.time;
            yield return null;
        }
        rt.offsetMin = new Vector2(150, rt.offsetMin.y);

        yield return new WaitForSeconds(holdDuration);


        while (rt.offsetMin.x - step.x < -375)
        {
            rt.offsetMin = rt.offsetMin + step * Time.time;
            yield return null;
        }
        rt.offsetMax = new Vector2(-375, rt.offsetMin.y);

        
    }
}
