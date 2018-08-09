using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour {

    public string WindowName;

    public void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ShowWindow);
    }

    public void ShowWindow()
    {

    }
}
