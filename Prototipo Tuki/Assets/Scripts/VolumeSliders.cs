using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    
    public enum VolumType{
        MASTER,
        AMBIENCE,
        SFX,
        MUSIC
    }

    [Header("Type")]
    [SerializeField] private VolumType volumType;
    private Slider volumeSlider;

    private void Awake(){
        volumeSlider = GetComponent<Slider>();
    }

    private void Update(){

        switch(volumType){

            case VolumType.MASTER:
                volumeSlider.value = AudioManager.instance.masterVolume;
                break;
            case VolumType.AMBIENCE:
                volumeSlider.value = AudioManager.instance.ambienceVolume;
                break;
            case VolumType.SFX:
                volumeSlider.value = AudioManager.instance.SFXVolume;
                break;
            case VolumType.MUSIC:
                volumeSlider.value = AudioManager.instance.MusicVolume;
                break;
            default:
                Debug.LogWarning("Volume Type not supported: " + volumType);
                break;
        }

    }

    public void OnSliderValueChanged(){

        switch(volumType){

            case VolumType.MASTER:
                AudioManager.instance.masterVolume = volumeSlider.value;
                break;
            case VolumType.AMBIENCE:
                AudioManager.instance.ambienceVolume = volumeSlider.value;
                break;
            case VolumType.SFX:
                AudioManager.instance.SFXVolume = volumeSlider.value;
                break;
            case VolumType.MUSIC:
            AudioManager.instance.MusicVolume = volumeSlider.value;
            break;
            default:
                Debug.LogWarning("Volume Type not supported: " + volumType);
                break;
        }

    }





}
