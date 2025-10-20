using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerControler : MonoBehaviour
{
    Transform T;
    Keyboard Current;
    ForceMode ForceMode;

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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        T = transform;
        Current = Keyboard.current;
        ForceMode = ForceMode.Impulse;

        JumpF = new Vector3(0, 5, 0);
        _rb = GetComponent<Rigidbody>();
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
            T.position -= T.forward * Speed;
        }

        if (Current.aKey.isPressed)
        {
            T.position += T.right * Speed;
        }

        if (Current.dKey.isPressed)
        {
            T.position -= T.right * Speed;
        }

        //Jump
        if (Current.spaceKey.isPressed)
        {
            Jumping();
        }

        Ground();
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
                _rb.AddForce(JumpF, ForceMode.Impulse);
                isJumping = true;

                Debug.Log("jump");
            }
        }
        Ground();
    }
    private void Ground()
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
    public bool CheckGroundStatus()
    {
        direction = Vector3.down;
        position = transform.position + offset;
        Ray ray = new Ray(position, direction);
        Debug.DrawRay(position, direction * distance, Color.red);

        return Physics.Raycast(ray, distance, groundLayers);
    }
}
