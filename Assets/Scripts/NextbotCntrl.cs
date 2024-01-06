using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class NextbotCntrl : MonoBehaviour
{
    public GameObject obj;
    public GameObject screamer;
    public float screamerTime;
    public int[] perNightAI;
    public List<NextbotPathNode> pathNodes;

    private Transform trans;
    private int nodeId = 0;

    private void Start()
    {
        trans = transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            MoveNextNode();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            MoveNextNode();
        }
    }

    private bool MovePrevNode()
    {
        if (nodeId <= 0) return false;
        int prevNodeId = -1;
        for (; prevNodeId < pathNodes[nodeId].prevPathNodes.Length; prevNodeId++)
        {
            if (Random.Range(0, 100) < (pathNodes[nodeId].prevPathNodes[prevNodeId].moveProbability * 100f)) break;
        }

        if (prevNodeId == -1) return false;
        nodeId = pathNodes[prevNodeId].id;
        trans.position = pathNodes[nodeId].transform.position;
        transform.rotation = pathNodes[nodeId].transform.rotation;
        return true;
    }

    private bool MoveNextNode()
    {
        if (nodeId >= pathNodes.Count) return false;
        int nextNodeId = -1;
        for (; nextNodeId < pathNodes[nodeId].nextPathNodes.Length; nextNodeId++)
        {
            if (Random.Range(0, 100) < (pathNodes[nodeId].nextPathNodes[nextNodeId].moveProbability * 100f)) break;
        }

        if (nextNodeId == -1) return false;
        nodeId = pathNodes[nextNodeId].id;
        trans.position = pathNodes[nodeId].transform.position;
        transform.rotation = pathNodes[nodeId].transform.rotation;
        return true;
    }
}
