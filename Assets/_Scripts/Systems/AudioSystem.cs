using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : StaticInstance<AudioSystem>
{
    [SerializeField]
    private AudioSource _musicSource;

    [SerializeField]
    private AudioSource _soundsSource;

    [SerializeField]
    private List<Sound> _musicSounds,
        _sfxSounds;

    public void PlayMusic(string name)
    {
        Sound s = _musicSounds.Find(s => s.name == name);
        if (s == null)
        {
            Debug.Log("Sound not found: " + name);
        }
        else
        {
            _musicSource.clip = s.clip;
            _musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = _sfxSounds.Find(s => s.name == name);
        if (s == null)
        {
            Debug.Log("Sound not found: " + name);
        }
        else
        {
            _soundsSource.PlayOneShot(s.clip);
        }
    }

    public void SetMuteSFX(bool isOn)
    {
        _soundsSource.mute = !isOn;
    }
}

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}
