using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessVol : MonoBehaviour
{
    private int _wave = 0;
    private ColorGrading _colorGrading = null;

    [SerializeField]
    private SpawnManager _spawnManager;
    [SerializeField]
    private PostProcessVolume _volume;
    
    private void Start()
    {
        _volume.profile.TryGetSettings(out _colorGrading);

    }

    // Update is called once per frame
    void Update()
    {
        if (_colorGrading == null)
        {
            Debug.Log("color grading is null");
        }

        _wave = _spawnManager.GetWaveNumber();

        if (_wave == 1)
        {
            _colorGrading.hueShift.value = 0;
            _colorGrading.saturation.value = 100;
        }

        if (_wave == 2)
        {
            _colorGrading.hueShift.value = -180;
            _colorGrading.saturation.value = 0;
        }
        
        else if (_wave == 3)
        {
            _colorGrading.hueShift.value = -112;
        }
        
        else if (_wave == 4)
        {
            _colorGrading.hueShift.value = 98;
        }
    }
}
