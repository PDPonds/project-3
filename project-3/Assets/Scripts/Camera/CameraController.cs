using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform target;
    [Header("===== Follow =====")]
    [SerializeField] float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        FollowTarget();
    }

    public void Setup(Transform target)
    {
        this.target = target;
    }

    void FollowTarget()
    {
        if (target != null)
        {
            Vector3 desiredPostion = target.position;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPostion, smoothSpeed);
            transform.position = smoothedPos;
        }
    }

}
