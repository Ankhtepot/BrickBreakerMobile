using Assets.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour {

    [SerializeField] public bool ShowHintBoards = true;
    [SerializeField] private bool soundAndMusicOn = true;
    [SerializeField] public const int LivesBase = 10;
    [SerializeField] public int LivesCurrent;
    [SerializeField] int highestLevel = 0;
    [SerializeField] public int baseForScore = 10;
    [SerializeField] int score = 0;
    [SerializeField] int ScoreItemsCount = 5;
    [SerializeField] List<ScoreItem> scoreList = new List<ScoreItem>();
    [SerializeField] float delayForLoadingData = 0.2f;

    //caches
    SceneLoader SL;
    Persistance persistance;
    [SerializeField] SoundSystem SS;
    //scenes where to save score
    List<String> saveScoreScenes = new List<string>{ scenes.GAME_OVER, scenes.WIN };

    public int HighestLevel {
        get {
            return highestLevel;
        }
        set {
            if (HighestLevel < value) {
                highestLevel = value;
                persistance.SaveOptions();
            }
            if (value == intconstants.MRBRICKWORM) highestLevel = intconstants.MRBRICKWORM;
            else highestLevel = value;
        }
    }

    public int Score {
        get { return score; }
        set { score = value; }
    }

    public bool SoundAndMusicOn {
        get => soundAndMusicOn;
        set {
            soundAndMusicOn = value;
            SS.SetMusicOnOff(value);
            //print("Options: Saving Options from SoundAndMusicOn : " + value);
            persistance.SaveOptions();
        } }

    private void Start() {
        InicializeCaches();
        StartCoroutine(LoadData(persistance.LoadAllData()));
    }

    private IEnumerator LoadData(OptionsSet OS) {
        yield return new WaitForSeconds(delayForLoadingData);
        this.ShowHintBoards = OS.ShowHintBoards;
        this.soundAndMusicOn = OS.MusicOn;
        SS.SetMusicOnOff(soundAndMusicOn);
        this.highestLevel = OS.HighestLevel;
        //print("Loaded optionsSet: " + OS.ToString());
    }

    private void InicializeCaches() {
        SceneManager.sceneLoaded += OnScreenLoad;
        SL = FindObjectOfType<SceneLoader>();
        SS = FindObjectOfType<SoundSystem>();
        LivesCurrent = LivesBase;
        //scoreList = new List<ScoreItem>();
        persistance = FindObjectOfType<Persistance>();
    }

    public void AddScore(int newScore) {
        if (score + newScore < 0) score = 0;
        else score += newScore;
    }

    public void ToggleShowHintBoards() {
        ShowHintBoards = !ShowHintBoards;
        persistance.SaveOptions();
    }

    private void OnScreenLoad(Scene loadedScene, LoadSceneMode mode) {
        ProcessGameData();
        if(saveScoreScenes.Contains(SceneManager.GetActiveScene().name))   persistance.SaveScores();
    }

    private void ProcessGameData() {
        if (SL && !SL.isCurrentSceneLevel()) {
            ProcessLives();
            if(Score > 0) ProcessScore();
        } else if (!SL) print("Options/ProcessGameData: missing sceneLoader");
    }

    private void ProcessLives() {
        //print("Options/OnScreenLoad: setting LivesCurrent to " + LivesBase);
        LivesCurrent = LivesBase;
    }

    private void ProcessScore() {
        ScoreItem newScore = new ScoreItem(Score, HighestLevel-1);
        //print("Options/ProcessScore: Score: " + Score + " scoreList.count: " + scoreList.Count + " ScoreItemsCount: " + ScoreItemsCount);
        Score = 0;
        if (scoreList.Count > 0) {
            foreach (ScoreItem record in scoreList) record.isNewRecord = false;
            for (int i = 0; i < Mathf.Min(scoreList.Count, ScoreItemsCount); i++) {
                if (scoreList[i].Score < newScore.Score) {
                    //print("Options/ProcessScore: adding new score to list at " + i + " position.");
                    scoreList.Insert(i, newScore);
                    if (scoreList.Count > ScoreItemsCount) scoreList.RemoveAt(ScoreItemsCount);
                    break;
                }
            }
        }
        if (scoreList.Count == 0 || (scoreList.Count < ScoreItemsCount && !scoreList.Contains(newScore))) {
            //print("Options/ProcessScore: Adding score to first empty position");
            scoreList.Add(newScore);
        }
    }

    public List<ScoreItem> GetScoreItems() {
        return scoreList;
    }

    public void SetScoreItems(List<ScoreItem> scoreList) {
        this.scoreList = scoreList;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnScreenLoad;
    }
}
