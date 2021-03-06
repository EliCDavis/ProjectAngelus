﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using ISO.FX;

public class Player : NetworkBehaviour {

    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead;  }
        protected set { _isDead = value;  }
    }

	/// <summary>
	/// Max player health
	/// </summary>
    [SerializeField]
    private float m_MaxHealth = 100f;
    
	/// <summary>
	/// Current player health
	/// </summary>
	[SyncVar]
    private float m_CurrentHealth;

	/// <summary>
	/// The sound that plays when the player takes damage
	/// </summary>
	[SerializeField]
	private AudioSource damageSound;



	[SerializeField]
    private Behaviour[] disabledOnDeath;

	/// <summary>
	/// The graphics to the mech
	/// </summary>
	[SerializeField]
	private GameObject[] graphics;
    
	/// <summary>
	/// The highlights that glow around the metal of the mech
	/// </summary>
	[SerializeField]
	private Material highlights;

	/// <summary>
	/// booleans that represent whether or not the
	/// disabled on death objects where originally
	/// disabled to begin with
	/// </summary>
	private bool[] wasEnabled;

    [SerializeField]
    private Collider[] m_CollidersToDisable;

    [SerializeField]
	private AudioSource m_DeathSound;

    private NetworkStartPosition[] m_SpawnLocations;
    private Dictionary<float, Transform> m_SpawnSummations;

    [SyncVar]
    private int m_PlayerScore = 0;
    [SyncVar]
    private int m_MatchTotalScore;
    [SyncVar]
    public string m_PlayerName;

    private Rigidbody m_RidgidBody;
    private FreeForAllGameMode m_MatchManager;
    private NetworkManager m_NetworkLobbyManager;


    public void Setup ()
    {  
        CmdBradcastNewPlayerSetup();
        m_NetworkLobbyManager = NetworkLobbyManager.singleton;

        //m_NetworkLobbyManager.OnClientSceneChanged();
    }



    void Update()
    {
        // Never do this...
        // Could not find the right function to call when all clints have loaded the scene and the network 
        // manager had loaded in all network objects thus the FreeForAllGameMode game object could not be found.
        // This is just a quick fix...
        if (m_MatchManager == null)
        {
            //m_MatchManager = FreeForAllGameMode.m_Singleton;
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartClient();

        wasEnabled = new bool[disabledOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disabledOnDeath[i].enabled;
        }
        SetDefaults();

        highlights.color = new Color (0, 0.8f, 0.8f);
		highlights.SetColor ("_EmissionColor", new Color (0, 0.8f, 0.8f));

        if (!isServer)
        {
            m_MatchManager = FreeForAllGameMode.m_Singleton;
            if (m_MatchManager == null)
                Debug.LogError("Player: Error finding FreeForAllGameMode");
        }
        m_CurrentHealth = m_MaxHealth;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(WaitForNetSpawn());

        m_CurrentHealth = m_MaxHealth;
    }

    IEnumerator WaitForNetSpawn()
    {
        yield return new WaitForSeconds(1f);
        if (isServer)
        {
            m_MatchManager = FreeForAllGameMode.m_Singleton;
            if (m_MatchManager == null)
                Debug.LogError("Player: Error finding FreeForAllGameMode");
        }
    }

    

    void Awake()
    {
        m_SpawnLocations = FindObjectsOfType<NetworkStartPosition>();
        m_RidgidBody = GetComponent<Rigidbody>();
    }


    [Command]
    private void CmdBradcastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        m_CurrentHealth = m_MaxHealth;
        wasEnabled = new bool[disabledOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disabledOnDeath[i].enabled;
        }
    }

    /// <summary>
    /// Removes the passed amount by that much from health and syncs accross the network
    /// </summary>
    /// <param name="_amount">amount to damage</param>
    [ClientRpc]
    public void RpcTakeDamage(float _amount, string _enemyPlayer)
    {

        if (isDead)
        {
            return;
        }
        m_CurrentHealth -= _amount;

        Debug.Log(transform.name + " now has " + m_CurrentHealth + " health.");

		if (m_CurrentHealth <= 0) {
			Die ();
            
			if (!isLocalPlayer) {
				Player _hitPlayer;
				_hitPlayer = GameManager.GetPlayer (_enemyPlayer);
				_hitPlayer.GetKill (_enemyPlayer);
                ShootingFactory.CreateDeathEffect(transform.position);
                Debug.Log(m_PlayerName);
            }
		} else {
			StartCoroutine (AnimateTakingDamage());
		}
    }

	private IEnumerator AnimateTakingDamage(){
		damageSound.Play ();
		yield return new WaitForSeconds (.5f);
	}

    [Command]
    void CmdUpdateTotalKillCount()
    {
        m_MatchManager.RpcAddDeath();
        
    }

	[ClientRpc]
	public void RpcAnimateGunshot(Vector3 start, Vector3 end) {
		ShootingFactory.CreateShootEffect (start, end);
        ShootingFactory.CreatHitEffect(end);
	}

    public void GetKill(string _playerID)
    {
        m_PlayerScore++;
        CmdUpdateTotalKillCount();
        Debug.Log(this.name + " score is: " + m_PlayerScore);
    }


    /// <summary>
    /// Get health of player hit
    /// </summary>
    private void Die()
    {
        isDead = true;

        for (int i = 0; i < disabledOnDeath.Length; i++)
        {
            disabledOnDeath[i].enabled = false;
        }

        Collider _col = GetComponent<Collider>();
		if (_col != null) {
			_col.enabled = false;
		}


        for (int i = 0; i < m_CollidersToDisable.Length; i++)
        {
            m_CollidersToDisable[i].enabled = false;
        }

        m_RidgidBody.isKinematic = true;

        Renderer[] _render = GetComponentsInChildren<Renderer>();


        for (int i = 0; i < _render.Length; i++)
        {
            _render[i].enabled = false;
        }

		m_DeathSound.Play();

        Debug.Log(transform.name + " died!");

        StartCoroutine(Respawn());
    }


    /// <summary>
    /// Get the summations of playerlocations for each point
    /// Which ever is larger is the nest point.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);

        SetDefaults();

        List<Transform> _playerLocations = GameManager.GetPlayerLocations();
        m_SpawnSummations = new Dictionary<float, Transform>();
        Transform _toSpawn = null;

        for (int i = 0; i != m_SpawnLocations.Length; i++ )
        {
            float _playerDistance = 0;
            for (int j = 0; j != _playerLocations.Count; j++)
            {
                _playerDistance += Mathf.Log(Vector3.Distance(m_SpawnLocations[i].transform.position, _playerLocations[j].position));
            }

            m_SpawnSummations.Add(_playerDistance, m_SpawnLocations[i].transform);
        }

        float maxDistance = 0;
        
        foreach (var kvp in m_SpawnSummations)
        {
            if (kvp.Key > maxDistance)
            {
                maxDistance = kvp.Key;
                _toSpawn = kvp.Value;
            }
        }

        transform.position = _toSpawn.position;
        transform.rotation = _toSpawn.rotation;

        Debug.Log("Respawning Player: " + transform.name);
        Renderer[] _render = GetComponentsInChildren<Renderer>();


        for (int i = 0; i < _render.Length; i++)
        {
            _render[i].enabled = true;
        }
    }

    /// <summary>
    /// Set player defaults
    /// </summary>
    public void SetDefaults()
    {
        isDead = false;
        m_CurrentHealth = m_MaxHealth;
        if (isLocalPlayer)
        {
            for (int i = 0; i < disabledOnDeath.Length; i++)
            {
                disabledOnDeath[i].enabled = wasEnabled[i];
            }
        }   

        for (int i = 0; i < m_CollidersToDisable.Length; i++)
        {
			if (m_CollidersToDisable [i] != null) {
				m_CollidersToDisable[i].enabled = true;
			}
        }

        m_RidgidBody.isKinematic = false;
    }

    public float GetMaxHealth()
    {
        return m_MaxHealth;
    }

    public float GetCurrentHealth()
    {
        return m_CurrentHealth;
    }


    public int GetCurrentScore()
    {
        return m_PlayerScore;
    }

    public int GetPing()
    {
        return 1;
    }
}