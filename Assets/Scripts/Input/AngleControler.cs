using UnityEngine;
using UnityEngine.InputSystem;

public class AngleControler : MonoBehaviour
{
    private InputSystem_Actions _inputSystem;

    private void Awake()
    {
        _inputSystem = new InputSystem_Actions();
        _inputSystem.Player.Look.started += OnLook;
        _inputSystem.Player.Look.performed += OnLook;
        _inputSystem.Player.Look.canceled += OnLook;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        
    }
}
