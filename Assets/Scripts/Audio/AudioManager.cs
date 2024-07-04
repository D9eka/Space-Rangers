using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private GameObject _audioObjectPrefab;
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioClip[] _musicArray;

    public Action<bool> ChangeSoundState;
    public Action<bool> ChangeMusicState;

    public static AudioManager Instance { get; private set; }

    public void OnChangeAudioState(bool state)
    {
        PlayerPrefs.SetInt("Audio", state ? 1 : 0);
        ChangeSoundState?.Invoke(state);
    }

    public void OnChangeMusicState(bool state)
    {
        PlayerPrefs.SetInt("Music", state ? 1 : 0);
        ChangeMusicState?.Invoke(state);
    }

    private void Awake()
    {
        Instance = this;
        _musicAudioSource.loop = true;
    }

    private void Start()
    {
        PlayMusic(_musicArray);
    }

    public void PlayMusic(AudioClip[] clips)
    {
        PlayMusic(clips[Random.Range(0, clips.Length)]);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (_musicAudioSource.clip == clip)
            return;
        _musicAudioSource.Stop();
        _musicAudioSource.clip = clip;
        _musicAudioSource.Play();
    }

    public void PlaySound(AudioClip[] clips, Vector3 position)
    {
        PlaySound(clips[Random.Range(0, clips.Length)], position);
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        AudioObject soundGO = Instantiate(_audioObjectPrefab, position, Quaternion.identity).GetComponent<AudioObject>();
        soundGO.Initialize(clip, 1f);
    }
}