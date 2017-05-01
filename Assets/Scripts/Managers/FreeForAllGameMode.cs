using UnityEngine;
using UnityEngine.Networking;

public class FreeForAllGameMode : NetworkBehaviour
{

    /// <summary>
    /// Static variable to make finding the match manager easer
    /// </summary>
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
    public int m_PlayersToStart = 0;
    public int m_KillsToWin = 10;

    /// <summary>
    /// Time of match in seconds
    /// 1000 = 10 minuites
    /// </summary>
    [SerializeField]
    private float m_MatchTime = 100f;
    [SyncVar]
    private float m_TimeRemaining = 0f;
    public int m_TotalKills = 0;
    private int m_CurrentPlayers = 0;

	// Use this for initialization
	void Start () {
        if (isServer)
        {
            m_CurrentState = MatchState.NotStarted;
            m_TimeRemaining = m_MatchTime;
        }
    }

    void Update()
    {
        if (m_CurrentState == MatchState.InProgress)
        {
            m_TimeRemaining -= Time.deltaTime;
            //Debug.Log(m_TimeRemaining);
        }

        if (m_TimeRemaining <= 0)
            EndMatch();
    }

    private void EndMatch()
    {
        m_CurrentState = MatchState.Ended;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

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
	
    [ClientRpc]
    public void RpcAddPlayer()
    {
        m_CurrentPlayers++;
        if (m_CurrentPlayers == m_PlayersToStart)
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

    public float GetCurrentTimeLeft()
    {
        return m_TimeRemaining;
    }

    [ClientRpc]
    public void RpcAddDeath()
    {
        m_TotalKills++;
        Debug.Log("Total kills: " + m_TotalKills);
    }
}