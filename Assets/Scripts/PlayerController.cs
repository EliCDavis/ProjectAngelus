using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : NetworkBehaviour {

    [SerializeField]
    private float m_Speed = 5f;

    [SerializeField]
    private float m_LookSpeed;

    [SerializeField]
    private float m_MaxLookRadious;

    private PlayerMotor m_Motor;
    private float m_XLookTotal;

	// Use this for initialization
	void Start () {
        m_Motor = GetComponent<PlayerMotor>();	
	}
	
	// Update is called once per frame
	void Update () {
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

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
