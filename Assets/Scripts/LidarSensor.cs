using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarSensor : MonoBehaviour
{
    public float MaxAngle = 10;
    public float MinAngle = -10;
    public int NumberOfLayers = 16;
    public int NumberOfIncrements = 360;
    public float MaxRange = 100f;

    float m_VertIncrement;
    float m_AzimutIncrAngle;

    [HideInInspector] public float[] m_Distances;
    public float[] m_Azimuts;


    // Use this for initialization
    void Start()
    {
        m_Distances = new float[NumberOfLayers * NumberOfIncrements];
        m_Azimuts = new float[NumberOfIncrements];
        m_VertIncrement = (float) (MaxAngle - MinAngle) / (float) (NumberOfLayers - 1);
        m_AzimutIncrAngle = (float) (360.0f / NumberOfIncrements);
    }

    void FixedUpdate()
    {
        Vector3 fwd = new Vector3(0, 0, 1);
        Vector3 dir;
        RaycastHit hit;
        int indx = 0;
        float angle;

        for (int incr = 0; incr < NumberOfIncrements; incr++)
        {
            for (int layer = 0; layer < NumberOfLayers; layer++)
            {
                //print("incr "+ incr +" layer "+layer+"\n");
                indx = layer + incr * NumberOfLayers;
                angle = MinAngle + (float) layer * m_VertIncrement;
                m_Azimuts[incr] = incr * m_AzimutIncrAngle;
                dir = transform.rotation * Quaternion.Euler(-angle, m_Azimuts[incr], 0) * fwd;

                if (Physics.Raycast(transform.position, dir, out hit, MaxRange))
                {
                    m_Distances[indx] = (float) hit.distance;
                    Debug.DrawRay(transform.position, dir * hit.distance, Color.green);
                }
                else
                {
                    m_Distances[indx] = 100.0f;
                }
            }
        }
    }
}