using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switcher : MonoBehaviour {

    public GameObject[] panels;
    public Button[] switcherButtons;
    public int activeIndex = 0;

	
    void Awake()
    {

    }

    public void SetPanelActive(int index)
    {
        for(int i = 0; i < panels.Length; i++)
        {
            if(i == index)
            {
                panels[i].SetActive(true);
            }else
            {
                panels[i].SetActive(false);
            }
        }
    }
}
