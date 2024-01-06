using UnityEngine;

public class NextbotPathNode : MonoBehaviour
{
    public int id;
    public Transform transform;
    public NextbotPathNode[] nextPathNodes;
    public NextbotPathNode[] prevPathNodes;
    public float moveProbability;
}
