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

    public List<PlayerPawn> pawnsOnSquare;

    public PositionSquareLocation location;

    public PositionSquare squareToLeft;
    public PositionSquare squareToRight;

    void Awake()
    {
        pawnsOnSquare = new List<PlayerPawn>();
    }
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().material.color = color;
    }

    public PositionSquare[] GetAdjacentSquares()
    {
        if (squareToLeft == null)
        {
            return new PositionSquare[] { squareToRight };
        }

        if (squareToRight == null)
        {
            return new PositionSquare[] { squareToLeft };

     }
        
        return new PositionSquare[]{squareToLeft, squareToRight };

    }
}
