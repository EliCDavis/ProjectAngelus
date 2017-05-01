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
            //Debug.Log(gamePlayer);
            if (FreeForAllGameMode.m_Singleton == null)
            {
                //Debug.LogError("SetUpPlayerOnLoad: Error finding FreeForAllGameMode");
            }
        }
    }
}
