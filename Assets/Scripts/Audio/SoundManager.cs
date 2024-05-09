using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class SoundManager
{
    private readonly ISoundPlayer _soundPlayer;
    private readonly Dictionary<string, AudioClip> _soundDictionary;
    private float _sfxVolume;

    public SoundManager(ISoundPlayer player, Dictionary<string, AudioClip> sounds, float volume)
    {
        _soundPlayer = player;
        _soundDictionary = sounds;
        _sfxVolume = volume;
    }

    public void UpdateVolume(float volume)
    {
        _sfxVolume = volume;
    }

    public void PlaySound(string soundName, float pitch = 1.0f)
    {
        if (_soundDictionary.ContainsKey(soundName))
        {
            float volumeLevel = _sfxVolume;
            _soundPlayer.PlaySound(_soundDictionary[soundName], volumeLevel, pitch);
        }
        else
        {
            Debug.LogWarning("Sound not found: " + soundName);
        }
    }
}
