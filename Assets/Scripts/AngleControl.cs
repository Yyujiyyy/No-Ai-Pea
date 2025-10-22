using UnityEngine.InputSystem;
using UnityEngine;

public class AngleControl : MonoBehaviour
{
    Transform T;

    float MouseposX, MouseposY;
    float NextMouseX, NextMouseY;
    Vector2 PreviousMousePos;
    float Sensitivity = 0.1f;
    Vector3 Angle;

    public Vector3 Direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        T = transform;

        

        Cursor.visible = false;
        
        PreviousMousePos = Mouse.current.position.ReadValue();
        //Debug.Log(PreviousMousePos);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(MouseposX + "    " + MouseposY);
        MousePosition();

        Direction = T.eulerAngles.normalized;
    }

    Vector3 MousePosition()
    {
        NextMouseX = Mouse.current.position.ReadValue().x;
        NextMouseY = Mouse.current.position.ReadValue().y;

        MouseposX = NextMouseX - PreviousMousePos.x;
        MouseposY = NextMouseY - PreviousMousePos.y;

        T.eulerAngles = new Vector3(MouseposX, MouseposY);      //計算結果を反映

        Angle = T.eulerAngles * Sensitivity;                    //感度調節

        return Angle;                                           //Angleを返す
    }
}
