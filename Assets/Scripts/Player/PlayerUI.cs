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
    private Text m_TextTimeLeft;
    private Player m_CurrentPlayer;
    private FreeForAllGameMode m_FreeForAllMode;
    private float m_maxHealth;
    private float m_CurrentHealth;

    void GetUIComponets()
    {
        m_HealthSlider = GetComponentInChildren<Slider>();
        m_TextElements = GetComponentsInChildren<Text>();
        for (int i = 0; i != m_TextElements.Length; i++)
        {
            if (m_TextElements[i].name == "Score")
                m_ScoreText = m_TextElements[i];

            if (m_TextElements[i].name == "HealthNumber")
                m_HelathNumber = m_TextElements[i];

            if (m_TextElements[i].name == "MatchKills")
                m_MatchScore = m_TextElements[i];

            if (m_TextElements[i].name == "MatchTime")
                m_TextTimeLeft = m_TextElements[i];

        }
    }

    public void SetUpUI(Player _player)
    {
        if (_player != null)
        {
            GetUIComponets();
            m_CurrentPlayer = _player;
            m_maxHealth = m_CurrentPlayer.GetMaxHealth();
            m_HealthSlider.maxValue = m_maxHealth;
            m_HealthSlider.value = m_maxHealth;
            m_ScoreText.text = "Score: " + m_CurrentPlayer.GetCurrentScore().ToString();
            m_FreeForAllMode = FreeForAllGameMode.m_Singleton;
            if (m_FreeForAllMode == null)
                Debug.LogError("PlayerUI: Error finding FreeForAllGameMode!");
            m_MatchScore.text = "Match Kills: " + m_FreeForAllMode.GetCurrentScore();
            m_TextTimeLeft.text = "Time Left: " + m_FreeForAllMode.GetCurrentTimeLeft();
        }
        else
        {
            Debug.LogError("PlayerUI: Error setting the Player object!");
        }
    }

    void Update()
    {
        //Never do this...
        if (m_FreeForAllMode == null)
        {
            m_FreeForAllMode = FreeForAllGameMode.m_Singleton;
        }
        m_CurrentHealth = m_CurrentPlayer.GetCurrentHealth();
        m_HealthSlider.value = m_CurrentHealth;
        m_HelathNumber.text = m_CurrentHealth.ToString();
        m_ScoreText.text = "Score: " + m_CurrentPlayer.GetCurrentScore().ToString();
        m_MatchScore.text = "Match Kills: " + m_FreeForAllMode.GetCurrentScore();
        m_TextTimeLeft.text = "Time Left: " + m_FreeForAllMode.GetCurrentTimeLeft();
    }
}