using UnityEngine;
using UnityEngine.InputSystem;

//RequireComponentを使ったスクリプトをゲームオブジェクトにアタッチすると、
//必要なコンポーネントが自動的にそのゲームオブジェクトに加えられるようになる
[RequireComponent(typeof(Rigidbody))]
public class MoveControl : MonoBehaviour
{
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
    
    [SerializeField,Header("移動関連")]
    private Vector3 _movedir;
    private Vector3 _w, _a, _s, _d;
    public static Camera _camera;
    [SerializeField] GameObject _playerHead;
    private float _speed = 0.05f;

    private bool _onWall;
    RaycastHit _frontWall;

    [SerializeField] AngleControler _angleControler;

    private void Awake()
    {
        //インスタンス化
        _rigidbody = GetComponent<Rigidbody>();
        _inputSystem = new InputSystem_Actions();
        _tr = transform;

        // Actionイベント登録
        //移動
        _inputSystem.Player.Move.started += OnMove;
        _inputSystem.Player.Move.performed += OnMove;
        _inputSystem.Player.Move.canceled += OnMove;
        //ジャンプ
        _inputSystem.Player.Jump.performed += OnJump;
        //しゃがみ
        _inputSystem.Player.Crouch.started += OnCrouch;
        _inputSystem.Player.Crouch.performed += OnCrouch;
        _inputSystem.Player.Crouch.canceled += OnCrouch;
        //ダッシュ
        _inputSystem.Player.Sprint.started += OnSprint;
        _inputSystem.Player.Sprint.performed += OnSprint;
        _inputSystem.Player.Sprint.canceled += OnSprint;
        //つかみ
        _inputSystem.Player.Attack.started += OnGrab;
        _inputSystem.Player.Attack.performed += OnGrab;
        _inputSystem.Player.Attack.canceled += OnGrab;

        // Input Actionを機能させるためには有効化する必要がある
        _inputSystem.Enable();

        //ジャンプ関連
        isJumping = false;

        //カメラ関連
        _camera = Camera.main;

        _w = _playerHead.transform.forward;
        _s = _playerHead.transform.forward;
        _a = _playerHead.transform.right;
        _d = _playerHead.transform.right;

        //Climb
        _onWall = false;
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
        if(context.started)
            _speed = 0.1f;
        else if(context.canceled)
            _speed = 0.05f;
        
    }

    /// <summary>
    /// 始点の目の前に壁があるかつ左クリックをしている間登る
    /// </summary>
    /// <param name="context"></param>
    private void OnGrab(InputAction.CallbackContext context)
    {
        if(_onWall)
            _angleControler.WhereWallCheck();
    }

    private void Update()
    {
        if (_angleControler.CheckWallStatus())
        {
            //壁をつかむ
            _onWall = true;
        }
        else
        {
            _onWall = false;
        }

        MoveOfPlayer();
        //移動キーで上下に動かないようにする
        _movedir.y = 0;
        this._tr.position += _movedir * _speed;

        JumpJudge();
        //Debug.Log(_moveInputValue);

        if (_onWall)
            Debug.Log("hai");
    }

    /// <summary>
    /// カメラの向きを参照する移動
    /// </summary>
    void MoveOfPlayer()
    {   //カメラの方向取得
        _w = _playerHead.transform.forward;
        _s = _playerHead.transform.forward;
        _a = _playerHead.transform.right;
        _d = _playerHead.transform.right;
        
        //初期化
        _movedir = Vector3.zero;

        //登り
        if(_onWall)
        {
            //重力をオフ
            _rigidbody.useGravity = false;
            //目の前にある壁の情報を取得
            _frontWall = _angleControler.WhereWallCheck();
            //取得した壁に沿って垂直に移動する
            _w = _frontWall.transform.up;
            _s = -_frontWall.transform.up;
            _a = _frontWall.transform.right;
            _d = -_frontWall.transform.right;

            WASD();
        }
        //地上
        if (!_onWall)
        {
            WASD();
        }
        //0.7より小さい数字じゃないとダメ
        void WASD()
        {
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
