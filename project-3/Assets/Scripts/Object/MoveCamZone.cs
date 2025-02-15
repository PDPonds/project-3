using UnityEngine;

public class MoveCamZone : MonoBehaviour
{
    [SerializeField] float YRotation;
    [SerializeField] float moveRotationSpeed;
    [SerializeField] BoxCollider collider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.curCameraController.SetYRotation(YRotation , moveRotationSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        if (collider != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position + collider.center, collider.size);
        }
    }

}
