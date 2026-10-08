using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    private static SoundManager Instance;

    private static AudioSource audioSource;
    private static SoundLibrary soundLibrary;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            soundLibrary = GetComponent<SoundLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName)
    {
        AudioClip audioClip = soundLibrary.GetRandom(soundName);
        if(audioClip != null)
        {
            Debug.LogWarning($"Звук з іменем {soundName} не знайдено в SoundManager!");
            audioSource.PlayOneShot(audioClip);
        }
    }

    void Start()
    {
        sfxSlider.onValueChanged.AddListener(delegate {onValueChanged();});
    }

    private static void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    private void onValueChanged()
    {
        SetVolume(sfxSlider.value);
    }
}
