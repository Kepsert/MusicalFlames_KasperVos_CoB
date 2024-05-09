using System;
using System.Collections;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour
{
    public static SoundtrackManager Instance { get; private set; }

    [SerializeField] AudioClip[] _musicTracks;

    // Hard-code volume cause volume sliders are out of scope
    float _masterVolume = .5f;
    float _musicVolume = .2f;
    [SerializeField] float _crossFadeDuration = 1.0f;

    private AudioSourcePool _audioSourcePool;
    private AudioSource _currentTrack;
    private Coroutine _crossfadeCoroutine;

    string _currentTrackName = String.Empty;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _audioSourcePool = new AudioSourcePool(_musicTracks[0], _musicTracks.Length, transform);
    }

    private void Start()
    {
        PlayTrack("Placeholder");
    }

    public void PlayTrack(string trackName)
    {
        if (trackName == string.Empty)
        {
            StopMusic();
        }
        else
        {
            trackName = "Music_" + trackName;
            if (_currentTrackName != trackName)
            {
                _currentTrackName = trackName;
                AudioClip clip = GetMusicTrackByName(trackName);
                if (clip != null)
                {
                    if (_currentTrack != null)
                    {
                        StartCoroutine(CrossfadeOut(clip));
                    }
                    else
                    {
                        PlayNewTrack(clip);
                    }
                }
                else
                {
                    Debug.LogWarning("Soundtrack was not found: " + trackName);
                }
            }
        }
    }

    public void StopMusic()
    {
        if (_currentTrack != null)
        {
            StartCoroutine(FadeOutCurrentTrack());
        }
    }

    private AudioClip GetMusicTrackByName(string trackName)
    {
        foreach (var clip in _musicTracks)
        {
            if (clip.name == trackName)
            {
                return clip;
            }
        }
        return null;
    }

    private IEnumerator CrossfadeOut(AudioClip nextClip)
    {
        if (_crossfadeCoroutine != null)
        {
            StopCoroutine(_crossfadeCoroutine);
        }

        float timer = 0;
        while (timer < _crossFadeDuration)
        {
            float t = timer / _crossFadeDuration;
            _currentTrack.volume = Mathf.Lerp(_musicVolume * _masterVolume, 0, t);
            timer += Time.deltaTime;
            yield return null;
        }

        PlayNewTrack(nextClip);
    }

    private IEnumerator FadeOutCurrentTrack()
    {
        float timer = 0;
        while (timer < _crossFadeDuration)
        {
            float t = timer / _crossFadeDuration;
            _currentTrack.volume = Mathf.Lerp(_musicVolume * _masterVolume, 0, t);
            timer += Time.deltaTime;
            yield return null;
        }

        _currentTrack.gameObject.SetActive(false);
        _currentTrack = null;
    }

    private void PlayNewTrack(AudioClip clip)
    {
        if (_currentTrack != null)
        {
            _currentTrack.gameObject.SetActive(false);
        }

        _currentTrack = _audioSourcePool.GetAudioSource();
        _currentTrack.clip = clip;
        _currentTrack.volume = _musicVolume * _masterVolume;
        _currentTrack.loop = true;
        _currentTrack.gameObject.SetActive(true);
        _currentTrack.Play();
    }
}
