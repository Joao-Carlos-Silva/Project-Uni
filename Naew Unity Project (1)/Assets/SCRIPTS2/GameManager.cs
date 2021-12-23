using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    private readonly Vector3 itemSize = new Vector3(0.5f, 1.0f, 0.5f);
    [SerializeField] private GameState currentGame;
    [SerializeField] private PawnInterface[] pawns;
    private RandomPlayer _randomPlayer = new RandomPlayer();
    private PawnInterface _selectedPawn;
    private Place[] places;
    private Stack<GameState> previousStates;
    public static GameManager Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        previousStates = new Stack<GameState>();
        places = FindObjectsOfType<Place>();
        
        // for (var i = 0; i < pawns.Length; i++)
        // {
        //     pawns[i].GamePawn = currentGame.Pawns[i];
        //     pawns[i].transform.position =
        //         currentGame.Pawns[i].CurrentPlace.transform.position;
        // }
        UpdateAllPawnInterfaces();
        if (Instance != null)
        {
            Debug.LogWarning("Multiple Game Managers");
        }

        Instance = this;
    }

    public void UpdateAllPawnInterfaces()
    {
        for (var i = 0; i < pawns.Length; i++)
        {
            pawns[i].GamePawn = currentGame.Pawns[i];
            pawns[i].transform.position =
                currentGame.Pawns[i].CurrentPlace.transform.position;
        }
    }

    public void Click(PawnInterface pawn)
    {
        if (pawn == _selectedPawn)
        {
            _selectedPawn = null;
        }
        else
        {
            _selectedPawn = pawn;
        }
    }

    private void OnDrawGizmos()
    {
        if (_selectedPawn != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_selectedPawn.transform.position, 1.0f);
        }

        foreach (var i in currentGame.Items)
        {
            if (i.Position != null)
            {
                Gizmos.color = i.Type == ItemType.BLACK ? Color.black : Color.white;
                Gizmos.DrawCube(i.Position.gameObject.transform.position,itemSize);
            }

            if (i.Type == ItemType.BLACK)
            {
                Gizmos.color = Color.black;
            }
            else
            {
                Gizmos.color = Color.white;
            }
        }
    }

    public void Click(Place place)
    {
        if (_selectedPawn == null)
        {
            return;
        }

        if (CanMove(_selectedPawn.GamePawn, place))
        {
            Move(_selectedPawn.GamePawn, place);
            UpdatedPawnInterface(_selectedPawn);
            _selectedPawn = null;
        }
        
        
    }

    public bool CanMove(Pawn pawn, Place place)
    {
        foreach (var p in  currentGame.Pawns)
        {
            if (p.CurrentPlace == place)
            {
                return false;
            }
        }
        List<Connection> placeConnections = pawn.CurrentPlace.Connections;
        foreach (var c in placeConnections)
        {
            if (c.Destination != place) continue;
            if (c.Type == ConnectionType.BLUE || c.Type == pawn.Color)
            {
                return true;
            }
            
                
            
        }

        return false;
        //  return pawn.CurrentPlace.Connections.Contains(place) ||
        //  place.Connections.Contains(pawn.CurrentPlace);
    }

    public void Move(Pawn pawn, Place place)
    {
        previousStates.Push(currentGame.DeepCopy());
        pawn.CurrentPlace = place;
        for (var i=currentGame.Items.Count-1; i>=0; i-- )
        {
            if (currentGame.Items[i].Position == place) //currentGame.Items.RemoveAll(i=> i.Position == place); substitui o ciclo for
            {
                currentGame.Items.RemoveAt(i);
            }
        }

        if (currentGame.Items.Count == 0)
        {
            Debug.Log("game Ended");
        }
    }
// Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Z) && previousStates.Count > 0)
        {
            currentGame = previousStates.Pop();
            UpdateAllPawnInterfaces();
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            _randomPlayer.RandomMove(currentGame);
            UpdateInterfacePawns();
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            string json = JsonUtility.ToJson(currentGame);
            File.WriteAllText("save.json", json);
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            previousStates.Clear();
            string json = File.ReadAllText("save.json");
            currentGame = JsonUtility.FromJson<GameState>(json);
            UpdateAllPawnInterfaces();
        }

    }
    public void UpdatedPawnInterface(PawnInterface pawn)
    {
        pawn.transform.position = pawn.GamePawn.CurrentPlace.transform.position;
    }
    private void UpdateInterfacePawns()
    {
        foreach (var p in pawns)
        {
            p.transform.position = p.GamePawn.CurrentPlace.transform.position;
        }
    }
    
    public Movement[] GetAllMovements()
    {
        
        List<Movement> allMovements = new List<Movement>();
        foreach (var p in currentGame.Pawns)
        {
            foreach (var c in p.CurrentPlace.Connections)
            {
                var m = new Movement(p.CurrentPlace, c.Destination, p);
                if (CanMove(p, c.Destination))
                {
                    allMovements.Add(m);
                }
                
            }
        }
        return allMovements.ToArray();
    }


    
    
    
}
