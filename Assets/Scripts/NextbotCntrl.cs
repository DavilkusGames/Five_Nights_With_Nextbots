using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class NextbotCntrl : MonoBehaviour
{
    public int id;
    public GameObject obj;
    public GameObject screamer;
    public float screamerTime;
    public int[] perNightAI;
    public float moveChanceTime;
    public List<NextbotPathNode> pathNodes;

    private Transform trans;
    private int ai = 0;
    private int nodeId = 0;
    private bool isEnabled = false;

    private void Start()
    {
        trans = transform;
        ai = perNightAI[GameData.SelectedNightId];
        if (ai == 0) obj.SetActive(false);
        else
        {
            isEnabled = true;
            StartCoroutine(nameof(MoveTimer));
        }
    }

    private bool MovePrevNode()
    {
        if (nodeId <= 0) return false;
        int prevNodeLocalId = -1;
        while (prevNodeLocalId < pathNodes[nodeId].prevPathNodes.Length - 1)
        {
            prevNodeLocalId++;
            if (Random.Range(0, 100) < (pathNodes[nodeId].prevPathNodes[prevNodeLocalId].moveProbability * 100f)) break;
        }
        if (prevNodeLocalId == -1) return false;

        int prevNodeGlobalId = pathNodes[nodeId].prevPathNodes[prevNodeLocalId].id;
        if (TabletCntrl.Instance.GetCameraId() == pathNodes[nodeId].camId ||
            TabletCntrl.Instance.GetCameraId() == pathNodes[prevNodeGlobalId].camId) TabletCntrl.Instance.DisableCams();
        nodeId = prevNodeGlobalId;
        trans.position = pathNodes[nodeId].transform.position;
        transform.rotation = pathNodes[nodeId].transform.rotation;
        return true;
    }

    private bool MoveNextNode()
    {
        if (nodeId >= pathNodes.Count-1) return false;
        int nextNodeLocalId = -1;
        while (nextNodeLocalId < pathNodes[nodeId].nextPathNodes.Length-1)
        {
            nextNodeLocalId++;
            if (Random.Range(0, 100) < (pathNodes[nodeId].nextPathNodes[nextNodeLocalId].moveProbability * 100f)) break;
        }
        if (nextNodeLocalId == -1) return false;

        int nextNodeGlobalId = pathNodes[nodeId].nextPathNodes[nextNodeLocalId].id;
        if (TabletCntrl.Instance.GetCameraId() == pathNodes[nodeId].camId ||
            TabletCntrl.Instance.GetCameraId() == pathNodes[nextNodeGlobalId].camId) TabletCntrl.Instance.DisableCams();
        nodeId = nextNodeGlobalId;
        trans.position = pathNodes[nodeId].transform.position;
        transform.rotation = pathNodes[nodeId].transform.rotation;
        return true;
    }

    private IEnumerator MoveTimer()
    {
        while (isEnabled)
        {
            yield return new WaitForSeconds(moveChanceTime);
            if (isEnabled && Random.Range(1, 21) <= ai)
            {
                if (!pathNodes[nodeId].isInOffice) MoveNextNode();
                else
                {
                    obj.SetActive(false);
                    NextbotManager.Instance.Screamer(id);
                }
            }
        }
    }

    public void Disable()
    {
        isEnabled = false;
    }
}
