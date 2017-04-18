using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FreeForAllGameMode : NetworkBehaviour
{
    enum MatchState
    {
        NotStarted,
        Starting,
        InProgress,
        End,
        Ended
    }

    private MatchState m_CurrentState;
    public int m_PlayersToStart;
    public int m_KillsToWin;
    public float m_MatchTime;
    private int m_TimeRemaining;
    [SyncVar]
    public int m_TotalKills;
    private int m_CurrentPlayers;

	// Use this for initialization
	void Start () {
        if (isServer)
        {
            m_CurrentState = MatchState.NotStarted;
            m_CurrentPlayers = GameManager.CurrentPlayerCount();
        }
    }
	
    void AddPlayer()
    {
        m_CurrentPlayers++;
        if (m_CurrentPlayers >= m_PlayersToStart)
        {
            m_CurrentState = MatchState.Starting;
            StartMatch();
            Debug.Log("Starting Match!");
        }
    }

    void StartMatch()
    {
        m_TotalKills = 0;
        m_CurrentState = MatchState.InProgress;
    }

    public int GetCurrentState()
    {
        return (int) m_CurrentState;
    }


    public void AddDeath()
    {
        m_TotalKills++;
        Debug.Log("Total kills: " + m_TotalKills);

    }


    void EndMatch()
    {

    }
}
