using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GraphBFS : MonoBehaviour {

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

        BFS(GraphNodeList, nodeS);

        PrintPath(GraphNodeList, nodeS, nodeY);

    }

    private void PrintPath(List<GraphNode> G, GraphNode source, GraphNode target)
    {
        if (source == target)
        {
            Debug.Log(source.value);
            return;
        }
        else
        {
            if (target.preGraphNode == null)
            {
                Debug.Log("Not Find Path");
                return;
            }
            else
            {
                Debug.Log(target.value);
                PrintPath(G, source, target.preGraphNode);
            }
        }
    }

    private void BFS(List<GraphNode> G, GraphNode s)
    {
        Queue<GraphNode> queue = new Queue<GraphNode>();
        queue.Enqueue(s);
        s.vistedState = 0;
        s.distance = 0;
        s.preGraphNode = null;

        foreach (GraphNode graphNode in G)
        {
            if (graphNode != s)
            {
                graphNode.vistedState = -1;
                graphNode.distance = Int32.MaxValue;
                graphNode.preGraphNode = null;
            }
        }

        while (queue.Count != 0)
        {
            GraphNode node = queue.Dequeue();

            foreach (GraphNode graphNode in node.AbjList)
            {
                if (graphNode.vistedState == -1)
                {
                    graphNode.vistedState = 0;
                    graphNode.distance = node.distance + 1;
                    graphNode.preGraphNode = node;

                    queue.Enqueue(graphNode);

                }
            }

            node.vistedState = 1;
            //Debug.Log(node.value);

        }

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
        if(node != this)
            this.AbjList.Add(node);
        else
        {
            Debug.LogError("Can't Add It ! ! !");
        }
    }

}