#pragma warning disable 0414

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour {

    [SerializeField] List<GameObject> messageBoards;
    [SerializeField] Image SSB;
    [SerializeField] Sprite NormalSS;
    [SerializeField] Sprite LogoSS;

    //cache
    MessageBoard chosenMessageBoard = null;


    void triggerNextLevel() {
        SceneLoader SL = FindObjectOfType<SceneLoader>();
        Options options = FindObjectOfType<Options>();
        if (options && options.ShowHintBoards) {
            ActivateRandomMessageBoard(); 
        }
        if (SL) SceneManager.LoadScene(SL.SceneToBeLoaded);
        else print("SplashScreen/triggerNextLevel: SceneLoader not found.");
    }

    void ActivateRandomMessageBoard() {
        deactivateMessageBoards();
        messageBoards[Random.Range(0, messageBoards.Count)].
            GetComponentInChildren<MessageBoard>().isActive = true;
    }

    void deactivateMessageBoards() {
        foreach (GameObject board in messageBoards)
            board.GetComponentInChildren<MessageBoard>().isActive = false;
    }

    public enum BackgroundChoice {Logo, Normal };
    public void SwapBackground(BackgroundChoice background) {
        if (background == BackgroundChoice.Logo) SSB.GetComponent<Image>().sprite = LogoSS;
        if (background == BackgroundChoice.Normal) SSB.GetComponent<Image>().sprite = NormalSS;
    }

    //private void Start() {
    //    SceneManager.sceneLoaded += OnSceneLoadMessageBoard;
    //}

    //void OnSceneLoadMessageBoard(Scene loadedScene, LoadSceneMode mode) {

    //}

    //private void OnDisable() {
    //    SceneManager.sceneLoaded -= OnSceneLoadMessageBoard;
    //}
}
