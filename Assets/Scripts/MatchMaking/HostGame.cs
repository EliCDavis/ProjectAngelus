using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {

    [SerializeField]
    private uint m_RoomSize = 4;

    private string m_RoomName;

    private NetworkManager m_NetworkManager;

    void Start()
    {
        m_NetworkManager = NetworkManager.singleton;

        if (m_NetworkManager.matchMaker == null)
        {
            m_NetworkManager.StartMatchMaker();
        }
    }

    public void SetRoomName(string _name)
    {
        m_RoomName = _name;
    }

    public void CreateRoom()
    {
        if (m_RoomName != "" && m_RoomName != null)
        {
            Debug.Log("Creating Room:" + m_RoomName + " with size for " + m_RoomSize + " players.");
            m_NetworkManager.matchMaker.CreateMatch(m_RoomName, m_RoomSize, true, "", "", "", 0, 0, m_NetworkManager.OnMatchCreate);
            
        }
        else
        {
            Debug.LogError("HostGame: " + m_RoomName + " is not a valid room name.");
        }
    }
	
}
