using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace Prototype.NetworkLobby
{
    public class SetUpPlayerOnLoad : LobbyHook
    {
        public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
        {
            LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
            Player _player = gamePlayer.GetComponent<Player>();
            PlayerSetup _playerSetup = gamePlayer.GetComponent<PlayerSetup>();

            _player.m_PlayerName = lobbyPlayer.name;
            _playerSetup.m_PlayerName = lobbyPlayer.name;

            Debug.Log(lobbyPlayer.name);

        }
    }
}
