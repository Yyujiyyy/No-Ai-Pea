using UnityEngine;
using UnityEngine.InputSystem;

public class AngleControler : MonoBehaviour
{
    Transform _tr;

    private InputSystem_Actions _inputSystem;
    private Vector2 _mouseDifference;

    [SerializeField,Range(0,1)]private float _sensitivity;
    public float X; 
    public float Y;
    public float premouseX; 
    public float premouseY;

    private void Awake()
    {
        _tr = transform;

        _inputSystem = new InputSystem_Actions();
        _inputSystem.Player.Look.started += OnLook;
        _inputSystem.Player.Look.performed += OnLook;
        _inputSystem.Player.Look.canceled += OnLook;

        //有効化
        _inputSystem.Enable();
    }

    private void OnDestroy()
    {   //イベント情報を廃棄する
        _inputSystem.Dispose();//メモリの確保のため？C++に変換するときに楽になる？
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        _mouseDifference = context.ReadValue<Vector2>();
    }
    
    private void Update()
    {
        //x,yは逆
        this._tr.eulerAngles += new Vector3(_mouseDifference.y * _sensitivity, _mouseDifference.x * _sensitivity, 0);
        
        //Debug.Log(_mouseDifference);
    }
}
