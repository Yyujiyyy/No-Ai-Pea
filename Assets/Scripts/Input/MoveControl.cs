using UnityEngine;
using UnityEngine.InputSystem;

//RequireComponentを使ったスクリプトをゲームオブジェクトにアタッチすると、
//必要なコンポーネントが自動的にそのゲームオブジェクトに加えられるようになる
[RequireComponent(typeof(Rigidbody))]
public class MoveControl : MonoBehaviour
{
    [SerializeField] private float _speed = 0.1f;
    [SerializeField] private float _jumpForce = 5;

    private InputSystem_Actions _inputSystem;
    private Rigidbody _rigidbody;
    private Vector2 _moveInputValue;
    private Transform _tr;

    // 接地判定を行う対象レイヤーマスク
    [SerializeField, Header("Jump関連")] LayerMask groundLayers = 0;
    // 原点から見たRayの始点弄るためのoffset
    [SerializeField] Vector3 offset = new Vector3(0, 0.1f, 0f);
    private Vector3 direction, position;
    // Rayの長さ
    [SerializeField] float distance = 0.35f;
    bool isJumping;

    [Header("移動関連")]
    private Vector3 _movedir;
    private Vector3 _w, _a, _s, _d;
    public static Camera _camera;

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

        _inputSystem.Player.Crouch.started += OnCrouch;
        _inputSystem.Player.Crouch.performed += OnCrouch;
        _inputSystem.Player.Crouch.canceled += OnCrouch;
        _inputSystem.Player.Sprint.started += OnSprint;
        _inputSystem.Player.Sprint.performed += OnSprint;
        _inputSystem.Player.Sprint.canceled += OnSprint;
        
        // Input Actionを機能させるためには有効化する必要がある
        _inputSystem.Enable();

        //ジャンプ関連
        isJumping = false;

        //カメラ関連
        _camera = Camera.main;

        _w = _camera.transform.forward;
        _s = _camera.transform.forward;
        _a = _camera.transform.right;
        _d = _camera.transform.right;
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
        //_rigidbody.AddForce(Vector3.up * _speed, ForceMode.Impulse);

        if (!isJumping)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);

            //Debug.Log("jump");
        }
    }

    private void OnCrouch(InputAction.CallbackContext context)
    {

    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        _speed *= 2;
    }

    private void Update()
    {
        MoveOfPlayer();
        //移動キーで上下に動かないようにする
        _movedir.y = 0;
        this._tr.position += _movedir * _speed;

        JumpJudge();

        //Debug.Log(_moveInputValue);
    }

    /// <summary>
    /// カメラの向きを参照する移動
    /// </summary>
    void MoveOfPlayer()
    {   //カメラの方向取得
        _w = _camera.transform.forward;
        _s = _camera.transform.forward;
        _a = _camera.transform.right;
        _d = _camera.transform.right;
        
        //初期化
        _movedir = Vector3.zero;

        switch (_moveInputValue.x)
        {   //-0.5以下
            case <= -0.5f:
                _movedir -= _d;
                break;
            //0.5以上
            case >= 0.5f:
                _movedir += _a;
                break;
        }

        switch (_moveInputValue.y)
        {
            //-0.5以下
            case <= -0.5f:
                _movedir -= _s;
                break;
            //0.5以上
            case >= 0.5f:
                _movedir += _w;
                break;
        }
    }

    /// <summary>
    ///ジャンプ中か判定 
    /// </summary>
    void JumpJudge()
    {
        if (CheckGroundStatus())
        {
            isJumping = false;
        }
        else
        {
            isJumping = true;
        }
    }

    ///<summary>
    ///Rayの範囲にgroundLayersで指定したレイヤーが存在するかどうかをBoolで返す。
    /// </summary>
    public bool CheckGroundStatus()
    {
        direction = Vector3.down;
        position = transform.position + offset;
        Ray ray = new Ray(position, direction);
        Debug.DrawRay(position, direction * distance, Color.red);

        return Physics.Raycast(ray, distance, groundLayers);
    }
}
