using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    private Slider m_HealthSlider;
    private Text m_HelathNumber;
    private Player m_CurrentPlayer;
    private float m_maxHealth;
    private float m_CurrentHealth;

    void Start()
    {
        m_HealthSlider = GetComponent<Slider>();
        m_HelathNumber = GetComponentInChildren<Text>();
        m_HealthSlider.maxValue = m_maxHealth;
        m_HealthSlider.value = m_maxHealth;
        m_HelathNumber.text = m_maxHealth.ToString();
    }

    public void SetUpUI(Player _player)
    {
        m_CurrentPlayer = _player;
        m_maxHealth = m_CurrentPlayer.GetMaxHealth();
    }

    void Update()
    {
        m_CurrentHealth = m_CurrentPlayer.GetCurrentHealth();
        m_HealthSlider.value = m_CurrentHealth;
        m_HelathNumber.text = m_CurrentHealth.ToString();
    }
}