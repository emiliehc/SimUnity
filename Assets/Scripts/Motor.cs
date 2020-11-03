using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Motor : MonoBehaviour
{
    public float MaxAngularSpeed = 20.0f;
    public float MinAngularSpeed = -20.0f;

    /// <summary>
    /// DO NOT MODIFY, FOR EDITOR USE ONLY
    /// </summary>
    public bool InvertRotationDirection;

    /// <summary>
    /// DO NOT MODIFY, FOR EDITOR USE ONLY
    /// </summary>
    public float TargetAngularSpeed;

    /// <summary>
    /// DO NOT MODIFY, FOR EDITOR USE ONLY
    /// </summary>
    public bool TargetAngularSpeedOverride;

    public float TargetAngularSpeedAbsolute
    {
        get => TargetAngularSpeed;
        set
        {
            if (!TargetAngularSpeedOverride)
            {
                TargetAngularSpeed = Mathf.Clamp(value, MinAngularSpeed, MaxAngularSpeed);
            }
        }
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
        if (Mathf.Abs(currentAngularSpeed - Mathf.Abs(TargetAngularSpeed)) > 1.0f)
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
        if (TargetAngularSpeed > 0.0f)
        {
            if (currentAngularSpeed < TargetAngularSpeed)
            {
                if (InvertRotationDirection)
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
    SerializedProperty InvertRotationDirection;
    SerializedProperty TargetAngularSpeedOverride;
    SerializedProperty TargetAngularSpeed;
    SerializedProperty MaxAngularSpeed;
    SerializedProperty MinAngularSpeed;

    void OnEnable()
    {
        InvertRotationDirection = serializedObject.FindProperty("InvertRotationDirection");
        TargetAngularSpeedOverride = serializedObject.FindProperty("TargetAngularSpeedOverride");
        TargetAngularSpeed = serializedObject.FindProperty("TargetAngularSpeed");
        MaxAngularSpeed = serializedObject.FindProperty("MaxAngularSpeed");
        MinAngularSpeed = serializedObject.FindProperty("MinAngularSpeed");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(InvertRotationDirection);
        EditorGUILayout.PropertyField(TargetAngularSpeedOverride);
        EditorGUILayout.PropertyField(TargetAngularSpeed);
        EditorGUILayout.PropertyField(MaxAngularSpeed);
        EditorGUILayout.PropertyField(MinAngularSpeed);
        serializedObject.ApplyModifiedProperties();
    }
}