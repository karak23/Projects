using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenManager : MonoBehaviour {

    public GameObject loadingScreenObj;

    AsyncOperation async;

	public void LoadScreen(string name)
    {
        StartCoroutine(LoadingScreen(name));
    }

    IEnumerator LoadingScreen(string name)
    {
        loadingScreenObj.SetActive(true);
        async = PhotonNetwork.LoadLevelAsync(name);
        async.allowSceneActivation = false;

        while(!async.isDone)
        {
            if(async.progress == 0.9f)
            {
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
