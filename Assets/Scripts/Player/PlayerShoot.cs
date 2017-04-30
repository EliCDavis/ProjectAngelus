using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using ISO.FX;

public class PlayerShoot : NetworkBehaviour {

    public PlayerWeapon m_PlayerWeapon;             //Reference to player weapon
    private const string PLAYER_NAME = "Player";    //Plater name prefix

	/// <summary>
	/// Reference to player camera
	/// </summary>
    [SerializeField]
    private Camera m_PlayerCamera;

	/// <summary>
	/// Mask for raycasting
	/// </summary>
    [SerializeField]
    private LayerMask m_Mask;

	/// <summary>
	/// The bullet spawn.
	/// </summary>
	[SerializeField]
	private Transform bulletSpawn;

	/// <summary>
	/// The effect instantiated where our bullet hits
	/// </summary>
    [SerializeField]
    private GameObject m_HitEffect;

	/// <summary>
	/// How many shots the player can fire a second
	/// </summary>
	[SerializeField]
	private int fireRate = 5;

	private float timeOfLastFire;

    void Start()
    {

		timeOfLastFire = -1f;

        if (m_PlayerCamera == null)
        {
            Debug.LogError("PlayerShoot: Failed to find localplayer camera");
            this.enabled = false;
        }
    }

    void Update()
    {
        //Just doing single fire for now...
		if (timeOfLastFire + (1f/(float)fireRate) < Time.time &&Input.GetButtonDown("Fire1"))
        {
			timeOfLastFire = Time.time;
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
		Vector3 shootTowards = Vector3.zero;

		// Get the point where the camera is looking
		if (Physics.Raycast(m_PlayerCamera.transform.position, m_PlayerCamera.transform.forward, out _hit, m_PlayerWeapon.m_Range, m_Mask))
        {
			shootTowards = _hit.point;

            if (shootTowards != Vector3.zero)
            {

                // Now raycast from the bullet spawn
                if (Physics.Raycast(bulletSpawn.position, (shootTowards - bulletSpawn.position).normalized, out _hit, m_PlayerWeapon.m_Range, m_Mask))
                {
                    CmdAnimateShot(bulletSpawn.position, _hit.point);
                    GameObject _hitEffect = Instantiate(m_HitEffect, _hit.point, Quaternion.identity);
                    Destroy(_hitEffect, 1);
                    //_hitEffect.transform.SetParent(this.transform);

                    if (_hit.collider.tag == PLAYER_NAME)
                    {
                        CmdPlayerShot(_hit.transform.name, m_PlayerWeapon.m_Damage);
                    }
                }
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
        _player.RpcTakeDamage(_damage, this.name);
    }


	/// <summary>
	/// Animates the shot across the entire network for everyone to see
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	[Command]
	void CmdAnimateShot(Vector3 start, Vector3 end) {
		// do something with entry.Value or entry.Key
		this.RpcAnimateGunshot(start, end);
	}


	/// <summary>
	/// Animates the gunshot for anyone who has this
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	[ClientRpc]
	public void RpcAnimateGunshot(Vector3 start, Vector3 end) {
		ShootingFactory.CreateShootEffect (start, end);
	}
}
