using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public static Waypoint goal;
    public Waypoint state;
    public Waypoint npc;
    public Waypoint helper;
    public Node parent;

    private const int CLOSED_DOOR_COST = 1000;

    public double evaluation
    {
        get { return cost + heuristic; }
        set { cost = value; 
            heuristic = getHeuristic(); }
    }
    public double cost;
    public double heuristic;
    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        Node node = obj as Node;
        if (node == null) return false;
        else return npc == node.npc && helper == node.helper;
    }

    public static int getScore(Waypoint npcS, Waypoint npcE,
    Waypoint helperS, Waypoint helperE)
    {
        int score = 0;
        if (helperS != helperE) score += scoreOne(helperS, helperE);
        if (npcS != npcE)
        {
            if (npcE.door && !helperE.key) score += CLOSED_DOOR_COST;
            else
            {
                score += scoreOne(npcS, npcE);
            }
        }
        return score;
    }

    public static int scoreOne(Waypoint ws, Waypoint we)
    {
        if (ws.color.Equals(we.color)) return 1;
        if ((ws.color.Equals(Color.red) && we.color.Equals(Color.blue)) ||
        (ws.color.Equals(Color.blue) && we.color.Equals(Color.red)))
            return 3;
        return 2;
    }
    private Node createNode(Node parent, Waypoint npcE, Waypoint helperE)
    {
        Node child = new Node();
        child.parent = parent;
        child.npc = npcE;
        child.helper = helperE;
        child.cost = cost + getScore(parent.npc, child.npc, parent.helper, child.helper);
        child.heuristic = child.getHeuristic();
        return child;
    }
    public List<Node> expand()
    {
        List<Node> childs = new List<Node>();
        foreach (Waypoint wp in npc.edges) childs.Add(createNode(this, wp, helper));
        foreach (Waypoint wp in helper.edges) childs.Add(createNode(this, npc, wp));
        return childs;
    }
    public int getHeuristic()
    {
        if (npc == goal) return 0;
        int min = int.MaxValue / 2;
        foreach (Waypoint wp in npc.edges)
        {
            int value = scoreOne(npc, wp);
            if (npc.door || wp.door && !helper.key) value++;
            if (wp != goal) value += scoreOne(wp, goal);
            if (value < min) min = value;
        }
        
        if (!helper.key)
            foreach (Waypoint wp in helper.edges)
            {
                int value = scoreOne(helper, wp);
                value += scoreOne(npc, goal);
                if (npc.door && !wp.key) value++;
                if (value < min) min = value;
            }
        return min;
    }
}
