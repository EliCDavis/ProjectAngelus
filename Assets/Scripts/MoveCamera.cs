using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {
    [SerializeField]
    private float m_MaxCameraAngle;
    [SerializeField]
    private float m_CameraDistance;
    [SerializeField]
    private float m_LookSpeed;
    [SerializeField]
    private Transform m_LookAt;

    private Camera m_Cam;
    private float m_CurrentX;
    private float m_CurrentY;
    
    void Start()
    {
        m_Cam = GetComponent<Camera>();
    }

    void Update()
    {
        m_CurrentX += Input.GetAxis("Mouse X") * m_LookSpeed;
        m_CurrentY -= Input.GetAxis("Mouse Y") * m_LookSpeed;

        m_CurrentY = Mathf.Clamp(m_CurrentY, -m_MaxCameraAngle, m_MaxCameraAngle);
    }

    void LateUpdate()
    {
        Vector3 dir = new Vector3(0f, 0f, -m_CameraDistance);
        //Vector3 offset = new Vector3(0f, 5f, 0f);
        Quaternion rotation = Quaternion.Euler(m_CurrentY, m_CurrentX, 0f);
        transform.position = m_LookAt.position + rotation * dir;
        m_Cam.transform.LookAt(m_LookAt.position);
    }	
}
