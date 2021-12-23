using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinder : MonoBehaviour
{
    public Waypoint initialState;
    public Waypoint goalState;
    public GameObject npc;
    public GameObject helper;
    public float speed;
    private List<Node> path;
    private int position;
    private void Start() 
    {
        path = FindPath(initialState, goalState);
        position = 0;
        string pList = "Path: ";
        foreach (Node n in path)
        {
            pList += " " + n.npc.gameObject.name + "-" + n.helper.gameObject.name;
        }
        Debug.Log(pList);
    }
    private void Update() 
    {
        if (path.Count > position)
        {
            float step = speed * Time.deltaTime;
            npc.gameObject.transform.position = Vector3.MoveTowards(
            npc.gameObject.transform.position,
            path[position].npc.gameObject.transform.position, step);
            helper.gameObject.transform.position = Vector3.MoveTowards(
            helper.gameObject.transform.position,
            path[position].helper.gameObject.transform.position, step);
            if (npc.gameObject.transform.position.Equals(
            path[position].npc.gameObject.transform.position) &&
            helper.gameObject.transform.position.Equals(
            path[position].helper.gameObject.transform.position))
                position++;
        }
    }
    private LinkedList<Node> insertSorted(Node node, LinkedList<Node> set)
    {
        LinkedListNode<Node> sNode = set.First;
        for (; sNode != null &&
        node.evaluation > sNode.Value.evaluation;
        sNode = sNode.Next) ;
        if (sNode == null) set.AddLast(node);
        else set.AddBefore(sNode, node);
        return set;
    }
    private List<Node> FindPath(Waypoint start, Waypoint goal) 
    {
        LinkedList<Node> openSet = new LinkedList<Node>();
        Dictionary<Node, Node> closedSet = new Dictionary<Node, Node>();

        Node startNode = new Node();
        startNode.npc = start;
        startNode.helper = start;
        Node.goal = goal;
        startNode.evaluation = 0;
        openSet.AddFirst(startNode);
        string oList = "Stack Open order: ";
        while (openSet.Count > 0)
        {
            Node current = openSet.First.Value;
            openSet.RemoveFirst();
            if (!closedSet.ContainsKey(current))
            {
                closedSet[current] = current;
                oList += " " + current.npc.gameObject.name + "-" +
                current.helper.gameObject.name + "(" +
                current.evaluation + ")";
                if (current.npc == goal)
                {
                    Debug.Log(oList);
                    return ReconstructPath(current);
                }
                foreach (Node child in current.expand())
                {
                    LinkedListNode<Node> existingNode = openSet.Find(child);
                    if (existingNode == null)
                    {
                        insertSorted(child, openSet);
                    }
                    else
                    {
                        if (existingNode.Value.evaluation > child.evaluation)
                        {
                            openSet.Remove(existingNode);
                            insertSorted(child, openSet);
                        }
                    }
                }
            }

        }
        Debug.Log(oList + " *** failed ***");
        return null;
    }
    private List<Node> ReconstructPath(Node goalNode)
    {
        List<Node> path = new List<Node>();
        Node current = goalNode;
        path.Add(current);
        while (current.parent != null)
        {
            current = current.parent;
            path.Insert(0, current);
        }
        return path;
    }
}
