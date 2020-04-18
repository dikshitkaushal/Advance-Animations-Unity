using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun_logic : MonoBehaviour
{
    [SerializeField]
    Transform spawnpos;
    [SerializeField]
    GameObject hitimpactpos;
    [SerializeField]
    AudioClip gunsound;
    [SerializeField]
    AudioClip emptygunsound;
    [SerializeField]
    AudioClip gunreloadsound;

    const int MAX_Bullet = 30;
    int m_currentbullet = MAX_Bullet;
    const float Maxcooldown = 0.1f;
    float m_currentcooldown = 0;
    AudioSource m_audiosource;
    MeshRenderer m_hitimpactrenderer;
    LineRenderer m_linerendrer;
    float m_linerendererlength = 10f;
    bool isreloading = false;
    Animator m_animator;



    // Start is called before the first frame update
    void Start()
    {
        m_audiosource = GetComponent<AudioSource>();
        m_hitimpactrenderer = hitimpactpos.GetComponent<MeshRenderer>();
        m_hitimpactrenderer.enabled = false;
        m_linerendrer = GetComponent<LineRenderer>();
        m_animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if(m_currentcooldown>0)
        {
            m_currentcooldown -= Time.deltaTime;
        }
        if(Input.GetButton("Fire1") && m_currentcooldown<=0 && !isreloading)
        {
            if (m_currentbullet > 0)
            {             
                shoot();               
            }
            else
            {
                playsound(emptygunsound);
            }
            m_currentcooldown = Maxcooldown;
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            reload();
        }
        updatelinerenderer();
    }

    private void reload()
    {
        isreloading = true;
        m_animator.SetTrigger("reloading");
        playsound(gunreloadsound);
        m_currentbullet = MAX_Bullet;
    }
    public void reload_state(bool reload)
    {
        isreloading = reload;
    }

    private void shoot()
    {
        m_animator.SetTrigger("shoot");
        --m_currentbullet;
        playsound(gunsound);
    }
    void playsound(AudioClip m_clip)
    {
        m_audiosource.PlayOneShot(m_clip);
    }
    void updatelinerenderer()
    {
        if(m_linerendrer)
        {
            m_linerendrer.SetPosition(0, spawnpos.position);
            Ray ray = new Ray(spawnpos.position, spawnpos.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, m_linerendererlength))
            {
                m_linerendrer.SetPosition(1, hit.point);
                hitimpactpos.transform.position = hit.point;
                m_hitimpactrenderer.enabled = true;
            }
            else
            {
                m_linerendrer.SetPosition(1, spawnpos.position + spawnpos.transform.forward * m_linerendererlength);
                m_hitimpactrenderer.enabled = false;
            }
           
        }
    }
}
