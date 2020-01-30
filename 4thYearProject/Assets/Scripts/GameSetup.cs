using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSetup : MonoBehaviour {

    public static GameSetup GS;
    public Transform[] spawnPoints;
    public Transform[] randomSpawnPoints;
    public GameObject restartButton;

    bool customs = true;
    double timerIncrementValue;
    double startTime;
    [SerializeField] double timer = 5;
    ExitGames.Client.Photon.Hashtable CustomeValue;
    public Text timeText;
    public bool ifCountdown;
    public bool isReadyToRespawn = false;
    float respawnTime = 5;
    public LoadingScreenManager loadingSceneMg;
    private bool isLoadingScene = false;
    private void Awake()
    {
        if(GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }

    public void EnableRestart()
    {
        restartButton.SetActive(true);
    }

    void Update()
    {
        if(ifCountdown)
            AddTimeCountdown();
        if (isReadyToRespawn)
            RespawnPlayer();
    }

    void AddTimeCountdown()
    { 
        if (PhotonNetwork.playerList.Length == 2)
        {
            StartCoroutine(StartCountdown());
        }
        else
        {
            timeText.text = "Waiting for other players!";
        }
    }

    IEnumerator StartCountdown()
    {
        timeText.text = "Game Starts in";

        yield return new WaitForSeconds(2f);
        if (!isLoadingScene)
        {
            if (customs)
            {
                if (PhotonNetwork.player.IsMasterClient)
                {
                    CustomeValue = new ExitGames.Client.Photon.Hashtable();
                    startTime = PhotonNetwork.time;
                    CustomeValue.Add("StartTime", startTime);
                    PhotonNetwork.room.SetCustomProperties(CustomeValue);
                }
                else
                {
                    startTime = double.Parse(PhotonNetwork.room.CustomProperties["StartTime"].ToString());
                }
                customs = false;
            }

            timerIncrementValue = PhotonNetwork.time - startTime;
            double getfrom = timer - timerIncrementValue;
            timeText.text = string.Format("{0:0}", getfrom);

            if (getfrom < 1)
            {
                timeText.text = "GO!";
            }

            if (timerIncrementValue >= timer)
            {
                StartCoroutine(SetTestToBlank());
                loadingSceneMg.LoadScreen("TestGame");
                isLoadingScene = true;
                //PhotonNetwork.LoadLevel("TestGame");
            }
        }
    }

    IEnumerator SetTestToBlank()
    {
        yield return new WaitForSeconds(1f);
        timeText.text = "";
    }

    void RespawnPlayer()
    {
        respawnTime -= Time.deltaTime;
        if (respawnTime <= 0)
        {
            photoHandler.instance.RandomPlayerSpawn();
            respawnTime = 5;
            isReadyToRespawn = false;
        }
    }
}
