using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.Messages;
using RosSharp.RosBridgeClient.Messages.Standard;
using Int32 = RosSharp.RosBridgeClient.Messages.Standard.Int32;
using Random = System.Random;
using Time = RosSharp.RosBridgeClient.Messages.Standard.Time;

public class RosConnection : MonoBehaviour
{
    private RosSocket m_Socket;

    public static RosSocket RosConnectionSocket =>
        GameObject.Find("RosConnection").GetComponent<RosConnector>().RosSocket;

    void Start()
    {
        RosConnector connector = GetComponent<RosConnector>();
        m_Socket = connector.RosSocket;
        m_Socket.Subscribe<ArmMotorCommand>("/arm_control_data", msg =>
        {
            for (int i = 0; i < 6; i++)
            {
                Debug.Log($"Velocity output of motor #{i} is {msg.MotorVel[i]}");
            }
        });
        m_Socket.Advertise<ProcessedControllerInput>("/processed_arm_controller_input");
        m_Socket.Advertise<WheelSpeed>("/WheelSpeed");
        //m_Socket.Advertise<WheelSpeed>("/TestTopic");
        //m_Socket.Subscribe<WheelSpeed>("/WheelSpeed", speed =>
        //{
        //    Debug.Log(speed.Wheel_Speed[0]);
        //    Debug.Log(speed.Wheel_Speed[1]);
        //});
        //m_Socket.Subscribe<WheelSpeed>("/TestTopic", num =>
        //{
        //    //Debug.Log(num.Wheel_Speed);
        //});
    }

    private static readonly Random s_RandomInstance = new Random();

    void Update()
    {
        float[] randomInput = new float[6];
        for (int i = 0; i < 6; i++)
        {
            randomInput[i] = (float) (s_RandomInstance.NextDouble() * 2 - 1);
        }

        m_Socket.Publish("/processed_arm_controller_input", new ProcessedControllerInput
        {
            ControllerInput = randomInput
        });

        m_Socket.Publish("/WheelSpeed", new WheelSpeed
        {
            Wheel_Speed = new[] { 5.0f, 5.0f }
        });

        //m_Socket.Publish("/TestTopic", new WheelSpeed());
    }
}