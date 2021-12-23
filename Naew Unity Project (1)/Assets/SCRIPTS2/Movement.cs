using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement
{
    private Place origin;

    private Place destination;

    private Pawn pawnToMove;

    public Place Origin => origin;

    public Place Destination => destination;

    public Pawn PawnToMove => pawnToMove;

    public Movement(Place placeOrigin, Place placeDestination, Pawn pawn)
    {
        origin = placeOrigin;
        destination = placeDestination;
        pawnToMove = pawn;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
