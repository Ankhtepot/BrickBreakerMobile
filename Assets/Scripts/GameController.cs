﻿#pragma warning disable 0414

using Assets.Classes;
using Assets.Interfaces;
using Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    [Header("GameplaySetups")]
    [SerializeField] int brickCount = 0;
    [SerializeField] int ballCount = 0;
    [SerializeField] int Lives = 3;
    [SerializeField] public bool inputIsEnabled = false;
    [SerializeField] public bool isBossSessionInProgress = false;
    [Header("PlayFieldSetups")]
    [SerializeField] public Vector3 basePaddleBallRelation;
    [SerializeField] bool checkForBricks = true;
    //[SerializeField] float SplashScreenShowUpDuration = 2f;
    private TextMeshProUGUI LivesText;
    private TextMeshProUGUI ScoreText;
    private GameObject canvas;
    //[SerializeField] float xMinKe;
    [Header("BallShakerProps")]
    [Range(0, 10)] [SerializeField] float xMin = 3f;
    [Range(0, 10)] [SerializeField] float xMax = 6f;
    [Range(0, 10)] [SerializeField] float yMin = 0f;
    [Range(0, 10)] [SerializeField] float yMax = 5f;

    //Caches
    SceneLoader sceneLoader;
    Options options;
    PaddleMovement PM;
    //static GameSession instance = null;
    bool brickCheckCDIsOff = true;

    private void Start() {
        SetCaches();
        InicializeLevel();
    }

    private void InicializeLevel() {
        if (!sceneLoader.isCurrentSceneLevel()) {
            if (canvas) canvas.SetActive(false);
        }
        if (options && sceneLoader.isCurrentSceneLevel()) {
            PM.FindAndAssignPaddle();
            Lives = options.LivesCurrent;
            options.HighestLevel = SceneManager.GetActiveScene().buildIndex;
            if (LivesText) UpdateLivesText();
            else if(!LivesText)print("GameSession/InicializeLevel: missing LiveText");
            if (ScoreText) UpdateScoreText();
            else if(!ScoreText) print("GameSession/InicializeLevel: missing ScoreText");
            StartCoroutine(DelayGameplay());
        } else if (!options) print("GameSession/InicializeLevel: missing options");
    }

    private void SetCaches() {
        LivesText = GameObject.Find(gameobjects.LIVESTEXT).GetComponent<TextMeshProUGUI>();
        ScoreText = GameObject.Find(gameobjects.SCORETEXT).GetComponent<TextMeshProUGUI>();
        canvas = GameObject.Find(gameobjects.GAME_CANVAS);
        SceneManager.sceneLoaded += OnSceneLoadGameSession;
        options = FindObjectOfType<Options>();
        sceneLoader = FindObjectOfType<SceneLoader>();
        PM = FindObjectOfType<PaddleMovement>();
    }

    private void Update() {
        ScreenShake();
        if (!isBossSessionInProgress) {
            while (checkForBricks && brickCheckCDIsOff) {
                StartCoroutine(checkBrickCount());
            }
        }
    }

    private void ScreenShake() {
        if (Input.GetButtonDown(triggers.JUMP)) {
            //print("Jump button pressed");
            PerformScreenShake();
        }
    }

    public void PerformScreenShake() {
        Animator shakeAnimation = FindObjectOfType<Camera>().GetComponent<Animator>();
        if (shakeAnimation) {
            shakeAnimation.SetTrigger(triggers.SHAKECAMERA);
            Vector2 shakeVector = new Vector2(UnityEngine.Random.Range(xMin, xMax),
                    UnityEngine.Random.Range(yMin, yMax));
            foreach (Ball ball in FindObjectsOfType<Ball>()) {
                ball.GetComponent<Rigidbody2D>().velocity += shakeVector;
            }
            foreach (PhysicalBall ball in FindObjectsOfType<PhysicalBall>()) {
                ball.GetComponent<Rigidbody2D>().velocity += shakeVector;
            }
        }
    }

    private IEnumerator checkBrickCount() {
        brickCheckCDIsOff = false;
        yield return new WaitForSeconds(2f);
        //print("Update: Checking if all bricks are gone.");
        CheckBrickCount();
        brickCheckCDIsOff = true;
    }

    public void AddBall() {
        ballCount = BallAmount();
    }

    public void RetractBall() {
        ballCount = BallAmount();
        if (ballCount <= 1) RetractLife();
    }

    public int BallAmount() {
        return FindObjectsOfType<Ball>().Length;
    }

    private void CheckBrickCount() {
        if (CountBricks() <= 0 && sceneLoader.isCurrentSceneLevel()) {
            //Debug.Log("All bricks gone, loading next screen");
            IBoss boss = FindObjectOfType<Boss>() as IBoss;
            if (boss != null) {
                isBossSessionInProgress = true;
                boss.StartEncounter();
                options.HighestLevel = intconstants.MRBRICKWORM;
            } else {
                NextLevel();
            }
        }
    }

    public void NextLevel() {
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
        if (sceneLoader) {
            if (sceneLoader.isCurrentSceneLevel()) sceneLoader.LoadScene();
        } else print("GameSession/NextLevel: SceneLoader not found");
    }

    private int CountBricks() {
        //print("In count bricks");
        int counter = 0;
        foreach (Brick brick in FindObjectsOfType<Brick>()) {
            if (brick.tag == tags.BRICK) counter++;
        }
        brickCount = counter;
        //print("Brick counter: " + counter);
        return counter;
    }

    public void AddLife() {
        Lives++;
        options.LivesCurrent++;
        UpdateLivesText();
        Animator paddle = FindObjectOfType<LifeAdjustment>().GetComponent<Animator>();
        if (paddle) paddle.SetTrigger(triggers.PLUS);
    }

    public void RetractLife() {
        if (Lives <= 0) {
            FindObjectOfType<SceneLoader>().LoadGameOverScene();
        } else {
            Lives--;
            options.LivesCurrent--;
            Animator paddle = FindObjectOfType<LifeAdjustment>().GetComponent<Animator>();
            if (paddle) paddle.SetTrigger(triggers.MINUS);
            UpdateLivesText();
        }
        //Debug.Log("GameSession: RetractLife: afterRetract, Lives = " + Lives);
    }

    private void UpdateLivesText() {
        if (LivesText) LivesText.text = Lives.ToString();
    }

    private void UpdateScoreText() {
        if (ScoreText) {
            //print("Updating Score Text with value: " + options.Score);
            ScoreText.text = options.Score.ToString();
        }
    }

    private void OnSceneLoadGameSession(Scene loadedScene, LoadSceneMode mode) {
        //Debug.Log("GameSession/OnSceneLoad: start");
        if (options) {
            StartCoroutine(DelayGameplay());
        } else print("GameSession/DelayGameplay: options not found");
        if (sceneLoader.isCurrentSceneLevel()) {
            if (canvas) canvas.SetActive(false);
        }
    }

    IEnumerator DelayGameplay() {
        //Debug.Log("GameSession/DelayGameplay: start");
        inputIsEnabled = false;
        float waitTime = options.ShowHintBoards ? sceneLoader.SplashScreenDelay : 0f;
        //Debug.Log("GameSession/DelayGameplay: waitTime: " + (waitTime + sceneLoader.SplScrProlongOffset));
        yield return new WaitForSeconds(waitTime + sceneLoader.SplScrProlongOffset);
        inputIsEnabled = true;
    }

    public void LockBallAndPaddle(bool isLocked) {
        if (isLocked) {
            FindObjectOfType<Ball>().lockToPaddle();
        }
        inputIsEnabled = !isLocked;
        //print("Movement is Enabled = " + inputIsEnabled);
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoadGameSession;
    }

    public void AddScore(int brickHPMultiplier) {
        if (options) {
            int addedScore = options.baseForScore * brickHPMultiplier * Mathf.Max(options.LivesCurrent,1);
            //print("In add score. Adding: bfs: " + options.baseForScore + " bHPm: " + brickHPMultiplier + "LC: " + options.LivesCurrent + " total: " + addedScore);
            options.AddScore(addedScore);
            UpdateScoreText();
        } else if (!options) print("GameSession/AddScore: no Options found");
    }
}
