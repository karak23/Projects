using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class photonConnect : Photon.MonoBehaviour {

    public string versionName = "0.1";

    public void connectToPhoton()
    {
        PhotonNetwork.ConnectUsingSettings(versionName);

        Debug.Log("Connecting to photon...");
    }

    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);

        Debug.Log("We are Connected to Master");
    }

    private void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    private void OnDisconnectedFromPhoton()
    {
        Debug.Log("Dis from photon");
        PhotonNetwork.LoadLevel("MainMenu");
    }



}
