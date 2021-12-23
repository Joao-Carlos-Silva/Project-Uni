using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameState
{
    [SerializeField] private List<Item> items; 
    public List<Item> Items => items;
    //[SerializeField] private Pawn[] pawns;
    [SerializeField] private List<Pawn> pawns;
    // Start is called before the first frame update
    //public Pawn[] Pawns => pawns;
    public List<Pawn> Pawns => pawns;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameState DeepCopy()
    {
        GameState copy = new GameState();
        // copy.pawns = new Pawn[this.pawns.Length];
        // for (int i = 0; i < this.pawns.Length; i++)
        // {
        //     copy.pawns[i] = this.pawns[i].DeepCopy();
        // }
        copy.items = new List<Item>(this.items);
        copy.pawns = new List<Pawn>();
        for (var i = 0; i < this.pawns.Count; i++)
        {
            copy.pawns.Add(this.pawns[i].DeepCopy()); //copy.pawns = this.pawns.ConvertAll(p => p.DeepCopy()); Alternativa 
        }
        return copy;
    }
}
