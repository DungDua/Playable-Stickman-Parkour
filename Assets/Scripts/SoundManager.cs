using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skymare
{
    public enum SoundType
    {
        Click, Fall, Landed, Portal, Slime , Claim, CountCoin,TNT, Spin, FireWork, Jump
    }


    public class SoundManager : MonoBehaviour
    {
        private static SoundManager _instance;
        public static SoundManager Instance => _instance;

        [SerializeField] private List<AudioInfo> _listAudio;
        [SerializeField] private AudioSource _audioRun;
        [SerializeField] private AudioSource _audioSound;
        [SerializeField] private AudioClip _audioMenu;
        [SerializeField] private AudioClip _audioGamePlay;
        [SerializeField] private List<AudioClip> _listMusicBand;
        [SerializeField] private AudioSource _audioFXband;

        private AudioSource _audioSource;
        private bool _isTurnSound;
        private float _volumeSound, _volumeMusic;

        // Start is called before the first frame update
        private void Awake()
        {
            _instance = this;
            _audioSource = this.GetComponent<AudioSource>();
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            _audioSound.volume = _audioRun.volume = PlayerPrefs.GetFloat("volume-sound", 1f);
            _audioSource.volume = PlayerPrefs.GetFloat("volume-music", 1f);

            _isTurnSound = _audioSound.volume != 0;
        }



        public void OnPlaySound(SoundType type)
        {
            if (!_isTurnSound) return;

            AudioInfo audio = _listAudio.Find(x => x.Type == type);
            if (audio != null) _audioSound.PlayOneShot(audio.Audio);
        }

        public void OnPlayMusic(bool turn)
        {
            if (turn) _audioSource.Play();
            else _audioSource.Stop();
        }

        public void OnTurnSound(bool turn)
        {
            _isTurnSound = turn;
        }

        public void OnPlaySoundJump()
        {
            if (!_isTurnSound) return;
            this.OnPlaySound(SoundType.Jump);
        }

        public void OnPlaySoundLand()
        {
            if (!_isTurnSound) return;
            this.OnPlaySound(SoundType.Landed);
        }

        public void OnPlaySoundRun(bool isPlay)
        {
            if (isPlay) _audioRun.Play();
            else _audioRun.Stop();
        }

        public void OnSetVolumSound(float volume)
        {
            _audioSound.volume = _audioRun.volume = volume;
            _isTurnSound = volume != 0f;
            PlayerPrefs.SetFloat("volume-sound",volume);
        }

        public void OnSetVolumeMusic(float volume)
        {
            _audioSource.volume = volume;
            PlayerPrefs.SetFloat("volume-music", volume);
        }

        public void OnPlayMusicInGame()
        {
            _audioSource.clip = _audioGamePlay;
            _audioSource.Play();
        }

        public void OnPlayMusicMenu(bool band)
        {
            if (!band)
            {
                _audioSource.clip = _audioMenu;
            }
            else
            {
                _audioSource.clip = _listMusicBand[Random.Range(0, 2)];
            }

            _audioSource.Play();
        }

        public void OnPlayFXBand(int id)
        {
            AudioClip audio = _listMusicBand[id];
            _audioFXband.PlayOneShot(audio);
            StartCoroutine(IEVolume(audio.length));
            

            IEnumerator IEVolume(float delay)
            {
                float vl = _audioSource.volume;
                while(vl > 0.1f)
                {
                    vl -= Time.deltaTime;
                    _audioSource.volume = vl;
                    yield return null;
                }
                yield return new WaitForSeconds(delay + 1f);
                float volum = PlayerPrefs.GetFloat("volume-music", 1f);
                while(vl < volum)
                {
                    vl += Time.deltaTime;
                    _audioSource.volume = vl;
                    yield return null;
                }
            }
        }

        
    }

    [System.Serializable]
    public class AudioInfo
    {
        public string Name;
        public AudioClip Audio;
        public SoundType Type;
    }
}