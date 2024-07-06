using Managers;
using System;
using System.Collections;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioObject : MonoBehaviour
    {
        public enum AudioType
        {
            Sound,
            Music
        }

        [SerializeField] private AudioType _type;

        private AudioSource _source;
        private AudioClip _sound;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void Start()
        {
            switch (_type) 
            { 
                case AudioType.Sound:
                    _source.mute = !SaveManager.Instance.GetSoundState();
                    AudioManager.Instance.ChangeSoundState += AudioManager_ChangeSoundState;
                    break;
                case AudioType.Music:
                    _source.mute = !SaveManager.Instance.GetMusicState();
                    AudioManager.Instance.ChangeMusicState += AudioManager_ChangeMusicState;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void AudioManager_ChangeSoundState(bool state)
        {
            _source.mute = !state;
        }

        private void AudioManager_ChangeMusicState(bool state)
        {
            _source.mute = !state;
        }

        public void Initialize(AudioClip clip, float volume)
        {
            _sound = clip;
            _source.volume = volume;
            StartCoroutine(PlaySound());
        }

        private IEnumerator PlaySound()
        {
            _source.PlayOneShot(_sound);
            yield return new WaitForSecondsRealtime(_sound.length);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (!AudioManager.Instance)
            {
                return;
            }    

            switch (_type)
            {
                case AudioType.Sound:
                    AudioManager.Instance.ChangeSoundState -= AudioManager_ChangeSoundState;
                    break;
                case AudioType.Music:
                    AudioManager.Instance.ChangeMusicState -= AudioManager_ChangeMusicState;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}