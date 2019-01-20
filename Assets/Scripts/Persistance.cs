using Assets.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Load), typeof(Save))]
public class Persistance : MonoBehaviour
{
    [SerializeField] Load load;
    [SerializeField] Save save;
    [SerializeField] public Options options;

    private void Awake() {
        load = GetComponent<Load>();
        save = GetComponent<Save>();
        options = FindObjectOfType<Options>();
    }

    private void Start() {
    }

    public void SaveAllData() {
        save.SaveAllData();
    }

    public void SaveOptions() {
        save.SaveOptions();
    }

    public void SaveScores() {
        save.SaveScoreItems();
    }

    public OptionsSet LoadAllData() {
        return load.LoadAllData();
    }
}
