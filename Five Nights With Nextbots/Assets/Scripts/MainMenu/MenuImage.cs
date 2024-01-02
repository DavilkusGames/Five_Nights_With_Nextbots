using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuImage : MonoBehaviour
{
    public Texture[] textures;
    public float minDelay = 1.0f;
    public float maxDelay = 2.0f;
    public float minShowtime = 0.2f;
    public float maxShowtime = 0.5f;

    public float minBlackoutDelay = 1.0f;
    public float maxBlackoutDelay = 2.0f;
    public float minBlackoutShowtime = 0.2f;
    public float maxBlackoutShowtime = 0.5f;

    private RawImage img;

    private void Start()
    {
        img = GetComponent<RawImage>();
        StartCoroutine(nameof(ChangeTimer));
        StartCoroutine(nameof(BlackoutTimer));
    }

    private IEnumerator ChangeTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            img.texture = textures[Random.Range(1, textures.Length)];
            yield return new WaitForSeconds(Random.Range(minShowtime, maxShowtime));
            img.texture = textures[0];
        }
    }

    private IEnumerator BlackoutTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minBlackoutDelay, maxBlackoutDelay));
            img.color = new Color(1f, 1f, 1f, Random.Range(0.3f, 0.7f));
            yield return new WaitForSeconds(Random.Range(minBlackoutShowtime, maxBlackoutShowtime));
            img.color = Color.white;
        }
    }
}
