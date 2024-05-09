using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class AudioSourcePool
{
    private Queue<AudioSource> _availableSources = new Queue<AudioSource>();
    private GameObject _container;

    public AudioSourcePool(AudioClip clip, int initialSize, Transform parentTransform)
    {
        _container = new GameObject("AudioSourcePoolContainer");
        _container.transform.parent = parentTransform;

        for (int i = 0; i < initialSize; i++)
        {
            CreateNewAudioSource(clip);
        }
    }

    public AudioSource GetAudioSource()
    {
        if (_availableSources.Count == 0)
        {
            Debug.Log("Creating new audio source");
            // Pool is empty, create a new source
            return CreateNewAudioSource();
        }
        return _availableSources.Dequeue();
    }

    public void ReturnAudioSource(AudioSource source)
    {
        source.gameObject.SetActive(false);
        _availableSources.Enqueue(source);
    }

    private AudioSource CreateNewAudioSource(AudioClip clip = null)
    {
        GameObject go = new GameObject("AudioSource");
        go.transform.parent = _container.transform;
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.playOnAwake = false;
        _availableSources.Enqueue(audioSource);
        return audioSource;
    }
}
