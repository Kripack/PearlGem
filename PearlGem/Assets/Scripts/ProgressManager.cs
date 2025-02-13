using System.Collections;
using PearlGem.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PearlGem
{
    public class ProgressManager : MonoBehaviour
    {
        const string LevelProgressKey = "LevelProgress";

        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            GlobalEventBus.Instance.OnLevelWin += HandleLevelWin;
        }

        void OnDestroy()
        {
            if (GlobalEventBus.Instance != null)
            {
                GlobalEventBus.Instance.OnLevelWin -= HandleLevelWin;
            }
        }
        
        void HandleLevelWin()
        {
            int levelsCompleted = PlayerPrefs.GetInt(LevelProgressKey, 0);
            levelsCompleted++;
            PlayerPrefs.SetInt(LevelProgressKey, levelsCompleted);

            int nextLevel = levelsCompleted + 1;
            string nextLevelScene = "Level " + nextLevel;

            LevelLoader.nextSceneToLoad = nextLevelScene;
        }
    }
}