using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {

    public Text userName;

    [PunRPC]
    private void SetName(string name)
    {
        userName.text = name;
    }
    void Start()
    {
        userName.text = PhotonNetwork.player.NickName;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
