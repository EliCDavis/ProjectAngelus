﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PlayerShoot : NetworkBehaviour {

    public PlayerWeapon m_PlayerWeapon;             //Reference to player weapon
    private const string PLAYER_NAME = "Player";    //Plater name prefix

    [SerializeField]
    private Camera m_PlayerCamera;                  //Regerence to player camera
    [SerializeField]
    private LayerMask m_Mask;                       //Mask for raycasting

    void Start()
    {
        if (m_PlayerCamera == null)
        {
            Debug.LogError("PlayerShoot: Failed to find localplayer camera");
            this.enabled = false;
        }
    }

    void Update()
    {
        //Just doing single fire for now...
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    /// <summary>
    /// When called will create a raycast from center of camera to however many feet of weapon
    /// Checks to see if what was hit was a player and does damage to that player
    /// </summary>
    [Client]
    void Shoot()
    {

		Cursor.lockState = CursorLockMode.Locked;

        RaycastHit _hit;
        if (Physics.Raycast(m_PlayerCamera.transform.position, m_PlayerCamera.transform.forward, out _hit, m_PlayerWeapon.m_Range, m_Mask))
        {

			CmdAnimateShot (m_PlayerCamera.transform.position, _hit.point);

            if (_hit.collider.tag == PLAYER_NAME)
            {
                CmdPlayerShot(_hit.transform.name, m_PlayerWeapon.m_Damage);
            }

        }

    }

    /// <summary>
    /// The remote player has been hit and needs to do damage
    /// </summary>
    /// <param name="_playerID">Player tag including ID</param>
    /// <param name="_damage">Amount of damage to do</param>
    [Command]
    void CmdPlayerShot(string _playerID, float _damage)
    {
        Debug.Log(_playerID + " has beed shot!");

        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);
    }

	/// <summary>
	/// Animates the shot across the entire network for everyone to see
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	[Command]
	void CmdAnimateShot(Vector3 start, Vector3 end) {
		foreach(KeyValuePair<string, Player> entry in GameManager.GetCurrentRegisteredPlayers())
		{
			// do something with entry.Value or entry.Key
			entry.Value.RpcAnimateGunshot(start, end);
		}
	}

}
