using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager singleton;

    public MatchSettings m_MatchSettings;

    public static Player m_PlayerHost;

    void Awake()
    {
		//print ("IM RIGHT HERE: "+ transform.name);
        if (singleton != null)
        {
            Debug.LogError("More than one GameManger!");
        }
        else
        {
            singleton = this;
        }

    }

    #region playerTracking

    /// <summary>
    /// All players ID's and references are stored in a dectionary for easey reference
    /// Keys are "Player " with ID appenede to end of tag.
    /// </summary>
    private const string m_PlayerIdPrefix = "Player ";
    private static Dictionary<string, Player> m_Players = new Dictionary<string, Player>();

    /// <summary>
    /// On join register player in dictonary for easey reference
    /// </summary>
    /// <param name="_netID">Just player ID number</param>
    /// <param name="_player">Reference to Player object</param>
    public static void RegisterPlayer(string _netID, Player _player)
    {
        string _playerID = m_PlayerIdPrefix + _netID;
        m_Players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    /// <summary>
    /// If player dies or disconnects form server remove that player from the dieconary
    /// </summary>
    /// <param name="_playerID">Player tag including ID</param>
    public static void UnRegisterPlayer(string _playerID)
    {
        m_Players.Remove(_playerID);
    }

    /// <summary>
    /// Get reference to Player object
    /// </summary>
    /// <param name="_playerID">Player tag including ID</param>
    /// <returns>Reference to Player object for that player</returns>
    public static Player GetPlayer(string _playerID)
    {
        return m_Players[_playerID];
    }

    public static List<Player> GetAllPlayers()
    {
        List<Player> _players = new List<Player>();

        foreach (var item in m_Players)
        {
            _players.Add(item.Value);
        }
        return _players;
    }


	/// <summary>
	/// Gets the current registered players as a dictionary for easy indexing
	/// </summary>
	/// <returns>The current registered players.</returns>
	public static Dictionary<string, Player> GetCurrentRegisteredPlayers()
	{
		return m_Players;
	}
    
    public static List<Transform> GetPlayerLocations()
    {
        List<Transform> _playerlocations = new List<Transform>();

        foreach (var item in m_Players)
        {
            _playerlocations.Add(item.Value.transform);
        }

        return _playerlocations;
    }

    public static int CurrentPlayerCount()
    {
        return m_Players.Count;
    }

    public static Player GetPlayerHost()
    {
        return m_PlayerHost;
    }

    public static void SetPLayerHost(Player _playerHost)
    {
        m_PlayerHost = _playerHost;
    }

    #endregion

}