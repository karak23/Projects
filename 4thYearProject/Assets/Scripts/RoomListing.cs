using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour {

    public GameObject roomList;
    private List<GameObject> listOfRooms;
    private float updateTime = 5f;

    // Use this for initialization
    void Start()
    {
        listOfRooms = new List<GameObject>();
        UpdateList();
    }
	
	// Update is called once per frame
	void Update () {
        updateTime -= Time.deltaTime;

        if(updateTime <= 0)
        {
            UpdateList();
            updateTime = 30f;
        }
	}

    void UpdateList()
    {
        if (PhotonNetwork.GetRoomList().Length >= 1)
        {
            if (listOfRooms.Count > 0)
            {
                foreach (GameObject obj in listOfRooms)
                {
                    Destroy(obj);
                }

                listOfRooms.Clear();
            }

            foreach (RoomInfo game in PhotonNetwork.GetRoomList())
            {
                if (game.PlayerCount < game.MaxPlayers)
                {
                    GameObject roomListObj = Instantiate(roomList);
                    roomListObj.GetComponent<Button>().onClick.AddListener(() => OnClickJoinRoom(game.Name));
                    roomListObj.transform.SetParent(transform, false);
                    roomListObj.transform.GetComponentInChildren<Text>().text = game.Name + "   " +
                        "players: " + game.PlayerCount + "/" + game.MaxPlayers;
                    listOfRooms.Add(roomListObj);
                }
            }
        }
    }

    void OnClickJoinRoom(string name)
    {
        photoHandler.instance.onClickJoinRoom(name);
    }
}
