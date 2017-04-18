using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : NetworkBehaviour {

    [SerializeField]
    private float m_Speed = 10f;
    [SerializeField]
    private float m_LookSpeed;
    [SerializeField]
    private float m_MaxLookRadious;
    [SerializeField]
    private float m_DistanceToGround;
    [SerializeField]
    private float m_JumpForce;
    [SerializeField]
    private LayerMask m_LayerMask;
    private PlayerMotor m_Motor;
    private float m_XLookTotal;

	void Start () {
        m_Motor = GetComponent<PlayerMotor>();	
	}
	
	void Update () {

        if (Input.GetButtonDown("Jump") && Physics.Raycast(transform.position, -transform.up, m_DistanceToGround))
        {
            m_Motor.Jump(m_JumpForce);
        }

		Vector3 _movHorizontal = transform.right * Input.GetAxisRaw("Horizontal");
		Vector3 _movVertical = transform.forward * Input.GetAxisRaw("Vertical");

        Vector3 _velocity = (_movHorizontal + _movVertical).normalized * m_Speed;

        m_Motor.Move(_velocity);

        float _yRot = Input.GetAxis("Mouse X");
        float _xRot = Input.GetAxis("Mouse Y");


        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * m_LookSpeed;
        float _cameraRotation = _xRot * m_LookSpeed;

        m_Motor.RotateCamera(_cameraRotation);
        m_Motor.Rotate(_rotation);
	}
}
