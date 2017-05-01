using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraHitWall : MonoBehaviour
{

    private Vector3 m_OrginalDistance;

    [SerializeField]
    private float m_SphereCastSize = 0.1f;

    void Awake()
    {
        m_OrginalDistance = transform.position;   
    }

    void Update()
    {
        RaycastHit hit;

        Vector3 _pos = transform.position;

        // Cast a sphere around the camera. If the sphere hits anyhting then move the camera there.

        if (Physics.SphereCast(_pos, m_SphereCastSize, -transform.forward, out hit, 2))
        {
            transform.position = hit.transform.position;
            Debug.Log("Camera hit the wall at pos: " + hit.transform.position);
        }
    }
}