using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Connection
{
    [SerializeField] public Place destination;

    [SerializeField] private ConnectionType type;

    public Place Destination => destination;

    public ConnectionType Type => type;

    public Connection(Place destination, ConnectionType type)
    {
        this.destination = destination;
        this.type = type;
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
