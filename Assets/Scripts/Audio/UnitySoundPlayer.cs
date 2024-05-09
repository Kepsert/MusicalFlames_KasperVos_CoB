
using Misc;
using System.Collections;
using UnityEngine;

public class UnitySoundPlayer : ISoundPlayer
{
    private AudioSourcePool _audioSourcePool;

    public UnitySoundPlayer(AudioClip clip, int poolSize, Transform parentTransform)
    {
        _audioSourcePool = new AudioSourcePool(clip, poolSize, parentTransform);
    }

    public void PlaySound(AudioClip clip, float volume, float pitch)
    {
        AudioSource source = _audioSourcePool.GetAudioSource();
        source.gameObject.SetActive(true);
        source.volume = volume;
        source.pitch = pitch;
        source.PlayOneShot(clip);

        Timer.Instance.AddTimer(clip.length, () =>
        {
            _audioSourcePool.ReturnAudioSource(source);
        });
    }
}
