using Assets.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour {

    [SerializeField] Persistance source;

    private void Awake() {
        source = FindObjectOfType<Persistance>();
    }

    public OptionsSet LoadAllData() {
        LoadScoreItems();
        return LoadOptions();
    }

    private OptionsSet LoadOptions() {
        OptionsSet OS = new OptionsSet();

        //String loadingDataMessage = "Loading options: ";
        if (ES3.KeyExists(persistance.HIGHESTLEVEL)) {
            //loadingDataMessage += " HighestLevel: " + ES3.Load<int>(persistance.HIGHESTLEVEL);
            OS.HighestLevel = ES3.Load<int>(persistance.HIGHESTLEVEL);
        }
        if (ES3.KeyExists(persistance.SHOWHINTBOARDS)) {
            //loadingDataMessage += " Loading ShowHintBoards: " + ES3.Load<bool>(persistance.SHOWHINTBOARDS);
            OS.ShowHintBoards = ES3.Load<bool>(persistance.SHOWHINTBOARDS);
        }
        if (ES3.KeyExists(persistance.MUSICON)) {
            //loadingDataMessage += " Loading MusicIsOn:" + ES3.Load<bool>(persistance.MUSICON);
            OS.MusicOn = ES3.Load<bool>(persistance.MUSICON);
        }
        //print(OS.ToString());
        return OS;
    }

    private void LoadScoreItems() {
        //print("Loading ScoreItems");
        String[] scores = { persistance.SCORE1, persistance.SCORE2, persistance.SCORE3,
            persistance.SCORE4, persistance.SCORE5 };
        List<ScoreItem> loadedScoreItems = new List<ScoreItem>();

        for (int i = 0; i < scores.Length; i++) {
            //print("Loading ScoreItem: \"" + scores[i] + "\"");
            if (ES3.KeyExists(scores[i])) {
                loadedScoreItems.Add(ES3.Load<ScoreItem>(scores[i]));
                //print("HighestLevel: " + loadedScoreItems[i].HighestLevel + " Date: " + loadedScoreItems[i].Date + " Score: " + loadedScoreItems[i].Score);
            }
        }

        if (loadedScoreItems != null) {
            source.options.SetScoreItems(loadedScoreItems);
        }
    }
}
