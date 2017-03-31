using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    private Behaviour[] m_ThingsToDisable;

    private Camera m_ScnenCamera;


	void Start () {
		if (!isLocalPlayer)
        {
            for (int i = 0; i < m_ThingsToDisable.Length; i++)
            {
                m_ThingsToDisable[i].enabled = false;
            }
        }
        else
        {
            m_ScnenCamera = Camera.main;
            if (m_ScnenCamera != null)
            {
                m_ScnenCamera.gameObject.SetActive(false);
            }
        }
	}

    void OnDisable()
    {
        if (m_ScnenCamera != null)
        {
            m_ScnenCamera.gameObject.SetActive(true);
        }
    }
}
