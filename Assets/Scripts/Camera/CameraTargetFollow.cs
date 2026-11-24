using UnityEngine;

[DefaultExecutionOrder(-50)]
public class CameraTargetFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime = 0.2f;

    private Vector3 currVelocity;

    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offset, ref currVelocity, smoothTime);
    }
}
