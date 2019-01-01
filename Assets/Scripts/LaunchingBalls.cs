using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchingBalls : MonoBehaviour
{
    public void LaunchBalls() {
        foreach(Ball ball in FindObjectsOfType<Ball>()) {
            ball.launchOnClick();
        }
    }
}
