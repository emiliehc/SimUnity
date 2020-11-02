using System;
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
    private bool m_Sprinting;
    private float m_Fov = 60.0f;
    private float m_FovTarget = 70.0f;
    private DateTime m_FovAnimationStartTime;
    private bool m_DoFovAnimation;

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
            Vector4 input = GetUserInput();
            
            if (Math.Abs(input.w - 1.0f) < 0.01f)
            {
                if (!m_Sprinting)
                {
                    m_Sprinting = true;
                    // start fov animation
                    m_FovAnimationStartTime = DateTime.Now;
                    m_DoFovAnimation = true;
                }

                if (m_DoFovAnimation)
                {
                    TimeSpan deltaTime = DateTime.Now - m_FovAnimationStartTime;
                    if (deltaTime.Milliseconds < 75)
                    {
                        // do animation
                        m_Camera.fieldOfView = Mathf.Lerp(m_Fov, m_FovTarget, deltaTime.Milliseconds / 100.0f);
                    }
                    else
                    {
                        m_DoFovAnimation = false;
                    }
                }
                else
                {
                    m_Camera.fieldOfView = m_FovTarget;
                }

            }
            else
            {
                if (m_Sprinting)
                {
                    m_Sprinting = false;
                    m_Camera.fieldOfView = m_Fov;
                    // start fov animation
                    m_FovAnimationStartTime = DateTime.Now;
                    m_DoFovAnimation = true;
                }

                if (m_DoFovAnimation)
                {
                    TimeSpan deltaTime = DateTime.Now - m_FovAnimationStartTime;
                    if (deltaTime.Milliseconds < 75)
                    {
                        // do animation
                        m_Camera.fieldOfView = Mathf.Lerp(m_FovTarget, m_Fov, deltaTime.Milliseconds / 100.0f);
                    }
                    else
                    {
                        m_DoFovAnimation = false;
                    }
                }
                else
                {
                    m_Camera.fieldOfView = m_Fov;
                }
            }

            input *= 25.0f * Time.deltaTime;
            if (m_Sprinting)
            {
                input *= 2.0f;
            }

            // up / down motion is absolute
            m_Camera.transform.Translate(new Vector3(0.0f, input.y, 0.0f), Space.World);
            // forward / backward / left / right motion is relative to the plane parallel to the ground through the camera
            m_Camera.transform.Translate(Vector3.ProjectOnPlane(m_Camera.transform.forward, Vector3.up).normalized * input.z, Space.World);
            m_Camera.transform.Translate(Vector3.ProjectOnPlane(m_Camera.transform.right, Vector3.up).normalized * input.x, Space.World);
        }
        // prevent rotation about the forward direction
        m_Camera.transform.rotation = Quaternion.Euler(m_Camera.transform.rotation.eulerAngles.x,
            m_Camera.transform.rotation.eulerAngles.y, 0.0f);
    }

    private Vector4 GetUserInput()
    {
        Vector4 input = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
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

        if (Input.GetKey(KeyCode.LeftControl))
        {
            input.w = 1.0f;
        }

        return input;
    }
}