using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pawn
{
    [SerializeField] private ConnectionType color;
    //[SerializeField] private string rgb;
    [SerializeField] private Place currentPlace;


    public ConnectionType Color => color;
    public Place CurrentPlace
    {
        get => currentPlace;
        set => currentPlace = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Pawn DeepCopy()
    {
        Pawn copy = new Pawn();
        copy.currentPlace = this.currentPlace;
        copy.color = this.color;
        return copy;
    }
}
