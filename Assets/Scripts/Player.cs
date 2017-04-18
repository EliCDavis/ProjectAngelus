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


    public void Setup ()
    {
        wasEnabled = new bool[disabledOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disabledOnDeath[i].enabled;
        }

        SetDefaults();
    }
  

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

	[ClientRpc]
	public void RpcAnimateGunshot(Vector3 start, Vector3 end) {
		Debug.Log ("Animating...");
		GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = (start + end) / 2f;
	}

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

	public override void OnStartLocalPlayer()
	{
		if (highlights == null) {
			return; 
		}
		highlights.color = new Color (0, 0.8f, 0.8f);
		highlights.SetColor ("_EmissionColor", new Color (0, 0.8f, 0.8f));
	}

}