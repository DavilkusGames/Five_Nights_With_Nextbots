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
    public float runSpeed = 3f;

    [Header("CAMS")]
    public RotateToCam rotateToCam;
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
        TabletCntrl.Instance.SubscribeToCamChange(CamChanged);
        trans = transform;
        if (!GameData.IsCustomNight) ai = perNightAI[GameData.SelectedNightId];
        else ai = GameData.CustomAI[3];
        if (ai == 0) obj.SetActive(false);
        else
        {
            isEnabled = true;
            activateTime -= ai * 0.75f;
            runWaitTime -= ai * 0.1f;
            attackWaitTime -= ai * 0.02f;
            rotateToCam.SetTarget(normalCam);
            StartCoroutine(nameof(MoveTimer));
        }
    }

    private IEnumerator MoveTimer()
    {
        //yield return new WaitForSeconds(activateTime);
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
                        isWatched = false;
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

    public void CamChanged(int id)
    {
        if (id == 0 && nodeId > 0) isWatched = true;
    }

    public void MovePrevNode()
    {
        nodeId--;
        if (nodeId < 0)
        {
            nodeId = 0;
            return;
        }
        trans.position = pathNodes[nodeId].position;
    }

    public void MoveNextNode()
    {
        nodeId++;
        if (nodeId >= pathNodes.Length) StartCoroutine(nameof(RunTimer));
        else trans.position = pathNodes[nodeId].position;
    }

    private IEnumerator RunTimer()
    {
        isRunning = true;
        roomCntrl.AlarmState(true);

        float timeoutTime = Time.time + runWaitTime;
        while (true)
        {
            yield return null;
            if (Time.time >= timeoutTime || NextbotManager.Instance.IsPlayerWatching(1)) break;
        }

        // RUN ANIMATION
        float runProgress = 0f;
        while (true)
        {
            rotateToCam.SetTarget((NextbotManager.Instance.IsPlayerWatching(1)) ? runCam : playerCam);

            runProgress += runSpeed * Time.deltaTime;
            if (runProgress > 1f) runProgress = 1f;
            trans.position = Vector3.Lerp(runNodes[0].position, runNodes[1].position, runProgress * Time.deltaTime);
            if (runProgress == 1f) break;
        }
        yield return new WaitForSeconds(attackWaitTime);
        if (!NextbotManager.Instance.IsDoorClosed(1))
        {
            if (isEnabled)
            {
                obj.SetActive(false);
                NextbotManager.Instance.Screamer(3);
            }
        }
        else
        {
            NextbotManager.Instance.WaitForLightBlink(1, MoveOutOfOffice);
        }
    }

    public void MoveOutOfOffice()
    {
        NextbotManager.Instance.NextbotLeftDoor(1);
        isRunning = false;

        rotateToCam.SetTarget(normalCam);
        nodeId = 0;
        trans.position = pathNodes[0].position;
    }

    public void Disable()
    {
        isEnabled = false;
    }
}
