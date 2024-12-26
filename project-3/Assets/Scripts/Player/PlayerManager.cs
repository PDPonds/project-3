using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Vector3 moveDir;
    Rigidbody rb;
    PersonManager personManager;

    [SerializeField] float rotationSpeed;

    [Header("===== Interactive =====")]
    [SerializeField] float interactiveRange;
    [SerializeField] LayerMask interactiveMask;

    public void Setup()
    {
        rb = GetComponent<Rigidbody>();
        personManager = GetComponent<PersonManager>();
        personManager.SwitchState(PersonState.Normal);
    }

    private void Update()
    {
        MoveHandle();
        RotationHandle();

        CheckInteractive();
    }

    #region Movement

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

    #region Interactive

    public void CheckInteractive()
    {
        if (personManager.IsState(PersonState.Normal) || personManager.IsState(PersonState.Injury))
        {
            Collider[] interactivCol = Physics.OverlapSphere(transform.position, interactiveRange, interactiveMask);
            if (interactivCol.Length > 0)
            {
                Collider targetCol = interactivCol[0];
                if (targetCol.TryGetComponent<IActionObject>(out IActionObject iaction))
                {
                    UIManager.Instance.ShowInteractiveKey(targetCol.transform.position);
                    GameManager.Instance.curInteractiveObj = targetCol.gameObject;
                }
                else
                {
                    UIManager.Instance.HideInteractiveKey();
                    GameManager.Instance.curInteractiveObj = null;
                    UIManager.Instance.HideInteractiveChoice();
                }
            }
            else
            {
                UIManager.Instance.HideInteractiveKey();
                GameManager.Instance.curInteractiveObj = null;
                UIManager.Instance.HideInteractiveChoice();
            }
        }
        else if (personManager.IsState(PersonState.Interact))
        {
            UIManager.Instance.HideInteractiveKey();
            GameManager.Instance.curInteractiveObj = null;
        }
        else
        {
            UIManager.Instance.HideInteractiveKey();
            GameManager.Instance.curInteractiveObj = null;
            UIManager.Instance.HideInteractiveChoice();
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactiveRange);
    }

}
