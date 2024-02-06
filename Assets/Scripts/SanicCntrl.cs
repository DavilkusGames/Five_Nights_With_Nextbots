using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SanicCntrl : MonoBehaviour
{
    public SanicRoomCntrl roomCntrl;
    public GameObject obj;
    public Transform[] pathNodes;
    public Transform[] runNodes;
    public int[] perNightAI;
    public float moveChanceTime;
    public float runWaitTime;
    public float attackWaitTime;
    public float moveMinPeriod;

    [Header("CAMS")]
    public Transform normalCam;
    public Transform runCam;
    public Transform playerCam;

    private Transform trans;
    private int ai = 0;
    private int nodeId = 0;
    private float activateTime = 45f;
    private float prevMoveTime = 0f;
    private bool isRunning = false;
    private bool isEnabled = false;
    private bool isWatched = false;

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

    private IEnumerator MoveTimer()
    {
        yield return new WaitForSeconds(activateTime);
        while (true)
        {
            yield return new WaitForSeconds(moveChanceTime);
            if (isEnabled && !isRunning && Random.Range(1, 21) <= ai)
            {
                if (!NextbotManager.Instance.IsPlayerWatching(0))
                {
                    if (Time.time < prevMoveTime + moveMinPeriod) yield return new WaitForSeconds((prevMoveTime + moveMinPeriod) - Time.time);
                    prevMoveTime = Time.time;

                    if (isWatched)
                    {
                        MovePrevNode();
                    }
                    else
                    {
                        MoveNextNode();
                        if (nodeId == pathNodes.Length-1)
                        {
                            if (NextbotManager.Instance.CanEnterDoor(1))
                            {
                                isEnabled = false;
                                NextbotManager.Instance.NextbotEnteredDoor(1, 3);
                            }
                            else MovePrevNode();
                        }
                    }
                }
            }
        }
    }

    public void MovePrevNode()
    {

    }

    public void MoveNextNode()
    {

    }

    public void Disable()
    {
        isEnabled = false;
    }
}
