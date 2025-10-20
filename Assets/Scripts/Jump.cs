using UnityEngine;

public class Jump : MonoBehaviour
{
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

    public bool CheckGroundStatus()
    {
        direction = Vector3.down;
        position = transform.position + offset;
        Ray ray = new Ray(position, direction);
        Debug.DrawRay(position, direction * distance, Color.red);

        return Physics.Raycast(ray, distance, groundLayers);
    }
}
