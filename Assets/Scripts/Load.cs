using Assets.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour {

    [SerializeField] Persistance source;

    private void Start() {
        source = FindObjectOfType<Persistance>();
    }

    public void LoadAllData() {
        LoadScoreItems();
        LoadOptions();
    }

    private void LoadOptions() {
        print("Loading options");
        if (ES3.KeyExists(persistance.VOLUME)) {
            print("Loading Volume:" + ES3.Load<float>(persistance.VOLUME));
            source.soundSystem.SetVolume(ES3.Load<float>(persistance.VOLUME));
        }
        if (ES3.KeyExists(persistance.HIGHESTLEVEL)) {
            print("Loading HIghestLevel: " + ES3.Load<int>(persistance.HIGHESTLEVEL));
            source.options.HighestLevel = ES3.Load<int>(persistance.HIGHESTLEVEL);
        }
        if (ES3.KeyExists(persistance.SHOWHINTBOARDS)) {
            print("Loading ShowHintBoards: " + ES3.Load<bool>(persistance.SHOWHINTBOARDS));
            source.options.ShowHintBoards = ES3.Load<bool>(persistance.SHOWHINTBOARDS);
        }
    }

    private void LoadScoreItems() {
        print("Loading ScoreItems");
        String[] scores = { persistance.SCORE1, persistance.SCORE2, persistance.SCORE3,
            persistance.SCORE4, persistance.SCORE5 };
        List<ScoreItem> loadedScoreItems = new List<ScoreItem>();

        for (int i = 0; i < scores.Length; i++) {
            print("Loading ScoreItem: \"" + scores[i] + "\"");
            if (ES3.KeyExists(scores[i])) {
                loadedScoreItems.Add(ES3.Load<ScoreItem>(scores[i]));
                print("HighestLevel: " + loadedScoreItems[i].HighestLevel + " Date: " + loadedScoreItems[i].Date + " Score: " + loadedScoreItems[i].Score);
            }
        }

        if (loadedScoreItems != null) {
            source.options.SetScoreItems(loadedScoreItems);
        }
    }
}
