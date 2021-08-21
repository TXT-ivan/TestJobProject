using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCam : MonoBehaviour
{
    public Transform Target;

    // Вращение
    private float MouseSens = 2.5f;

    // Приближение
    public float FOVmin = 20.0f;
    public float FOVmax = 100.0f;
    public float MouseWheelSpeed = 5.0f;

    // Перемещение
    float MainSpeed = 20.0f;
    float ShiftAdd = 250.0f;
    float MaxShift = 1000.0f;
    private float totalRun = 1.0f;
    Vector3 hit_position = Vector3.zero;
    Vector3 current_position = Vector3.zero;
    Vector3 camera_position = Vector3.zero;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            hit_position = Input.mousePosition;
            camera_position = transform.position;

        }
        if (Input.GetMouseButton(2))
        {
            current_position = Input.mousePosition;
            MiddleMouseDrag();
        }
        if (Input.GetMouseButton(1))
        {
            Rotate();
        }

        Vector3 p = GetBaseInput();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalRun += Time.deltaTime;
            p = p * totalRun * ShiftAdd;
            p.x = Mathf.Clamp(p.x, -MaxShift, MaxShift);
            p.y = Mathf.Clamp(p.y, -MaxShift, MaxShift);
            p.z = Mathf.Clamp(p.z, -MaxShift, MaxShift);
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * MainSpeed;
        }

        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else
        {
            transform.Translate(p);
        }

        ZoomFOV();
    }

    /// <summary>
    /// Вращение камеры
    /// </summary>
    private void Rotate()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        transform.RotateAround(Target.position, Vector3.up, x * MouseSens * 300 * Time.deltaTime);
    }

    /// <summary>
    /// Перемещение камеры по нажатию на центральную кнопку мыши
    /// </summary>
    private void MiddleMouseDrag()
    {
        current_position.z = hit_position.z = camera_position.y;

        Vector3 direction = Camera.main.ScreenToWorldPoint(current_position) - Camera.main.ScreenToWorldPoint(hit_position);
        direction = direction * -1;
        Vector3 position = camera_position + direction;

        transform.position = position;
    }

    /// <summary>
    /// Приближение колесиком мыши
    /// </summary>
    private void ZoomFOV()
    {

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {

            GetComponent<Camera>().fieldOfView = GetComponent<Camera>().fieldOfView - MouseWheelSpeed;

            if (GetComponent<Camera>().fieldOfView <= FOVmin) { GetComponent<Camera>().fieldOfView = FOVmin; }

        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {

            GetComponent<Camera>().fieldOfView = GetComponent<Camera>().fieldOfView + MouseWheelSpeed;

            if (GetComponent<Camera>().fieldOfView >= FOVmax) { GetComponent<Camera>().fieldOfView = FOVmax; }

        }

    }

    /// <summary>
    /// Передвижение по клавишам WASD
    /// </summary>
    private Vector3 GetBaseInput()
    {
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
}
