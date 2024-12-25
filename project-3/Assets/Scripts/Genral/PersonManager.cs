using UnityEngine;

public class PersonManager : MonoBehaviour
{
    [Header("===== Move Speed =====")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [HideInInspector] public float curSpeed;

    private void Update()
    {
        HandleSpeed();
    }

    void HandleSpeed()
    {
        if (GameManager.Instance.isRunning) curSpeed = runSpeed;
        else curSpeed = walkSpeed;
    }

}
