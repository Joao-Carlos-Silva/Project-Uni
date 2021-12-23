using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Item
{
    [SerializeField] private Place position;
    [SerializeField] private ItemType type;

    public Place Position => position;

    public ItemType Type => type;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
