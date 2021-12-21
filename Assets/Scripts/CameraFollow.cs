using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField][Range(0.01f,1f)]
    private float smoothSpeed = 0.125f;

    [SerializeField]
    private Vector3 offset;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        Vector3 desiredPosition = offset;
        desiredPosition.y += player.position.y;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
    }

    public void SetTarget(Transform transform)
    {
        player = transform;
    }
}