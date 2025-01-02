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

            inputSystem.PlayerInput.Sprint.performed += i => GameManager.Instance.isRunning = true;
            inputSystem.PlayerInput.Sprint.canceled += i => GameManager.Instance.isRunning = false;

            inputSystem.PlayerInput.Interactive.performed += i => UIManager.Instance.ShowInteractiveChoice();

            inputSystem.PlayerInput.ToggleInventory.performed += i => UIManager.Instance.ToggleInventory(ShowInventoryType.Inventory);

            inputSystem.PlayerInput.Select_HandSlot_1.performed += i => GameManager.Instance.SelectHandSlot(1);
            inputSystem.PlayerInput.Select_HandSlot_2.performed += i => GameManager.Instance.SelectHandSlot(2);

            inputSystem.PlayerInput.Attack.performed += i => GameManager.Instance.curPlayer.Attack();

            inputSystem.PlayerInput.UseItem.performed += i => GameManager.Instance.curPlayer.UseItem();

        }

        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

}
