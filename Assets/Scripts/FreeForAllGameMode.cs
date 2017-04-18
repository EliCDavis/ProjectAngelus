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
    public int m_PlayersToStart = 2;
    public int m_KillsToWin = 10;
    public float m_MatchTime = 10f;
    private int m_TimeRemaining;
    [SyncVar]
    public int m_TotalKills = 0;
    private int m_CurrentPlayers = 0;
    private Player m_ServerPlayer;

	// Use this for initialization
	void Start () {
        if (isServer)
        {
            m_CurrentState = MatchState.NotStarted;
            m_CurrentPlayers = GameManager.CurrentPlayerCount();
        }
        m_ServerPlayer = GetComponent<Player>();
        if (m_ServerPlayer == null)
            Debug.LogError("FreeForAllGameMode: Error finding Player Script");
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

    public int GetCurrentScore()
    {
        return m_TotalKills;
    }

    public void AddDeath()
    {
        m_TotalKills++;
        UpdateTotalScors(m_TotalKills);
        Debug.Log("Total kills: " + m_TotalKills);
    }

    
    public void UpdateTotalScors(int m_TotalKills)
    {
        m_ServerPlayer.CmdUpdateTotalScores(m_TotalKills);
    }
}
