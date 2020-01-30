using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class VuforiaBehaviourChanged : MonoBehaviour, ITrackableEventHandler
{

    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NOT_FOUND)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;
        if (!start)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("ReadyPlayer", PhotonTargets.AllBuffered);
        }
    }


    protected virtual void OnTrackingLost()
    {
        Debug.Log("tracking lost");
    }

    #endregion // PROTECTED_METHODS

    public GameObject readyButton;

    private bool playerIsReady = false;
    double timer = 120;
    ExitGames.Client.Photon.Hashtable CustomeValue;
    double timerIncrementValue;
    double startTime;
    private float playerReady = 0;
    bool start = false;
    public UnityEngine.UI.Image timeImg;
    public Text timeText;
    public Text players;
    bool playersReady = false;
    bool gameEnd = false;
    bool noPlayerSpawn = true;
    public Text winOrLoseTxt;
    public Text scoreTxt;

    #region GameObjects
    public GameObject mainUi;
    public GameObject gameOverUi;
    public GameObject looseImg;
    public GameObject winImg;
    public GameObject exitUI;
    #endregion

    void TurnButtonOn()
    {
        ShowPlayerNames();
        if (PhotonNetwork.player.IsMasterClient)
        {
            CustomeValue = new ExitGames.Client.Photon.Hashtable();
            startTime = PhotonNetwork.time;
            CustomeValue.Add("TimerTime", startTime);
            PhotonNetwork.room.SetCustomProperties(CustomeValue);
            start = true;
        }
        else
        {
            StartCoroutine(GameStarting());
        }
    }

    IEnumerator GameStarting()
    {
        yield return new WaitForSeconds(2f);
        startTime = double.Parse(PhotonNetwork.room.CustomProperties["TimerTime"].ToString());
        start = true;
    }

    void Update()
    {
        if(gameEnd)
        {
            return;
        }
        if (start)
        {
            if(noPlayerSpawn)
            {
                ShowPlayer();
                noPlayerSpawn = false;
            }
            timerIncrementValue = PhotonNetwork.time - startTime;
            float getfrom = (float)(timer - timerIncrementValue) % 60;
            float getMinutes = Mathf.Floor((float)(timer - timerIncrementValue) / 60f);
            float time = (float)(timer - timerIncrementValue) / (float)timer;

            timeText.text = string.Format("{0:0}:{1:00}",getMinutes, getfrom);
            timeImg.fillAmount = Mathf.Clamp(time, 0, (float)timer);

            if (timerIncrementValue >= timer)
            {
                EndGame();
                gameEnd = true;
            }
        }
        else
        {
            if (playerReady >= 2)
            {
                TurnButtonOn();
            }
        }

    }

    void EndGame()
    {
        mainUi.SetActive(false);
        exitUI.SetActive(false);
        gameOverUi.SetActive(true);

        float playerScore = PhotonNetwork.player.GetScore();
        bool loose = false;
        scoreTxt.text = "";
        foreach (PhotonPlayer p in PhotonNetwork.playerList)
        {
            if(p.IsLocal)
            {
                scoreTxt.text += "Your Score: " + p.GetScore().ToString() + "\n";
                continue;
            }

            scoreTxt.text += p.NickName + ": " + p.GetScore().ToString() + "\n";


            if(playerScore < p.GetScore())
            {
                loose = true;
            }
        }

        if (loose)
        {
            looseImg.SetActive(true);
            winOrLoseTxt.text = "You Loose!";
        }
        else
        {
            winImg.SetActive(true);
            winOrLoseTxt.text = "You WIN!";
        }

    }

    void ShowPlayerNames()
    {
        players.text = "";
        foreach(PhotonPlayer p in PhotonNetwork.playerList)
        {
            players.text += p.NickName + ": " + p.GetScore().ToString() + "\n";
        }
    }

    [PunRPC]
    void ReadyPlayer()
    {
        playerReady += 1;
    }

    public void ShowPlayer()
    {
        photoHandler.instance.SpawnPlayer();
    }

    void OnPhotonPlayerPropertiesChanged()
    {
        ShowPlayerNames();
    }
}
