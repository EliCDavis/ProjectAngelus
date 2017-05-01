using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    private GameObject m_PlayerStatPrefab;

    [SerializeField]
    private GameObject m_PlayerStatsMenu;

    [SerializeField]
    private GameObject m_PlayerStatsParent;

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
    private bool m_HasBeenOpen = false;
    private List<GameObject> m_PlayerStatsObjects;


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
        StartCoroutine(SetupUIAfterSpawn(_player));
    }


    IEnumerator SetupUIAfterSpawn(Player _player)
    {

        yield return new WaitForSeconds(0f);
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
		if (m_FreeForAllMode == null) {
			m_FreeForAllMode = FreeForAllGameMode.m_Singleton;
		} else {
			m_CurrentHealth = m_CurrentPlayer.GetCurrentHealth();
			m_HealthSlider.value = m_CurrentHealth;
			m_HelathNumber.text = m_CurrentHealth.ToString();
			m_ScoreText.text = "Score: " + m_CurrentPlayer.GetCurrentScore().ToString();
			m_MatchScore.text = "Match Kills: " + m_FreeForAllMode.GetCurrentScore();
			m_TextTimeLeft.text = "Time Left: " + m_FreeForAllMode.GetCurrentTimeLeft().ToString("0.00");
		}

        if (Input.GetKey(KeyCode.Tab) && !m_HasBeenOpen)
        {
            ShowPlayerStats();
            m_HasBeenOpen = true;
        }
        else if(!Input.GetKey(KeyCode.Tab) && m_HasBeenOpen)
        {
            m_PlayerStatsMenu.SetActive(false);
            m_HasBeenOpen = false;
            for (int i = 0; i != m_PlayerStatsObjects.Count; i++)
            {
                Destroy(m_PlayerStatsObjects[i]);
            }
        }
    }

    void ShowPlayerStats()
    {
        m_PlayerStatsMenu.SetActive(true);

        List<Player> _players = new List<Player>();

        _players = GameManager.GetAllPlayers();

        m_PlayerStatsObjects = new List<GameObject>();

        for (int i = 0; i != _players.Count; i++)
        {
            GameObject _stats = Instantiate(m_PlayerStatPrefab);
            m_PlayerStatsObjects.Add(_stats);
            _stats.transform.SetParent(m_PlayerStatsParent.transform);
            Text[] m_PlayerTextInfo;

            m_PlayerTextInfo = _stats.GetComponentsInChildren<Text>();

            m_PlayerTextInfo[0].text = _players[i].name;
            m_PlayerTextInfo[1].text = _players[i].GetComponent<Player>().GetCurrentScore().ToString();
            m_PlayerTextInfo[2].text = _players[i].GetComponent<Player>().GetPing().ToString();
            
        }
    }
}
