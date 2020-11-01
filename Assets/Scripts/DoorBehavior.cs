using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    private GameObject m_Door;

    // Start is called before the first frame update
    void Start()
    {
        m_Door = transform.parent.gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        // open door
        if (other.CompareTag("Player"))
        {
            m_Door.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // close door
        if (other.CompareTag("Player"))
        {
            m_Door.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}