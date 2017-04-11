using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager singleton;

    public MatchSettings m_MatchSettings;

    void Awake()
    {
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
    #endregion
}