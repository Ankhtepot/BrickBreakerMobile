using Assets.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicOnOff : MonoBehaviour
{
    [SerializeField] Color onColor;
    [SerializeField] Color offColor;
    [SerializeField] Image SR;
    [SerializeField] SoundSystem SS;

    private void Start() {
        SR = GameObject.Find(gameobjects.MUSICONOFFIMAGE).GetComponent<Image>();
        SS = FindObjectOfType<SoundSystem>();
        setSpriteColor();
    }

    private void setSpriteColor() {
        if (SS.muted) {
            SR.color = offColor;
        } else {
            SR.color = onColor;
        }
    }

    public void SwitchMusicOnOff() {
        SS.musicOnOff();
        setSpriteColor();
    }
}
