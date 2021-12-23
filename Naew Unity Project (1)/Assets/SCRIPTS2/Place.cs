using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Place : MonoBehaviour
{
 
    [SerializeField] private List<Connection> connections;

    public List<Connection> Connections => connections;


    private void OnMouseUp()
    {
        Debug.Log("Clicked A Place");
        GameManager.Instance.Click(this);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        foreach (var p in connections)
        {
            if (p.Destination == null)
            {
                continue;
            }

            var gizmoPathColor = Color.yellow;
            if (p.Type == ConnectionType.RED)
            {
                gizmoPathColor = Color.red;
            }
            if (p.Type == ConnectionType.GREEN)
            {
                gizmoPathColor = Color.green;
            }
            else if (p.Type == ConnectionType.BLUE)
            {
                gizmoPathColor = Color.blue;
            }
            Gizmos.color = gizmoPathColor;
            Gizmos.DrawLine(transform.position, p.Destination.transform.position);
        }
    }
    
    private void Awake()
    {
        foreach (var p in connections)
        {
            List<Connection> pConnections = p.Destination.Connections;
            if (!pConnections.Exists(c =>c.Destination == this))
            {
                pConnections.Add(new Connection(this, p.Type));
            }
        }
    }
}
