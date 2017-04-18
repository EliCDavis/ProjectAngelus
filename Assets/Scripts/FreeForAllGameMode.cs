using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FreeForAllGameMode : NetworkBehaviour
{
    enum MatchState
    {
        NotStarted,
        InProgress,
        Ended
    }

    MatchState m_CurrentState;

    private static int m_TimeRemaining;
    private static int m_TotalKills;

	// Use this for initialization
	void Start () {
        if (isServer)
        {
            m_CurrentState = MatchState.NotStarted;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(isServer)
        {
            
        }
	}

    void OnStartServer()
    {

    }
}
