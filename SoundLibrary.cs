using UnityEngine;
using System.Collections.Generic;

public class SoundLibrary : MonoBehaviour
{
    [SerializeField] private SoundEffectGroup[] soundEffectGroups;
    private Dictionary<string, List<AudioClip>> soundDictionary;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        soundDictionary = new Dictionary<string, List<AudioClip>>();
        foreach(SoundEffectGroup SoundEffectGroup in soundEffectGroups)
        {
            soundDictionary[SoundEffectGroup.name] = SoundEffectGroup.audioClips;
        }
    }

    public AudioClip GetRandom(string name)
    {
        if(soundDictionary.ContainsKey(name))
        {
            List<AudioClip> audioClips = soundDictionary[name];
            if(audioClips.Count > 0)
            {
                return audioClips[UnityEngine.Random.Range(0,audioClips.Count)];
            }
        }
        return null;
    }
    
}

[System.Serializable]

    public struct SoundEffectGroup
    {
        public string name;
        public List<AudioClip> audioClips;
    }
