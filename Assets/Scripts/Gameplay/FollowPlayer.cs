using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool restrictX;
    [SerializeField] private bool restrictY;
    [SerializeField] private bool restrictZ;

    private void LateUpdate()
    {
        Vector3 newPos = transform.position;

        if (!restrictX) newPos.x = player.transform.position.x + offset.x;
        if (!restrictY) newPos.y = player.transform.position.y + offset.y;
        if (!restrictZ) newPos.z = player.transform.position.z + offset.z;

        transform.position = newPos;
    }
}
