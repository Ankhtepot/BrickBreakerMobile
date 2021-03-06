﻿using Assets.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    [SerializeField] Options options;
    [SerializeField] SceneLoader SL;

    void Start() {
        options = FindObjectOfType<Options>();
        SL = FindObjectOfType<SceneLoader>();
        SetContinueButtonText();
    }

    private void SetContinueButtonText() {
        TextMeshProUGUI targetText =
                            GameObject.Find(gameobjects.TARGET_TEXT).GetComponentInChildren<TextMeshProUGUI>();
        if (options && targetText) {
            //print("options.highestLevel: " + options.HighestLevel);
            String levelToGoTo = "Level ";
            switch (options.HighestLevel) {
                case -1: case 0: case 1: case 2: levelToGoTo += intconstants.FIRSTLEVEL-1 ; break; //-1 for SplashScreen
                case intconstants.MRBRICKWORM: levelToGoTo = "Mr. BrickWorm"; break;
                default: levelToGoTo += (options.HighestLevel-1).ToString(); break;
            }
            if (!options) print("SceneLoader/SetContinueButtonText: missing options");
            //print("SceneLoader/SetContinueButtonText: targetText: " + levelToGoTo);
            targetText.text = levelToGoTo;
        }
    }

    public void ContinueButtonClick() {
        int targetLevel = 1;
        if (options) {
            print("SceneLoader/ContinueButtonClick: HighestLevel is: " + options.HighestLevel);
            switch (options.HighestLevel) {
                case -1: case 0: case 1: case 2: targetLevel = intconstants.FIRSTLEVEL; break;
                case intconstants.MRBRICKWORM:
                    targetLevel = SL.SceneIndexFromName(scenes.MRBRICKWORMLEVEL); break;
                default: targetLevel = options.HighestLevel; break; 
            }
        } else if (!options) print("SceneLoader/ContinueButtonClick: missing options");
        SL.LoadScene(targetLevel);
    }
}
