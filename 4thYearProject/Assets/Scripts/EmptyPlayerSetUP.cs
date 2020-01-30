using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyPlayerSetUP : MonoBehaviour {

    // Use this for initialization
    public PhotonView nameTag;
    void Awake () {
        nameTag.RPC("SetName", PhotonTargets.AllBuffered, PhotonNetwork.playerName);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
