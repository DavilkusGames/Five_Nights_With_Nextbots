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

    private void MovePrevNode()
    {
        if (nodeId <= 0) return;
        int prevNodeId = 0;
        for (; prevNodeId < pathNodes[nodeId].prevPathNodes.Length; prevNodeId++)
        {
            if (Random.Range(0, 100) < (pathNodes[nodeId].prevPathNodes[prevNodeId].moveProbability * 100f)) break;
        }

        nodeId = pathNodes[prevNodeId].id;
        trans.position = pathNodes[nodeId].transform.position;
        transform.rotation = pathNodes[nodeId].transform.rotation;
    }

    private void MoveNextNode()
    {
        if (nodeId >= pathNodes.Count) return;
        int nextNodeId = 0;
        for (; nextNodeId < pathNodes[nodeId].nextPathNodes.Length; nextNodeId++)
        {
            if (Random.Range(0, 100) < (pathNodes[nodeId].nextPathNodes[nextNodeId].moveProbability * 100f)) break;
        }

        nodeId = pathNodes[nextNodeId].id;
        trans.position = pathNodes[nodeId].transform.position;
        transform.rotation = pathNodes[nodeId].transform.rotation;
    }
}
