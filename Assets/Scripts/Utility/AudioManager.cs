using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour
{
    [Header("UI References")]
    public Toggle sfxToggle;
    public Toggle musicToggle;
    public Slider volumeSlider;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    private const string SFX_PREF = "SFX_Toggle";
    private const string MUSIC_PREF = "Music_Toggle";
    private const string VOLUME_PREF = "Audio_Volume";

    private void Start()
    {
        LoadSettings();

        sfxToggle.onValueChanged.AddListener(delegate { ToggleSFX(sfxToggle.isOn); });
        musicToggle.onValueChanged.AddListener(delegate { ToggleMusic(musicToggle.isOn); });
        volumeSlider.onValueChanged.AddListener(delegate { SetVolume(volumeSlider.value); });
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
            {
                Debug.Log("Clicked UI: " + EventSystem.current.currentSelectedGameObject.name);

                if (sfxSource != null)
                {
                    Debug.Log("Playing SFX...");
                    sfxSource.Play();
                }
                else
                {
                    Debug.LogWarning("sfxSource is NULL!");
                }
            }
        }
    }

    private void LoadSettings()
    {
        sfxToggle.isOn = PlayerPrefs.GetInt(SFX_PREF, 1) == 1;
        musicToggle.isOn = PlayerPrefs.GetInt(MUSIC_PREF, 1) == 1;
        volumeSlider.value = PlayerPrefs.GetFloat(VOLUME_PREF, 1f);

        ToggleSFX(sfxToggle.isOn);
        ToggleMusic(musicToggle.isOn);
        SetVolume(volumeSlider.value);
    }

    public void ToggleSFX(bool isOn)
    {
        if (sfxToggle != null)
        {
            sfxSource.mute = !isOn;
            PlayerPrefs.SetInt(SFX_PREF, isOn ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public void ToggleMusic(bool isOn)
    {
        if (musicToggle != null)
        {
            musicSource.mute = !isOn;
            PlayerPrefs.SetInt(MUSIC_PREF, isOn ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public void SetVolume(float volume)
    {
        sfxSource.volume = volume;
        musicSource.volume = volume;
        PlayerPrefs.SetFloat(VOLUME_PREF, volume);
        PlayerPrefs.Save();
    }
}