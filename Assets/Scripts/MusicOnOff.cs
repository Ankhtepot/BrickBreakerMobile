using Assets.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicOnOff : MonoBehaviour
{
    [SerializeField] Color onColor;
    [SerializeField] Color offColor;
    [SerializeField] Image onOffImage;
    [SerializeField] SoundSystem SS;

    private void Start() {
        onOffImage = GameObject.Find(gameobjects.MUSICONOFFIMAGE).GetComponent<Image>();
        SS = FindObjectOfType<SoundSystem>();
        setSpriteColor();
    }

    private void setSpriteColor() {
        if (SS.muted) {
            onOffImage.color = offColor;
        } else {
            onOffImage.color = onColor;
        }
    }

    public void SwitchMusicOnOff() {
        FindObjectOfType<Options>().SoundAndMusicOn = !FindObjectOfType<Options>().SoundAndMusicOn;
        setSpriteColor();
    }
}
