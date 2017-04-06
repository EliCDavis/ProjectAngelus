using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    [SerializeField]
    private float m_MaxHealth = 100f;   //Max player health

    [SyncVar]
    private float m_CurrentHealth;      //Current player health

    void Awake()
    {
        SetDefaults();
    }

    /// <summary>
    /// Removes the passed amount by that much from health
    /// </summary>
    /// <param name="_amount">amount to damage</param>
    public void TakeDamage(float _amount)
    {
        m_CurrentHealth -= _amount;

        Debug.Log(transform.name + " now has " + m_CurrentHealth + " health.");
    }

    /// <summary>
    /// Set player defaults
    /// </summary>
    public void SetDefaults()
    {
        m_CurrentHealth = m_MaxHealth;
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