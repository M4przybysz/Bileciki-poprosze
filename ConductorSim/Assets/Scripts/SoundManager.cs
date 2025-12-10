using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] AudioMixer mainMixer;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] TextMeshProUGUI SFXOutput;
    [SerializeField] TextMeshProUGUI musicOutput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadVolume();
    }

    public void LoadVolume()
    {
        SFXSlider.value = GameManager.SFXVolume;
        musicSlider.value = GameManager.musicVolume;

        SetSFXVolume();
        SetMusicVolume();
    }

    public void SetSFXVolume()
    {
        float sliderValue = SFXSlider.value;

        SFXOutput.text = (int)(sliderValue * 100) + "%"; // Change volume text
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20); // Change mixer settings
        GameManager.SFXVolume = sliderValue; // Change data to save
    }

    public void SetMusicVolume()
    {
        float sliderValue = musicSlider.value;

        musicOutput.text = (int)(sliderValue * 100) + "%"; // Change volume text
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20); // Change mixer settings
        GameManager.musicVolume = sliderValue; // Change data to save
    }
}
