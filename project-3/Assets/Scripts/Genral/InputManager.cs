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
        }

        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

}
