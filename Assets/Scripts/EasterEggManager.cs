using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class EE
{
    public GameObject obj;
    public int camId;
    public float showTime = 10f;
}

public class EasterEggManager : MonoBehaviour
{
    public EE[] ees;

    public float chanceDelay = 30f;
    public float eeChance = 0.5f;

    private void Start()
    {
        StartCoroutine(nameof(EETimer));
    }

    private IEnumerator EETimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(chanceDelay);
            if (UnityEngine.Random.Range(0, 10) <= eeChance * 10)
            {
                int eeId = UnityEngine.Random.Range(0, ees.Length);
                if (TabletCntrl.Instance.IsTabletUp() && TabletCntrl.Instance.GetCameraId() == ees[eeId].camId) TabletCntrl.Instance.DisableCams();
                ees[eeId].obj.SetActive(true);
                yield return new WaitForSeconds(ees[eeId].showTime);
                if (TabletCntrl.Instance.IsTabletUp() && TabletCntrl.Instance.GetCameraId() == ees[eeId].camId) TabletCntrl.Instance.DisableCams();
                ees[eeId].obj.SetActive(false);
            }
        }
    }
}
