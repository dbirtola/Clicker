using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PositionSquareLocation
{
    left,
    center,
    right
}

public class PositionSquare : MonoBehaviour {

    public PositionSquareLocation location;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().material.color = color;
    }
}
