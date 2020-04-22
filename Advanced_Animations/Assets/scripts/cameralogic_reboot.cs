using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameralogic_reboot : MonoBehaviour
{
    Transform player;
    Vector3 cameratarget;
    Vector3 cameraoffset;

    float m_mouse_x;
    float m_mouse_y;
    public float lowerbound = -20f;
    public float upperbound = 20f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        cameratarget = player.position;
        cameratarget.y += 2f;
        m_mouse_y += Input.GetAxis("Mouse X");
        m_mouse_x -= Input.GetAxis("Mouse Y");

        

        m_mouse_x = Mathf.Clamp(m_mouse_x, lowerbound, upperbound);
    }
    private void LateUpdate()
    {
        Quaternion rot = Quaternion.Euler(m_mouse_x, m_mouse_y, 0);
        cameraoffset = new Vector3(0, 0, -2.49f);

        transform.position = cameratarget + rot * cameraoffset;
        transform.LookAt(cameratarget);
    }
    public Vector3 getforwardvector()
    {
        Quaternion rotx = Quaternion.Euler(0, m_mouse_y, 0);
        return rotx * Vector3.forward;
    }
    public float getrotation()
    {
        return m_mouse_x;
    }
    public float getrotationy()
    {
        return m_mouse_y;
    }
}
