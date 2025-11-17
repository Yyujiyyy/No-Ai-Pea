using UnityEngine;
using UnityEngine.InputSystem;

//RequireComponentを使ったスクリプトをゲームオブジェクトにアタッチすると、
//必要なコンポーネントが自動的にそのゲームオブジェクトに加えられるようになる
[RequireComponent(typeof(Rigidbody))]
public class MoveControl : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    [SerializeField] private float _jumpForce = 5;

    private InputSystem_Actions _inputSystem;
    private Rigidbody _rigidbody;
    private Vector2 _moveInputValue;
    private Transform _tr;

    private void Awake()
    {
        //インスタンス化
        _rigidbody = GetComponent<Rigidbody>();
        _inputSystem = new InputSystem_Actions();
        _tr = transform;

        // Actionイベント登録
        _inputSystem.Player.Move.started += OnMove;
        _inputSystem.Player.Move.performed += OnMove;
        _inputSystem.Player.Move.canceled += OnMove;
        _inputSystem.Player.Jump.performed += OnJump;

        // Input Actionを機能させるためには有効化する必要がある
        _inputSystem.Enable();
    }

    private void OnDestroy()
    {   //イベント情報を廃棄する
        _inputSystem.Dispose(); 
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
        //Debug.Log(_moveInputValue);
        //_rigidbody.AddForce(new Vector3(_moveInputValue.x * 10, 0, _moveInputValue.y * 10));
        //transform.positionに変えた
        _tr.position += new Vector3(_moveInputValue.x * 0.1f, 0, _moveInputValue.y * 0.1f);
    }
}
