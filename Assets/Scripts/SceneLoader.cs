#pragma warning disable 0414

using Assets.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    [SerializeField] int sceneToReturnTo = 0;
    [SerializeField] public int SceneToBeLoaded;
    [SerializeField] public float SplashScreenDelay = 2f;
    [SerializeField] public float SplScrProlongOffset = 0.75f;
    [SerializeField] int creditsSceneNr;

    //Caches
    Animator splashScreenAnimator;
    [SerializeField] SplashScreen splashScreen;
    Options options;
    List<string> notLevelScenes = new List<string> {
        scenes.START, scenes.WIN, scenes.GAME_OVER, scenes.CREDITS, scenes.SPLASHSCREEN
    };
    SoundSystem SFXPlayer;

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void Start() {
        splashScreen = FindObjectOfType<SplashScreen>();
        splashScreenAnimator = splashScreen.GetComponent<Animator>();
        SFXPlayer = FindObjectOfType<SoundSystem>();
        options = FindObjectOfType<Options>();
        creditsSceneNr = SceneIndexFromName(scenes.CREDITS);
    }

    /// <summary>
    /// Loads scene with BuildIndex as current scene +1
    /// </summary>
    public void LoadScene() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        FetchLevel(currentSceneIndex + 1);
    }

    /// <summary>
    /// Loads scene with BuildIndex sceneNr
    /// </summary>
    /// <param name="sceneNr"></param>
    public void LoadScene(int sceneNr) {
        FetchLevel(sceneNr);
    }

    void FetchLevel(int sceneToBeLoaded) {
        //Debug.Log("SceneLoader/FetchLevel: Loading Screen index: " + (sceneToBeLoaded));
        SceneToBeLoaded = sceneToBeLoaded;
        if (splashScreenAnimator) {
            if (isCurrentSceneName(scenes.SPLASHSCREEN)) {
                //print("Changing background of the splashScreen to Normal.");
                splashScreen.SwapBackground(SplashScreen.BackgroundChoice.Normal);
            }
            splashScreenAnimator.SetTrigger(triggers.SHOW_UP);
        } else {
            print("SceneLoader/FetchLevel: splashScreen not found, fetching level without animation");
            SceneManager.LoadScene(sceneToBeLoaded);
        }
    }

    public void LoadFirstScene() {
        LoadScene(SceneIndexFromName(scenes.START));
    }

    public void QuitApplication() {
        Application.Quit();
        print("Request to quit received");
    }

    public void LoadGameOverScene() {
        int gameOverSceneNr = SceneIndexFromName(scenes.GAME_OVER);
        LoadScene(gameOverSceneNr);
    }

    public void LoadCreditsScene() {
        LoadScene(creditsSceneNr);
    }

    public bool isCurrentSceneLevel() {
        if (notLevelScenes.Contains(SceneManager.GetActiveScene().name)) return false;
        return true;
    }

    void OnSceneLoad(Scene loadedScene, LoadSceneMode mode) {
        //print("SceneLoader: scene buildIndex: " + SceneManager.GetActiveScene().buildIndex + " loaded. SceneLoader Hash: " + this.GetHashCode());
        ChooseMusicByScene();
        RunSplashScreen();
    }

    private void ChooseMusicByScene() {
        if (SFXPlayer) {
            if (isCurrentSceneLevel()) SFXPlayer.PlayGameMusic();
            else SFXPlayer.PlayMenuMusic();
        } //else print("SceneLoader/ChooseMusicByScene: no SFX player found");
    }

    private void RunSplashScreen() {
        if (!splashScreenAnimator) splashScreenAnimator = FindObjectOfType<SplashScreen>().GetComponent<Animator>();
        if (!options) options = FindObjectOfType<Options>();
        if (splashScreenAnimator) {
            if (options.ShowHintBoards && isCurrentSceneLevel()) {
                //print("SceneLoader/OnSceneLoad: before CORoutine");
                StartCoroutine(DelaySplScrFade());
            } else splashScreenAnimator.SetTrigger(triggers.FADE);
        } else print("SceneLoader/OnSceneLoad: No splashScreen found");
    }

    IEnumerator DelaySplScrFade() {
        //print("SceneLoader: DelaySplScrFade - start ");
        yield return new WaitForSeconds(SplashScreenDelay);
        //print("SceneLoader: DelaySplScrFade - delayed " + SplashScreenDelay + "s");
        splashScreenAnimator.SetTrigger(triggers.FADE);
    }

    private string NameFromIndex(int BuildIndex) { ///@Author:  Iamsodarncool/UnityAnswers
        string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }

    public int SceneIndexFromName(string sceneName) {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
            string testedScreen = NameFromIndex(i);
            //print("SceneLoader/sceneIndexFromName: i: " + i + " sceneName = " + testedScreen);
            if (testedScreen == sceneName)
                return i;
        }
        return -1;
    }

    private bool isCurrentSceneName(String sceneName) {
        if (SceneManager.GetActiveScene().name == sceneName) return true;
        return false;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }
}
