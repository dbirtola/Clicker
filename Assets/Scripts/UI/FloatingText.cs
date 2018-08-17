using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText: MonoBehaviour {

    float fadeSpeed = 1;
    float floatSpeed = 60;

    Text text;

	// Use this for initialization
	void Awake () {
        text = GetComponent<Text>();
	}
	
	public void Float(Vector2 position, string txt)
    {
        Vector2 offset = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
        transform.position = position + offset;
        //eww
        if(text == null)
        {
            text = GetComponent<Text>();
        }
        text.text = txt;
        StartCoroutine(FloatAndFade());

    }

    IEnumerator FloatAndFade()
    {
        
        while(text.color.a > 0)
        {
            //fade
            Color c = text.color;
            c.a -= fadeSpeed * Time.deltaTime;
            text.color = c;

            //float
            transform.position = new Vector2(transform.position.x, transform.position.y + floatSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject);
    }
}
