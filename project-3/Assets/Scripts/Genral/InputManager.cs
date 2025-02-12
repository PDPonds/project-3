using UnityEngine;

public class InputManager : MonoBehaviour
{
    InputSystem inputSystem;

    private void OnEnable()
    {
        if (inputSystem == null)
        {
            inputSystem = new InputSystem();
            inputSystem.PlayerInput.Movement.performed += i => GameManager.Instance.moveInput = i.ReadValue<Vector2>();
            inputSystem.PlayerInput.MouseInput.performed += i => GameManager.Instance.mousePos = i.ReadValue<Vector2>();

            inputSystem.PlayerInput.ArrowInput.performed += i => GameManager.Instance.arrowInput = i.ReadValue<Vector2>();

            inputSystem.PlayerInput.Sprint.performed += i => GameManager.Instance.isRunning = true;
            inputSystem.PlayerInput.Sprint.canceled += i => GameManager.Instance.isRunning = false;

            inputSystem.PlayerInput.Interactive.performed += i => UIManager.Instance.ShowInteractiveChoice();

            inputSystem.PlayerInput.ToggleInventory.performed += i => UIManager.Instance.ToggleInventory(ShowInventoryType.Inventory);

            inputSystem.PlayerInput.Select_HandSlot_1.performed += i => GameManager.Instance.SelectHandSlot(1);
            inputSystem.PlayerInput.Select_HandSlot_2.performed += i => GameManager.Instance.SelectHandSlot(2);

            inputSystem.PlayerInput.Attack.performed += i => AttackPerformed();
            inputSystem.PlayerInput.Attack.canceled += i => AttackCancle();

            inputSystem.PlayerInput.UseItem.performed += i => UseItemPerformed();

            inputSystem.PlayerInput.Dash.performed += i => Dash_performed();

        }

        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

    private void Dash_performed()
    {
        if (!GameManager.Instance.IsPhase(GamePhase.DuringGame)) return;
        if (GameManager.Instance.curPlayer == null) return;

        if (GameManager.Instance.curPlayer.IsState(PlayerState.ShowUI))
        {
            if (UIManager.Instance.IsLockPickActive())
            {
                UIManager.Instance.TryPickLock();
            }
        }
        else
        {
            GameManager.Instance.curPlayer.Dash();
        }
    }

    public void AttackPerformed()
    {
        if (!GameManager.Instance.IsPhase(GamePhase.DuringGame)) return;
        GameManager.Instance.curPlayer.isAttack = true;
    }

    public void AttackCancle()
    {
        if (!GameManager.Instance.IsPhase(GamePhase.DuringGame)) return;
        GameManager.Instance.curPlayer.isAttack = false;
    }

    public void UseItemPerformed()
    {
        if (!GameManager.Instance.IsPhase(GamePhase.DuringGame)) return;
        GameManager.Instance.curPlayer.UseItem();
    }

}
