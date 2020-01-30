using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class RoomScript : MonoBehaviour {
    public GameObject mainSetup;
    public GameObject finishGame;

	public void LeaveRoom()
    {
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.player.SetScore(0);
        PhotonNetwork.LeaveRoom();
    }

    void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    public void LeaveRoomEnabler()
    {
        finishGame.SetActive(true);
        mainSetup.SetActive(false);
    }

    public void StayInRoom()
    {
        finishGame.SetActive(false);
        mainSetup.SetActive(true);
    }
}
