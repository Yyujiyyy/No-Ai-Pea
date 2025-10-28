using UnityEngine;
using UnityEngine.InputSystem;

//RequireComponentを使ったスクリプトをゲームオブジェクトにアタッチすると、
//必要なコンポーネントが自動的にそのゲームオブジェクトに加えられるようになる
[RequireComponent(typeof(Rigidbody))]
public class MoveControl : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _jumpForce = 5;

    private InputSystem_Actions _InputSystem;
    private Rigidbody _rigidbody;
    private Vector2 _moveInputValue;
    private Transform _tr;

    private void Awake()
    {
        //インスタンス化
        _rigidbody = GetComponent<Rigidbody>();
        _InputSystem = new InputSystem_Actions();
        _tr = transform;

        // Actionイベント登録
        _InputSystem.Player.Move.started += OnMove;
        _InputSystem.Player.Move.performed += OnMove;
        _InputSystem.Player.Move.canceled += OnMove;
        _InputSystem.Player.Jump.performed += OnJump;

        // Input Actionを機能させるためには有効化する必要がある
        _InputSystem.Enable();
    }

    private void OnDestroy()
    {   //イベント情報を廃棄する
        _InputSystem.Dispose(); 
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInputValue = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _rigidbody.AddForce(Vector3.up * _speed, ForceMode.Impulse);
    }

    private void Update()
    {
        Debug.Log(_moveInputValue);
        _rigidbody.AddForce(new Vector3(_moveInputValue.x * 10, 0, _moveInputValue.y * 10));
        //transform.positionに変える
        _tr.position += new Vector3(_moveInputValue.x * 10, 0, _moveInputValue.y);
    }
}
