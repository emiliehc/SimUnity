using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraController : MonoBehaviour
{
    private Camera m_Camera;
    private bool m_MouseLocked;

    void Start()
    {
        m_Camera = GetComponent<Camera>();
        // look at the rover
        m_Camera.transform.LookAt(GameObject.Find("Rover").transform);
    }

    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (Input.GetKey(KeyCode.Escape))
        {
            m_MouseLocked = false;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetMouseButton(0))
        {
            m_MouseLocked = true;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (m_MouseLocked)
        {
            m_Camera.transform.Rotate(Vector3.up, 300.0f * Time.deltaTime * Input.GetAxis("Mouse X"));
            m_Camera.transform.Rotate(Vector3.right, -300.0f * Time.deltaTime * Input.GetAxis("Mouse Y"));
            m_Camera.transform.Translate(50.0f * Time.deltaTime * GetUserInput()); // relative
        }
        // prevent rotation about the forward direction
        m_Camera.transform.rotation = Quaternion.Euler(m_Camera.transform.rotation.eulerAngles.x,
            m_Camera.transform.rotation.eulerAngles.y, 0.0f);
    }

    private Vector3 GetUserInput()
    {
        Vector3 input = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            input.z = 1.0f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            input.x = -1.0f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            input.z = -1.0f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            input.x = 1.0f;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            input.y = 1.0f;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            input.y = -1.0f;
        }

        return input;
    }
}