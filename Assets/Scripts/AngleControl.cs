using UnityEngine.InputSystem;
using UnityEngine;

public class AngleControl : MonoBehaviour
{
    Transform T;

    float MouseposX, MouseposY;
    float NextMouseX, NextMouseY;
    Vector2 PreviousMousePos;
    [SerializeField] float Sensitivity = 0.1f;

    public Vector3 Direction;

    [SerializeField] GameObject Body;
    [SerializeField] GameObject Camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        T = transform;

        Cursor.lockState = CursorLockMode.Locked;       //カーソルロックする

        PreviousMousePos = Mouse.current.position.ReadValue();

        Debug.Log(PreviousMousePos);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(MouseposX + "    " + MouseposY);

        MousePosition();

        Debug.Log(T.eulerAngles);

        Direction = T.forward;          //現在の向きを取得

        if (90 <= T.eulerAngles.x) //XをClamp
        {//localEulerAnglesはQuaternion型なので変数を使わないといけない
            var Clamp = T.eulerAngles;
            Clamp.x = 90;
            T.eulerAngles = Clamp;
        }
        else if (T.localEulerAngles.x <= -90)
        {
            var Clamp = T.eulerAngles;
            Clamp.x = -90;
            T.eulerAngles = Clamp;
        }

        //Camera.transform.position = T.position;
        //Camera.transform.rotation = T.rotation;

        PreviousMousePos = Mouse.current.position.ReadValue();
    }

    void MousePosition()
    {
        NextMouseX = Mouse.current.position.ReadValue().x;
        NextMouseY = Mouse.current.position.ReadValue().y;

        MouseposX = (NextMouseX - PreviousMousePos.x) * Sensitivity;
        MouseposY = (NextMouseY - PreviousMousePos.y) * Sensitivity;

         T.Rotate(NextMouseX, NextMouseY, 0, Space.Self);      //計算結果を反映
        //Space.Selfとは、今向いている方向に対して回転するという指定
    }
}
