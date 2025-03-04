using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerControllers playerControls;
    public InputAction HorizontalMovement;

    private void Awake()
    {
        playerControls = new PlayerControllers();

        HorizontalMovement = playerControls.InGame.HorizontalMovement;
        HorizontalMovement.Enable();

    }

    private void OnDisable()
    {
        HorizontalMovement.Disable();
    }


}
