using System.Collections;
using System.Collections.Generic;
using PearlGem.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PearlGem
{
    public class LevelWinHandler : MonoBehaviour
    {
        [SerializeField] List<GameObject> effectsList;
        [SerializeField] AudioClip victorySound;
        void OnEnable()
        {
            GlobalEventBus.Instance.OnLevelWin += OnVictory;
        }

        void OnVictory()
        {
            ShowEffects();
            StartCoroutine(LoadLevelDelay());
        }
        
        void ShowEffects()
        {
            SoundFXManager.Instance.PlayAudioClip(victorySound, transform.position);
            foreach (var effect in effectsList)
            {
                Instantiate(effect, transform.position, Quaternion.identity);
            }
        }
        
        IEnumerator LoadLevelDelay()
        {
            yield return new WaitForSeconds(3.5f);
            
            SceneManager.LoadScene("Loading");
        }
        
        void OnDisable()
        {
            GlobalEventBus.Instance.OnLevelWin -= OnVictory;
        }
    }
}