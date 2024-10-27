using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class SoundManager : Singleton<SoundManager>
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    [Header("Sound Effects")]
    public Sound[] sounds;

    [Header("Background Music")]
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;
    [SerializeField] private AudioSource audioSource;

    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();

    protected override void Awake()
    {
        base.Awake();

        // Initialize soundDictionary
        foreach (Sound s in sounds)
        {
            soundDictionary.Add(s.name, s);
        }
        audioSource.volume = this.volume;
        audioSource.pitch = this.pitch;
    }

    public void PlaySound(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound sound))
        {
            audioSource.PlayOneShot(sound.clip);
        }
        else
        {
            Debug.LogWarning($"Sound: {name} not found!");
        }
    }

    public void StopSound()
    {
        audioSource.Stop();
    }
}