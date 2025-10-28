using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerControler : MonoBehaviour
{
    Transform T;
    Keyboard Current;
    ForceMode _ForceMode;

    [Header("Jump")]
    // 接地判定を行う対象レイヤーマスク
    [SerializeField] LayerMask groundLayers = 0;
    // 原点から見たRayの始点弄るためのoffset
    [SerializeField] Vector3 offset = new Vector3(0, 0.1f, 0f);
    private Vector3 direction, position;
    // Rayの長さ
    [SerializeField] float distance = 0.35f;
    /*
    Boolで返す。
    Rayの範囲にgroundLayersで指定したレイヤーが存在するかどうか
    */
    bool isJumping = false;
    [Tooltip("ジャンプの強さ")] Vector3 JumpF;
    Rigidbody _rb;
    [SerializeField] GameObject _foot;

    //Move
    private float Speed = 0.1f;
    private float BackSpeed = 0.1f;

    //Climb
    //[SerializeField] private Vector3 flontoffset = new Vector3.forward;
    [SerializeField] LayerMask WallLayers = 0;
    private float Walldistance = 0.3f;
    private Vector3 JumpW;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        T = transform;
        Current = Keyboard.current;
        _rb = GetComponent<Rigidbody>();

        _ForceMode = ForceMode.Impulse;
        JumpF = new Vector3(0, 5, 0);
        JumpW = new Vector3(0, 3, 0);

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();




    }

    void PlayerMove()
    {
        //WASD移動
        if (Current.wKey.isPressed)
        {
            T.position += T.forward * Speed;
        }

        if (Current.sKey.isPressed)
        {
            T.position -= T.forward * BackSpeed;    //後方にダッシュはしない
        }

        if (Current.dKey.isPressed)
        {
            T.position += T.right * Speed;
        }

        if (Current.aKey.isPressed)
        {
            T.position -= T.right * Speed;
        }

        //Jump
        if (Current.spaceKey.isPressed)
        {
            Jumping();
        }

        Ground();

        //dash
        if (Current.leftShiftKey.isPressed)
        {
            Speed = 0.15f;
        }
    }

    //地面判定
    void Jumping()
    {
        //Jump
        //Debug.Log(isJumping);
        if (!isJumping)
        {
            if (Current.spaceKey./*押された瞬間*/wasPressedThisFrame)
            {
                _rb.AddForce(JumpF, _ForceMode);
                isJumping = true;

                Debug.Log("jump");
            }
        }
        Ground();
    }
    private void Ground()
    {
        if (CheckGroundStatus())        //Raycastは何かに当たったらtrueを返す
        {
            isJumping = false;
        }
        else
        {
            isJumping = true;
        }
    }
    public bool CheckGroundStatus()
    {
        direction = Vector3.down;
        position = transform.position + offset;
        Ray ray = new Ray(position, direction);
        Debug.DrawRay(position, direction * distance, Color.red);

        return Physics.Raycast(ray, distance, groundLayers);
    }

    void Climb()
    {
        if (CheckWallStatus())
        {
            _rb.AddForce(JumpW, _ForceMode);
            //transform.position = カーソルを合わせた位置
        }
    }

    public bool CheckWallStatus()
    {
        var direction = Vector3.forward;
        position = transform.position + direction * 0.1f;
        Ray ray = new Ray(position, direction);
        Debug.DrawRay(position, direction * Walldistance, Color.green);
        return Physics.Raycast(ray, Walldistance, WallLayers);
    }
}
