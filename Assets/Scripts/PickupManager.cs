using Assets.Classes;
using Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour {
    [Header("Setup")]
    [SerializeField] GameObject[] ListOfUsedPickups;
    [SerializeField] float PickupDropChance = 40f;
    [SerializeField] float dropCD = 10f;
    [SerializeField] bool dropOffCD = true;
    [Header("Glue setup")]
    [SerializeField] float GlueDuration = 10f;
    [SerializeField] Color BasePaddleColor;
    [SerializeField] Color GlueColor;
    [Header("Laser setup")]
    [SerializeField] float LaserDuration = 10f;
    [SerializeField] MagicShot magicShot;
    [SerializeField] float MagicShotYOffset = 0.2f;
    [Header("Fireball setup")]
    [SerializeField] float FireballDuration = 10f;
    [SerializeField] Sprite FireballSprite;
    [SerializeField] Sprite NormalBallSprite;
    [Header("Enlarge setup")]
    [SerializeField] float EnlargeDuration = 10f;
    [SerializeField] float BaseXScale;
    [SerializeField] float EnlargedXScaleMax = 2f;
    [SerializeField] float SinglePickupEnlargeBonus = 0.3f;
    [SerializeField] float PaddleXScale;
    [SerializeField] float ResizeStep = 0.01f;
    [Header("Multiple setup")]
    [SerializeField] int MaxBallsSpawnedAtOnce = 10;

    //Caches
    Paddle paddle;

    //status
    GameController gameSession;
    bool isLaserActive = false;
    bool isEnlargeActive = false;
    Vector3 MagicShotOffset;
    private int GlueCount = 0;
    private int LaserCount = 0;
    private int FireballCount = 0;
    private int EnlargeCount = 0;

    private void Start() {
        gameSession = FindObjectOfType<GameController>();
        MagicShotOffset = new Vector3(0, MagicShotYOffset, 0);
        paddle = FindObjectOfType<Paddle>();
        BaseXScale = paddle.transform.localScale.x;
        PaddleXScale = BaseXScale;
        BasePaddleColor = paddle.GetComponent<SpriteRenderer>().color;
    }

    private void Update() {
        //if (isLaserActive && Input.GetMouseButtonDown(0)) {
        //    FireMagicShot();
        //}
        if (paddle.transform.localScale.x < PaddleXScale - 0.01 ||
            paddle.transform.localScale.x > PaddleXScale + 0.01) AdjustPaddleXScale();
        if (Input.GetKeyDown(KeyCode.Keypad1)) ApplyEffect(Pickup.PickupType.Enlarge);
        if (Input.GetKeyDown(KeyCode.Keypad2)) ApplyEffect(Pickup.PickupType.Glue);
        if (Input.GetKeyDown(KeyCode.Keypad3)) ApplyEffect(Pickup.PickupType.Laser);
        if (Input.GetKeyDown(KeyCode.Keypad4)) ApplyEffect(Pickup.PickupType.Multiple);
        if (Input.GetKeyDown(KeyCode.Keypad5)) ApplyEffect(Pickup.PickupType.Life);
        if (Input.GetKeyDown(KeyCode.Keypad6)) ApplyEffect(Pickup.PickupType.Fireball);
    }

    private void AdjustPaddleXScale() {
        paddle.transform.localScale +=
            new Vector3(paddle.transform.localScale.x < PaddleXScale ? ResizeStep : -ResizeStep, 0, 0);
    }

    public void FireMagicShot() {
        if (isLaserActive) {
            MagicBall[] balls = FindObjectsOfType<MagicBall>();
            foreach (MagicBall ball in balls) {
                Instantiate(magicShot, ball.transform.position + MagicShotOffset, Quaternion.identity);
            } 
        }
    }

    public GameObject[] GetList() {
        return ListOfUsedPickups;
    }

    public void ProcessPickupChanceOfSpawning(Vector2 spawnPosition) {
        int chanceRoll = UnityEngine.Random.Range(1, 101);
        if (dropOffCD && chanceRoll <= PickupDropChance) {
            SpawnPickup(spawnPosition);
            StartCoroutine(StartDropCD());
        }
    }

    IEnumerator StartDropCD() {
        dropOffCD = false;
        yield return new WaitForSeconds(dropCD);
        dropOffCD = true;
    }

    private void SpawnPickup(Vector2 spawnPosition) {
        int chanceRatio = UnityEngine.Random.Range(0, ListOfUsedPickups.Length);
        Instantiate(ListOfUsedPickups[chanceRatio], spawnPosition, new Quaternion(0, 0, 0, 0));
    }

    public void ApplyEffect(Pickup.PickupType pickupType) {
        //Debug.Log("Applying effect of Pickup: " + pickupType.ToString());
        switch (pickupType) {
            case (Pickup.PickupType.Glue): StartCoroutine(ActivateGlue()); break;
            case (Pickup.PickupType.Life): ActivateLife(); break;
            case (Pickup.PickupType.Laser): StartCoroutine(ActivateLaser()); break;
            case (Pickup.PickupType.Multiple): ActivateMultiple(); break;
            case (Pickup.PickupType.Enlarge): StartCoroutine(ActivateEnlarge()); break;
            case (Pickup.PickupType.Fireball): StartCoroutine(ActivateFireball()); break;
        }
    }

    IEnumerator ActivateFireball() {
        foreach (Ball ball in FindObjectsOfType<Ball>()) {
            ball.GetComponent<SpriteRenderer>().sprite = FireballSprite;
            ball.tag = tags.FIREBALL;
            foreach (ParticleSystem effect in ball.GetComponentsInChildren<ParticleSystem>())
                effect.Play();
        }
        foreach (Brick brick in FindObjectsOfType<Brick>()) {
            brick.GetComponent<PolygonCollider2D>().isTrigger = true;
        }
        FireballCount++;
        yield return new WaitForSeconds(FireballDuration);
        FireballCount--;
        if (FireballCount <= 0) {
            foreach (Ball ball in FindObjectsOfType<Ball>()) {
                ball.GetComponent<SpriteRenderer>().sprite = NormalBallSprite;
                ball.tag = tags.BALL;
                foreach (ParticleSystem effect in ball.GetComponentsInChildren<ParticleSystem>()) {
                    if (effect.name != gameobjects.SPARKLES) effect.Stop();
                }
            }
            foreach (Brick brick in FindObjectsOfType<Brick>()) {
                brick.GetComponent<PolygonCollider2D>().isTrigger = false;
            }
        }
    }

    IEnumerator ActivateEnlarge() {
        PaddleXScale = Mathf.Clamp(PaddleXScale + SinglePickupEnlargeBonus, 0, EnlargedXScaleMax);
        EnlargeCount++;
        isEnlargeActive = true;
        yield return new WaitForSeconds(EnlargeDuration);
        EnlargeCount--;
        if (EnlargeCount <= 0) {
            PaddleXScale = BaseXScale;
            isEnlargeActive = false;
        }
    }

    private void ActivateMultiple() {
        if (gameSession.BallAmount() < MaxBallsSpawnedAtOnce + 1) {
            Ball[] balls = FindObjectsOfType<Ball>();
            foreach (Ball oldBall in balls) {
                for (int i = 0; i < 3; i++) {
                    Vector2 spawnPosition = new Vector2(Mathf.Clamp(oldBall.transform.position.x + i / 10 + 0.2f, 1, 15), Mathf.Clamp(oldBall.transform.position.y + i + 0.2f, 1, 11));
                    if (FindObjectsOfType<Ball>().Length < MaxBallsSpawnedAtOnce + 1) {
                        Ball newBall = Instantiate(oldBall, spawnPosition, Quaternion.identity);
                        newBall.isGlueApplied = oldBall.isGlueApplied;
                        newBall.GetComponent<Rigidbody2D>().velocity = oldBall.GetComponent<Rigidbody2D>().velocity;
                        newBall.hasStarted = oldBall.hasStarted;
                    }
                }
            }
        }
    }

    IEnumerator ActivateLaser() {
        print("Activating laser");
        if (!isLaserActive) {
            foreach (MagicBall ball in FindObjectsOfType<MagicBall>()) {
                ball.GetComponent<Animator>().SetTrigger(triggers.ARRIVE);
            }
        }
        isLaserActive = true;
        LaserCount++;
        yield return new WaitForSeconds(LaserDuration);
        LaserCount--;
        if (LaserCount <= 0) {
            foreach (MagicBall ball in FindObjectsOfType<MagicBall>()) {
                ball.GetComponent<Animator>().SetTrigger(triggers.VANISH);
            }
            isLaserActive = false;
        }
    }

    private void ActivateLife() {
        gameSession.AddLife();
    }

    IEnumerator ActivateGlue() {
        GlueActiveEffect glueEffectOnPaddle = FindObjectOfType<GlueActiveEffect>();
        if (glueEffectOnPaddle) {
            paddle.GetComponent<SpriteRenderer>().color = GlueColor;
            glueEffectOnPaddle.GetComponent<ParticleSystem>().Play();
        }
        foreach (Ball ball in FindObjectsOfType<Ball>()) {
            ball.isGlueApplied = true;
        }
        GlueCount++;
        yield return new WaitForSeconds(GlueDuration);
        GlueCount--;
        if (GlueCount <= 0) {
            if (glueEffectOnPaddle) {
                paddle.GetComponent<SpriteRenderer>().color = BasePaddleColor;
                glueEffectOnPaddle.GetComponent<ParticleSystem>().Stop();
            }
            foreach (Ball ball in FindObjectsOfType<Ball>()) {
                ball.isGlueApplied = false;
            }
        }
    }

    private void SwitchSpriteOff<T>() where T : Component {
        FindObjectOfType<T>().GetComponent<SpriteRenderer>().enabled = false;
    }

}
