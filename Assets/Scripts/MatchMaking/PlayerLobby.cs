using UnityEngine;
using UnityEngine.Networking;

public class PlayerLobby : NetworkBehaviour
{

    void Start()
    {
        transform.name = GetComponent<NetworkIdentity>().netId.ToString();
    }

}
