using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    [SerializeField] Transform leftarmpos;
    [SerializeField] Transform leftfoot;
    [SerializeField] Transform rightfoot;

    cameralogic_reboot fpcam;
    gun_logic gun;

    Vector3 movement_x;
    Vector3 movement_y;
    Vector3 jump;

    float horizontal_x;
    float vertical_y;
    float m_jumpheight = 0.25f;
    float m_gravity = 0.98f;
    float m_speed = 2f;

    bool isjumping = false;
    bool crouching = false;

    CharacterController m_charactercontroller;
    Animator m_animator;
    // Start is called before the first frame update
    void Start()
    {
        gun = GetComponentInChildren<gun_logic>();
        fpcam = GameObject.Find("Main Camera").GetComponent<cameralogic_reboot>();
        m_animator = GetComponent<Animator>();
        m_charactercontroller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)&&!isjumping)
        {
            crouching = !crouching;
            m_animator.SetBool("iscrouching", crouching);
        }
        if(crouching)
        {
            horizontal_x = 0;
            vertical_y = 0;
            return;
        }
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
        if(Mathf.Abs(horizontal_x)>0.1f || Mathf.Abs(vertical_y)>0.1f )
        {
            transform.forward = fpcam.getforwardvector();
        }
        if(m_charactercontroller)
        {
            m_charactercontroller.Move(movement_x + movement_y + jump);
        }
        if(m_charactercontroller.isGrounded)
        {
            jump.y = 0;
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if(m_animator)
        {
            m_animator.SetBoneLocalRotation(HumanBodyBones.Neck, Quaternion.Euler(fpcam.getrotation(), 0, 0));
            m_animator.SetBoneLocalRotation(HumanBodyBones.RightShoulder, Quaternion.Euler(fpcam.getrotation(), 0, 0));
            m_animator.SetBoneLocalRotation(HumanBodyBones.LeftShoulder, Quaternion.Euler(fpcam.getrotation(), 0, 0));
        }
       /* if (m_animator && gun.shootingteller())
        {
            m_animator.SetBoneLocalRotation(HumanBodyBones.Neck, Quaternion.Euler(0,fpcam.getrotationy(), 0));
            m_animator.SetBoneLocalRotation(HumanBodyBones.RightShoulder, Quaternion.Euler(0, fpcam.getrotationy(), 0));
            m_animator.SetBoneLocalRotation(HumanBodyBones.LeftShoulder, Quaternion.Euler(0, fpcam.getrotationy(), 0));
        }*/
        if (m_animator && leftarmpos)
        {
            if (!gun.reloadingteller())
            {
                m_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                m_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                m_animator.SetIKPosition(AvatarIKGoal.LeftHand, leftarmpos.position);
                m_animator.SetIKRotation(AvatarIKGoal.LeftHand, leftarmpos.rotation);
            }
            else
            {
                m_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                m_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            }

        }
        if (leftfoot)
        {
            updatefoot(AvatarIKGoal.LeftFoot, leftfoot);
        }
        if (rightfoot)
        {
            updatefoot(AvatarIKGoal.RightFoot, rightfoot);
        }
    }
    void updatefoot(AvatarIKGoal avatargoal,Transform foottransform)
    {
        Vector3 targetpos = foottransform.position;
        targetpos.y += 0.5f;
        Ray ray = new Ray(targetpos, Vector3.down);
        RaycastHit hit;
        LayerMask obstaclemask = LayerMask.GetMask("Obstacle");
        if(Physics.Raycast(ray,out hit,1.0f,obstaclemask))
        {
            Vector3 hitpos = hit.point;
            hitpos.y += 0.15f;
            m_animator.SetIKPositionWeight(avatargoal, 1);
            m_animator.SetIKRotationWeight(avatargoal, 1);
            m_animator.SetIKPosition(avatargoal, hitpos);
            m_animator.SetIKRotation(avatargoal, foottransform.rotation);
        }
    }
}
