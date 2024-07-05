using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string SOUND_STATE = "Sound";
    private const string MUSIC_STATE = "Music";

    public static SaveManager Instance;

    public bool GetSoundState()
    {
        return GetPlayerPrefState(SOUND_STATE);
    }

    public bool GetMusicState()
    {
        return GetPlayerPrefState(MUSIC_STATE);
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AudioManager.Instance.ChangeSoundState += AudioManager_ChangeSoundState;
        AudioManager.Instance.ChangeMusicState += AudioManager_ChangeMusicState;
    }

    private void AudioManager_ChangeSoundState(bool state)
    {
        SetPlayerPrefState(SOUND_STATE, state);
    }

    private void AudioManager_ChangeMusicState(bool state)
    {
        SetPlayerPrefState(MUSIC_STATE, state);
    }

    private bool GetPlayerPrefState(string key)
    {
        return !PlayerPrefs.HasKey(key) || PlayerPrefs.GetInt(key) == 1;
    }

    private void SetPlayerPrefState(string key, bool value) 
    { 
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }
}