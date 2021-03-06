﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    private Behaviour[] m_ThingsToDisable;                  //List of objects to disable if not player
    [SerializeField]
    private string m_RemoteLayerMaskName = "RemotePlayer";  //Remote player mask string
    [SerializeField]
    private GameObject m_PlayerUIPrefab;
    private GameObject m_Canvas;
    private Camera m_ScnenCamera;                           //Reference to scene camera
    private string m_PlayerID;                              //Player ID
    private Player m_ActivePlayer;
    private PlayerUI m_PlayerUI;
    [SyncVar]
    public string m_PlayerName = "";

	void Start () {
        //If not player
		if (!isLocalPlayer)
        {
            DisableComponets();
            AssigneRemoteLayerMask();
        }
        else
        {
            m_ScnenCamera = Camera.main;
            if (m_ScnenCamera != null)
            {
                m_ScnenCamera.gameObject.SetActive(false);
            }

            m_ActivePlayer = GetComponent<Player>();
            SetupPlayerUI();
        }
	}

    /// <summary>
    /// When client starts register ID with gamemanager and sync to server
    /// </summary>
    public override void OnStartClient()
    {
        base.OnStartClient();
        //string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        //string _netID = this.name;
        //Player _player = GetComponent<Player>();
        //GameManager.RegisterPlayer(_netID, _player);
        StartCoroutine(RegisterPlayers());
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        if (isLocalPlayer)
        {

            gameObject.AddComponent<FreeForAllGameMode>();
            GetComponent<Player>().Setup();
            CmdSynRegisterPlayers();
        }
            
        
    }

    IEnumerator WaitForSpawn()
    {
        yield return new WaitForSeconds(1f);
        CmdUpdatePlayers();
    }

    [Command]
    public void CmdSynRegisterPlayers()
    {
        RpcRegisterPlayers();
    }

    [ClientRpc]
    public void RpcRegisterPlayers()
    {
        StartCoroutine(WaitForSpawn());
    }

    IEnumerator RegisterPlayers()
    {
        yield return new WaitForSeconds(2f);
        //string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        string _netID = m_PlayerName;

        this.name = _netID;

        Debug.Log(m_PlayerName);
        Player _player = GetComponent<Player>();
        GameManager.RegisterPlayer(_netID, _player);
    }

    [Command]
    public void CmdUpdatePlayers()
    {
        FreeForAllGameMode.m_Singleton.RpcAddPlayer();
        Debug.Log("Adding player!"); 
    } 

    /// <summary>
    /// Returns Player object that is the local user is controlling
    /// </summary>
    /// <returns></returns>
    public Player GetActivePlayer()
    {
        return m_ActivePlayer;
    }

    void SetupPlayerUI()
    {
        GameObject _playerUI = Instantiate(m_PlayerUIPrefab);
        m_Canvas = GameObject.Find("Canvas");               //Tried very had to not use this...

        _playerUI.transform.SetParent(m_Canvas.transform, false);

        m_PlayerUI = _playerUI.GetComponent<PlayerUI>();

        m_PlayerUI.SetUpUI(m_ActivePlayer);     
    }

    /// <summary>
    /// Assign other players to remote player mask
    /// </summary>
    void AssigneRemoteLayerMask()
    {
        gameObject.layer = LayerMask.NameToLayer(m_RemoteLayerMaskName);
    }

    /// <summary>
    /// Disalbe listed componets
    /// </summary>
    void DisableComponets()
    {
        for (int i = 0; i < m_ThingsToDisable.Length; i++)
        {
            m_ThingsToDisable[i].enabled = false;
        }
    }

    /// <summary>
    /// If player leavs or disconnected then unregister then with the server
    /// </summary>
    void OnDisable()
    {
        if (m_ScnenCamera != null)
        {
            m_ScnenCamera.gameObject.SetActive(true);
        }

        GameManager.UnRegisterPlayer(transform.name);
    }
}