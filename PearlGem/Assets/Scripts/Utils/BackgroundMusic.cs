using UnityEngine;

namespace PearlGem.Utils
{
    public class BackgroundMusic : MonoBehaviour
    {
        [SerializeField] AudioClip musicClip;
        
        [Range(0f, 1f)]
        [SerializeField] float volume = 0.5f;
        
        static BackgroundMusic _instance;
        
        AudioSource _audioSource;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                
                _audioSource = gameObject.AddComponent<AudioSource>();
                _audioSource.clip = musicClip;
                _audioSource.volume = volume;
                _audioSource.loop = true;
                _audioSource.playOnAwake = false;
                
                _audioSource.Play();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}