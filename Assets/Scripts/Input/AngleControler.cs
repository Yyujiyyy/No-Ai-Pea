using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class AngleControler : MonoBehaviour
{
    Transform _tr;

    private InputSystem_Actions _inputSystem;
    private Vector2 _mousePos;

    [SerializeField,Range(0,1)]private float _sensitivity;
    float yaw, pitch;

    // 壁判定を行う対象レイヤーマスク
    [SerializeField, Header("Climb関連")] LayerMask wallLayers = 0;
    // 原点から見たRayの始点弄るためのoffset
    [SerializeField] Vector3 offset = new Vector3(0, 0f, 1f);
    private Vector3 direction, position;
    // Rayの長さ
    [SerializeField] float distance = 2f;

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

    private void Start()
    {
        this._tr.rotation = Quaternion.identity;
    }

    private void OnDestroy()
    {   //イベント情報を廃棄する
        _inputSystem.Dispose();//メモリの確保のため？C++に変換するときに楽になる？
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        _mousePos = context.ReadValue<Vector2>();
    }
    
    private void Update()
    {
        yaw += _mousePos.x * _sensitivity;
        //-=    上を向く＝角度をマイナスにする
        pitch -= _mousePos.y * _sensitivity;

        //Debug.Log(_mousePos);

        if (pitch <= -90)
        {
            pitch = -90;
        }
        else if (90 <= pitch)
        {
            pitch = 90;
        }
        
        //x,yは逆
        this._tr.rotation = Quaternion.Euler(pitch, yaw, 0);
        //eulerAnglesで+=してしまうとジンバルロックが起こり想定した挙動と異なってしまう
    }

    Ray ray;
    RaycastHit _raycastHit;

    /// <summary>
    /// 目の前に壁があるかどうかを判定
    /// </summary>
    /// <returns></returns>
    public bool CheckWallStatus()
    {
        direction = MoveControl._camera.transform.forward;
        position = transform.position + offset;
        ray = new Ray(position, direction);
        Debug.DrawRay(position, direction * distance, Color.red);

        return Physics.Raycast(ray, distance, wallLayers);
    }

     public RaycastHit WhereWallCheck()
    {
        Physics.Raycast(ray, out _raycastHit, distance);
        return _raycastHit;
    }
}
