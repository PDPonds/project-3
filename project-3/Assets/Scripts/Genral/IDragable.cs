using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class IDragable : MonoBehaviour
{
    Rigidbody rb;
    Vector3 moveDir;
    public bool isDraging;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void BeginDrag()
    {
        UIManager.Instance.CloseInteractiveChoice();
        GameManager.Instance.curPlayer.SwitchState(PlayerState.Draging);
        isDraging = true;
        GameManager.Instance.curPlayer.LookAt(transform.position);
        GameManager.Instance.curPlayer.curDragObject = this;
    }

    public void OnDraging()
    {
        if (isDraging)
        {
            MoveHandle();
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    public void EndDrag()
    {
        isDraging = false;
        GameManager.Instance.curPlayer.curDragObject = null;
        GameManager.Instance.curPlayer.SwitchState(PlayerState.EndAnyAction);
    }


    public string DragName()
    {
        return $"Drag";
    }

    void MoveHandle()
    {
        moveDir = Camera.main.transform.forward * GameManager.Instance.moveInput.y;
        moveDir = moveDir + Camera.main.transform.right * GameManager.Instance.moveInput.x;
        moveDir.Normalize();
        moveDir.y = 0;
        moveDir = moveDir * GameManager.Instance.curPlayer.curSpeed;


        rb.linearVelocity = new Vector3(moveDir.x, rb.linearVelocity.y, moveDir.z);

    }
}
