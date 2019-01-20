using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenSceneManager : MonoBehaviour
{
    [SerializeField] float StartScreenLoadDelay = 0.5f;
    [SerializeField] SceneLoader SL;
    [SerializeField] SplashScreen SS;

    private void Start() {
        SL = FindObjectOfType<SceneLoader>();
        SS = FindObjectOfType<SplashScreen>();
        StartCoroutine(loadStartScreen());
    }

    IEnumerator loadStartScreen() {
        yield return new WaitForSeconds(StartScreenLoadDelay);
        //SS.SwapBackground(SplashScreen.BackgroundChoice.Normal);
        SL.LoadFirstScene();
    }
}
