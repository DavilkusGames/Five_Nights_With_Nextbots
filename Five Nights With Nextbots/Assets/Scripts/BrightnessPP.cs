using UnityEngine;

public class BrightnessPP : MonoBehaviour
{
    public Shader shader;
    [Range(0, 2)] public float brightness;

    private Material mat;

    private void Start()
    {
        mat = new Material(shader);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        mat.SetFloat("_Brightness", brightness);
        Graphics.Blit(source, destination, mat);
    }
}