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
    bool isaiming = false;
    float aimposx = 0.65f;
    float aimposy = 1.55f;
    float aimposz = -0.7f;
    float m_aimrotationy;

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
        if (Input.GetButtonDown("Fire3"))
        {
            isaiming = !isaiming;
            if (isaiming)
            {
                m_aimrotationy = m_mouse_y;
                player.transform.rotation = Quaternion.Euler(0, m_aimrotationy, 0);
            }
            else
            {
                m_mouse_y = m_aimrotationy;
            }
        }


        m_mouse_x = Mathf.Clamp(m_mouse_x, lowerbound, upperbound);
    }
    private void LateUpdate()
    {
        if (isaiming)
        {
            Quaternion rot = Quaternion.Euler(m_mouse_x, m_mouse_y, 0);
            cameraoffset = new Vector3(0, 0, -2.49f);

            transform.position = cameratarget + rot * cameraoffset;
            transform.LookAt(cameratarget);
        }
        else
        {
            cameratarget = player.transform.position;
            Vector3 cameraoffset = new Vector3(aimposx, aimposy, aimposz);
            Quaternion camerarot = player.transform.rotation;
            transform.position = cameratarget + camerarot * cameraoffset;
            transform.rotation = Quaternion.Euler(0, m_aimrotationy, 0);
        }
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
