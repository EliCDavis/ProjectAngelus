using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

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
    private bool[] wasEnabled;


    public void Setup ()
    {
        wasEnabled = new bool[disabledOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disabledOnDeath[i].enabled;
        }

        SetDefaults();
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
    /// Removes the passed amount by that much from health
    /// </summary>
    /// <param name="_amount">amount to damage</param>
    [ClientRpc]
    public void RpcTakeDamage(float _amount)
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
        }
    }

    private void Die()
    {
        isDead = true;

        for (int i = 0; i < disabledOnDeath.Length; i++)
        {
            disabledOnDeath[i].enabled = false;
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;

        Debug.Log(transform.name + " died!");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {

        yield return new WaitForSeconds(3f);


        SetDefaults();

        Transform _startPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _startPoint.position;
        transform.rotation = _startPoint.rotation;

        Debug.Log("Respawning Player: " + transform.name);
    }

    /// <summary>
    /// Set player defaults
    /// </summary>
    public void SetDefaults()
    {
        isDead = false;

        m_CurrentHealth = m_MaxHealth;

        for (int i = 0; i < disabledOnDeath.Length; i++)
        {
            disabledOnDeath[i].enabled = wasEnabled[i];
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;
    }

    public float GetMaxHealth()
    {
        return m_MaxHealth;
    }

    public float GetCurrentHealth()
    {
        return m_CurrentHealth;
    }
}