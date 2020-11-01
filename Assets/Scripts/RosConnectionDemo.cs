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

public class RosConnectionDemo : MonoBehaviour
{
    private RosSocket m_Socket;
    private int m_Counter = 0;

    void Start()
    {
        RosConnector connector = GetComponent<RosConnector>();
        m_Socket = connector.RosSocket;
        m_Socket.Subscribe<Int32>("/topic", msg => { Debug.Log(msg.data); });
        m_Socket.Subscribe<ArmMotorCommand>("/arm_control_data", msg =>
        {
            for (int i = 0; i < 6; i++)
            {
                Debug.Log($"Velocity output of motor #{i} is {msg.MotorVel[i]}");
            }
        });
        m_Socket.Advertise<Int32>("/topic");
        m_Socket.Advertise<ProcessedControllerInput>("/processed_arm_controller_input");
    }

    private static readonly Random s_RandomInstance = new Random();

    void Update()
    {
        m_Socket.Publish("/topic", new Int32() {data = m_Counter});
        m_Counter++;

        float[] randomInput = new float[6];
        for (int i = 0; i < 6; i++)
        {
            randomInput[i] = (float) (s_RandomInstance.NextDouble() * 2 - 1);
        }

        m_Socket.Publish("/processed_arm_controller_input", new ProcessedControllerInput
        {
            ControllerInput = randomInput
        });
    }
}