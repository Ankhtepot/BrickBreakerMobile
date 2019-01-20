#pragma warning disable 0414

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;

public class SoundSystem : MonoBehaviour {

    [Header("Volumes")]
    [SerializeField] float baseVolume = 0.1f;
    [SerializeField] float musicVolume = 0.1f;
    [SerializeField] public bool muted = false;
    [SerializeField] AudioClip MenuMusic;
    //[Range(0, 1)] [SerializeField] float MenuMusicVolume = 0.1f;
    [SerializeField] AudioClip GameMusic;
    //[Range(0, 1)] [SerializeField] float GameMusicVolume = 0.1f;
    [Header("Sound Lists")]
    [SerializeField] AudioClip[] BrickSounds;
    [SerializeField] AudioClip[] AppleSounds;
    [SerializeField] AudioClip[] BossSounds;

    AudioSource audioSource;
    [SerializeField] AudioClip playedMusic;
    [Header("Caches")]
    [SerializeField] Options options;

    public enum PlayListID { Brick, Apple, Boss }

    void Start() {
        options = FindObjectOfType<Options>();
        audioSource = GetComponent<AudioSource>();
        SetMusicOnOff(options.SoundAndMusicOn);
        playedMusic = null;
        if (GetBaseVolume() != 0) PlayMenuMusic();
        //print("MusicPlayer is now active");
    }

    public float GetBaseVolume() {
        return baseVolume;
    }

    public float GetCurrentVolume() {
        return audioSource.volume;
    }

    /// <summary>
    /// Sets volume of AudioSource of the SoundSystem.
    /// </summary>
    /// <param name="newVolume">Value of a new volume.</param>
    public void SetVolume(float newVolume) {
        print("Setting newVolume to: " + newVolume);
        if (audioSource) audioSource.volume = newVolume;
        else if (!audioSource) print("SoundSystem/SetVolume: audioSource is missing");
    }

    private void onSceneLoaded(Scene loadedScene, LoadSceneMode mode) {
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= onSceneLoaded;
    }

    public void PlayClipOnce(AudioClip clip) {
        if (audioSource) audioSource.PlayOneShot(clip);
    }

    private void unMute() {
        if (audioSource) audioSource.volume = baseVolume;
    }

    public void SetMusicOnOff(bool stateTSwitchTo) {
        //print("Toggling music on/off");        
        if (stateTSwitchTo) {
            audioSource.volume = baseVolume;
            muted = false;
        } else {
            audioSource.volume = 0f;
            muted = true;
        }        
    }
    public void PlayRandomSoundFromList(PlayListID playListID) {
        AudioClip[] playList = null;
        switch (playListID) {
            case PlayListID.Apple: playList = AppleSounds; break;
            case PlayListID.Brick: playList = BrickSounds; break;
            case PlayListID.Boss: playList = BossSounds; break;
        }
        //print("SoundSystem/PlayRandomSoundFromList: playing based on PlayListID: " + playListID.ToString());
        playFromPlayList(playList);
    }

    private void playFromPlayList(AudioClip[] playList) {
        if (playList != null || playList.Length != 0) {
            PlayClipOnce(playList[Random.Range(0, playList.Length)]);
        } else {
            print("SoundSystem/playFromPlayList: failed geting playlist, playing BrickSounds instead");
            PlayClipOnce(BrickSounds[0]);
        }
    }

    public void PlayMenuMusic() {
        PlayMusicClip(MenuMusic);
        //musicVolume = MenuMusicVolume;
    }

    public void PlayGameMusic() {
        PlayMusicClip(GameMusic);
        //musicVolume = GameMusicVolume;
    }

    private void PlayMusicClip(AudioClip musicClip) {
        if (playedMusic != musicClip) {
            playedMusic = musicClip;
            audioSource.clip = musicClip;
            audioSource.Play();
        }
    }
}
