using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Vector3 moveDir;
    Rigidbody rb;
    PersonManager personManager;

    [SerializeField] float rotationSpeed;

    public void Setup()
    {
        rb = GetComponent<Rigidbody>();
        personManager = GetComponent<PersonManager>();
        personManager.SwitchState(PersonState.Normal);
    }

    #region Movement

    private void Update()
    {
        MoveHandle();
        RotationHandle();
    }

    void MoveHandle()
    {
        moveDir = Camera.main.transform.forward * GameManager.Instance.moveInput.y;
        moveDir = moveDir + Camera.main.transform.right * GameManager.Instance.moveInput.x;
        moveDir.Normalize();
        moveDir.y = 0;
        moveDir = moveDir * personManager.curSpeed;


        rb.linearVelocity = new Vector3(moveDir.x, rb.linearVelocity.y, moveDir.z);

    }

    void RotationHandle()
    {
        Vector3 targetDir = Vector3.zero;
        targetDir = Camera.main.transform.forward * GameManager.Instance.moveInput.y;
        targetDir = targetDir + Camera.main.transform.right * GameManager.Instance.moveInput.x;
        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            Quaternion playerRot = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            transform.rotation = playerRot;
        }
    }

    #endregion

}
