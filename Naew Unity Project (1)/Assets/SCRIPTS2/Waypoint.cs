using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint[] edges;
    public Color color;
    public bool door = false;
    public bool key = false;

    private void Start ()
    {
        var renderer = this.GetComponent<Renderer>();
        renderer.material.SetColor("_Color", color);
        if (door)
        {
            renderer.material.EnableKeyword("_EMISSION");
            renderer.material.SetColor("_EmissionColor", Color.yellow * 0.3f);
        }
        else if (key)
        {
            renderer.material.EnableKeyword("_METALLICGLOSSMAP");
            renderer.material.SetFloat("_Methalic", 0.5f);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (Waypoint e in edges)
        {
            Gizmos.DrawLine(transform.position, e.gameObject.transform.position);
        }
    }
}
