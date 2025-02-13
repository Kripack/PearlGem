using System;
using PearlGem.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PearlGem.UI
{
    public class MainMenuController : MonoBehaviour
    {
        const string LevelProgressKey = "LevelProgress";
        
        public void OnPlayButtonClicked()
        {
            int levelsCompleted = PlayerPrefs.GetInt(LevelProgressKey, 0);
            int nextLevel = levelsCompleted + 1;
            string nextLevelScene = "Level " + nextLevel;

            LevelLoader.nextSceneToLoad = nextLevelScene;

            SceneManager.LoadScene("Loading");
        }
        
        public void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}