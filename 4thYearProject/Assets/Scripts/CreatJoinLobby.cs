using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatJoinLobby : MonoBehaviour {
    public GameObject gameSearching;
    public GameObject gamePanel;

    // Use this for initialization
    public void CreateRoom()
    {
        gameSearching.SetActive(true);
        gamePanel.SetActive(false);
        photoHandler.instance.JoinGame();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }
}
