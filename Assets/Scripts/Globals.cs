using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour {

    public static Globals instance = null;

    void Awake() {
        if (instance != null && instance != this) {
            print("Destroying duplicate Globals");
            gameObject.SetActive(false);
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

}
