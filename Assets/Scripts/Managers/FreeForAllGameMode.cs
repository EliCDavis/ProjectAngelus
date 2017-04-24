using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FreeForAllGameMode : NetworkBehaviour
{
    public static FreeForAllGameMode m_Singleton;

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
    public int m_TotalKills = 0;
    private int m_CurrentPlayers = 0;
    private Player m_ServerPlayer;
    private NetworkIdentity m_NetID;

	// Use this for initialization
	void Start () {
        if (isServer)
        {
            m_CurrentState = MatchState.NotStarted;
        }

    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        m_NetID = GetComponent<NetworkIdentity>();
        m_Singleton = this;
        if (m_Singleton == null)
        {
            Debug.LogError("FreeForAllGameMode: Error setting singleton!");
        }

        if (isServer)
        {
            m_CurrentState = MatchState.NotStarted;
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

    public int GetCurrentScore()
    {
        return m_TotalKills;
    }

    [ClientRpc]
    public void RpcAddDeath()
    {
        m_TotalKills++;
        Debug.Log("Total kills: " + m_TotalKills);
    }
}
