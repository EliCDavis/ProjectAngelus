using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : NetworkBehaviour {

    [SerializeField]
    private Transform m_Head;
    [SerializeField]
    private Transform m_Arm;

    private Vector3 m_Velocity = Vector3.zero;
    private Vector3 m_Rotation = Vector3.zero;
    private float m_CameraRotationX = 0f;
    private float m_CurrentRotataionX = 0f;
    private float m_MaxCameraRotation = 90f;
    private Rigidbody m_RB;


	void Start () {
        m_RB = GetComponent<Rigidbody>();
    }
	
    public void Move(Vector3 _velocity)
    {
		Debug.Log (_velocity);
        m_Velocity = _velocity;
    }

    public void Rotate(Vector3 _rotation)
    {
        m_Rotation = _rotation;
    }

    public void RotateCamera(float _camerRotation)
    {
        m_CameraRotationX = _camerRotation;
    }

    public void Jump(float _jumpSpeed)
    {
        m_RB.velocity = new Vector3(m_RB.velocity.x, _jumpSpeed, m_RB.velocity.z);
    }

    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    void PerformMovement ()
    {
        if (m_Velocity != Vector3.zero)
        {
			print ("Moving...");
			m_RB.MovePosition(m_RB.position + (m_Velocity * Time.fixedDeltaTime));
        }
    }
    
    void PerformRotation()
    {
        m_RB.MoveRotation(m_RB.rotation * Quaternion.Euler(m_Rotation));
        if (m_Head != null && m_Arm != null)
        {
            m_CurrentRotataionX -= m_CameraRotationX;
            m_CurrentRotataionX = Mathf.Clamp(m_CurrentRotataionX, -m_MaxCameraRotation, m_MaxCameraRotation);

            m_Head.transform.localEulerAngles = new Vector3(m_CurrentRotataionX, 0f, 0f);
            m_Arm.transform.localEulerAngles = new Vector3(m_CurrentRotataionX, 0f, 0f);
        }
        else
        {
            Debug.Log("Error finding components to rotate!");
        }
    }

}
