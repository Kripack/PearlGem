using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PearlGem.Utils
{
    public class LevelLoader : MonoBehaviour
    {
        public static string nextSceneToLoad;

        void Start()
        {
            StartCoroutine(LoadLevelAsync());
        }

        IEnumerator LoadLevelAsync()
        {
            if (string.IsNullOrEmpty(nextSceneToLoad))
            {
                Debug.LogError("Ім'я сцени для завантаження не встановлено!");
                yield break;
            }
            
            AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneToLoad);
            
            while (!operation.isDone)
            {
                yield return null;
            }
        }
    }
}