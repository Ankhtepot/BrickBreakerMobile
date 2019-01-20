using Assets.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{

    [SerializeField] Persistance source;

    private void Start() {
        source = FindObjectOfType<Persistance>();
    }

    public void SaveAllData() {
        SaveOptions();
        SaveScoreItems();
    }

    public void SaveScoreItems() {
        //print("Saving ScoreItems");
        String[] scores = { persistance.SCORE1, persistance.SCORE2, persistance.SCORE3,
            persistance.SCORE4, persistance.SCORE5 };
        List<ScoreItem> scoreItemsToSave = source.options.GetScoreItems();

        for (int i = 0; i < scoreItemsToSave.Count; i++) {
            //print("Saving ScoreItem: \""+scores[i]+"\" : HighestLevel: "+scoreItemsToSave[i].HighestLevel+" Date: "+ scoreItemsToSave[i].Date+ " Score: " + scoreItemsToSave[i].Score);
            ES3.Save<ScoreItem>(scores[i], scoreItemsToSave[i]);
        }
    }

    public void SaveOptions() {
        //print("Saving options: MusicIsOn: " + source.options.SoundAndMusicOn + " HIghestLevel: " + source.options.HighestLevel + " ShowHintBoards: " + source.options.ShowHintBoards);
        ES3.Save<int>(persistance.HIGHESTLEVEL, source.options.HighestLevel);
        ES3.Save<bool> (persistance.SHOWHINTBOARDS, source.options.ShowHintBoards);
        ES3.Save<bool>(persistance.MUSICON, source.options.SoundAndMusicOn);
    }
}
