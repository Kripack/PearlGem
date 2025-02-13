using UnityEngine;

namespace PearlGem.Utils
{
    public class SoundFXManager : MonoBehaviour
    {
        public static SoundFXManager Instance;
        [SerializeField] AudioSource soundFXObject;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else Destroy(gameObject);
            
            DontDestroyOnLoad(gameObject);
        }

        public void PlayAudioClip(AudioClip clip, Vector3 spawnPosition, float volume = 1f)
        {
            if (clip == null) return;
        
            AudioSource audioSource = Instantiate(soundFXObject, spawnPosition, Quaternion.identity); // потрібен пул
        
            audioSource.PlayOneShot(clip, volume);

            float clipLength = clip.length;
            Destroy(audioSource.gameObject, clipLength);
        }
    
        public void PlayAudioClip(AudioClip clip, AudioSource audioSource, float volume = 1f)
        {
            if (clip != null)
            {
                audioSource.PlayOneShot(clip, volume);
            }
        }
    
        public void PlayRandomAudioClip(AudioClip[] clips, AudioSource audioSource, float volume = 1f)
        {
            if (clips == null) return;
            if (clips.Length == 0) return;
            int randomIndex = Random.Range(0, clips.Length);
            audioSource.PlayOneShot(clips[randomIndex], volume);
        }
    
        public void PlayRandomAudioClip(AudioClip[] clips, Vector3 spawnPosition, float volume = 1f)
        {
            if (clips == null) return;
            if (clips.Length == 0) return;
            AudioSource audioSource = Instantiate(soundFXObject, spawnPosition, Quaternion.identity);
            
            int randomIndex = Random.Range(0, clips.Length);
            audioSource.clip = clips[randomIndex];
            audioSource.volume = volume;
            audioSource.Play();
            
            float clipLength = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLength);
        }
    }
}
