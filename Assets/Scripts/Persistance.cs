using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Load), typeof(Save))]
public class Persistance : MonoBehaviour
{
    [SerializeField] Load load;
    [SerializeField] Save save;
    [SerializeField] public SoundSystem soundSystem;
    [SerializeField] public Options options;

    private void Awake() {
        load = GetComponent<Load>();
        save = GetComponent<Save>();
    }

    private void Start() {
        soundSystem = FindObjectOfType<SoundSystem>();
        options = FindObjectOfType<Options>();
        load.LoadAllData();
    }

    public void SaveAllData() {
        save.SaveAllData();
    }

    public void SaveOptions() {
        save.SaveOptions();
    }

    public void LoadAllData() {
        load.LoadAllData();
    }
}
