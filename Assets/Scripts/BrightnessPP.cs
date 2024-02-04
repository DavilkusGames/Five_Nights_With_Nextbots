using UnityEngine;

[ExecuteInEditMode]
public class BrightnessPP : MonoBehaviour
{
    public Shader shader;
    [Range(0, 2)] public float brightness;

    private Material mat;

    private void Start()
    {
        if (!shader.isSupported)
        {
            mat = new Material(shader);
            Debug.Log("PP SHADER '" + shader.name + "' IS NOT SUPPORTED ON YOUR GPU. TURNED OFF.");
            enabled = false;
        }
        else Debug.Log("PP Shader '" + shader.name + "' supported and active.");
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!shader.isSupported) destination = source;
        else
        {
            mat.SetFloat("_Brightness", brightness);
            Graphics.Blit(source, destination, mat);
        }
    }
}