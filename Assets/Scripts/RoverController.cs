using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverController : MonoBehaviour
{
    private GameObject[] m_Wheels;

    void Start()
    {
        m_Wheels = new GameObject[4];
        m_Wheels[0] = GameObject.Find("WheelFL");
        m_Wheels[1] = GameObject.Find("WheelRL");
        m_Wheels[2] = GameObject.Find("WheelFR");
        m_Wheels[3] = GameObject.Find("WheelRR");
    }

    void Update()
    {
        
    }
}
