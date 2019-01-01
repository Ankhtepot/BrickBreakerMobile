using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsButton : MonoBehaviour {

    [SerializeField] SceneLoader SL;

    private void Start() {
        SL = FindObjectOfType<SceneLoader>();
    }

    public void OnCreditsButtonClick() {
        if (SL) SL.LoadCreditsScene();
        else if(!SL){
            print("CreditsButton: OnCreditButtonClick: SceneLoader not found");
        }
    }

    public void OnReturnToGameButtonClick() {
        if (SL) SL.LoadFirstScene();
        else if (!SL) {
            print("CreditsButton: OnReturnToGameButtonClick: SceneLoader not found");
        }
    }
}
