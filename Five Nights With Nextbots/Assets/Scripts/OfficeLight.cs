using UnityEngine;

public class OfficeLight : MonoBehaviour
{
    public GameObject pointLight;
    public Material[] mats;

    private MeshRenderer renderer;

    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    public void SetState(bool isOn)
    {
        renderer.material = mats[isOn ? 0 : 1];
        pointLight.SetActive(isOn);
    }
}
