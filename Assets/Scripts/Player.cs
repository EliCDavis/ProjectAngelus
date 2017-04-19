using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Player : NetworkBehaviour {

    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead;  }
        protected set { _isDead = value;  }
    }

    [SerializeField]
    private float m_MaxHealth = 100f;   //Max player health
    [SyncVar]
    private float m_CurrentHealth;      //Current player health
    [SerializeField]
    private Behaviour[] disabledOnDeath;
    [SerializeField]
    private Collider[] m_CollidersToDisable;
    private bool[] wasEnabled;
    private NetworkStartPosition[] m_SpawnLocations;
    private Dictionary<float, Transform> m_SpawnSummations;

    [SerializeField]
    private int m_PlayerScore = 0;
    [SyncVar]
    private int m_MatchTotalScore;
    private Rigidbody m_RidgidBody;


    public void Setup ()
    {  
        CmdBradcastNewPlayerSetup();
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
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        m_CurrentHealth = m_MaxHealth;
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
    
    /*
    //For testing
    void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(100);
        }
    }
    */

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

        if (m_CurrentHealth <= 0)
        {
            Die();
            
            if (!isLocalPlayer)
            {
                Player _hitPlayer;
                _hitPlayer = GameManager.GetPlayer(_enemyPlayer);
                _hitPlayer.GetKill(_enemyPlayer);
            }
        }
    }

    public void GetKill(string _playerID)
    {
        m_PlayerScore++;
        m_MatchTotalScore++;
        Debug.Log("Match total score" + m_MatchTotalScore);
        //GameManager.GetPlayer(_playerID).CmdUpdateTotalScore(m_PlayerScore);
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



        for (int i = 0; i < m_CollidersToDisable.Length; i++)
        {
            m_CollidersToDisable[i].enabled = false;
        }

        m_RidgidBody.isKinematic = true;

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

        //Transform _startPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _toSpawn.position;
        transform.rotation = _toSpawn.rotation;

        Debug.Log("Respawning Player: " + transform.name);
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
            m_CollidersToDisable[i].enabled = true;
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
}