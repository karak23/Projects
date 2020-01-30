using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class photoHandler : Photon.MonoBehaviour {

    public static photoHandler instance = null;
    public GameObject mainPlayer;
    public GameObject playerEmpty;
    [SerializeField] Transform[] spawnPoints;
    int numberPlayers;
    public int playersReady = 0;
    public bool gameIsReady = false;
    public PhotonView pV;
    private GameObject playerObj;
    public bool startTheGame = false;
    public bool isThereAGame = false;
    private int retry = 0;
    public string versionName = "0.1";

    private void Awake()
    {
        if (instance == null)
        { 
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        PhotonNetwork.sendRate = 30;
        PhotonNetwork.sendRateOnSerialize = 20;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        pV = GetComponent<PhotonView>();
    }

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

    public void CreateRoom(string room_name)
    {
        PhotonNetwork.CreateRoom(room_name, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public void JoinGame()
    {
        PhotonNetwork.JoinRandomRoom();
        retry = 1;
    }

    public void OnPhotonRandomJoinFailed()
    {
        if (retry == 3)
        {
            CreateRoom(PhotonNetwork.player.NickName + " Game");
        }
        else
        {
            StartCoroutine(Search());
        }
    }

    IEnumerator Search()
    {
        int time = Random.Range(1, 5);
        yield return new WaitForSeconds(time);
        PhotonNetwork.JoinRandomRoom();
        retry += 1;
    }

    public void onClickJoinRoom(string room_name)
    {
        PhotonNetwork.JoinRoom(room_name);
    }

    private void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Ready");
        StartCoroutine(SpawnEmptyPlayer());
        Debug.Log("We are Connected to room");
    }

    private void OnLeftRoom()
    {
        playersReady -= 1;
    }

    IEnumerator SpawnPlayer(float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);


        if (PhotonNetwork.player.ID == 1)
        {
            playerObj = PhotonNetwork.Instantiate(mainPlayer.name, GameSetup.GS.spawnPoints[0].position, GameSetup.GS.spawnPoints[0].rotation, 0);
            numberPlayers = 2;
        }

        else if (PhotonNetwork.player.ID == 2)
        {
            playerObj = PhotonNetwork.Instantiate(mainPlayer.name, GameSetup.GS.spawnPoints[1].position, GameSetup.GS.spawnPoints[1].rotation, 0);
            numberPlayers = 1;
        }
    }

    IEnumerator RandomSpawnPlayer(float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);
        int random = Random.Range(0, GameSetup.GS.randomSpawnPoints.Length);
        playerObj = PhotonNetwork.Instantiate(mainPlayer.name, GameSetup.GS.randomSpawnPoints[random].position, GameSetup.GS.randomSpawnPoints[random].rotation, 0);

    }



    IEnumerator SpawnEmptyPlayer()
    {
        yield return new WaitForSeconds(0.01f);

        if (PhotonNetwork.player.ID == 1)
        {
            playerObj = PhotonNetwork.Instantiate(playerEmpty.name, GameSetup.GS.spawnPoints[0].position, GameSetup.GS.spawnPoints[0].rotation, 0);
        }
        else if (PhotonNetwork.player.ID == 2)
        {
            playerObj = PhotonNetwork.Instantiate(playerEmpty.name, GameSetup.GS.spawnPoints[1].position, GameSetup.GS.spawnPoints[1].rotation, 0);
        }
    }

    private void Update()
    {
        if(!gameIsReady)
        {
            return;
        }

        if(playersReady == 1 & gameIsReady)
        {
            gameIsReady = false;
            startTheGame = true;
        }
    }

    public void SpawnPlayer()
    {
        StartCoroutine(SpawnPlayer(1));
    }

    public void RandomPlayerSpawn()
    {
        StartCoroutine(RandomSpawnPlayer(1));
    }

    [PunRPC]
    public void PlayersReady()
    {
        playersReady += 1;
        gameIsReady = true;
    }

}
