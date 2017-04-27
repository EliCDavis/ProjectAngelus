using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    private Slider m_HealthSlider;
    private Text[] m_TextElements;
    private Text m_HelathNumber;
    private Text m_ScoreText;
    private Text m_MatchScore;
    private Player m_CurrentPlayer;
    private float m_maxHealth;
    private float m_CurrentHealth;

    void Awake()
    {
        m_HealthSlider = GetComponent<Slider>();
        m_TextElements = GetComponentsInChildren<Text>();
        for (int i = 0; i != m_TextElements.Length; i++)
        {
            if (m_TextElements[i].name == "Score")
                m_ScoreText = m_TextElements[i];

            if (m_TextElements[i].name == "HealthNumber")
                m_HelathNumber = m_TextElements[i];

            if (m_TextElements[i].name == "MatchKills")
                m_MatchScore = m_TextElements[i];
        }
    }

    public void SetUpUI(Player _player)
    {
        m_CurrentPlayer = _player;
        m_maxHealth = m_CurrentPlayer.GetMaxHealth();
        m_HealthSlider.maxValue = m_maxHealth;
        m_HealthSlider.value = m_maxHealth;
        m_ScoreText.text = "Score: " + m_CurrentPlayer.GetCurrentScore().ToString();
        //m_MatchScore.text = "Match Kills: " + GameManager.GetCurrentTotalScore();
    }

    void Update()
    {
        m_CurrentHealth = m_CurrentPlayer.GetCurrentHealth();
        m_HealthSlider.value = m_CurrentHealth;
        m_HelathNumber.text = m_CurrentHealth.ToString();
        m_ScoreText.text = "Score: " + m_CurrentPlayer.GetCurrentScore().ToString();
        //m_MatchScore.text = "Match Kills: " + GameManager.GetCurrentTotalScore();
    }
}