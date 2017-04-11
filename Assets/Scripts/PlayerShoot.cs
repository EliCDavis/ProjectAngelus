using UnityEngine;
using UnityEngine.Networking;

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
        RaycastHit _hit;
        if (Physics.Raycast(m_PlayerCamera.transform.position, m_PlayerCamera.transform.forward, out _hit, m_PlayerWeapon.m_Range, m_Mask))
        {
            if (_hit.collider.tag == PLAYER_NAME)
                CmdPlayerShot(_hit.transform.name, m_PlayerWeapon.m_Damage);
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

}
