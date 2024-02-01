using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public enum NextbotType
{
    Normal, Raider, Runner
};

public enum NextbotCamMoveType
{
    IgnoreCams, StunIfWatching, Random, BackIfWatching
};

public enum NextbotAttackType
{
    IgnoreTablet, AttackOnTabletDown
};

public class NextbotCntrl : MonoBehaviour
{
    public int id;
    public GameObject obj;
    public GameObject screamer;
    public NextbotType type;
    public NextbotCamMoveType moveType;
    public NextbotAttackType attackType;
    public float screamerTime;
    public int[] perNightAI;
    public float moveChanceTime;
    public float walkBackChance = 0f;
    public float attackTime = 10f;
    public float moveMinPeriod = 8.0f;
    public List<NextbotPathNode> pathNodes;

    private Transform trans;
    private float prevMoveTime = 0f;
    private float activateTime = 0f;
    private int ai = 0;
    private int nodeId = 0;
    private bool isEnabled = false;

    [Header("Amogus")]
    public int[] randomSpawnsId;
    public Vector2 standardSpawnTimeRange = Vector2.zero;
    public float spawnTimeRangeAiK = 1f;

    private void Start()
    {
        trans = transform;
        ai = perNightAI[GameData.SelectedNightId];
        standardSpawnTimeRange = new Vector2(standardSpawnTimeRange.x - ai * spawnTimeRangeAiK, 
            standardSpawnTimeRange.y - ai * spawnTimeRangeAiK);
        if (ai == 0) obj.SetActive(false);
        else
        {
            isEnabled = (type != NextbotType.Raider);
            obj.SetActive(type != NextbotType.Raider);
            if (type == NextbotType.Raider) Invoke(nameof(RaiderRespawn), Random.Range(standardSpawnTimeRange.x, standardSpawnTimeRange.y));
            activateTime = (20 - ai) * 1.5f;
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
        if (TabletCntrl.Instance.IsTabletUp() && (TabletCntrl.Instance.GetCameraId() == pathNodes[nodeId].camId ||
            TabletCntrl.Instance.GetCameraId() == pathNodes[prevNodeGlobalId].camId)) TabletCntrl.Instance.DisableCams();
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
        if (TabletCntrl.Instance.IsTabletUp() && (TabletCntrl.Instance.GetCameraId() == pathNodes[nodeId].camId ||
            TabletCntrl.Instance.GetCameraId() == pathNodes[nextNodeGlobalId].camId)) TabletCntrl.Instance.DisableCams();
        nodeId = nextNodeGlobalId;
        trans.position = pathNodes[nodeId].transform.position;
        transform.rotation = pathNodes[nodeId].transform.rotation;
        return true;
    }

    private IEnumerator MoveTimer()
    {
        if (type != NextbotType.Raider) yield return new WaitForSeconds(activateTime);
        while (true)
        {
            yield return new WaitForSeconds(moveChanceTime);
            if (isEnabled && Random.Range(1, 21) <= ai)
            {
                if (!NextbotManager.Instance.IsPlayerWatching(pathNodes[nodeId].camId) ||
                    moveType == NextbotCamMoveType.IgnoreCams || 
                    (moveType == NextbotCamMoveType.Random && Random.Range(0, 10) > 4))
                {
                    if (Time.time < prevMoveTime + moveMinPeriod) yield return new WaitForSeconds((prevMoveTime + moveMinPeriod) - Time.time);
                    prevMoveTime = Time.time;

                    if (pathNodes[nodeId].prevPathNodes.Length > 0 && Random.Range(0, 100) < (walkBackChance * 100f))
                    {
                        MovePrevNode();
                    }
                    else {
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
        }
    }

    private IEnumerator InOfficeTimer()
    {
        yield return new WaitForSeconds(attackTime - (ai * 0.18f));
        if (!NextbotManager.Instance.IsDoorClosed(pathNodes[nodeId].officeDoorId))
        {
            if (attackType == NextbotAttackType.IgnoreTablet)
            {
                Screamer();
            }
            else
            {
                NextbotManager.Instance.BreakDoor(pathNodes[nodeId].officeDoorId);
                NextbotManager.Instance.TabletDownCallback(Screamer);
                yield return new WaitForSeconds(15f);
                if (enabled) Screamer();
            }
        }
        else
        {
            NextbotManager.Instance.WaitForLightBlink(pathNodes[nodeId].officeDoorId, MoveOutOfOffice);
        }
    }

    public void Screamer()
    {
        obj.SetActive(false);
        NextbotManager.Instance.Screamer(id);
    }

    public void MoveOutOfOffice()
    {
        NextbotManager.Instance.NextbotLeftDoor(pathNodes[nodeId].officeDoorId);
        isEnabled = (type != NextbotType.Raider);
        obj.SetActive(type != NextbotType.Raider);
        if (type == NextbotType.Raider) Invoke(nameof(RaiderRespawn), Random.Range(standardSpawnTimeRange.x, standardSpawnTimeRange.y));
        MoveNextNode();
    }

    private void RaiderRespawn()
    {
        int spawnId = Random.Range(0, randomSpawnsId.Length);
        nodeId = pathNodes[randomSpawnsId[spawnId]].id;
        trans.position = pathNodes[nodeId].transform.position;
        trans.rotation = pathNodes[nodeId].transform.rotation;
        if (TabletCntrl.Instance.IsTabletUp() && (TabletCntrl.Instance.GetCameraId() == pathNodes[nodeId].camId)) TabletCntrl.Instance.DisableCams();
        prevMoveTime = Time.time;
        obj.SetActive(true);
        isEnabled = true;
    }

    public void Disable()
    {
        isEnabled = false;
    }
}
