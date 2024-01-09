using UnityEngine;

public class NextbotPathNode : MonoBehaviour
{
    public int id;
    public int camId;
    public NextbotPathNode[] nextPathNodes;
    public NextbotPathNode[] prevPathNodes;
    [Range(0f, 1f)] public float moveProbability;
    public int officeDoorId = -1;

    private Transform trans;

    private void Start()
    {
        trans = transform;
    }

    public Transform GetTransform()
    {
        return trans;
    }
}
