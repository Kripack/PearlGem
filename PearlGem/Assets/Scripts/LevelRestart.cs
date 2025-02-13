using UnityEngine;
using UnityEngine.SceneManagement;

namespace PearlGem
{
    public class LevelRestart : MonoBehaviour
    {
        public void RestartLevel()
        {
            SceneManager.LoadScene("Loading");
        }
    }
}