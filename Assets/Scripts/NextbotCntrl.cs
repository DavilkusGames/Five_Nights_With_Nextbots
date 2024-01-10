using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public enum NextbotCamMoveType
{
    IgnoreCams, StunIfWatching, Random
}

public enum NextbotAttackType
{
    IgnoreTablet, AttackOnTabletDown
}

public class NextbotCntrl : MonoBehaviour
{
    public int id;
    public GameObject obj;
    public GameObject screamer;
    public NextbotCamMoveType moveType;
    public NextbotAttackType attackType;
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
        while (true)
        {
            yield return new WaitForSeconds(moveChanceTime);
            if (isEnabled && Random.Range(1, 21) <= ai)
            {
                MoveNextNode();
                if (pathNodes[nodeId].officeDoorId != -1)
                {
                    if (NextbotManager.Instance.CanEnterDoor(pathNodes[nodeId].officeDoorId))
                    {
                        isEnabled = false;
                        NextbotManager.Instance.NextbotEnteredDoor(pathNodes[nodeId].officeDoorId, id);
                        StartCoroutine(nameof(InOfficeTimer));
                    }
                    else MovePrevNode();
                }
            }
        }
    }

    private IEnumerator InOfficeTimer()
    {
        float attackTimeK = 1f;
        if (ai > 7) attackTimeK = 0.85f;
        if (ai > 13) attackTimeK = 0.75f;
        yield return new WaitForSeconds(Random.Range(6, 12) * attackTimeK);
        if (!NextbotManager.Instance.IsDoorClosed(pathNodes[nodeId].officeDoorId))
        {
            obj.SetActive(false);
            NextbotManager.Instance.Screamer(id);
        }
        else
        {
            NextbotManager.Instance.WaitForLightBlink(pathNodes[nodeId].officeDoorId, MoveOutOfOffice);
        }
    }

    public void MoveOutOfOffice()
    {
        isEnabled = true;
        obj.SetActive(true);
        NextbotManager.Instance.NextbotLeftDoor(pathNodes[nodeId].officeDoorId);
        MoveNextNode();
    }

    public void Disable()
    {
        isEnabled = false;
    }
}
