using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ISO.GameModes {

	public class TeamDeathMatch : NetworkManager {

		/// <summary>
		/// Called on the server when a new client connects
		/// </summary>
		/// <param name="conn">Conn.</param>
		public override void OnServerConnect(NetworkConnection conn)
		{
			Debug.Log("OnPlayerConnected");
		}

		/// <summary>
		/// This hook is invoked when a server is started - including when a host is started
		/// </summary>
		public override void OnStartServer()
		{
			Debug.Log("OnStartConnected");
		}

		public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
		{
			Debug.Log ("player controller id");
		}

		void Update () {
			print ("num of players: " + this.numPlayers);
		}

	}

}