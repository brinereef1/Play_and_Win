using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        LoadSettings();

        if (sfxToggle != null) sfxToggle.onValueChanged.AddListener(ToggleSFX);
        if (musicToggle != null) musicToggle.onValueChanged.AddListener(ToggleMusic);
        if (volumeSlider != null) volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void Update()
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
        bool isSfxOn = PlayerPrefs.GetInt(SFX_PREF, 1) == 1;
        bool isMusicOn = PlayerPrefs.GetInt(MUSIC_PREF, 1) == 1;
        float volume = PlayerPrefs.GetFloat(VOLUME_PREF, 1f);

        if (sfxToggle != null) sfxToggle.isOn = isSfxOn;
        if (musicToggle != null) musicToggle.isOn = isMusicOn;
        if (volumeSlider != null) volumeSlider.value = volume;

        ToggleSFX(isSfxOn);
        ToggleMusic(isMusicOn);
        SetVolume(volume);
    }

    public void ToggleSFX(bool isOn)
    {
        if (sfxSource != null)
        {
            sfxSource.mute = !isOn;
            PlayerPrefs.SetInt(SFX_PREF, isOn ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public void ToggleMusic(bool isOn)
    {
        if (musicSource != null)
        {
            musicSource.mute = !isOn;
            PlayerPrefs.SetInt(MUSIC_PREF, isOn ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public void SetVolume(float volume)
    {
        if (sfxSource != null) sfxSource.volume = volume;
        if (musicSource != null) musicSource.volume = volume;
        PlayerPrefs.SetFloat(VOLUME_PREF, volume);
        PlayerPrefs.Save();
    }
}
