using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Motor : MonoBehaviour
{
    public float MaxAngularSpeed = 20.0f;
    public float MinAngularSpeed = -20.0f;

    // DO NOT MODIFY, FOR EDITOR USE ONLY
    public float m_AngularSpeed;

    public float AngularSpeedAbsolute
    {
        get => m_AngularSpeed;
        set => m_AngularSpeed = Mathf.Clamp(value, MinAngularSpeed, MaxAngularSpeed);
    }

    public float TestProp;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = float.MaxValue;
        
        InvokeRepeating("CheckWheelSpeed", 5.0f, 1.0f);
    }

    void CheckWheelSpeed()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float currentAngularSpeed = Vector3.Project(rb.angularVelocity, transform.forward).magnitude;
        if (Mathf.Abs(currentAngularSpeed - Mathf.Abs(m_AngularSpeed)) > 1.0f)
        {
            Debug.LogWarning($"Wheel speed above / below from target: {currentAngularSpeed}");
        }
    }

    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        //Debug.Log(transform.forward);
        //Debug.Log(rb.angularVelocity);
        //Debug.Log(Vector3.Cross(rb.angularVelocity.normalized, transform.forward.normalized).magnitude);
        float currentAngularSpeed = Vector3.Project(rb.angularVelocity, transform.forward).magnitude;
        //switch (name)
        //{
        //    //Debug.Log(currentAngularSpeed);
        //    //rb.AddTorque(new Vector3(0.0f, 0.0f, 100.0f), ForceMode.Force);
        //    case "WheelFL":
        //    case "WheelRL":
        //    {
        //        //Debug.Log(transform.localEulerAngles);
        //        Vector3 angles = transform.localEulerAngles;
        //        angles.x = 90.0f;
        //        transform.localEulerAngles = angles;
        //        break;
        //    }
        //    case "WheelFR":
        //    case "WheelRR":
        //    {
        //        //Debug.Log(transform.localEulerAngles);
        //        Vector3 angles = transform.localEulerAngles;
        //        angles.x = -90.0f;
        //        transform.localEulerAngles = angles;
        //        break;
        //    }
        //    default:
        //    {
        //        Debug.Break();
        //        break;
        //    }
        //}
        if (m_AngularSpeed > 0.0f)
        {
            if (currentAngularSpeed < m_AngularSpeed)
            {
                if (gameObject.name == "WheelRR" || gameObject.name == "WheelFR")
                {
                    rb.AddRelativeTorque(new Vector3(0.0f, 0.0f, -20.0f), ForceMode.Force);
                }
                else
                {
                    rb.AddRelativeTorque(new Vector3(0.0f, 0.0f, 20.0f), ForceMode.Force);
                }
            }
        }
    }

}

[CustomEditor(typeof(Motor))]
[CanEditMultipleObjects]
public class MotorEditor : Editor
{
    SerializedProperty AngularSpeedAbsolute;
    SerializedProperty MaxAngularSpeed;
    SerializedProperty MinAngularSpeed;

    void OnEnable()
    {
        AngularSpeedAbsolute = serializedObject.FindProperty("m_AngularSpeed");
        MaxAngularSpeed = serializedObject.FindProperty("MaxAngularSpeed");
        MinAngularSpeed = serializedObject.FindProperty("MinAngularSpeed");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(AngularSpeedAbsolute);
        EditorGUILayout.PropertyField(MaxAngularSpeed);
        EditorGUILayout.PropertyField(MinAngularSpeed);
        serializedObject.ApplyModifiedProperties();
    }
}
