using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public Slider volumeSlider;
    public GameObject SettingPopup;

    private void Start()
    {
        // Load volume sebelumnya dari PlayerPrefs
        float savedVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);

        // Set ke AudioSource BGMManager
        if (BGMManager.instance != null && BGMManager.instance.bgmAudioSource != null)
        {
            BGMManager.instance.bgmAudioSource.volume = savedVolume;
        }

        // Set ke slider
        volumeSlider.value = savedVolume;

        // Pasang listener ke slider
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
    }

    public void UpdateVolume(float value)
    {
        if (BGMManager.instance != null && BGMManager.instance.bgmAudioSource != null)
        {
            BGMManager.instance.bgmAudioSource.volume = value;
        }

        // Simpan ke PlayerPrefs
        PlayerPrefs.SetFloat("BGMVolume", value);
    }

    public void ClosePopup()
    {
        SettingPopup.SetActive(false);
        Time.timeScale = 1f;
    }
}
