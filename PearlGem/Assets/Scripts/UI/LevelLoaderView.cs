using PearlGem.Utils;
using UnityEngine;
using TMPro;

namespace PearlGem.UI
{
    public class LevelLoaderView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI levelInfoText;

        void Start()
        {
            levelInfoText.text = LevelLoader.nextSceneToLoad;
        }
    }
}