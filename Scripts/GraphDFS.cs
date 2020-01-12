using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphDFS : MonoBehaviour {

    private List<GraphNode> GraphNodeList = new List<GraphNode>();

    // Use this for initialization
    void Start ()
    {
        BuildGraph();

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    private void BuildGraph()
    {
        GraphNode nodeR = new GraphNode('r');
        GraphNode nodeS = new GraphNode('s');
        GraphNode nodeT = new GraphNode('t');
        GraphNode nodeU = new GraphNode('u');

        GraphNode nodeV = new GraphNode('v');
        GraphNode nodeW = new GraphNode('w');
        GraphNode nodeX = new GraphNode('x');
        GraphNode nodeY = new GraphNode('y');

        nodeR.AddAdj(nodeS);
        nodeR.AddAdj(nodeV);

        nodeS.AddAdj(nodeR);
        nodeS.AddAdj(nodeW);

        nodeT.AddAdj(nodeW);
        nodeT.AddAdj(nodeX);
        nodeT.AddAdj(nodeU);

        nodeU.AddAdj(nodeT);
        nodeU.AddAdj(nodeX);
        nodeU.AddAdj(nodeY);

        nodeV.AddAdj(nodeR);

        nodeW.AddAdj(nodeS);
        nodeW.AddAdj(nodeT);
        nodeW.AddAdj(nodeX);

        nodeX.AddAdj(nodeW);
        nodeX.AddAdj(nodeT);
        nodeX.AddAdj(nodeU);
        nodeX.AddAdj(nodeY);

        nodeY.AddAdj(nodeX);
        nodeY.AddAdj(nodeU);

        GraphNodeList.Add(nodeR);
        GraphNodeList.Add(nodeS);
        GraphNodeList.Add(nodeT);
        GraphNodeList.Add(nodeU);
        GraphNodeList.Add(nodeV);
        GraphNodeList.Add(nodeW);
        GraphNodeList.Add(nodeX);
        GraphNodeList.Add(nodeY);

        DFS(GraphNodeList);

        //PrintPath(GraphNodeList, nodeS, nodeY);

    }

    private int time;
    private void DFS(List<GraphNode> G)
    {
        foreach (GraphNode graphNode in G)
        {
            graphNode.vistedState = -1;
            graphNode.preGraphNode = null;
        }

        time = 0;

        foreach (GraphNode graphNode in G)
        {
            if(graphNode.vistedState == -1)
                DFS(G, graphNode);
        }

    }

    private void DFS(List<GraphNode> G, GraphNode s)
    {
        time = time + 1;
        s.distance = time;
        s.vistedState = 0;

        foreach (GraphNode graphNode in s.AbjList)
        {
            if (graphNode.vistedState == -1)
            {
                graphNode.preGraphNode = s;
                DFS(G, graphNode);
            }
        }

        s.vistedState = 1;
        Debug.Log(s.value);
        time = time + 1;

    }


}

public class GraphNode
{
    public char value;
    public List<GraphNode> AbjList;

    public int distance;
    public GraphNode preGraphNode;

    /// <summary>
    /// -1 - white, 0 - grey, 1 - black
    /// </summary>
    public int vistedState;

    public GraphNode(char value)
    {
        this.value = value;
        AbjList = new List<GraphNode>();
    }

    public void AddAdj(GraphNode node)
    {
        if (node != this)
            this.AbjList.Add(node);
        else
        {
            Debug.LogError("Can't Add It ! ! !");
        }
    }

}