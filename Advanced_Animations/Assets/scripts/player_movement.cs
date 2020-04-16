using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    Vector3 movement_x;
    Vector3 movement_y;
    Vector3 jump;

    float horizontal_x;
    float vertical_y;
    float m_jumpheight = 0.25f;
    float m_gravity = 0.98f;
    float m_speed = 2f;

    bool isjumping = false;

    CharacterController m_charactercontroller;
    Animator m_animator;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_charactercontroller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal_x = Input.GetAxis("Horizontal");
        vertical_y = Input.GetAxis("Vertical");

        if(m_animator)
        {
            m_animator.SetFloat("movement_x", horizontal_x);
            m_animator.SetFloat("movement_y", vertical_y);
        }
        if(Input.GetButtonDown("Jump") && m_charactercontroller.isGrounded)
        {
            isjumping = true;
        }
    }
    private void FixedUpdate()
    {
        if(isjumping)
        {
            jump.y = m_jumpheight;
            isjumping = false;
        }
        jump.y -= m_gravity * Time.deltaTime;
        movement_x = transform.right * horizontal_x * m_speed * Time.deltaTime;
        movement_y = transform.forward * vertical_y * m_speed * Time.deltaTime;
        if(m_charactercontroller)
        {
            m_charactercontroller.Move(movement_x + movement_y + jump);
        }
        if(m_charactercontroller.isGrounded)
        {
            jump.y = 0;
        }
    }
}
