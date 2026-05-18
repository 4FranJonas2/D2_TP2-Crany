using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [Header("UI Settings")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private float musicVolume = 1.0f;
    private float sfxVolume = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadSettings();

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;

        musicVolume = Mathf.Clamp(musicVolume, 0.0001f, 1f);

        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);

        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        
        sfxVolume = Mathf.Clamp(sfxVolume, 0.0001f, 1f);

        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
        audioMixer.SetFloat("UIVolume", Mathf.Log10(sfxVolume) * 20);

        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);

        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);

        musicSlider.value = musicVolume;

        sfxSlider.value = sfxVolume;

        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }
}