using Messaging;
using Messaging.Messages;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private SoundManager _soundManager;

    public List<AudioClip> _soundClips = new List<AudioClip>();
    private Dictionary<string, AudioClip> _soundDictionary;

    // Hard-code volume cause volume sliders are out of scope
    float _masterVolumeVariable = .5f;
    float _sfxVolumeVariable = .2f;

    [SerializeField] int _initialPoolSize = 5;
    [SerializeField] Transform _parentTransform;

    void Start()
    {
        MessageHub.Subscribe<PlaySFXMessage>(this, PlaySoundEffect);

        ISoundPlayer player = new UnitySoundPlayer(_soundClips[0], _initialPoolSize, _parentTransform);

        _soundDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in _soundClips)
        {
            if (!_soundDictionary.ContainsKey(clip.name))
            {
                _soundDictionary.Add(clip.name, clip);
            }
        }

        _soundManager = new SoundManager(player, _soundDictionary, _sfxVolumeVariable * _masterVolumeVariable);
    }

    void OnDestroy()
    {
        MessageHub.Unsubscribe<PlaySFXMessage>(this);
    }

    void PlaySoundEffect(PlaySFXMessage obj)
    {
        PlaySoundByName(obj.Name, obj.Pitch);
    }

    void PlaySoundByName(string name, float pitch = 1.0f)
    {
        _soundManager.PlaySound("SFX_" + name, pitch);
    }
}
