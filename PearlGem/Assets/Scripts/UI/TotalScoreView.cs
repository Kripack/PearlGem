using TMPro;
using UnityEngine;

namespace PearlGem.UI
{
    public class TotalScoreView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] Score scoreComponent;
        void Update()
        {
            scoreText.text = scoreComponent.TotalPoints.ToString();
        }
    }
}