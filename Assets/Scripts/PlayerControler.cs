using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    Transform T;

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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        T = transform;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();




    }

    void PlayerMove()
    {
        //WASD移動
        if (Input.GetKey(KeyCode.W))
        {
            T.position += T.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            T.position -= T.forward;
        }

        if (Input.GetKey(KeyCode.D))
        {
            T.position += T.right;
        }

        if (Input.GetKey(KeyCode.A))
        {
            T.position -= T.right;
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
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
            if (Input.GetKeyDown(KeyCode.Space))
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
